using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

public enum JT809_0x9503_MsgPriority : Byte
{
    紧急 = 0x00,
    一般 = 0x01
}

/// <summary>
/// 下发车辆报文请求
/// <para>子业务类型标识:DOWN_CTRL_MSG_TEXT_INFO</para>
/// <para>描述:用于上级平台向下级平台下发报文到某指定车辆</para>
/// <para>2011版数据体见JT/T 809-2011 表62，2019版数据体见JT/T 809-2019 表82，2019版去掉消息ID序号</para>
/// </summary>
[MessageKind(MessageKinds.下发车辆报文请求)]
public class JT809_0x9500_0x9503 : IAccessor
{
    /// <summary>
    /// 消息ID序号。2011版使用
    /// </summary>
    public UInt32 MsgSequence { get; set; }
    /// <summary>
    /// 报文优先级
    /// </summary>
    public JT809_0x9503_MsgPriority MsgPriority { get; set; }
    /// <summary>
    /// 报文信息长度
    /// </summary>
    public UInt32 MsgLength { get; set; }
    /// <summary>
    /// 报文信息内容
    /// </summary>
    [FieldSize(nameof(MsgLength))]
    public String MsgContent { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        if (reader.Version != "2019") MsgSequence = reader.ReadUInt32();

        MsgPriority = (JT809_0x9503_MsgPriority)reader.ReadByte();

        var len = reader.ReadInt32();
        if (len < 0 || len > stream.Length - stream.Position) return false;

        MsgLength = (UInt32)len;
        if (len > 0) MsgContent = Helper.Encoding.GetString(reader.ReadBytes(len));

        return true;
    }

    /// <summary>写入</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Write(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var writer = Helper.CreateWriter(stream, header);

        if (writer.Version != "2019") writer.Write(MsgSequence);

        writer.Write((Byte)MsgPriority);

        var buf = MsgContent.IsNullOrEmpty() ? Array.Empty<Byte>() : Helper.Encoding.GetBytes(MsgContent);
        MsgLength = (UInt32)buf.Length;

        writer.Write(buf.Length);
        writer.Write(buf, 0, buf.Length);

        return true;
    }
}