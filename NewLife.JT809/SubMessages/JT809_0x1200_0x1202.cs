using NewLife.JT809.Protocols;

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
/// <para>2011版数据体见JT/T 809-2011 表69，2019版数据体见JT/T 809-2019 表90</para>
/// </summary>
/// <remarks>数据体结构与从链路 0x9202 完全一致，实现见基类 <see cref="JTGnssLocationBody"/></remarks>
[MessageKind(MessageKinds.实时上传车辆定位信息)]
public class JT809_0x1200_0x1202 : JTGnssLocationBody
{
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