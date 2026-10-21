using System.Globalization;
using System.Text;
using NewLife.Data;
using NewLife.JT809.Protocols;
using NewLife.JT809.Serialization;
using NewLife.Serialization;

namespace NewLife.JT809;

/// <summary>
/// 序列化助手
/// </summary>
public static class Helper
{
    /// <summary>
    /// 字符串编码
    /// </summary>
    public static Encoding Encoding { get; }

    static Helper()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding = Encoding.GetEncoding("GBK");
    }

    ///// <summary>
    ///// 创建读取器
    ///// </summary>
    ///// <param name="stream"></param>
    ///// <returns></returns>
    //public static Binary CreateReader(Stream stream)
    //{
    //    var reader = new Binary()
    //    {
    //        Stream = stream,
    //        Encoding = Encoding,
    //        IsLittleEndian = false,
    //        UseFieldSize = true,
    //        UseRef = false,
    //        TrimZero = true,
    //    };

    //    //// 替换编码处理器，自定义序列化行为
    //    //for (var i = 0; i < reader.Handlers.Count; i++)
    //    //{
    //    //    if (reader.Handlers[i] is BinaryGeneral)
    //    //        reader.Handlers[i] = new MyBinaryGeneral { Host = reader };
    //    //}

    //    return reader;
    //}

    ///// <summary>
    ///// 创建写入器
    ///// </summary>
    ///// <param name="stream"></param>
    ///// <returns></returns>
    //public static Binary CreateWriter(Stream stream)
    //{
    //    var writer = new Binary()
    //    {
    //        Stream = stream,
    //        Encoding = Encoding,
    //        IsLittleEndian = false,
    //        UseFieldSize = true,
    //        UseRef = false,
    //        TrimZero = true,
    //    };

    //    //// 替换编码处理器，自定义序列化行为
    //    //for (var i = 0; i < writer.Handlers.Count; i++)
    //    //{
    //    //    if (writer.Handlers[i] is BinaryGeneral)
    //    //        writer.Handlers[i] = new MyBinaryGeneral { Host = writer };
    //    //}

    //    return writer;
    //}

    /// <summary>
    /// 创建读取器
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Binary CreateReader(Stream stream, JTMessage message)
    {
        var reader = new Binary
        {
            Stream = stream,
            Encoding = Encoding,
            IsLittleEndian = false,
            UseFieldSize = true,
            UseRef = false,
            TrimZero = true,

            //Version = message == null ? null : (message.JTT2019 ? "2019" : "2011"),
        };
        if (message != null)
        {
            reader.Version = message.Version switch
            {
                JTVersion.JT2011 => "2011",
                JTVersion.JT2013 => "2013",
                JTVersion.JT2019 => "2019",
                _ => null,
            };
        }

        // 替换编码处理器，自定义序列化行为
        for (var i = 0; i < reader.Handlers.Count; i++)
        {
            if (reader.Handlers[i] is BinaryGeneral)
                reader.Handlers[i] = new MyBinaryGeneral { Host = reader };
        }

        return reader;
    }

    /// <summary>
    /// 创建写入器
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Binary CreateWriter(Stream stream, JTMessage message)
    {
        var writer = new Binary
        {
            Stream = stream,
            Encoding = Encoding,
            IsLittleEndian = false,
            UseFieldSize = true,
            UseRef = false,
            TrimZero = true,

            //Version = message == null ? null : (message.JTT2019 ? "2019" : "2011"),
        };
        if (message != null)
        {
            writer.Version = message.Version switch
            {
                JTVersion.JT2011 => "2011",
                JTVersion.JT2013 => "2013",
                JTVersion.JT2019 => "2019",
                _ => null,
            };
        }

        // 替换编码处理器，自定义序列化行为
        for (var i = 0; i < writer.Handlers.Count; i++)
        {
            if (writer.Handlers[i] is BinaryGeneral)
                writer.Handlers[i] = new MyBinaryGeneral { Host = writer };
        }

        return writer;
    }

    ///// <summary>
    ///// 快速读取
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    ///// <param name="data"></param>
    ///// <returns></returns>
    //public static T FastRead<T>(Byte[] data)
    //{
    //    var binary = new Binary
    //    {
    //        Stream = new MemoryStream(data),
    //        UseFieldSize = true,
    //        UseRef = false,
    //        //Log = XTrace.Log
    //    };
    //    return binary.Read<T>();
    //}

    //public static Int32 GetGBKLength(this String value) => Encoding.GetByteCount(value);

    /// <summary>写入GBK字符串</summary>
    /// <param name="binary"></param>
    /// <param name="value"></param>
    /// <exception cref="ArgumentOutOfRangeException">字符串的 GBK 编码超过 255 字节，长度前缀溢出</exception>
    public static void WriteString(this Binary binary, String value)
    {
        var buf = Encoding.GetBytes(value);

        // 长度前缀只有1字节，超长会被截断成错误长度
        if (buf.Length > 0xFF) throw new ArgumentOutOfRangeException(nameof(value), L.T("JT809 length-prefixed string cannot exceed 255 bytes."));

        binary.Write((Byte)buf.Length);
        binary.Write(buf, 0, buf.Length);
    }

    /// <summary>
    /// 读取GBK字符串
    /// </summary>
    /// <param name="binary"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    public static String ReadString(this Binary binary, Int32 len = -1)
    {
        if (len < 0) len = binary.ReadByte();
        if (len == 0) return null;

        var buf = binary.ReadBytes(len);

        // 剔除头尾非法字符
        Int32 s, e;
        for (s = 0; s < len && (buf[s] == 0x00 || buf[s] == 0xFF); s++) ;
        for (e = len - 1; e >= 0 && (buf[e] == 0x00 || buf[e] == 0xFF); e--) ;

        if (s >= len || e < 0) return null;

        return Encoding.GetString(buf, s, e - s + 1);
    }

    /// <summary>
    /// 按照GBK编码写入字符串
    /// </summary>
    /// <param name="binary"></param>
    /// <param name="value"></param>
    public static void WriteAllGBK(this Binary binary, String value)
    {
        var buf = Encoding.GetBytes(value);
        binary.Write(buf, 0, buf.Length);
    }

    /// <summary>
    /// 按照GBK编码读取剩余字节作为字符串
    /// </summary>
    /// <param name="binary"></param>
    /// <returns></returns>
    public static String ReadAllGBK(this Binary binary)
    {
        var len = binary.Stream.Length - binary.Stream.Position;
        var buf = binary.ReadBytes((Int32)len);
        return Encoding.GetString(buf)?.Trim('\0');
    }

    /// <summary>读取BCD时间</summary>
    /// <param name="binary"></param>
    /// <param name="len"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static DateTime ReadBCDTime(this Binary binary, Int32 len = 6, String format = "yyMMddHHmmss")
    {
        var str = binary.ReadBCD(len);
        if (DateTime.TryParseExact(str, format, null, DateTimeStyles.None, out var dt))
            return dt;

        return DateTime.MinValue;
    }

    /// <summary>写入BCD时间</summary>
    /// <param name="binary"></param>
    /// <param name="dt"></param>
    /// <param name="len"></param>
    /// <param name="format"></param>
    public static void WriteBCDTime(this Binary binary, DateTime dt, Int32 len = 6, String format = "yyMMddHHmmss")
    {
        var str = dt.ToString(format);
        binary.WriteBCD(str, len);
    }

    //public static UInt16 ReadUInt16(this Binary binary) => binary.Read<UInt16>();
    //public static Int16 ReadInt16(this Binary binary) => binary.Read<Int16>();
    //public static UInt32 ReadUInt32(this Binary binary) => binary.Read<UInt32>();
    //public static Int32 ReadInt32(this Binary binary) => binary.Read<Int32>();

    //public static void WriteByte(this Binary binary, Byte value) => binary.Write(value);
    //public static void WriteUInt16(this Binary binary, UInt16 value) => binary.Write(value);
    //public static void WriteInt16(this Binary binary, Int16 value) => binary.Write(value);
    //public static void WriteUInt32(this Binary binary, UInt32 value) => binary.Write(value);
    //public static void WriteInt32(this Binary binary, Int32 value) => binary.Write(value);

    //public static Byte FromBCD(this Byte b) => (Byte)((b >> 4) * 10 + (b & 0x0F));
    //public static Byte ToBCD(this Byte b) => (Byte)(((b / 10) << 4) | (b % 10));
    //public static Byte ToBCD(this Int32 b) => (Byte)(((b / 10) << 4) | (b % 10));

    //public static String ReadBCD(this Binary binary, Int32 len)
    //{
    //    var buf = binary.ReadBytes(len);
    //    var cs = new Char[len * 2];
    //    for (var i = 0; i < len; i++)
    //    {
    //        cs[i * 2] = (Char)('0' + (buf[i] >> 4));
    //        cs[i * 2 + 1] = (Char)('0' + (buf[i] & 0x0F));
    //    }

    //    return new String(cs).Trim('\0').TrimStart('0');
    //}

    //public static void WriteBCD(this Binary binary, String value, Int32 max)
    //{
    //    var len = max * 2;
    //    if (value.Length > len)
    //        value = value[..len];
    //    else if (value.Length < len)
    //        value = value.PadLeft(len, '0');

    //    var buf = new Byte[max];
    //    for (var i = 0; i < max; i++)
    //    {
    //        var a = value[i * 2] - '0';
    //        var b = value[i * 2 + 1] - '0';
    //        buf[i] = (Byte)((a << 4) | (b & 0x0F));
    //    }

    //    binary.Write(buf, 0, buf.Length);
    //}

    //public static DateTime ReadDateTime(this Binary binary)
    //{
    //    var rs = binary.ReadBytes(6);
    //    return new DateTime(rs[0].FromBCD() + 2000, rs[1].FromBCD(), rs[2].FromBCD(), rs[3].FromBCD(), rs[4].FromBCD(), rs[5].FromBCD());
    //}
    //public static void WriteDateTime(this Binary binary, DateTime value)
    //{
    //    var buf = new Byte[6];
    //    buf[0] = ((Byte)(value.Year - 2000)).ToBCD();
    //    buf[1] = ((Byte)value.Month).ToBCD();
    //    buf[2] = ((Byte)value.Day).ToBCD();
    //    buf[3] = ((Byte)value.Hour).ToBCD();
    //    buf[4] = ((Byte)value.Minute).ToBCD();
    //    buf[5] = ((Byte)value.Second).ToBCD();
    //    binary.Write(buf, 0, buf.Length);
    //}
    //public static DateTime ReadDate(this Binary binary)
    //{
    //    var rs = binary.ReadBytes(4);
    //    var year = rs[0].FromBCD() * 100 + rs[1].FromBCD();
    //    var month = rs[2].FromBCD();
    //    var day = rs[3].FromBCD();

    //    return new DateTime(year, month > 0 ? month : 1, day > 0 ? day : 1);
    //}
    //public static void WriteDate(this Binary binary, DateTime value)
    //{
    //    var buf = new Byte[4];
    //    buf[0] = ((Byte)(value.Year / 100)).ToBCD();
    //    buf[1] = ((Byte)(value.Year % 100)).ToBCD();
    //    buf[2] = ((Byte)value.Month).ToBCD();
    //    buf[3] = ((Byte)value.Day).ToBCD();
    //    binary.Write(buf, 0, buf.Length);
    //}

    //public static void WriteArray(this Binary binary, Byte[] value) => binary.Write(value, 0, value.Length);

    /// <summary>
    /// Json序列化，使用枚举字符串
    /// </summary>
    /// <param name="value"></param>
    /// <param name="indented"></param>
    /// <returns></returns>
    public static String ToJsonJT809(this Object value, Boolean indented = false)
    {
        var jw = new JsonWriter
        {
            //IgnoreNullValues = !nullValue,
            //CamelCase = camelCase,
            Indented = indented,
            //SmartIndented = indented,
            EnumString = true,
        };

        jw.Write(value);

        return jw.GetString();
    }
}