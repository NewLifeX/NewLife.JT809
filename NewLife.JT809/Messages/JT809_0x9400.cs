using NewLife.JT809.Protocols;

namespace NewLife.JT809.Messages
{
    /// <summary>
    /// 从链路报警信息交互消息
    /// <para>链路类型:从链路</para>
    /// <para>消息方向:上级平台往下级平台</para>
    /// <para>业务数据类型标识:DOWN_WARN_MSG</para>
    /// <para>描述：上级平台向下级平台发送报瞥信息业务</para>
    /// </summary>
    [MessageKind(MessageKinds.从链路报警信息交互消息)]
    public class JT809_0x9400 : JT809ExchangeMessageBodies
    {
    }
}