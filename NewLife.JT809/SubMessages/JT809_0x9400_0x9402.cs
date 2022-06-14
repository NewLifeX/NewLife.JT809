using NewLife.JT809.Models;
using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

///// <summary>
///// 报警信息来源
///// </summary>
//public enum JT809WarnSrc : Byte
//{
//    车载终端 = 0x01,
//    企业监控平台 = 0x02,
//    政府监管平台 = 0x03,
//    其他 = 0x09
//}

/// <summary>
///  报警类型
/// </summary>
public enum JT809WarnType : UInt16
{
    超速报警 = 0x0001,
    疲劳驾驶报警 = 0x0002,
    紧急报警 = 0x0003,
    进入指定区域报警 = 0x0004,
    离开指定区域报警 = 0x0005,
    越界报警 = 0x0008,
    盗警 = 0x0009,
    劫警 = 0x000A,
    偏离路线报警 = 0x000B,
    车辆移动报警 = 0x000C,
    超时驾驶报警 = 0x000D,
    违规行驶报警 = 0x0010,
    前撞报警 = 0x0011,
    车道偏离报警 = 0x0012,
    胎压异常报警 = 0x0013,
    动态信息异常报警 = 0x0014,
    其他报警 = 0x000E,

    超时停车 = 0xA001,
    车辆定位信息上报时间间隔异常 = 0xA002,
    车辆定位信息上报距离间隔异常 = 0xA003,
    下级平台异常断线 = 0xA004,
    下级平台数据传输异常 = 0xA005,
    路段堵塞报警 = 0xA006,
    危险路段报警 = 0xA007,
    雨雪天气报警 = 0xA008,
    驾驶员身份识别异常 = 0xA009,
    终端异常 = 0xA00A,
    平台接入异常 = 0xA00B,
    核心数据异常 = 0xA00C,
    其他报警1 = 0xA0FF,
}

/// <summary>
/// 报警预警2013
/// 下发报警预警消息2019
/// <para>子业务类型标识:DOWN_WARN_MSG_INFORM_TIPS</para>
/// <para>描述:用于上级平台向车辆归属或车辆跨域下级平台下发相关车辆的报警顶警或运行提示信息</para>
/// <para>本条消息下级平台无需应答</para>
/// </summary>
[MessageKind(MessageKinds.报警预警2013_下发报警预警消息2019)]
public class JT809_0x9400_0x9402
{
    /// <summary>
    /// 报警信息来源
    /// </summary>
    public JT809WarnSrc WarnSrc { get; set; }
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
    public UInt32 WarnLength { get; set; }
    /// <summary>
    /// 报警描述
    /// </summary>
    public String WarnContent { get; set; }

    //public JT809_0x9400_0x9402 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x9400_0x9402();
    //    if (config.Version == JT809Version.JTT2011)
    //    {
    //        value.WarnSrc = (JT809WarnSrc)reader.ReadByte();
    //    }
    //    else
    //    {
    //        value.SourcePlatformId = reader.ReadBigNumber(11);
    //    }
    //    value.WarnType = (JT809WarnType)reader.ReadUInt16();
    //    value.WarnTime = reader.ReadUTCDateTime();
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.StartTime = reader.ReadUTCDateTime();
    //        value.EndTime = reader.ReadUTCDateTime();
    //        value.VehicleNo = reader.ReadString(21);
    //        value.VehicleColor = (JT809VehicleColorType)reader.ReadByte();
    //        value.DestinationPlatformId = reader.ReadBigNumber(11);
    //        value.DRVLineId = reader.ReadUInt32();
    //    }
    //    value.WarnLength = reader.ReadUInt32();
    //    value.WarnContent = reader.ReadString((Int32)value.WarnLength);
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x9400_0x9402 value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2011)
    //    {
    //        writer.WriteByte((Byte)value.WarnSrc);
    //    }
    //    else
    //    {
    //        writer.WriteBigNumber(value.SourcePlatformId, 11);
    //    }
    //    writer.WriteUInt16((UInt16)value.WarnType);
    //    writer.WriteUTCDateTime(value.WarnTime);
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteUTCDateTime(value.StartTime);
    //        writer.WriteUTCDateTime(value.EndTime);
    //        writer.WriteStringPadRight(value.VehicleNo, 21);
    //        writer.WriteByte((Byte)value.VehicleColor);
    //        writer.WriteBigNumber(value.DestinationPlatformId, 11);
    //        writer.WriteUInt32(value.DRVLineId);
    //    }
    //    // 先计算内容长度（汉字为两个字节）
    //    writer.Skip(4, out Int32 lengthPosition);
    //    writer.WriteString(value.WarnContent);
    //    writer.WriteInt32Return(writer.GetCurrentPosition() - lengthPosition - 4, lengthPosition);
    //}
}
