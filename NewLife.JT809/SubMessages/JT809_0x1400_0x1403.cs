using NewLife;
using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.Serialization;

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
/// <para>子业务类型标识:UP_WARN_MSG_INFORM_TIPS</para>
/// <para>2019 描述:用于下级平台向上级平台上报相关报警预警或运行提示信息。本条消息上级平台无需应答</para>
/// <para>2019版数据体见JT/T 809-2019 表71</para>
/// </summary>
/// <remarks>未取得2013版规范正文，非2019版本按「信息ID + 处理结果」读写</remarks>
[MessageKind(MessageKinds.主动上报报警处理结果信息2013_上报报警预警信息2019)]
public class JT809_0x1400_0x1403 : IAccessor
{
    /// <summary>发起报警平台唯一编码。2019版使用</summary>
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

    /// <summary>线路ID。2019版使用</summary>
    public UInt32 DRVLineId { get; set; }

    /// <summary>信息长度</summary>
    public UInt32 InfoLength { get; set; }

    /// <summary>信息内容</summary>
    public String InfoContent { get; set; }

    /// <summary>信息ID。非2019版本使用</summary>
    public UInt32 InfoID { get; set; }

    /// <summary>报警处理结果。非2019版本使用</summary>
    public JT809_0x1403_Result Result { get; set; }

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
            InfoID = reader.ReadUInt32();
            Result = (JT809_0x1403_Result)reader.ReadByte();

            return true;
        }

        var len = reader.ReadInt32();
        if (len < 0 || len > stream.Length - stream.Position) return false;
        InfoLength = (UInt32)len;
        if (len > 0) InfoContent = Helper.Encoding.GetString(reader.ReadBytes(len));

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

            var buf = InfoContent.IsNullOrEmpty() ? Array.Empty<Byte>() : Helper.Encoding.GetBytes(InfoContent);
            InfoLength = (UInt32)buf.Length;
            writer.Write(buf.Length);
            writer.Write(buf, 0, buf.Length);
        }
        else
        {
            writer.Write(InfoID);
            writer.Write((Byte)Result);
        }

        return true;
    }
}