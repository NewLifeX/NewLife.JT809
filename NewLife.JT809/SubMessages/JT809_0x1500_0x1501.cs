using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 应答结果
/// </summary>
public enum JT809_0x1501_Result : Byte
{
    监听成功 = 0x00,
    监听失败 = 0x01
}

/// <summary>
/// 车辆单向监听应答
/// <para>子业务类型标识:UP_CTRL_MSG_MONITOR_VEHTCLE_ACK</para>
/// <para>描述:下级平台向上级平台上传车辆单向监听请求消息的应答</para>
/// </summary>
[MessageKind(MessageKinds.车辆单向监听应答)]
public class JT809_0x1500_0x1501
{
    /// <summary>
    /// 应答结果
    /// </summary>
    public JT809_0x1501_Result Result { get; set; }
}