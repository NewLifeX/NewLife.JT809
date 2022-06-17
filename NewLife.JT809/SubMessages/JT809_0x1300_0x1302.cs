using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 下发平台间报文应答消息
/// <para>子业务类型标识:UP_PLATFORM_MSG_INFO_ACK</para>
/// <para>描述:下级平台收到上级平台发送的"下发平台间报文请求"消息后，根据相应的下发报文对象类型，进行转发，并向上级平台发送应答消息</para>
/// </summary>
[MessageKind(MessageKinds.下发平台间报文应答消息)]
public class JT809_0x1300_0x1302
{
    /// <summary>
    /// 对应下发平台间报文请求消息源子业务类型标识
    /// </summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>
    ///  对应下发平台间报文请求消息源报文序列号
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }

    ///// <summary>
    ///// 信息ID
    ///// </summary>
    //public UInt32 InfoID { get; set; }

    //public JT809_0x1300_0x1302 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x1300_0x1302();
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.SourceDataType = reader.ReadUInt16();
    //        value.SourceMsgSn = reader.ReadUInt32();
    //    }
    //    else
    //    {
    //        value.InfoID = reader.ReadUInt32();
    //    }
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1300_0x1302 value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteUInt16(value.SourceDataType);
    //        writer.WriteUInt32(value.SourceMsgSn);
    //    }
    //    else
    //    {
    //        writer.WriteUInt32(value.InfoID);
    //    }
    //}
}