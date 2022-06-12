using NewLife.JT809.Protocols;

namespace NewLife.JT809.Messages;

/// <summary>
///从链路注销请求消息
///<para>链路类型:从链路</para>
///<para>消息方向:上级平台往下级平台</para>
///<para>业务数据类型标识:DOWN_DISCONNIrCT_REQ</para>
///<para>描述:从链路建立后，上级平台在取消该链路时，应向下级平台发送从链路注销请求消息</para>
/// </summary>
[MessageKind(MessageKinds.从链路注销请求消息)]
public class JT809_0x9003
{
    /// <summary>
    /// 校验码
    /// </summary>
    public UInt32 VerifyCode { get; set; }
}