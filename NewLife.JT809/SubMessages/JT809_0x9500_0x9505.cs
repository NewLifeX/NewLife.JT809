using NewLife.JT809.Protocols;
using NewLife.JT809.Serialization;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 车辆应急接入监管平台请求消息
/// <para>子业务类型标识:UP_CTRL_MSG_EMERGENCY_MONITORING_REQ</para>
/// <para>描述:发生应急情况时，政府监管平台需要及时监控该车辆时，就向该车辆归属的下级平台发送该命令</para>
/// </summary>
[MessageKind(MessageKinds.车辆应急接入监管平台请求消息)]
public class JT809_0x9500_0x9505
{
    /// <summary>
    /// 监管平台下发的鉴权码
    /// </summary>
    [BCDString(20)]
    public String AuthenticationCode { get; set; }
    /// <summary>
    /// 拨号点名称
    /// </summary>
    [FieldSize(20)]
    public String AccessPointName { get; set; }
    /// <summary>
    /// 拨号用户名
    /// </summary>
    [FieldSize(49)]
    public String UserName { get; set; }
    /// <summary>
    /// 拨号密码
    /// </summary>
    [FieldSize(22)]
    public String Password { get; set; }
    /// <summary>
    /// 地址
    /// </summary>
    [FieldSize(32)]
    public String ServerIP { get; set; }
    /// <summary>
    /// 服务器TCP端口
    /// </summary>
    public UInt16 TcpPort { get; set; }
    /// <summary>
    /// 服务器UDP端口
    /// </summary>
    public UInt16 UdpPort { get; set; }
    /// <summary>
    /// 结束时间
    /// </summary>
    [BCDTime(6, "yyMMddHHmmss")]
    public DateTime EndTime { get; set; }
}