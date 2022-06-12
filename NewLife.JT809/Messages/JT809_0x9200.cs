using NewLife.JT809.Protocols;

namespace NewLife.JT809.Messages;

/// <summary>
/// 从链路车辆动态信息交换业务
/// <para>链路类型:从链路</para>
/// <para>消息方向:上级平台台往下级平台</para>
/// <para>业务数据类型标识:DOWN_EXG_MSG</para>
/// <para>描述:上级平台作为客户端向下级平台服务端发送车辆动态信息交换业务</para>
/// </summary>
[MessageKind(MessageKinds.从链路车辆动态信息交换业务)]
public class JT809_0x9200 : JT809ExchangeMessageBodies
{
}