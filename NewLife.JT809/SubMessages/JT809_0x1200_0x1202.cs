using System.Runtime.Serialization;
using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 定位信息是否使用国家测绘局批准的地图保密插件进行加密。
/// </summary>
public enum JT809_VehiclePositionEncrypt : Byte
{
    未加密 = 0x00,
    已加密 = 0x01
}

/// <summary>
/// 实时上传车辆定位信息消息
/// <para>子业务类型标识:UP_EXG_MSG_REAL_LOCATION</para>
/// <para>车辆的实时定位信息</para>
/// <para>无需应答</para>
/// </summary>
[MessageKind(MessageKinds.实时上传车辆定位信息)]
public class JT809_0x1200_0x1202
{
    #region 属性
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
    [FieldSize(nameof(DataLength))]
    public Byte[] GnssData { get; set; }

    /// <summary>
    /// 监控平台唯一编码，由平台所在地行政区域代码和平台编码组成  11位
    /// </summary>
    [FieldSize(11)]
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
    [FieldSize(11)]
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
    [FieldSize(11)]
    public String PlatformId3 { get; set; }

    /// <summary>
    /// 报警状态，二进制标识 0 正常 1表示报警
    /// 具体定义按照808-2019中表18的规定
    /// </summary>
    public UInt32 Alarm3 { get; set; }

    /// <summary>卫星定位数据</summary>
    [IgnoreDataMember]
    public LocationMessage Location { get; set; }

    #endregion

    #region 方法
    /// <summary>解码卫星定位数据</summary>
    /// <returns></returns>
    public Boolean Decode()
    {
        var buf = GnssData;
        if (buf == null || buf.Length == 0) return false;

        var ms = new MemoryStream(buf);
        var location = new LocationMessage();
        if (!location.Read(ms, null)) return false;

        Location = location;

        return true;
    }

    /// <summary>编码卫星定位数据</summary>
    /// <returns></returns>
    public Boolean Encode()
    {
        if (Location == null) return false;

        var ms = new MemoryStream();
        if (!Location.Write(ms, null)) return false;

        var buf = ms.ToArray();
        DataLength = (UInt32)buf.Length;
        GnssData = buf;

        return true;
    }
    #endregion

    ///// <summary>
    ///// 车辆定位信息 2011版本
    ///// </summary>
    //public JT809VehiclePositionProperties VehiclePosition { get; set; }

    ///// <summary>
    ///// 车辆定位信息 2019版本
    ///// </summary>
    //public JT809VehiclePositionProperties_2019 VehiclePosition_2019 { get; set; }

    //public JT809_0x1200_0x1202 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    JT809_0x1200_0x1202 value = new JT809_0x1200_0x1202();
    //    if (config.Version == JT809Version.JTT2011)
    //    {
    //        value.VehiclePosition = config.GetMessagePackFormatter<JT809VehiclePositionProperties>().Deserialize(ref reader, config);
    //    }
    //    else if(config.Version == JT809Version.JTT2019)
    //    {
    //        value.VehiclePosition_2019 = config.GetMessagePackFormatter<JT809VehiclePositionProperties_2019>().Deserialize(ref reader, config);
    //    }
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1200_0x1202 value, IJT809Config config)
    //{
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
    //}
}