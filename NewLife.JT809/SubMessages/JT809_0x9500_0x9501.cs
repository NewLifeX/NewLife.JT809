using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 车辆单向监听请求消息
/// <para>子业务类型标识:DOWN_CTRL_MSG_MONITOR_VEHICLE_REQ</para>
/// <para>描述:上级平台向下级平台下发车辆单向监听清求消息</para>
/// </summary>
[MessageKind(MessageKinds.车辆单向监听请求消息)]
public class JT809_0x9500_0x9501
{
    /// <summary>
    /// 回拨电话号码
    /// </summary>
    [FieldSize(20)]
    public String MonitorTel { get; set; }
}