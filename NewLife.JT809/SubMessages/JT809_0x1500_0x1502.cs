using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 拍照应答标识
/// </summary>
public enum JT809_0x1502_PhotoRspFlag : Byte
{
    不支持拍照 = 0x00,
    完成拍照 = 0x01,
    完成拍照_照片数据稍后传送 = 0x02,
    未拍照_不在线 = 0x03,
    未拍照_无法使用指定镜头 = 0x04,
    未拍照_其他原因 = 0x05,
    车牌号码错误 = 0x09,
}

public enum JT809_0x9502_ImageType : Byte
{
    jpg = 0x01,
    gif = 0x02,
    tiff = 0x03,
    png = 0x04,
}

/// <summary>
/// 车辆拍照应答
/// <para>子业务类型标识:UP_ CTRL_ MSG _TAKE_ PHOTO_ ACK</para>
/// <para>描述:下级平台应答上级平台发送的车辆拍照请求消息，上传图片信息到上级平台</para>
/// <para>2011版数据体见JT/T 809-2011 表55，2019版数据体见JT/T 809-2019 表75，两版的拍照位置数据体见JT/T 809-2011 表69 与 JT/T 809-2019 表90</para>
/// </summary>
[MessageKind(MessageKinds.车辆拍照应答)]
public class JT809_0x1500_0x1502 : IAccessor
{
    /// <summary>
    /// 对应车辆拍照请求消息源子业务类型标识。2019版新增
    /// </summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>
    /// 对应车辆拍照请求消息源报文序列号。2019版新增
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }

    /// <summary>
    /// 拍照应答标识
    /// </summary>
    public JT809_0x1502_PhotoRspFlag PhotoRspFlag { get; set; }

    /// <summary>
    /// 拍照位置地点，结构见 <see cref="JTGnssLocationBody"/>
    /// </summary>
    public JTGnssLocationBody Gnss { get; set; } = new JTGnssLocationBody();

    /// <summary>
    /// 镜头ID
    /// </summary>
    public Byte LensID { get; set; }

    /// <summary>
    /// 图片长度
    /// </summary>
    public UInt32 PhotoLen { get; set; }

    /// <summary>
    /// 图片大小
    /// </summary>
    public JT809_0x9502_SizeType SizeType { get; set; }

    /// <summary>
    /// 图像格式
    /// </summary>
    public JT809_0x9502_ImageType Type { get; set; }

    /// <summary>
    /// 图片内容
    /// </summary>
    public Byte[] Photo { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        if (reader.Version == "2019")
        {
            SourceDataType = reader.ReadUInt16();
            SourceMsgSn = reader.ReadUInt32();
        }

        PhotoRspFlag = (JT809_0x1502_PhotoRspFlag)reader.ReadByte();

        var gnss = new JTGnssLocationBody();
        if (!gnss.Read(stream, context)) return false;
        Gnss = gnss;

        LensID = reader.ReadByte();

        var len = reader.ReadInt32();
        if (len < 0 || len > stream.Length - stream.Position) return false;

        PhotoLen = (UInt32)len;

        SizeType = (JT809_0x9502_SizeType)reader.ReadByte();
        Type = (JT809_0x9502_ImageType)reader.ReadByte();
        if (len > 0) Photo = reader.ReadBytes(len);

        return true;
    }

    /// <summary>写入</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Write(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var writer = Helper.CreateWriter(stream, header);

        if (writer.Version == "2019")
        {
            writer.Write(SourceDataType);
            writer.Write(SourceMsgSn);
        }

        writer.Write((Byte)PhotoRspFlag);

        var gnss = Gnss ?? new JTGnssLocationBody();
        if (!gnss.Write(stream, context)) return false;

        writer.Write(LensID);

        var buf = Photo ?? Array.Empty<Byte>();
        PhotoLen = (UInt32)buf.Length;

        writer.Write(buf.Length);
        writer.Write((Byte)SizeType);
        writer.Write((Byte)Type);
        writer.Write(buf, 0, buf.Length);

        return true;
    }

    //public JT809_0x1500_0x1502 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x1500_0x1502
    //    {
    //        PhotoRspFlag = (JT809_0x1502_PhotoRspFlag)reader.ReadByte()
    //    };
    //    if (config.Version == JT809Version.JTT2011)
    //    {
    //        value.VehiclePosition = config.GetMessagePackFormatter<JT809VehiclePositionProperties>().Deserialize(ref reader, config);
    //    }
    //    else if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.VehiclePosition_2019 = config.GetMessagePackFormatter<JT809VehiclePositionProperties_2019>().Deserialize(ref reader, config);
    //    }
    //    value.LensID = reader.ReadByte();
    //    value.PhotoLen = reader.ReadUInt32();
    //    value.SizeType = (JT809_0x9502_SizeType)reader.ReadByte();
    //    value.Type = (JT809_0x9502_ImageType)reader.ReadByte();
    //    if (value.PhotoLen > 0)
    //    {
    //        value.Photo = reader.ReadArray((Int32)value.PhotoLen).ToArray();
    //    }
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1500_0x1502 value, IJT809Config config)
    //{
    //    writer.WriteByte((Byte)value.PhotoRspFlag);
    //    if (config.Version == JT809Version.JTT2011)
    //    {
    //        if (value.VehiclePosition != null)
    //        {
    //            config.GetMessagePackFormatter<JT809VehiclePositionProperties>().Serialize(ref writer, value.VehiclePosition, config);
    //        }
    //    }
    //    else if (config.Version == JT809Version.JTT2019)
    //    {
    //        if (value.VehiclePosition_2019 != null)
    //        {
    //            config.GetMessagePackFormatter<JT809VehiclePositionProperties_2019>().Serialize(ref writer, value.VehiclePosition_2019, config);
    //        }
    //    }
    //    writer.WriteByte(value.LensID);
    //    var isPhoto = (value.Photo != null && value.Photo.Length > 0);
    //    writer.WriteUInt32(isPhoto ? (UInt32)value.Photo.Length : 0);
    //    writer.WriteByte(value.SizeType.ToByteValue());
    //    writer.WriteByte(value.Type.ToByteValue());
    //    if (isPhoto)
    //    {
    //        writer.WriteArray(value.Photo);
    //    }
    //}
}