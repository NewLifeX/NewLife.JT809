using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 车辆应急接入监管平台请求消息
/// <para>子业务类型标识:UP_CTRL_MSG_EMERGENCY_MONITORING_REQ</para>
/// <para>描述:发生应急情况时，政府监管平台需要及时监控该车辆时，就向该车辆归属的下级平台发送该命令</para>
/// <para>2011版数据体见JT/T 809-2011 表64，2019版数据体见JT/T 809-2019 表84，两版结构一致</para>
/// </summary>
[MessageKind(MessageKinds.车辆应急接入监管平台请求消息)]
public class JT809_0x9500_0x9505 : IAccessor
{
    /// <summary>
    /// 监管平台下发的鉴权码，20位BCD码
    /// </summary>
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
    /// 结束时间，用 UTC 时间表示，0 表示一直连接指定的服务器
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        AuthenticationCode = reader.ReadBCD(10);
        AccessPointName = reader.ReadFixedString(20);
        UserName = reader.ReadFixedString(49);
        Password = reader.ReadFixedString(22);
        ServerIP = reader.ReadFixedString(32);
        TcpPort = reader.ReadUInt16();
        UdpPort = reader.ReadUInt16();
        EndTime = reader.Read<DateTime>();

        return true;
    }

    /// <summary>写入</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Write(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var writer = Helper.CreateWriter(stream, header);

        writer.WriteBCD(AuthenticationCode, 10);
        writer.WriteFixedString(AccessPointName, 20);
        writer.WriteFixedString(UserName, 49);
        writer.WriteFixedString(Password, 22);
        writer.WriteFixedString(ServerIP, 32);
        writer.Write(TcpPort);
        writer.Write(UdpPort);
        writer.Write(EndTime);

        return true;
    }
}