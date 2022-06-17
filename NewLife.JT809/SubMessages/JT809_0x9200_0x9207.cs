using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 验证结果
/// </summary>
public enum JT809_0x9207_Result : Byte
{
    申请成功 = 0x00,
    上级平台没有该车数据 = 0x01,
    申请时间段错误 = 0x02,
    其它 = 0x03
}

/// <summary>
/// 申请交换指定车辆定位信息应答
/// <para>子业务类型标识:DOWN_EXG_MSG_APPLY_FOR_MONITOR_STARTUP_ACK</para>
/// <para>描述：应答下级平台申请交换指定车辆定位信息,请求消息."即 UP_EXG_MSG_APPLY_FOR_MONITOR_STARTUP</para>
/// </summary>
[MessageKind(MessageKinds.申请交换指定车辆定位信息应答)]
public class JT809_0x9200_0x9207
{
    /// <summary>
    /// 对应申请交换指定车辆定位信息请求消息源子业务类型标识
    /// </summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>
    ///  对应申请交换指定车辆定位信息请求消息源报文序列号
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }

    /// <summary>
    /// 返回结果
    /// </summary>
    public JT809_0x9207_Result Result { get; set; }

    //public JT809_0x9200_0x9207 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x9200_0x9207();
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.SourceDataType = reader.ReadUInt16();
    //        value.SourceMsgSn = reader.ReadUInt32();
    //    }
    //    value.Result = (JT809_0x9207_Result)reader.ReadByte();
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x9200_0x9207 value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteUInt16(value.SourceDataType);
    //        writer.WriteUInt32(value.SourceMsgSn);
    //    }
    //    writer.WriteByte((byte)value.Result);
    //}
}
