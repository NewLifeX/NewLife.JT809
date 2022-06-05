using NewLife.Data;
using NewLife.JT809.Protocols;
using NewLife.Messaging;
using NewLife.Model;
using NewLife.Net;

namespace NewLife.JT809;

/// <summary>JT809网络封包</summary>
public class JTCodec : Handler
{
    private Int32 _gid;

    /// <summary>写入数据</summary>
    /// <param name="context"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public override Object Write(IHandlerContext context, Object message)
    {
        if (message is JTMessage msg)
        {
            if (msg.MsgSn == 0) msg.MsgSn = (Byte)Interlocked.Increment(ref _gid);

            message = msg.ToPacket();
        }

        return base.Write(context, message);
    }

    /// <summary>读取数据</summary>
    /// <param name="context"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public override Object Read(IHandlerContext context, Object message)
    {
        if (message is not Packet pk) return base.Read(context, message);

        // 解码得到多个消息
        var list = Decode(context, pk);
        if (list == null) return null;

        foreach (var msg in list)
        {
            // 匹配输入回调，让上层事件收到分包信息。
            // 这里很可能处于网络IO线程，阻塞了下一个Tcp包的接收
            base.Read(context, msg);
        }

        return null;
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

    #region 粘包处理
    /// <summary>解码</summary>
    /// <param name="context"></param>
    /// <param name="pk">包</param>
    /// <returns></returns>
    protected IList<JTMessage> Decode(IHandlerContext context, Packet pk)
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
            if (!msg.Read(e)) return null;

            return msg;
        }).ToList();

        return list;
    }

    const Int32 HEADER_LENGTH = 1 + 4 + 4 + 2 + 4 + 3 + 1 + 4 + 8 + 1;
    //private static readonly Byte[] _start = new Byte[] { 0x5b };
    //private static readonly Byte[] _end = new Byte[] { 0x5d };
    /// <summary>
    /// 获取包含分割字节在内的数据长度
    /// </summary>
    /// <param name="pk"></param>
    /// <returns></returns>
    protected Int32 GetLength(Packet pk)
    {
        if (pk.Total < HEADER_LENGTH) return 0;

        return (Int32)pk.ReadBytes(1, 4).ToUInt32();
        //var idx = pk.IndexOf(_start);
        //if (idx < 0) return 0;

        //var idx2 = pk.IndexOf(_end, idx + 1);
        //if (idx2 < 0) return 0;

        //return idx2 - idx + 1;
    }
    #endregion
}