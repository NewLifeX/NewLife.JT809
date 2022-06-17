using NewLife.JT809.Protocols;
using NewLife.JT809.Serialization;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

public enum JT809WarnSrc : Byte
{
    车载终端 = 0x01,
    企业监控平台 = 0x02,
    政府监管平台 = 0x03,
    其他 = 0x09
}

public enum JT809_9401_SupervisionLevel
{
    紧急 = 0x00,
    一般 = 0x01
}

/// <summary>
/// 报警督办请求消息
/// <para>子业务类型标识:DOWN_WARN_MSG_URGE_TODO_REQ</para>
/// <para>描述:上级平台向车辆归属下级平台下发本消息，催促其及时处理相关车辆的报警信息</para>
/// </summary>
[MessageKind(MessageKinds.报警督办请求消息)]
public class JT809_0x9400_0x9401
{
    /// <summary>
    /// 报警信息来源
    /// </summary>
    public JT809WarnSrc WarnSrc { get; set; }

    /// <summary>
    /// 报警类型
    /// </summary>
    public UInt16 WarnType { get; set; }

    /// <summary>
    /// 报警时间UTCDateTime
    /// </summary>
    public DateTime WarnTime { get; set; }

    /// <summary>
    /// 报警督办ID HexString
    /// </summary>
    public UInt32 SupervisionID { get; set; }

    /// <summary>
    /// 督办截止时间
    /// </summary>
    public DateTime SupervisionEndTime { get; set; }

    /// <summary>
    /// 督办级别
    /// </summary>
    public JT809_9401_SupervisionLevel SupervisionLevel { get; set; }

    /// <summary>
    /// 督办人
    /// </summary>
    [FieldSize(16)]
    public String Supervisor { get; set; }

    /// <summary>
    /// 督办联系电话
    /// </summary>
    [FieldSize(20)]
    public String SupervisorTel { get; set; }

    /// <summary>
    /// 督办联系电子邮件
    /// </summary>
    [FieldSize(32)]
    public String SupervisorEmail { get; set; }
}