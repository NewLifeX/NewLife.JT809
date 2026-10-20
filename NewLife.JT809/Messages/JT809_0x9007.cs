using NewLife.JT809.Protocols;

namespace NewLife.JT809.Messages;

/// <summary>
/// 错误代码
/// </summary>
public enum JT809_0x9007_ReasonCode : Byte
{
    /// <summary>无法连接下级平台指定的服务IP与端口</summary>
    无法连接下级平台指定的服务IP与端口 = 0x00,

    /// <summary>上级平台客户端与下级平台服务端断开</summary>
    上级平台客户端与下级平台服务端断开 = 0x01,

    /// <summary>其他原因</summary>
    其他原因 = 0x02,

    /// <summary>其他原因。2019版取值</summary>
    其他原因2019 = 0xFF
}

/// <summary>
/// 从链路断开通知消息
/// <para>链路类型:主链路</para>
/// <para>消息方向:上级平台往下级平台</para>
/// <para>业务数据类型标识:DOWN_DISCONNECT_INFORM</para>
/// <para>描述:
/// 情景 1:上级平台与下级平台的从链路中断后，重连二次仍未成功时，上级平台通过主链路发送本消息给下级平台。
/// 情景 2:上级平台作为客户端向下级平台登录时，根据之前收到的 IP 地址及端口无法连接到下级平台服务端时发送本消息通知下级平台。
/// </para>
/// <para>本条消息无需被通知方应答</para>
/// </summary>
[MessageKind(MessageKinds.从链路断开通知消息)]
public class JT809_0x9007
{
    ///// <summary>
    ///// 错误代码 2019版本
    ///// </summary>
    //public JT809_0x1007_ErrorCode ErrorCode { get; set; }
    /// <summary>
    /// 错误代码
    /// </summary>
    public JT809_0x9007_ReasonCode ReasonCode { get; set; }
}