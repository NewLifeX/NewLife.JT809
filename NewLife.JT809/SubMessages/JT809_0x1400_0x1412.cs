using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 报警处理结果
/// </summary>
public enum JT809_0x1412_Result : byte
{
    处理中 = 0x00,
    已处理完毕 = 0x01,
    不做处理 = 0x02,
    将来处理 = 0x03
}

/// <summary>
/// 主动上报报警处理结果消息
/// <para>子业务类型标识:UP_WARN_MSG_ADPT_TODO_INFO</para>
/// <para>描述:下级平台主动向上级平台上报报警处理结果</para>
/// <para>本条消息上级平台无需应答</para>
/// </summary>
[MessageKind(MessageKinds.主动上报报警处理结果消息)]
public class JT809_0x1400_0x1412
{
    /// <summary>
    /// 对应报警督办请求消息源子业务类型标识
    /// </summary>
    public UInt16 SourceDataType { get; set; }
    /// <summary>
    /// 对应报警督办请求消息源报文序列号
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }
    /// <summary>
    /// 报警处理结果
    /// </summary>
    public JT809_0x1412_Result Result { get; set; }
}
