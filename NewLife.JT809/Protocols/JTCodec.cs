using NewLife.Data;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Messaging;
using NewLife.Model;
using NewLife.Net;
using NewLife.Net.Handlers;

namespace NewLife.JT809;

/// <summary>JT809网络封包</summary>
public class JTCodec : MessageCodec<JTMessage>
{
    private Int32 _gid;

    /// <summary>实例化网络封包编码器</summary>
    public JTCodec() => UserPacket = false;

    /// <summary>编码</summary>
    /// <param name="context"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    protected override Object Encode(IHandlerContext context, JTMessage msg)
    {
        if (msg.Sequence == 0) msg.Sequence = (Byte)Interlocked.Increment(ref _gid);

        var ss = context.Owner as IExtend;
        var config = ss["Config"] as JTConfig;
        var ms = new MemoryStream();
        if (msg.Write(ms, config))
        {
            ms.Position = 0;
            var pk = new Packet(ms);
            DefaultSpan.Current?.AppendTag(pk);

            return pk;
        }

        return null;
    }

    /// <summary>加入队列</summary>
    /// <param name="context"></param>
    /// <param name="msg"></param>
    /// <returns></returns>
    protected override void AddToQueue(IHandlerContext context, JTMessage msg)
    {
        if (!msg.Reply) base.AddToQueue(context, msg);
    }

    /// <summary>连接关闭时，清空粘包编码器</summary>
    /// <param name="context"></param>
    /// <param name="reason"></param>
    /// <returns></returns>
    public override Boolean Close(IHandlerContext context, String reason)
    {
        if (context.Owner is IExtend ss) ss["Codec"] = null;

        return base.Close(context, reason);
    }

    /// <summary>是否匹配响应</summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    protected override Boolean IsMatch(Object request, Object response)
    {
        if (request is not JTMessage req || response is not JTMessage res) return false;

        // 请求响应前后配对
        //if (req.Reply || !res.Reply) return false;
        if (req.Reply) return false;

        //return req.Sequence == res.Sequence;
        return true;
    }

    #region 粘包处理
    /// <summary>解码</summary>
    /// <param name="context"></param>
    /// <param name="pk">包</param>
    /// <returns></returns>
    protected override IList<JTMessage> Decode(IHandlerContext context, Packet pk)
    {
        var ss = context.Owner as IExtend;
        if (ss["Codec"] is not PacketCodec pc)
            ss["Codec"] = pc = new PacketCodec
            {
                MaxCache = 1024,
                GetLength = GetLength,
                Tracer = (context.Owner as ISocket)?.Tracer
            };

        var pks = pc.Parse(pk);
        var list = pks.Select(e =>
        {
            var msg = new JTMessage();
            if (!msg.Read(e, ss["Config"] as JTConfig)) return null;

            return msg;
        }).ToList();

        return list;
    }

    private const Int32 HEADER_LENGTH = 1 + 4 + 4 + 2 + 4 + 3 + 1 + 4 + 2 + 1;
    private const Int32 HEADER_LENGTH_2019 = 1 + 4 + 4 + 2 + 4 + 3 + 1 + 4 + 8 + 2 + 1;
    /// <summary>
    /// 获取包含分割字节在内的数据长度
    /// </summary>
    /// <param name="pk"></param>
    /// <returns></returns>
    public Int32 GetLength(Packet pk)
    {
        if (pk.Total < HEADER_LENGTH) return 0;

        var len = (Int32)pk.ReadBytes(1, 4).ToUInt32(0, false);
        if (len <= 0) return 0;

        // 遇到转义字符，增加长度
        for (var i = 0; i < pk.Total - 1; i++)
        {
            var b = pk[i];
            if (b == 0x5a)
            {
                var b2 = pk[i + 1];
                if (b2 == 0x01 || b2 == 0x02) len++;
            }
            else if (b == 0x5e)
            {
                var b2 = pk[i + 1];
                if (b2 == 0x01 || b2 == 0x02) len++;
            }
        }

        return len;
    }
    #endregion
}