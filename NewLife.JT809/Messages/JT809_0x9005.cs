using NewLife.JT809.Protocols;

namespace NewLife.JT809.Messages;

/// <summary>
/// 从链路连接保持请求消息
/// <para>链路类型:从链路</para>
/// <para>消息方向:上级平台往下级平台</para>
/// <para>业务数据类型标识:DOWN_LINKTEST_REQ</para>
/// <para>描述:从链路建立成功后，上级平台向下级平台发送从链路连接保持请求消息，以保持从链路的连接状态</para>
/// <para>从链路连接保持请求消息，数据体为空</para>
/// </summary>
[MessageKind(MessageKinds.从链路连接保持请求消息)]
public class JT809_0x9005
{
}