using NewLife.JT809.Protocols;

namespace NewLife.JT809.Messages;

/// <summary>
/// 错误代码
/// </summary>
public enum JT809_0x1007_ErrorCode : Byte
{
    /// <summary>主链路断开</summary>
    主链路断开 = 0x00,

    /// <summary>其他原因</summary>
    其他原因 = 0x01
}

/// <summary>
/// 主链路断开通知消息
/// <para>链路类型:从链路</para>
/// <para>消息方向:下级平台往上级平台</para>
/// <para>业务数据类型标识:UP_DISCONNECT_INFORM</para>
/// <para>描述:'当主链路中断后，下级平台可通过从链路向上级平台发送本消息通知上级平台主链路中断</para>
/// <para>主链路连接保持应答消息,数据体为空</para>
/// <para>本条消息无需被通知方应答</para>
/// </summary>
[MessageKind(MessageKinds.主链路断开通知消息)]
public class JT809_0x1007
{
    /// <summary>
    /// 错误代码
    /// </summary>
    public JT809_0x1007_ErrorCode ErrorCode { get; set; }
}