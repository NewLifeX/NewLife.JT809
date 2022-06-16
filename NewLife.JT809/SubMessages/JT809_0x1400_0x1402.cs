using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 上报报警信息消息
/// <para>子业务类型标识:UP_WARN_MSG_ADPT_INFO</para>
/// <para>2011 描述:下级平台向上级平台上报某车辆的报警信息</para>
/// <para>2019 描述:下级平台向上级平台“上报报警信息”。</para>
/// <para>本条消息上级平台无需应答</para>
/// </summary>
[MessageKind(MessageKinds.上报报警信息消息)]
public class JT809_0x1400_0x1402
{
    ///// <summary>
    ///// 报警信息来源
    ///// </summary>
    //public JT809WarnSrc WarnSrc { get; set; }
    /// <summary>
    /// 发起报警平台唯一编码，由平台所在地行政区域代码和平台编号组成
    /// </summary>
    [FieldSize(11)]
    public String SourcePlatformId { get; set; }
    /// <summary>
    /// 报警类型
    /// </summary>
    public JT809WarnType WarnType { get; set; }
    /// <summary>
    /// 报警时间
    /// </summary>
    public DateTime WarnTime { get; set; }
    /// <summary>
    /// 事件开始时间 utc
    /// </summary>
    public DateTime StartTime { get; set; }
    /// <summary>
    /// 事件结束时间 utc
    /// </summary>
    public DateTime EndTime { get; set; }
    /// <summary>
    /// 车牌号码 非车辆相关报警全填0
    /// </summary>
    [FieldSize(21)]
    public String VehicleNo { get; set; }
    /// <summary>
    /// 车牌颜色  非车辆相关报警全填0
    /// </summary>
    public JT809VehicleColorType VehicleColor { get; set; }
    /// <summary>
    /// 被报警平台唯一编码，由平台所在地行政区划代码和平台编号组成。非平台相关报警全填0
    /// </summary>
    [FieldSize(11)]
    public String DestinationPlatformId { get; set; }
    /// <summary>
    /// 线路ID 808-2019中0x8606规定的报文中的线路ＩＤ
    /// </summary>
    public UInt32 DRVLineId { get; set; }
    ///// <summary>
    ///// 信息ID
    ///// </summary>
    //public UInt32 InfoID { get; set; }
    /// <summary>
    /// 数据长度
    /// </summary>
    public UInt32 InfoLength { get; set; }
    /// <summary>
    /// 信息内容
    /// </summary>
    [FieldSize(nameof(InfoLength))]
    public String InfoContent { get; set; }

    //public JT809_0x1400_0x1402 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x1400_0x1402();
    //    if (config.Version == JT809Version.JTT2011)
    //    {
    //        value.WarnSrc = (JT809WarnSrc)reader.ReadByte();
    //    }
    //    else
    //    {
    //        value.SourcePlatformId = reader.ReadString(11);
    //    }
    //    value.WarnType = (JT809WarnType)reader.ReadUInt16();
    //    value.WarnTime = reader.ReadUTCDateTime();
    //    if (config.Version == JT809Version.JTT2011)
    //    {
    //        value.InfoID = reader.ReadUInt32();
    //    }
    //    else
    //    {
    //        value.StartTime = reader.ReadUTCDateTime();
    //        value.EndTime = reader.ReadUTCDateTime();
    //        value.VehicleNo = reader.ReadString(21);
    //        value.VehicleColor = (JT809VehicleColorType)reader.ReadByte();
    //        value.DestinationPlatformId = reader.ReadString(11);
    //        value.DRVLineId = reader.ReadUInt32();
    //    }
    //    value.InfoLength = reader.ReadUInt32();
    //    value.InfoContent = reader.ReadString((Int32)value.InfoLength);
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1400_0x1402 value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2011)
    //    {
    //        writer.WriteByte((Byte)value.WarnSrc);
    //    }
    //    else
    //    {
    //        writer.WriteStringPadRight(value.SourcePlatformId, 11);
    //    }
    //    writer.WriteUInt16((UInt16)value.WarnType);
    //    writer.WriteUTCDateTime(value.WarnTime);
    //    if (config.Version == JT809Version.JTT2011)
    //    {
    //        writer.WriteUInt32(value.InfoID);
    //    }
    //    else
    //    {
    //        writer.WriteUTCDateTime(value.StartTime);
    //        writer.WriteUTCDateTime(value.EndTime);
    //        writer.WriteStringPadRight(value.VehicleNo, 21);
    //        writer.WriteByte((Byte)value.VehicleColor);
    //        writer.WriteStringPadRight(value.DestinationPlatformId, 11);
    //        writer.WriteUInt32(value.DRVLineId);
    //    }
    //    // 先计算内容长度（汉字为两个字节）
    //    writer.Skip(4, out Int32 lengthPosition);
    //    writer.WriteString(value.InfoContent);
    //    writer.WriteInt32Return(writer.GetCurrentPosition() - lengthPosition - 4, lengthPosition);
    //}
}
