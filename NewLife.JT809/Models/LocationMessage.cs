using System.Globalization;
using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.Models;

/// <summary>
/// 位置信息汇报
/// </summary>
//[MessageKind(MessageKinds.位置信息汇报)]
public class LocationMessage : IAccessor
{
    /// <summary>
    /// 报警标志 
    /// </summary>
    public LocationAlarms Alarm { get; set; }

    /// <summary>
    /// 状态位标志
    /// </summary>
    public LocationStatus Status { get; set; }

    /// <summary>纬度。百万分之一度</summary>
    public Int32 Latitude { get; set; }

    /// <summary>经度。百万分之一度</summary>
    public Int32 Longitude { get; set; }

    /// <summary>海拔高度，单位为米</summary>
    public UInt16 Altitude { get; set; }

    /// <summary>速度 1/10km/h</summary>
    public UInt16 Speed { get; set; }

    /// <summary>方向 0-359，正北为 0，顺时针</summary>
    public UInt16 Direction { get; set; }

    /// <summary>GPS时间</summary>
    public DateTime GPSTime { get; set; }

    /// <summary>附加信息</summary>
    public IDictionary<AdditionalKinds, Object> Additionals { get; set; }

    /// <summary>读取消息</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        //var reader = context as Binary ?? Helper.CreateReader(stream);
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        Alarm = (LocationAlarms)reader.ReadUInt32();
        Status = (LocationStatus)reader.ReadUInt32();

        // 南纬
        if (Status.Has(LocationStatus.Latitude))
            Latitude = (Int32)reader.ReadUInt32();
        else
            Latitude = reader.ReadInt32();

        // 西经
        if (Status.Has(LocationStatus.Longitude))
            Longitude = (Int32)reader.ReadUInt32();
        else
            Longitude = reader.ReadInt32();

        Altitude = reader.ReadUInt16();
        Speed = reader.ReadUInt16();
        Direction = reader.ReadUInt16();
        //GPSTime = reader.ReadBCD(6);

        var str = reader.ReadBCD(6);
        if (DateTime.TryParseExact(str, "yyMMddHHmmss", null, DateTimeStyles.None, out var dt))
            GPSTime = dt;

        // 位置附加信息
        Additionals = reader.ReadAdditional();

        return true;
    }

    /// <summary>序列化消息</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Boolean Write(Stream stream, Object context)
    {
        //var writer = context as Binary ?? Helper.CreateWriter(stream);
        var header = context as JTMessage;
        var writer = Helper.CreateWriter(stream, header);

        writer.WriteUInt32((UInt32)Alarm);
        writer.WriteUInt32((UInt32)Status);

        if (Status.Has(LocationStatus.Latitude))
            writer.WriteUInt32((UInt32)Latitude);
        else
            writer.WriteInt32(Latitude);

        if (Status.Has(LocationStatus.Longitude))
            writer.WriteUInt32((UInt32)Longitude);
        else
            writer.WriteInt32(Longitude);

        writer.WriteUInt16(Altitude);
        writer.WriteUInt16(Speed);
        writer.WriteUInt16(Direction);
        writer.WriteBCD(GPSTime.ToString("yyMMddHHmmss"), 6);

        // 位置附加信息
        writer.WriteAdditional(Additionals);

        return true;
    }
}