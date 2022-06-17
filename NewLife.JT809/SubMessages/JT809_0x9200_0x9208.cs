using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 验证结果
/// </summary>
public enum JT809_0x9208_Result : Byte
{
    取消申请成功 = 0x00,
    之前没有对应申请信息 = 0x01,
    其它 = 0x02
}

/// <summary>
/// 取消交换指定车辆定位信息应答or取消申请交换指定车辆定位信息应答消息
/// <para>子业务类型标识:DOWN_EXG_MSG_APPLY_FOR_MONITOR_END_ACK</para>
/// <para>应答下级平台“取消申请交换指定车辆定位信息”</para>
/// </summary>
[MessageKind(MessageKinds.取消交换指定车辆定位信息应答or取消申请交换指定车辆定位信息应答消息)]
public class JT809_0x9200_0x9208
{
    /// <summary>
    /// 对应取消申请交换指定车辆定位信息请求消息源子业务类型标识
    /// </summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>
    ///  对应取消申请交换指定车辆定位信息请求消息源报文序列号
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }

    /// <summary>
    /// 返回结果
    /// </summary>
    public JT809_0x9208_Result Result { get; set; }

    //public JT809_0x9200_0x9208 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x9200_0x9208();
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.SourceDataType = reader.ReadUInt16();
    //        value.SourceMsgSn = reader.ReadUInt32();
    //    }
    //    value.Result = (JT809_0x9208_Result)reader.ReadByte();
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x9200_0x9208 value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteUInt16(value.SourceDataType);
    //        writer.WriteUInt32(value.SourceMsgSn);
    //    }
    //    writer.WriteByte((Byte)value.Result);
    //}
}
