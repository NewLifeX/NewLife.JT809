using NewLife.JT809.Protocols;

namespace NewLife.JT809.Messages;

/// <summary>
/// 验证结果
/// </summary>
public enum JT809_0x9002_Result : byte
{
    /// <summary>成功</summary>
    成功 = 0x00,

    /// <summary>VERIFY_CODE错误</summary>
    VERIFY_CODE错误 = 0x01,

    /// <summary>资源紧张_稍后再连接_已经占用</summary>
    资源紧张_稍后再连接_已经占用 = 0x02,

    /// <summary>其他</summary>
    其他 = 0x03,

    /// <summary>其他。2019版取值</summary>
    其他_2019 = 0xFF
}

/// <summary>
/// 从链路连接应答信息
/// <para>链路类型:从链路</para>
/// <para>消息方问:下级平台往上级平台</para>
/// <para>业务数据类型标识:DOWN_CONNNECT_RSP</para>
/// <para>描述：下级平台作为服务器端向上级平台客户端返回从链路连接应答消息，上级平台在接收到该应答消息结果后</para>
/// </summary>
[MessageKind(MessageKinds.从链路连接应答信息)]
public class JT809_0x9002
{
    /// <summary>
    /// 验证结果
    /// </summary>
    public JT809_0x9002_Result Result { get; set; }
}