using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 报警处理结果
/// </summary>
public enum JT809_0x1413_SupervisionLevel : Byte
{
    紧急 = 0x00,
    一般 = 0x01,
}

/// <summary>
/// 上报报警督办请求消息
/// <para>子业务类型标识:UP_WARN_MSG_URGE_TODO_REQ_INFO</para>
/// <para>描述:下级平台告知上级平台已对报警情况进行催办</para>
/// <para>本条消息上级平台无需应答</para>
/// </summary>
[MessageKind(MessageKinds.上报报警督办请求消息)]
public class JT809_0x1400_0x1413
{
    /// <summary>
    /// 发起报警平台唯一编码，由平台所在地行政区划大码和平台编号组成
    /// </summary>
    [FieldSize(11)]
    public String SourcePlatformId { get; set; }
    /// <summary>
    /// 报警时间 utc
    /// </summary>
    public DateTime WarnTime { get; set; }
    /// <summary>
    /// 对应报警督办请求消息源子业务类型标识
    /// </summary>
    public UInt16 SourceDataType { get; set; }
    /// <summary>
    /// 对应报警督办请求消息源报文序列号
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }
    /// <summary>
    /// 督办截止时间 utc
    /// </summary>
    public DateTime SupervisionEndTime { get; set; }
    /// <summary>
    /// 督办级别
    /// </summary>
    public JT809_0x1413_SupervisionLevel SupervisionLevel { get; set; }
    /// <summary>
    /// 督办人
    /// </summary>
    [FieldSize(16)]
    public String Supervisor { get; set; }
    /// <summary>
    /// 督办人联系电话
    /// </summary>
    [FieldSize(20)]
    public String SupervisorTel { get; set; }
    /// <summary>
    /// 督办人联系电子邮件
    /// </summary>
    [FieldSize(32)]
    public String SupervisorEmail { get; set; }

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1400_0x1413 value, IJT809Config config)
    //{
    //    writer.WriteBigNumber(value.SourcePlatformId, 11);
    //    writer.WriteUTCDateTime(value.WarnTime);
    //    writer.WriteUInt16(value.SourceDataType);
    //    writer.WriteUInt32(value.SourceMsgSn);
    //    writer.WriteUTCDateTime(value.SupervisionEndTime);
    //    writer.WriteByte((Byte)value.SupervisionLevel);
    //    writer.WriteStringPadRight(value.Supervisor, 16);
    //    writer.WriteStringPadRight(value.SupervisorTel, 20);
    //    writer.WriteStringPadRight(value.SupervisorEmail, 32);
    //}
}