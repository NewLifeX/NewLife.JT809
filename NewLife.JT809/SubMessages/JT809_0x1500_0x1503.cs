using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 应答结果
/// </summary>
public enum JT809_0x1503_Result : Byte
{
    下发成功 = 0x00,
    下发失败 = 0x01,
}

/// <summary>
/// 下发车辆报文应答消息
/// <para>子业务类型标识:UP_CTRL_MSG_TEXT_INFO_ACK</para>
/// <para>描述:下级平台应答上级平台下发的报文是否成功到达指定车辆</para>
/// </summary>
[MessageKind(MessageKinds.下发车辆报文应答消息)]
public class JT809_0x1500_0x1503
{
    /// <summary>
    /// 对应下发车辆报文请求消息源子业务类型标识
    /// </summary>
    public UInt16 SourceDataType { get; set; }
    /// <summary>
    ///  对应下发车辆报文请求消息源报文序列号
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }
    /// <summary>
    /// 消息ID
    /// 对应“下发车辆报文请求消息”中的MSG_ID
    /// </summary>
    public UInt32 MsgID { get; set; }
    /// <summary>
    /// 应答结果
    /// </summary>
    public JT809_0x1503_Result Result { get; set; }

    //public JT809_0x1500_0x1503 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x1500_0x1503();
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.SourceDataType = reader.ReadUInt16();
    //        value.SourceMsgSn = reader.ReadUInt32();
    //    }
    //    else
    //    {
    //        value.MsgID = reader.ReadUInt32();
    //    }
    //    value.Result = (JT809_0x1503_Result)reader.ReadByte();
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1500_0x1503 value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteUInt16(value.SourceDataType);
    //        writer.WriteUInt32(value.SourceMsgSn);
    //    }
    //    else
    //    {
    //        writer.WriteUInt32(value.MsgID);
    //    }
    //    writer.WriteByte((Byte)value.Result);
    //}
}
