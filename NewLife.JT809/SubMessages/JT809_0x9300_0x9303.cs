using NewLife.JT809.Protocols;
using NewLife.JT809.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 下发平台间消息补传请求消息
/// <para>子业务类型标识:DOWN_PLATFORM_MSG_RETRAN_REQ</para>
/// <para>描述:上级平台接收方在接受消息时，如发现消息报文序列号不连接，则立即发送消息补传请求。下级平台收到消息补传请求后，根据请求的消息报文序列号。重传相应的消息</para>
/// </summary>
[MessageKind(MessageKinds.下发平台间消息补传请求消息)]
public class JT809_0x9300_0x9303
{
    /// <summary>
    /// 需要重传消息的起始报文序列号和结束的报文序列号。如只请求重传一个消息，则起始消息报文序列号和结束消息报文序列号相同 
    /// 8位
    /// </summary>
    public Byte[] SerialList { get; set; }
    /// <summary>
    /// 重传起始系统utc时间
    /// 8位
    /// </summary>
    [BCDTime]
    public DateTime Time { get; set; }
}