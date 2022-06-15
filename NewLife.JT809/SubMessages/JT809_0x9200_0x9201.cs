using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 处理结果
/// </summary>
public enum JT809_0x9201_Result : Byte
{
    完成注册_加注释 = 0x00,
    审核通过_完成注册 = 0x01,
    信息错误_未完成注册 = 0x02,
    审核没通过_未完成注册 = 0x03
}

/// <summary>
///  车辆注册信息应答消息
///  <para>子业务类型标识:DOWN_EXG_MSG_REGISTER_ACK</para>
///  <para>描述:上级平台在收到下级平台上报的车辆注册信息后，向下级平台发送车辆注册应答消息</para>
/// </summary>
[MessageKind(MessageKinds.车辆注册信息应答消息)]
public class JT809_0x9200_0x9201 : ISubMessage
{
    /// <summary>
    /// 车辆注册醒醒消息源子业务类型标识
    /// </summary>
    public UInt16 DataType { get; set; }

    /// <summary>
    /// 车辆注册信息消息源报文序号
    /// </summary>
    public UInt32 MsgSn { get; set; }

    /// <summary>
    /// 处理结果
    /// </summary>
    public JT809_0x9201_Result Result { get; set; }
}