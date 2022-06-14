using NewLife.JT809.Models;
using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 报警处理结果
/// </summary>
public enum JT809_0x1403_Result : Byte
{
    处理中 = 0x00,
    已处理完毕 = 0x01,
    不做处理 = 0x02,
    将来处理 = 0x03
}

/// <summary>
/// 主动上报报警处理结果信息(2013)
/// 上报报警预警信息（2019）
/// <para>子业务类型标识:UP_WARN_MSG_ADPT_TODO_INFO_2013</para>
/// <para>子业务类型标识:UP_WARN_MSG_INFORM_TIPS_2019</para>
/// <para>描述:下级平台向上级平台上报报警处理结果2013</para>
/// <para>描述：用于下级平台向上级平台上报相关报警预警或运行提示信息2019</para>
/// <para>本条消息上级平台无需应答</para>
/// </summary>
[MessageKind(MessageKinds.主动上报报警处理结果信息2013_上报报警预警信息2019)]
public class JT809_0x1400_0x1403
{
    /// <summary>
    /// 发起报警平台唯一编码，由平台所在地行政区域代码和平台编号组成
    /// </summary>
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
    public String VehicleNo { get; set; }
    /// <summary>
    /// 车牌颜色  非车辆相关报警全填0
    /// </summary>
    public JT809VehicleColorType VehicleColor { get; set; }
    /// <summary>
    /// 被报警平台唯一编码，由平台所在地行政区划代码和平台编号组成。非平台相关报警全填0
    /// </summary>
    public String DestinationPlatformId { get; set; }
    /// <summary>
    /// 线路ID 808-2019中0x8606规定的报文中的线路ＩＤ
    /// </summary>
    public UInt32 DRVLineId { get; set; }
    /// <summary>
    /// 数据长度
    /// </summary>
    public UInt32 InfoLength { get; set; }
    /// <summary>
    /// 信息内容
    /// </summary>
    public String InfoContent { get; set; }
    /// <summary>
    /// 报警信息ID
    /// </summary>
    public UInt32 InfoID { get; set; }
    /// <summary>
    /// 处理结果
    /// </summary>
    public JT809_0x1403_Result Result { get; set; }

    //public JT809_0x1400_0x1403 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x1400_0x1403();
    //    if (config.Version == JT809Version.JTT2011)
    //    {
    //        value.InfoID = reader.ReadUInt32();
    //        value.Result = (JT809_0x1403_Result)reader.ReadByte();
    //    }
    //    else
    //    {
    //        value.SourcePlatformId = reader.ReadBigNumber(11);
    //        value.WarnType = (JT809WarnType)reader.ReadUInt16();
    //        value.WarnTime = reader.ReadUTCDateTime();
    //        value.StartTime = reader.ReadUTCDateTime();
    //        value.EndTime = reader.ReadUTCDateTime();
    //        value.VehicleNo = reader.ReadString(21);
    //        value.VehicleColor = (JT809VehicleColorType)reader.ReadByte();
    //        value.DestinationPlatformId = reader.ReadBigNumber(11);
    //        value.DRVLineId = reader.ReadUInt32();
    //        value.InfoLength = reader.ReadUInt32();
    //        value.InfoContent = reader.ReadString((Int32)value.InfoLength);
    //    }
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1400_0x1403 value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2011)
    //    {
    //        writer.WriteUInt32(value.InfoID);
    //        writer.WriteByte((Byte)value.Result);
    //    }
    //    else
    //    {
    //        writer.WriteBigNumber(value.SourcePlatformId, 11);
    //        writer.WriteUInt16((UInt16)value.WarnType);
    //        writer.WriteUTCDateTime(value.WarnTime);
    //        writer.WriteUTCDateTime(value.StartTime);
    //        writer.WriteUTCDateTime(value.EndTime);
    //        writer.WriteStringPadRight(value.VehicleNo, 21);
    //        writer.WriteByte((Byte)value.VehicleColor);
    //        writer.WriteBigNumber(value.DestinationPlatformId, 11);
    //        writer.WriteUInt32(value.DRVLineId);
    //        // 先计算内容长度（汉字为两个字节）
    //        writer.Skip(4, out Int32 lengthPosition);
    //        writer.WriteString(value.InfoContent);
    //        writer.WriteInt32Return(writer.GetCurrentPosition() - lengthPosition - 4, lengthPosition);
    //    }
    //}
}
