using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 上报车辆行驶记录应答消息
/// <para>子业务类型标识:UP_CTRL_MSG_TAKE_TRAVEL_ACK</para>
/// <para>描述:下级平台应答上级平台下发的"上报车辆行驶记录请求"消息，将车辆行驶记录数据上传至上级平台</para>
/// </summary>
[MessageKind(MessageKinds.上报车辆行驶记录应答消息)]
public class JT809_0x1500_0x1504
{
    /// <summary>
    /// 对应上报车辆行驶记录请求消息源子业务类型标识
    /// </summary>
    public UInt16 SourceDataType { get; set; }
    /// <summary>
    ///  对应上报车辆行驶记录请求消息源报文序列号
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }
    /// <summary>
    /// 命令字
    /// </summary>
    public JT809CommandType CommandType { get; set; }
    /// <summary>
    /// 车辆行驶记录数据体长度
    /// </summary>
    public UInt32 TraveldataLength { get; set; }
    /// <summary>
    /// 车辆行驶记录信息
    /// <para>19056应答帧元数据</para>
    /// </summary>
    [FieldSize(nameof(TraveldataLength))]
    public Byte[] TraveldataInfo { get; set; }

    //public JT809_0x1500_0x1504 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x1500_0x1504();
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.SourceDataType = reader.ReadUInt16();
    //        value.SourceMsgSn = reader.ReadUInt32();
    //        value.CommandType = (JT809CommandType)reader.ReadByte();
    //    }
    //    value.TraveldataLength = reader.ReadUInt32();
    //    value.TraveldataInfo = reader.ReadArray((Int32)value.TraveldataLength).ToArray();
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1500_0x1504 value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteUInt16(value.SourceDataType);
    //        writer.WriteUInt32(value.SourceMsgSn);
    //        writer.WriteByte((Byte)value.CommandType);
    //    }
    //    // 先计算内容长度（汉字为两个字节）
    //    writer.Skip(4, out Int32 lengthPosition);
    //    writer.WriteArray(value.TraveldataInfo);
    //    writer.WriteInt32Return(writer.GetCurrentPosition() - lengthPosition - 4, lengthPosition);
    //}
}
