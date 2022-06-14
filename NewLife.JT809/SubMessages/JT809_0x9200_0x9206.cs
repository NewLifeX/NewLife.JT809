using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 错误代码
/// </summary>
public enum JT809_0x9206_ReasonCode : Byte
{
    车辆离开指定区域 = 0x00,
    人工停止交换 = 0x01,
    紧急监控完成 = 0x02,
    车辆离线 = 0x03,
    其它原因 = 0x04
}

/// <summary>
/// 结束车辆定位信息交换请求
/// <para>子业务类型标识:DOWN_EXG_MSG_RETURN_END</para>
/// <para>描述:在进入非归属地区地理区域的车辆离开该地理区域、人上取消指定车辆定位信息交换和应急状态结束时，上级平台向下级平台发出结束车辆定位信息交换请求消息。下级平台收到该命令后应回复结束车辆定位信息交换应答消息，即 UP_EXG_MSG_RE_TURN_END_ACK</para>
/// </summary>
[MessageKind(MessageKinds.结束车辆定位信息交换请求)]
public class JT809_0x9200_0x9206
{
    /// <summary>
    /// 结束车辆定位信息交换请求原因
    /// </summary>
    public JT809_0x9206_ReasonCode ReasonCode { get; set; }
}
