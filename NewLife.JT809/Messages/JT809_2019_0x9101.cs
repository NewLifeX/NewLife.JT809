using NewLife.JT809.Protocols;

namespace NewLife.JT809.Messages;

/// <summary>
///  发送车辆定位信息数据量通知消息
/// <para>链路类型：主链路</para>
/// <para>消息方向:下级平台往上级平台</para>
/// <para>业务类型标识: DOWN_TOTAL_RECV_BACK_MSG</para>
/// <para>描述:下级平台向上级平台定量通知已经上传的车辆定位信息数量(如:每收到10,000 条车辆定位信息通知一次)</para>
/// <para>本条消息不需下级平台应答。</para>
/// </summary>
[MessageKind(MessageKinds.发送车辆定位信息数据量通知消息_2019)]
public class JT809_2019_0x9101
{
    /// <summary>
    /// START_TIME_END_TIME共收到的车辆定位信息数量
    /// </summary>
    public UInt32 DynamicInfoTotal { get; set; }

    /// <summary>
    /// 开始时间，用 UTC 时间表示
    /// 注：采用 UTC 时间表示，如 2010-1-10 9:7:54 的 UTC 值为 1263085674，其在协议中表示为0x000000004B49286A.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间，用 UTC 时间表示
    /// 注：采用 UTC 时间表示，如 2010-1-10 9:7:54 的 UTC 值为 1263085674，其在协议中表示为0x000000004B49286A.
    /// </summary>
    public DateTime EndTime { get; set; }
}