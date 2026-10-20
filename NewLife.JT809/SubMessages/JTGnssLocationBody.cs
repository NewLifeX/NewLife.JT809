using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>车辆定位信息数据体。主链路 0x1202 与从链路 0x9202 共用同一结构</summary>
/// <remarks>
/// 两个版本的结构不同：
/// 2011版见JT/T 809-2011 表69，为「加密标识 + 7字节时间 + 经纬度/速度/里程/方向/海拔/状态/报警」的36字节定长结构，使用定长属性；
/// 2019版见JT/T 809-2019 表90，为「加密标识 + 数据长度 + 定位数据 + 三级平台编码与报警状态」，定位数据按JT/T 808-2019 中8.12 的位置信息汇报格式承载，
/// 使用 <see cref="GnssData"/>（原始字节）与 <see cref="Location"/>（解析结果）读写。
/// </remarks>
public class JTGnssLocationBody : IAccessor
{
    #region 属性
    /// <summary>
    /// 是否使用国测局批准的地图保密插件进行加密 1 已加密 0未加密
    /// </summary>
    public JT809_VehiclePositionEncrypt Encrypt { get; set; }

    /// <summary>时间。2011版使用</summary>
    public DateTime Time { get; set; }

    /// <summary>经度。百万分之一度。2011版使用</summary>
    public Int32 Longitude { get; set; }

    /// <summary>纬度。百万分之一度。2011版使用</summary>
    public Int32 Latitude { get; set; }

    /// <summary>速度 km/h。2011版使用</summary>
    public UInt16 Speed { get; set; }

    /// <summary>行驶记录速度 km/h。2011版使用</summary>
    public UInt16 Speed2 { get; set; }

    /// <summary>总里程 km。2011版使用</summary>
    public UInt32 Mileage { get; set; }

    /// <summary>方向 0-359，正北为 0，顺时针。2011版使用</summary>
    public UInt16 Direction { get; set; }

    /// <summary>海拔高度，单位为米。2011版使用</summary>
    public UInt16 Altitude { get; set; }

    /// <summary>
    /// 状态位标志。2011版使用
    /// </summary>
    public LocationStatus Status { get; set; }

    /// <summary>
    /// 报警标志 
    /// </summary>
    public LocationAlarms Alarm { get; set; }

    /// <summary>车辆定位信息数据长度。2019版为定位数据的字节数</summary>
    public Int32 DataLength => GnssData?.Length ?? 0;

    /// <summary>车辆定位信息内容原始字节。2019版按 808-2019 中 8.12 的位置信息汇报格式，已加密时保留密文</summary>
    public Byte[] GnssData { get; set; }

    /// <summary>卫星定位数据。2019版由 <see cref="GnssData"/> 解码得到</summary>
    public LocationMessage Location { get; set; }

    /// <summary>监控平台唯一编码，由平台所在地行政区域代码和平台编码组成，11字节。2019新增</summary>
    public String PlatformId1 { get; set; }

    /// <summary>报警状态，具体定义按照 808-2019 中表18的规定。2019新增</summary>
    public LocationAlarms Alarm1 { get; set; }

    /// <summary>市级监管平台唯一编码，未填写时全填0，无市级平台应由省级平台全填1，11字节。2019新增</summary>
    public String PlatformId2 { get; set; }

    /// <summary>报警状态，具体定义按照 808-2019 中表18的规定。2019新增</summary>
    public LocationAlarms Alarm2 { get; set; }

    /// <summary>省级监管平台唯一编码，未填写时全填0，11字节。2019新增</summary>
    public String PlatformId3 { get; set; }

    /// <summary>报警状态，具体定义按照 808-2019 中表18的规定。2019新增</summary>
    public LocationAlarms Alarm3 { get; set; }
    #endregion

    #region 方法
    /// <summary>读取消息</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        Encrypt = (JT809_VehiclePositionEncrypt)reader.ReadByte();

        // 2019版按 809-2019 表90：加密标识 + 数据长度 + 定位数据 + 三级平台编码与报警状态
        // 子消息经 Binary.TryRead 分派时拿不到 JTMessage 上下文，版本以读取器的 Version 为准
        if (reader.Version == "2019") return Read2019(stream, reader);

        // 2011版按 809-2011 表69：加密标识 + 7字节时间 + 定长定位字段
        var buf = reader.ReadBytes(7);
        if (buf == null || buf.Length < 7) return false;

        // 时间字段异常时保持默认值，避免畸形报文引发异常
        var year = buf.ToUInt16(2, false);
        if (year is >= 1900 and <= 9999 && buf[1] is >= 1 and <= 12 && buf[0] >= 1 && buf[0] <= DateTime.DaysInMonth(year, buf[1]) &&
            buf[4] < 24 && buf[5] < 60 && buf[6] < 60)
            Time = new DateTime(year, buf[1], buf[0], buf[4], buf[5], buf[6]);

