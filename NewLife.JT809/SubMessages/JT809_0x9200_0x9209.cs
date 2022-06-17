using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 验证结果
/// </summary>
public enum JT809_0x9209_Result : byte
{
    成功_上级平台即刻补发 = 0x00,
    成功_上级平台择机补发 = 0x01,
    失败_上级平台无对应申请的定位数据 = 0x02,
    失败_申请内容不正确 = 0x03,
    其它原因 = 0x04,
}

/// <summary>
/// 补发车辆定位信息应答
/// <para>子业务类型标识:DOWN_EXG_MSG_APPLY_HISGNSSDATA_ACK</para>
/// <para>本条消息是上级平台应答下级平台发送的“补发车辆定位信息请求”消息，即 UP_EXG_MSG_APPLY_HISGNSSDATA_REQ</para>
/// </summary>
[MessageKind(MessageKinds.补发车辆定位信息应答)]
public class JT809_0x9200_0x9209
{
    /// <summary>
    /// 对应补发车辆定位信息请求消息源子业务类型标识
    /// </summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>
    ///  对应补发车辆定位信息请求消息源报文序列号
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }

    /// <summary>
    /// 返回结果
    /// </summary>
    public JT809_0x9209_Result Result { get; set; }

    //public JT809_0x9200_0x9209 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x9200_0x9209();
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.SourceDataType = reader.ReadUInt16();
    //        value.SourceMsgSn = reader.ReadUInt32();
    //    }
    //    value.Result = (JT809_0x9209_Result)reader.ReadByte();
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x9200_0x9209 value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteUInt16(value.SourceDataType);
    //        writer.WriteUInt32(value.SourceMsgSn);
    //    }
    //    writer.WriteByte((Byte)value.Result);
    //}
}
