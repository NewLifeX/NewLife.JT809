using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 补报车辆行驶路线信息应答消息
/// <para>子业务类型标识:UP_BASE_MSG_VEHICLE_ADDED_ACK</para>
/// <para>描述:下级平台向上级平台补报车辆行驶路线信息</para>
/// </summary>
[MessageKind(MessageKinds.补报车辆行驶路线信息应答消息)]
public class JT809_0x1600_0x1602
{
    /// <summary>
    /// 路线信息，808-2019中0x8606规定的报文格式
    /// </summary>
    public Byte[] DRVLine { get; set; }
}