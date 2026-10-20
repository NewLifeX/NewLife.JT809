using System.Buffers;
using System.Collections.Concurrent;
using NewLife.Data;
using NewLife.Messaging;

namespace NewLife.JT809.Protocols;

/// <summary>JT809网络封包。按 0x5b/0x5d 定界并处理转义，支持 2011 与 2019 两种帧头</summary>
/// <remarks>
/// <para>JT809 帧以 0x5b 开始、0x5d 结束，帧内出现 0x5b/0x5a/0x5d/0x5e 时按 0x5a01/0x5a02/0x5e01/0x5e02 转义，
/// 因此帧长与长度字段并不相等，只能通过扫描帧尾标识定界。</para>
/// <para>协议实例无状态可跨连接共享；各下级平台的协议版本按接入码（CenterId）学习并缓存，实现登录后自适应。</para>
/// </remarks>
public class JTCodec : IMessageCodec, IMessageMatcher
{
    private Int32 _gid;
    private readonly ConcurrentDictionary<UInt32, JTVersion> _versions = new();

    /// <summary>协议配置。提供加密密钥与默认协议版本</summary>
    public JTConfig Config { get; set; }

    #region 编解码
    /// <summary>定界并构造消息。从帧首窗口解析完整帧，帧未到齐返回 null</summary>
    /// <param name="buffer">帧首窗口</param>
    /// <returns>解析结果；null 表示帧未到齐</returns>
    public ParseResult? TryParse(ReadOnlySequence<Byte> buffer)
    {
        if (buffer.IsEmpty) return null;

        var len = FindFrameLength(buffer);
        if (len < 0) return null;
        if (len == 0) return new ParseResult { Invalid = true, HeaderSize = 1 };

        var frame = new Byte[len];
        buffer.Slice(0, len).CopyTo(frame);

        var content = JTMessage.Unescape(frame);
        if (content == null) return new ParseResult { Invalid = true, HeaderSize = len };

        var msg = new JTMessage();
        if (!msg.ReadContent(content, Resolve(msg, content)))
        {
            msg.Dispose();
            return new ParseResult { Invalid = true, HeaderSize = len };
        }

        // 记录该接入码的协议版本，后续指令按登录时识别到的版本解析
        if (msg.Version > 0) _versions[msg.CenterId] = msg.Version;

        return new ParseResult { Message = msg, HeaderSize = len, BodyLength = 0 };
    }

    /// <summary>整帧构建（头 + 内存体）</summary>
    /// <param name="message">消息</param>
    /// <returns>整帧拥有句柄，调用方负责 Dispose</returns>
    public IOwnerPacket Build(IMessage message)
    {
        if (message == null) throw new ArgumentNullException(nameof(message));
        if (message is not JTMessage msg) throw new ArgumentException(L.T("Message type [{0}] is not a JT809 message.", message.GetType().FullName), nameof(message));

        if (msg.Sequence == 0) msg.Sequence = (UInt32)Interlocked.Increment(ref _gid);

        var ms = new MemoryStream();
        if (!msg.Write(ms, Resolve(msg)))
        {
            ms.TryDispose();
            return null;
        }

        ms.Position = 0;
        return new OwnerPacket(ms, 0);
    }

    /// <summary>仅构建头部。JT809 帧长受转义影响，无法预先声明体长</summary>
    /// <param name="message">消息</param>
    /// <param name="bodyLength">消息体字节数</param>
    /// <returns>不返回，始终抛出异常</returns>
    /// <exception cref="NotSupportedException">JT809 帧长受转义影响，不支持头部先行发送</exception>
    public IOwnerPacket BuildHeader(IMessage message, Int64 bodyLength) => throw new NotSupportedException(L.T("JT809 frame length depends on escaped bytes; header-only build is not supported."));

    /// <summary>判断响应是否匹配请求。JT809 以请求-应答语义配对，应答消息可交付给挂起的请求</summary>
    /// <param name="request">挂起的请求消息</param>
    /// <param name="response">收到的响应消息</param>
    /// <returns></returns>
    public Boolean Match(IMessage request, IMessage response)
    {
        if (request is not JTMessage req || response is not JTMessage res) return false;

        return !req.Reply && res.Reply;
    }
    #endregion

    #region 辅助
    /// <summary>扫描帧长。返回完整帧（含头尾标识）的字节数，-1 表示数据不足，0 表示帧头非法</summary>
    /// <param name="buffer">帧首窗口</param>
    /// <returns></returns>
    public static Int32 FindFrameLength(ReadOnlySequence<Byte> buffer)
    {
        var reader = new SequenceReader<Byte>(buffer);

        // 头部标识探测
        if (!reader.TryRead(out var head)) return -1;
        if (head != 0x5b) return 0;

        var count = 1L;
        while (reader.TryRead(out var b))
        {
            count++;

            if (b == 0x5a || b == 0x5e)
            {
                // 转义对，跳过其后的一个字节
                if (!reader.TryRead(out _)) return -1;
                count++;
            }
            else if (b == 0x5d)
            {
                return (Int32)count;
            }
        }

        return -1;
    }

    /// <summary>按接入码解析该消息应使用的协议配置</summary>
    /// <param name="msg">消息</param>
    /// <param name="content">帧内容（已反转义）</param>
    /// <returns></returns>
    private JTConfig Resolve(JTMessage msg, Byte[] content)
    {
        var config = Config?.Clone() ?? new JTConfig();

        // 登录前尚未解析出接入码，直接从帧内容读取
        var centerId = msg.CenterId;
        if (centerId == 0 && content != null && content.Length >= 14)
            centerId = (UInt32)((content[10] << 24) | (content[11] << 16) | (content[12] << 8) | content[13]);

        if (centerId > 0 && _versions.TryGetValue(centerId, out var ver)) config.Version = ver;

        return config;
    }

    /// <summary>按接入码解析该消息应使用的协议配置</summary>
    /// <param name="msg">消息</param>
    /// <returns></returns>
    private JTConfig Resolve(JTMessage msg)
    {
        var config = Config?.Clone() ?? new JTConfig();

        if (msg.CenterId > 0 && _versions.TryGetValue(msg.CenterId, out var ver))
            config.Version = ver;
        else if (config.Version <= 0)
            config.Version = msg.Version;

        return config;
    }
    #endregion
}
