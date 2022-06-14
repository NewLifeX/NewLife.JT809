using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 报警处理结果
/// </summary>
public enum JT809_0x1401_Result : Byte
{
    处理中 = 0x00,
    已处理完毕 = 0x01,
    不做处理 = 0x02,
    将来处理 = 0x03
}

/// <summary>
/// 报警督办应答消息
/// <para>子业务类型标识:UP_WARN_MSG_URGE_TODO_ACK</para>
/// <para>描述:下级平台应答上级平台下发的报警督办请求消息，向上.级平台上报车辆的报瞥处理结果</para>
/// </summary>
[MessageKind(MessageKinds.报警督办应答消息)]
public class JT809_0x1400_0x1401
{
    /// <summary>
    /// 报警督办 ID
    /// </summary>
    public UInt32 SupervisionID { get; set; }
    /// <summary>
    /// 报警处理结果
    /// </summary>
    public JT809_0x1401_Result Result { get; set; }
}