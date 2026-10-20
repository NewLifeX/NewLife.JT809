using NewLife.JT809.Protocols;

namespace NewLife.JT809.Messages;

/// <summary>
/// 错误代码
/// </summary>
public enum JT809_0x9008_ReasonCode : Byte
{
    /// <summary>网关重启</summary>
    网关重启 = 0x00,

    /// <summary>其他原因</summary>
    其它原因 = 0x01,

    /// <summary>其他原因。2019版取值</summary>
    其它原因2019 = 0xFF
}

/// <summary>
/// 上级平台主动关闭链路通知消息
/// <para>业务数据类型标识:DOWN_CLOSELINK_INFORM</para>
/// </summary>
[MessageKind(MessageKinds.上级平台主动关闭链路通知消息)]
public class JT809_0x9008
{
    /// <summary>
    /// 链路关闭原因
    /// </summary>
    public JT809_0x9008_ReasonCode ReasonCode { get; set; }
}