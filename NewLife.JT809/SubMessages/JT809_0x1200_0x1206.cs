using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 结束车辆定位信息交换应答消息
/// <para>子业务类型标识:UP_EXG_MSG_RETURN_END_ACK</para>
/// <para>本条消息时下级平台对上级平台服务器下发的DOWN_EXG_MSG_RETURN_END消息的应答消息</para>
/// </summary>
[MessageKind(MessageKinds.结束车辆定位信息交换应答消息)]
public class JT809_0x1200_0x1206
{
    /// <summary>
    /// 对应结束车辆定位信息交换请求消息源子业务类型标识
    /// </summary>
    public UInt16 SourceDataType { get; set; }
    /// <summary>
    /// 对应结束车辆定位信息交换请求消息源报文序列号
    /// </summary>
    public UInt32 SourceMsgSN { get; set; }
    /// <summary>
    /// 后续数据长度  值为0x00
    /// </summary>
    public UInt32 DataLength { get; set; } = 0x00;
}