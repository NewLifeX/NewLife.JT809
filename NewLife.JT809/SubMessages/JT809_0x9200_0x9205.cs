using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 错误代码
/// </summary>
public enum JT809_0x9205_ReasonCode : Byte
{
    车辆进入指定区域 = 0x00,
    人工指定交换 = 0x01,
    应急状态下车辆定位信息回传 = 0x02,
    其它原因 = 0x03
}

/// <summary>
/// 启动车辆定位信息交换请求消息
/// <para>子业务类型标识:DOWN_EXG_MSG_RETURN_STARTUP</para>
/// <para>描述:在有车辆进入非归属地区地理区域、人工指定车辆定位信息交换和应急状态监控车辆时，上级平台向下级平台发出启动车辆定位信息交换清求消息。下级平台收到此命令后需要回复启动车辆定位信息交换应答消息给上级平台，即UP_EXG_MSG-RETURN-STARTUP_ ACK</para>
/// </summary>
[MessageKind(MessageKinds.启动车辆定位信息交换请求消息)]
public class JT809_0x9200_0x9205
{
    /// <summary>
    /// 启动车辆定位信息交换请求原因
    /// </summary>
    public JT809_0x9205_ReasonCode ReasonCode { get; set; }
}
