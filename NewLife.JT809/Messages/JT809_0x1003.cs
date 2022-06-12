using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.Messages;

/// <summary>
/// 主链路注销请求消息
/// <para>链路类型:主链路</para>
/// <para>消息方向：下级平台往上级平台</para>
/// <para>业务数据类型标识：UP-DISCONNECT-REQ</para>
/// <para>描述：下级平台在中断与上级平台的主链路连接时，应向上级平台发送主链路注销请求消息。</para>
/// </summary>
[MessageKind(MessageKinds.主链路注销请求消息)]
public class JT809_0x1003
{
    /// <summary>
    /// 用户名
    /// </summary>
    public UInt32 UserId { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [FieldSize(8)]
    public String Password { get; set; }
}