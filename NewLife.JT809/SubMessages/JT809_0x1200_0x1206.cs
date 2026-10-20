using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 结束车辆定位信息交换应答消息
/// <para>子业务类型标识:UP_EXG_MSG_RETURN_END_ACK</para>
/// <para>本条消息时下级平台对上级平台服务器下发的DOWN_EXG_MSG_RETURN_END消息的应答消息</para>
/// <para>2011版数据体见JT/T 809-2011 表23，2019版数据体见JT/T 809-2019 表30，2019版新增源子业务类型与源报文序列号</para>
/// </summary>
[MessageKind(MessageKinds.结束车辆定位信息交换应答消息)]
public class JT809_0x1200_0x1206 : IAccessor
{
    /// <summary>
    /// 对应结束车辆定位信息交换请求消息源子业务类型标识。2019版新增
    /// </summary>
    public UInt16 SourceDataType { get; set; }
    /// <summary>
    /// 对应结束车辆定位信息交换请求消息源报文序列号。2019版新增
    /// </summary>
    public UInt32 SourceMsgSN { get; set; }
    /// <summary>
    /// 后续数据长度  值为0x00
    /// </summary>
    public UInt32 DataLength { get; set; } = 0x00;

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        // 2019版在数据长度之前插入源子业务类型与源报文序列号
        if (reader.Version == "2019")
        {
            SourceDataType = reader.ReadUInt16();
            SourceMsgSN = reader.ReadUInt32();
        }

        DataLength = reader.ReadUInt32();

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

        if (writer.Version == "2019")
        {
            writer.Write(SourceDataType);
            writer.Write(SourceMsgSN);
        }

        writer.Write(DataLength);

        return true;
    }
}