        Longitude = reader.ReadInt32();
        Latitude = reader.ReadInt32();

        Speed = reader.ReadUInt16();
        Speed2 = reader.ReadUInt16();
        Mileage = reader.ReadUInt32();
        Direction = reader.ReadUInt16();
        Altitude = reader.ReadUInt16();

        Status = (LocationStatus)reader.ReadUInt32();
        Alarm = (LocationAlarms)reader.ReadUInt32();

        return true;
    }

    /// <summary>序列化消息</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Write(Stream stream, Object context)
    {
        //var writer = context as Binary ?? Helper.CreateWriter(stream);
        var header = context as JTMessage;
        var writer = Helper.CreateWriter(stream, header);

        writer.WriteByte((Byte)Encrypt);

        // 2019版按 809-2019 表90 写入
        if (writer.Version == "2019") return Write2019(writer);

        var dt = Time;
        var buf = new Byte[7];
        buf[0] = (Byte)dt.Day;
        buf[1] = (Byte)dt.Month;
        buf.Write((UInt16)dt.Year, 2, false);
        buf[4] = (Byte)dt.Hour;
        buf[5] = (Byte)dt.Minute;
        buf[6] = (Byte)dt.Second;
        writer.Write(buf, 0, buf.Length);

        writer.WriteInt32(Longitude);
        writer.WriteInt32(Latitude);

        writer.WriteUInt16(Speed);
        writer.WriteUInt16(Speed2);
        writer.WriteUInt32(Mileage);
        writer.WriteUInt16(Direction);
        writer.WriteUInt16(Altitude);

        writer.WriteUInt32((UInt32)Status);
        writer.WriteUInt32((UInt32)Alarm);

        return true;
    }

    /// <summary>读取2019版车辆定位信息数据体</summary>
    /// <param name="stream">数据流</param>
    /// <param name="reader">读取器</param>
    /// <returns></returns>
    private Boolean Read2019(Stream stream, Binary reader)
    {
        var len = reader.ReadInt32();
        if (len < 0 || len > stream.Length - stream.Position) return false;

        if (len > 0)
        {
            GnssData = reader.ReadBytes(len);

            // 定位数据经国家测绘局保密插件加密时保留密文，不做解析
            if (Encrypt == JT809_VehiclePositionEncrypt.未加密) Decode();
        }

        // 三级平台编码与报警状态为2019新增，部分平台实现未携带，剩余长度不足时跳过
        if (stream.Length - stream.Position >= 3 * (11 + 4))
        {
            PlatformId1 = reader.ReadFixedString(11);
            Alarm1 = (LocationAlarms)reader.ReadUInt32();
            PlatformId2 = reader.ReadFixedString(11);
            Alarm2 = (LocationAlarms)reader.ReadUInt32();
            PlatformId3 = reader.ReadFixedString(11);
            Alarm3 = (LocationAlarms)reader.ReadUInt32();
        }

        return true;
    }

    /// <summary>写入2019版车辆定位信息数据体</summary>
    /// <param name="writer">写入器</param>
    /// <returns></returns>
    private Boolean Write2019(Binary writer)
    {
        if (GnssData == null && Location != null) Encode();

        var buf = GnssData ?? Array.Empty<Byte>();
        writer.Write(buf.Length);
        writer.Write(buf, 0, buf.Length);

        writer.WriteFixedString(PlatformId1, 11);
        writer.Write((UInt32)Alarm1);
        writer.WriteFixedString(PlatformId2, 11);
        writer.Write((UInt32)Alarm2);
        writer.WriteFixedString(PlatformId3, 11);
        writer.Write((UInt32)Alarm3);

        return true;
    }

    /// <summary>解码卫星定位数据</summary>
    /// <remarks>把2019版原始定位数据 <see cref="GnssData"/> 解析为 <see cref="Location"/></remarks>
    /// <returns></returns>
    public Boolean Decode()
    {
        var buf = GnssData;
        if (buf == null || buf.Length == 0) return false;

        try
        {
            var location = new LocationMessage();
            if (!location.Read(new MemoryStream(buf), null)) return false;

            Location = location;

            return true;
        }
        catch (Exception)
        {
            // 定位数据不完整或格式不符时保持未解析状态，不影响消息其余字段
            return false;
        }
    }

    /// <summary>编码卫星定位数据</summary>
    /// <remarks>把 <see cref="Location"/> 序列化为2019版原始定位数据 <see cref="GnssData"/></remarks>
    /// <returns></returns>
    public Boolean Encode()
    {
        if (Location == null) return false;

        var ms = new MemoryStream();
        if (!Location.Write(ms, null)) return false;

        GnssData = ms.ToArray();

        return true;
    }
    #endregion
}
