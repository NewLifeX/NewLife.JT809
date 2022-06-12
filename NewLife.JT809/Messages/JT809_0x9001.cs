using NewLife.JT809.Protocols;

namespace NewLife.JT809.Messages;

/// <summary>
/// 从链路连接请求消息
/// <para>链路类型:从链路</para>
/// <para>消息方向:上级平台往下级平台</para>
/// <para>业务数据类型标识:DOWN_CONNECT_REQ</para>
/// <para>描述:主链路建立连接后，上级平台向下级平台发送从链路连接清求消息，以建立从链路连接</para>
/// <para>下级平台在收到本息后，根据本校验码 VERIFY CODE 来实现数据的校验，校验后，则返回DOWN CONNECT RSP 消息</para>
/// </summary>
[MessageKind(MessageKinds.从链路连接请求消息)]
public class JT809_0x9001
{
    /// <summary>
    /// 4.5.1.2 对应的校验码
    /// </summary>
    public UInt32 VerifyCode { get; set; }
}
