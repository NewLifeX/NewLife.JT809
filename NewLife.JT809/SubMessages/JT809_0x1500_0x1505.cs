using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 应答结果
/// </summary>
public enum JT809_0x1505_Result : Byte
{
    车载终端成功收到该命令 = 0x00,
    无该车辆 = 0x01,
    其它原因失败 = 0x02
}

/// <summary>
/// 车辆应急接入监管平台应答消息
/// <para>子业务类型标识: UP_CTRL_MSG_EMERGENCY_MONITORING_ACK</para>
/// <para>描述:下级平台应答上级平台下发的"车辆应急接入监管平台请求"消息应答</para>
/// <para>2011版数据体见JT/T 809-2011 表58，2019版数据体见JT/T 809-2019 表78，2019版新增源子业务类型与源报文序列号</para>
/// </summary>
[MessageKind(MessageKinds.车辆应急接入监管平台应答消息)]
public class JT809_0x1500_0x1505 : IAccessor
{
    /// <summary>
    /// 对应车辆应急接入监管平台请求消息源子业务类型标识。2019版新增
    /// </summary>
    public UInt16 SourceDataType { get; set; }
    /// <summary>
    ///  对应车辆应急接入监管平台请求消息源报文序列号。2019版新增
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }
    /// <summary>
    /// 应答结果
    /// </summary>
    public JT809_0x1505_Result Result { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        if (reader.Version == "2019")
        {
            SourceDataType = reader.ReadUInt16();
            SourceMsgSn = reader.ReadUInt32();
        }

        Result = (JT809_0x1505_Result)reader.ReadByte();

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
            writer.Write(SourceMsgSn);
        }

        writer.Write((Byte)Result);

        return true;
    }

    //public JT809_0x1500_0x1505 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x1500_0x1505();
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.SourceDataType = reader.ReadUInt16();
    //        value.SourceMsgSn = reader.ReadUInt32();
    //    }
    //    value.Result = (JT809_0x1505_Result)reader.ReadByte();
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1500_0x1505 value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteUInt16(value.SourceDataType);
    //        writer.WriteUInt32(value.SourceMsgSn);
    //    }
    //    writer.WriteByte((Byte)value.Result);
    //}
}