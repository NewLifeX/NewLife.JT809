using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 上报车辆电子运单应答消息
/// <para>子业务类型标识:UP_EXG_MSG_TAKE_EWAYBILL_ACK</para>
/// <para>描述:下级平台应答上级平台发送的上报车辆电子运单请求消息，向上级平台上传车辆当前电子运单</para>
/// </summary>
[MessageKind(MessageKinds.报警督办请求消息)]
public class JT809_0x1200_0x120B
{
    /// <summary>
    /// 对应上报车辆电子运单源子业务类型标识
    /// </summary>
    public UInt16 SourceDataType { get; set; }
    /// <summary>
    ///  对应上报车辆电子运单源报文序列号
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }
    /// <summary>
    /// 电子运单数据体长度
    /// </summary>
    public UInt32 EwaybillLength { get; set; }
    /// <summary>
    /// 电子运单数据内容
    /// </summary>
    public String EwaybillInfo { get; set; }

    //public JT809_0x1200_0x120B Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x1200_0x120B();
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.SourceDataType = reader.ReadUInt16();
    //        value.SourceMsgSn = reader.ReadUInt32();
    //    }
    //    value.EwaybillLength = reader.ReadUInt32();
    //    value.EwaybillInfo = reader.ReadString((Int32)value.EwaybillLength);
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1200_0x120B value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteUInt16(value.SourceDataType);
    //        writer.WriteUInt32(value.SourceMsgSn);
    //    }
    //    writer.WriteUInt32((UInt32)value.EwaybillInfo.Length);
    //    writer.WriteString(value.EwaybillInfo);
    //}
}
