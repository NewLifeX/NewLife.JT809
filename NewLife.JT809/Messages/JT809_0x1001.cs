using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.Messages;

/// <summary>
/// 主链路登录请求消息
/// <para>链路类型:主链路</para>
/// <para>消息方向:下级平台往上级平台</para>
/// <para>业务数据类型标识: UP-CONNECT-REQ</para>
/// <para>描述:下级平台向上级平台发送用户名和密码等登录信息</para>
/// </summary>
[MessageKind(MessageKinds.主链路登录请求消息)]
public class JT809_0x1001 : IAccessor
{
    /// <summary>
    /// 用户名
    /// </summary>
    public UInt32 UserId { get; set; }

    /// <summary>
    /// 密码
    /// 8位
    /// </summary>
    [FieldSize(8)]
    public String Password { get; set; }

    /// <summary>
    /// 下级平台接入码
    /// </summary>
    public UInt32 MsgGNSSCENTERID { get; set; }

    /// <summary>
    /// 下级平台提供对应的从链路服务端 IP 地址
    /// 32位
    /// </summary>
    [FieldSize(32)]
    public String DownLinkIP { get; set; }

    /// <summary>
    /// 下级平台提供对应的从链路服务器端口号
    /// </summary>
    public UInt16 DownLinkPort { get; set; }

    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        UserId = reader.ReadUInt32();
        Password = reader.ReadFixedString(8);

        if (header.JTT2019) MsgGNSSCENTERID = reader.ReadUInt32();

        DownLinkIP = reader.ReadFixedString(32);
        DownLinkPort = reader.ReadUInt16();

        return true;
    }

    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Write(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var writer = Helper.CreateWriter(stream, header);

        writer.Write(UserId);
        writer.WriteFixedString(Password, 8);

        if (header.JTT2019) writer.Write(MsgGNSSCENTERID);

        writer.WriteFixedString(DownLinkIP, 32);
        writer.Write(DownLinkPort);

        return true;
    }
}