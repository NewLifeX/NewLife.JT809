using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 下发平台间消息补传请求消息
/// <para>子业务类型标识:DOWN_PLATFORM_MSG_RETRAN_REQ</para>
/// <para>描述:上级平台接收方在接受消息时，如发现消息报文序列号不连接，则立即发送消息补传请求。下级平台收到消息补传请求后，根据请求的消息报文序列号。重传相应的消息</para>
/// <para>2019版数据体见JT/T 809-2019 表57：重传子业务类型(2) + 重传消息总数(1) + 起止报文序列号(8) + 重传起始UTC时间(8)</para>
/// </summary>
[MessageKind(MessageKinds.下发平台间消息补传请求消息)]
public class JT809_0x9300_0x9303 : IAccessor
{
    /// <summary>
    /// 对应需要重传消息地子业务类型标识
    /// </summary>
    public UInt16 RetranDataType { get; set; }

    /// <summary>
    /// 重传消息总数
    /// </summary>
    public Byte SerialCount { get; set; }

    /// <summary>
    /// 需要重传消息的起始报文序列号
    /// <para>如只请求重传一个消息，则起始消息报文序列号和结束消息报文序列号相同 </para>
    /// </summary>
    public UInt32 BeginMessageNumber { get; set; }

    /// <summary>
    /// 需要重传消息的结束的报文序列号
    /// <para>如只请求重传一个消息，则起始消息报文序列号和结束消息报文序列号相同 </para>
    /// </summary>
    public UInt32 EndMessageNumber { get; set; }

    /// <summary>
    /// 重传起始系统utc时间
    /// 8位
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        RetranDataType = reader.ReadUInt16();
        SerialCount = reader.ReadByte();
        BeginMessageNumber = reader.ReadUInt32();
        EndMessageNumber = reader.ReadUInt32();
        Time = reader.Read<DateTime>();

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

        writer.Write(RetranDataType);
        writer.Write(SerialCount);
        writer.Write(BeginMessageNumber);
        writer.Write(EndMessageNumber);
        writer.Write(Time);

        return true;
    }
}