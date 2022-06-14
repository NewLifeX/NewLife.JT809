using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

#warning   8.3.3.2.5 交换车辆行驶路线信息消息  无指定消息子类型，暂未对接
/// <summary>
/// 交换车辆定位信息消息
/// <para>子业务类型标识:DOWN_EXG_MSG_CAR_LOCATION</para>
/// <para>2011 描述:上级平台通过该消息不间断地向车辆驶入区域所属的下级平台发送车辆定位信息，直到该车驶离该区域</para>
/// <para>2019 描述：上级平台在以下四种情况下通过该消息不间断地向车辆进入区域所属的下级平台发送车辆定位信息，直到该车辆离开该区域
/// 1.车辆跨域时，上级平台通过该消息不间断地向车辆进入区域所属的下级平台发送车辆定位信息，直到该车辆离开该区域
/// 2.人工指定车辆定位信息交换时，上级平台通过该消息不间断地向指定交换对象下级平台发送车辆定位信息，直到人工指定“交换车辆定位信息”结束
/// 3.下级平台向上级平台“申请交换指定车辆定位信息”成功后，上级平台通过该消息不间断地向交换对象下级平台发送车辆定位信息，直到“申请交换指定车辆定位信息”结束
/// 4.应急状态监控车辆时，上级平台向车辆归属下级平台通过该消息不间断地发送车辆定位信息，实现车辆定位信息回传
/// </para>
/// </summary>
[MessageKind(MessageKinds.交换车辆定位信息消息)]
public class JT809_0x9200_0x9202
{
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
}
