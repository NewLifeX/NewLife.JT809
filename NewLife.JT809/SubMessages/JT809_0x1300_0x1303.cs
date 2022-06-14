using NewLife.JT809.Protocols;
using NewLife.JT809.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 上传平台间消息补传请求消息
/// <para>子业务类型标识:UP_PLATFORM_MSG_RETRAN_REQ</para>
/// <para>描述:下级平台在接收消息时，如发现消息报文序列号不连续，则立即发送消息补传请求。上级平台收到消息补传请求后，根据请求的消息报文序列号或起始时间（当仅填写一项时。另一项全为0），重传相应的消息</para>
/// </summary>
[MessageKind(MessageKinds.上传平台间消息补传请求消息)]
public class JT809_0x1300_0x1303
{
    /// <summary>
    /// 对应需要重传消息地子业务类型标识
    /// </summary>
    public UInt16 RetranDataType { get; set; }
    /// <summary>
    /// 重传消息总数
    /// </summary>
    public Byte SerialCount { get; set; }
    /// <summary>
    /// 需要重传消息的起始报文序列号
    /// <para>如只请求重传一个消息，则起始消息报文序列号和结束消息报文序列号相同 </para>
    /// </summary>
    public UInt32 BeginMessageNumber { get; set; }
    /// <summary>
    /// 需要重传消息的结束的报文序列号
    /// <para>如只请求重传一个消息，则起始消息报文序列号和结束消息报文序列号相同 </para>
    /// </summary>
    public UInt32 EndMessageNumber { get; set; }

    /// <summary>
    /// 重传起始系统utc时间
    /// 8位
    /// </summary>
    [BCDTime]
    public DateTime Time { get; set; }
}