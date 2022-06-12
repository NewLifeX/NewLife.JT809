using NewLife.JT809.Protocols;

namespace NewLife.JT809.Messages;

/// <summary>
/// 验证结果
/// </summary>
public enum JT809_0x1002_Result : Byte
{
    /// <summary>成功</summary>
    成功 = 0x00,

    /// <summary>IP地址不正确</summary>
    IP地址不正确 = 0x01,

    /// <summary>接入码不正确</summary>
    接入码不正确 = 0x02,

    /// <summary>用户没有注册</summary>
    用户没有注册 = 0x03,

    /// <summary>密码错误</summary>
    密码错误 = 0x04,

    /// <summary>资源紧张_稍后再连接_已经占用</summary>
    资源紧张_稍后再连接_已经占用 = 0x05,

    /// <summary>其他</summary>
    其他 = 0x06,

    /// <summary>其他_2019</summary>
    其他_2019 = 0xFF
}

/// <summary>
/// 主链路登录应答消息
/// <para>链路类型:主链路</para>
/// <para>消息方向:上级平台往下级平台</para>
/// <para>业务数据类型标识:UP_CONNCCT_RSP</para>
/// <para>描述:上级平台对下级平台登录请求信息、进行安全验证后，返回相应的验证结果。</para>
/// </summary>
[MessageKind(MessageKinds.主链路登录应答消息)]
public class JT809_0x1002
{
    /// <summary>
    /// 验证结果，定义如下：
    /// 0x00:成功;
    /// 0x01:IP 地址不正确;
    /// 0x02:接入码不正确;
    /// 0x03:用户没用注册;
    /// 0x04:密码错误;
    /// 0x05:资源紧张，稍后再连接(已经占用;
    /// 0x06:其他。
    /// </summary>
    public JT809_0x1002_Result Result { get; set; }

    /// <summary>
    /// 校验码
    /// </summary>
    public UInt32 VerifyCode { get; set; }
}