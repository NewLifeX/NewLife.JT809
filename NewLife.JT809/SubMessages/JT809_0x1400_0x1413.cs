using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 督办级别
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
/// <para>数据体见JT/T 809-2019 表72</para>
/// </summary>
[MessageKind(MessageKinds.上报报警督办请求消息)]
public class JT809_0x1400_0x1413 : IAccessor
{
    /// <summary>发起报警平台唯一编码，由平台所在地行政区域代码和平台编号组成</summary>
    public String SourcePlatformId { get; set; }

    /// <summary>报警时间</summary>
    public DateTime WarnTime { get; set; }

    /// <summary>对应报警消息源子业务类型标识</summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>对应报警督办请求消息源报文序列号</summary>
    public UInt32 SourceMsgSn { get; set; }

    /// <summary>督办截止时间</summary>
    public DateTime SupervisionEndTime { get; set; }

    /// <summary>督办级别</summary>
    public JT809_0x1413_SupervisionLevel SupervisionLevel { get; set; }

    /// <summary>督办人</summary>
    public String Supervisor { get; set; }

    /// <summary>督办联系电话</summary>
    public String SupervisorTel { get; set; }

    /// <summary>督办联系电子邮件</summary>
    public String SupervisorEmail { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        SourcePlatformId = reader.ReadFixedString(11);
        WarnTime = reader.Read<DateTime>();
        SourceDataType = reader.ReadUInt16();
        SourceMsgSn = reader.ReadUInt32();
        SupervisionEndTime = reader.Read<DateTime>();
        SupervisionLevel = (JT809_0x1413_SupervisionLevel)reader.ReadByte();
        Supervisor = reader.ReadFixedString(16);
        SupervisorTel = reader.ReadFixedString(20);
        SupervisorEmail = reader.ReadFixedString(32);

        return true;
    }

    /// <summary>写入</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Write(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var writer = Helper.CreateWriter(stream, header);

        writer.WriteFixedString(SourcePlatformId, 11);
        writer.Write(WarnTime);
        writer.Write(SourceDataType);
        writer.Write(SourceMsgSn);
        writer.Write(SupervisionEndTime);
        writer.Write((Byte)SupervisionLevel);
        writer.WriteFixedString(Supervisor, 16);
        writer.WriteFixedString(SupervisorTel, 20);
        writer.WriteFixedString(SupervisorEmail, 32);

        return true;
    }
}