using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

public enum JT809_0x9503_MsgPriority
{
    紧急 = 0x00,
    一般 = 0x01
}

/// <summary>
/// 下发车辆报文请求
/// <para>子业务类型标识:DOWN_CTRL_MSG_TEXT_INFO</para>
/// <para>描述:用于上级平台向下级平台下发报文到某指定车辆</para>
/// </summary>
[MessageKind(MessageKinds.下发车辆报文请求)]
public class JT809_0x9500_0x9503
{
    /// <summary>
    /// 消息ID序号
    /// </summary>
    public UInt32 MsgSequence { get; set; }
    /// <summary>
    /// 报文优先级
    /// </summary>
    public JT809_0x9503_MsgPriority MsgPriority { get; set; }
    /// <summary>
    /// 报文信息长度
    /// </summary>
    public UInt32 MsgLength { get; set; }
    /// <summary>
    /// 报文信息内容
    /// </summary>
    [FieldSize(nameof(MsgLength))]
    public String MsgContent { get; set; }
}