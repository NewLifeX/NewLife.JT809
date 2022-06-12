using NewLife.JT809.Protocols;

namespace NewLife.JT809.Messages;

/// <summary>
/// 从链路静态信息交换消息
/// <para>链路类型:从链路</para>
/// <para>消息方向:上级平台往下级台</para>
/// <para>业务数据类型标识:DOWN_BASE_MSG</para>
/// <para>描述:上级平台向下级平台发送车辆静态信息交换业务</para>
/// </summary>
[MessageKind(MessageKinds.从链路静态信息交换消息)]
public class JT809_0x9600 : JT809ExchangeMessageBodies
{
}