using NewLife.JT809.Protocols;

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

public enum JT809_0x9502_ImageType
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
/// </summary>
[MessageKind(MessageKinds.车辆拍照应答)]
public class JT809_0x1500_0x1502
{
    /// <summary>
    /// 拍照应答标识
    /// </summary>
    public JT809_0x1502_PhotoRspFlag PhotoRspFlag { get; set; }

    /// <summary>
    /// 是否使用国测局批准的地图保密插件进行加密 1 已加密 0未加密
    /// </summary>
    public JT809_VehiclePositionEncrypt Encrypt { get; set; }
    /// <summary>
    /// 车辆定位信息数据长度
    /// </summary>
    public UInt32 DataLength { get; set; }
    /// <summary>
    /// 车辆定位信息内容，包括车辆位置基本信息和位置附加信息
    /// 其数据格式安装 808-2019中8.12要求
    /// </summary>
    public Byte[] GnssData { get; set; }
    /// <summary>
    /// 监控平台唯一编码，由平台所在地行政区域代码和平台编码组成  11位
    /// </summary>
    public String PlatformId1 { get; set; }
    /// <summary>
    /// 报警状态，二进制标识 0 正常 1表示报警
    /// 具体定义按照808-2019中表18的规定
    /// </summary>
    public UInt32 Alarm1 { get; set; }
    /// <summary>
    /// 市级监控平台唯一编码，由平台所在地行政区域代码和平台编码组成
    /// 未填写时，全填0，无市级平台应由省级平台全填1  11位
    /// </summary>
    public String PlatformId2 { get; set; }
    /// <summary>
    /// 报警状态，二进制标识 0 正常 1表示报警
    /// 具体定义按照808-2019中表18的规定
    /// </summary>
    public UInt32 Alarm2 { get; set; }
    /// <summary>
    /// 省级监控平台唯一编码，由平台所在地行政区域代码和平台编码组成
    ///  未填写时，全填0  11位
    /// </summary>
    public String PlatformId3 { get; set; }
    /// <summary>
    /// 报警状态，二进制标识 0 正常 1表示报警
    /// 具体定义按照808-2019中表18的规定
    /// </summary>
    public UInt32 Alarm3 { get; set; }

    ///// <summary>
    ///// 车辆定位信息 2011版本
    ///// </summary>
    //public JT809VehiclePositionProperties VehiclePosition { get; set; }
    ///// <summary>
    ///// 车辆定位信息 2019版本
    ///// </summary>
    //public JT809VehiclePositionProperties_2019 VehiclePosition_2019 { get; set; }

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