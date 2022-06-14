using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 主动上报车辆行驶路线信息
/// <para>子业务类型标识:UP_BASE_MSG_DRVLINE_INFO</para>
/// </summary>
[MessageKind(MessageKinds.主动上报车辆行驶路线信息)]
public class JT809_0x1200_0x120E
{
    /// <summary>
    /// 路线信息 按照808-2019中8606规定的报文格式
    /// </summary>
    public Byte[] DRVLine { get; set; }
}