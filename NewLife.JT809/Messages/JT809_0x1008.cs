using NewLife.JT809.Protocols;

namespace NewLife.JT809.Messages;

/// <summary>
/// 错误代码
/// </summary>
public enum JT809_0x1008_ReasonCode : Byte
{
    网关重启 = 0x00,
    其他原因 = 0x01
}

/// <summary>
/// 下级平台主动关闭主从链路通知消息
/// <para>链路类型:从链路</para>
/// <para>消息方向:下级平台往上级平台</para>
/// <para>业务数据类型标识:UP_CLOSELINIC INFORM</para>
/// <para>描述:下级平台作为服务端，发现从链路出现异常时，下级平台通过从链路向上级平台发送本消息，通知上级平台下级平台即将关闭主从链路</para>
/// </summary>
[MessageKind(MessageKinds.下级平台主动关闭主从链路通知消息)]
public class JT809_0x1008
{
    /// <summary>
    /// 链路关闭原因
    /// </summary>
    public JT809_0x1008_ReasonCode ReasonCode { get; set; }
}