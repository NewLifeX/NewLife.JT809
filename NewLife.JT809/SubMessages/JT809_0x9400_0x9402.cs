using NewLife;
using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 下发报警预警消息
/// <para>子业务类型标识:DOWN_WARN_MSG_INFORM_TIPS</para>
/// <para>描述:用于上级平台向车辆归属或车辆跨域下级平台下发相关报警预警或运行提示信息。本条消息下级平台无需应答</para>
/// <para>2011版数据体见JT/T 809-2011 表51，2019版数据体见JT/T 809-2019 表69</para>
/// </summary>
[MessageKind(MessageKinds.报警预警2013_下发报警预警消息2019)]
public class JT809_0x9400_0x9402 : IAccessor
{
    /// <summary>报警信息来源。2011版使用</summary>
    public JT809WarnSrc WarnSrc { get; set; }

    /// <summary>发起报警平台唯一编码，由平台所在地行政区域代码和平台编号组成。2019版使用</summary>
    public String SourcePlatformId { get; set; }

    /// <summary>报警类型</summary>
    public JT809WarnType WarnType { get; set; }

    /// <summary>报警时间</summary>
    public DateTime WarnTime { get; set; }

    /// <summary>事件开始时间。2019版使用</summary>
    public DateTime StartTime { get; set; }

    /// <summary>事件结束时间。2019版使用</summary>
    public DateTime EndTime { get; set; }

    /// <summary>车牌号码，非车辆相关报警全填0。2019版使用</summary>
    public String VehicleNo { get; set; }

    /// <summary>车牌颜色，非车辆相关报警全填0。2019版使用</summary>
    public JT809VehicleColorType VehicleColor { get; set; }

    /// <summary>被报警平台唯一编码，非平台相关报警全填0。2019版使用</summary>
    public String DestinationPlatformId { get; set; }

    /// <summary>线路ID，808-2019中0x8606规定的报文中的线路ID。2019版使用</summary>
    public UInt32 DRVLineId { get; set; }

    /// <summary>报警信息内容长度，最长1024字节</summary>
    public UInt32 WarnLength { get; set; }

    /// <summary>报警描述，报警信息提示</summary>
    public String WarnContent { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        if (reader.Version == "2019")
        {
            SourcePlatformId = reader.ReadFixedString(11);
            WarnType = (JT809WarnType)reader.ReadUInt16();
            WarnTime = reader.Read<DateTime>();
            StartTime = reader.Read<DateTime>();
            EndTime = reader.Read<DateTime>();
            VehicleNo = reader.ReadFixedString(21);
            VehicleColor = (JT809VehicleColorType)reader.ReadByte();
            DestinationPlatformId = reader.ReadFixedString(11);
            DRVLineId = reader.ReadUInt32();
        }
        else
        {
            WarnSrc = (JT809WarnSrc)reader.ReadByte();
            WarnType = (JT809WarnType)reader.ReadUInt16();
            WarnTime = reader.Read<DateTime>();
        }

        var len = reader.ReadInt32();
        if (len < 0 || len > stream.Length - stream.Position) return false;
        WarnLength = (UInt32)len;
        if (len > 0) WarnContent = Helper.Encoding.GetString(reader.ReadBytes(len));

        return true;
    }

    /// <summary>写入</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Write(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var writer = Helper.CreateWriter(stream, header);

        if (writer.Version == "2019")
        {
            writer.WriteFixedString(SourcePlatformId, 11);
            writer.Write((UInt16)WarnType);
            writer.Write(WarnTime);
            writer.Write(StartTime);
            writer.Write(EndTime);
            writer.WriteFixedString(VehicleNo, 21);
            writer.Write((Byte)VehicleColor);
            writer.WriteFixedString(DestinationPlatformId, 11);
            writer.Write(DRVLineId);
        }
        else
        {
            writer.Write((Byte)WarnSrc);
            writer.Write((UInt16)WarnType);
            writer.Write(WarnTime);
        }

        var buf = WarnContent.IsNullOrEmpty() ? Array.Empty<Byte>() : Helper.Encoding.GetBytes(WarnContent);
        WarnLength = (UInt32)buf.Length;
        writer.Write(buf.Length);
        writer.Write(buf, 0, buf.Length);

        return true;
    }
}