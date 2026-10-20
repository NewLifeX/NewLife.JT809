using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 报警信息来源
/// </summary>
public enum JT809WarnSrc : Byte
{
    车载终端 = 0x01,
    企业监控平台 = 0x02,
    政府监管平台 = 0x03,
    其他 = 0x09
}

/// <summary>
/// 督办级别
/// </summary>
public enum JT809_9401_SupervisionLevel : Byte
{
    紧急 = 0x00,
    一般 = 0x01
}

/// <summary>
/// 报警督办请求消息
/// <para>子业务类型标识:DOWN_WARN_MSG_URGE_TODO_REQ</para>
/// <para>描述:上级平台向车辆归属下级平台下发本消息，催促其及时处理相关车辆的报警信息</para>
/// <para>2011版数据体见JT/T 809-2011 表50，2019版数据体见JT/T 809-2019 表68</para>
/// </summary>
[MessageKind(MessageKinds.报警督办请求消息)]
public class JT809_0x9400_0x9401 : IAccessor
{
    /// <summary>报警信息来源。2011版使用</summary>
    public JT809WarnSrc WarnSrc { get; set; }

    /// <summary>报警类型。2011版使用</summary>
    public UInt16 WarnType { get; set; }

    /// <summary>发起报警平台唯一编码。2019版使用</summary>
    public String SourcePlatformId { get; set; }

    /// <summary>报警时间。2019版使用</summary>
    public DateTime WarnTime { get; set; }

    /// <summary>报警消息源子业务类型标识。2019版使用</summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>报警信息消息或车辆定位信息报文序号。2019版使用</summary>
    public UInt32 MsgSn { get; set; }

    /// <summary>报警督办ID。2011版使用</summary>
    public UInt32 SupervisionID { get; set; }

    /// <summary>督办截止时间</summary>
    public DateTime SupervisionEndTime { get; set; }

    /// <summary>督办级别</summary>
    public JT809_9401_SupervisionLevel SupervisionLevel { get; set; }

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

        var j9 = reader.Version == "2019";
        if (j9)
        {
            SourcePlatformId = reader.ReadFixedString(11);
            WarnTime = reader.Read<DateTime>();
            SourceDataType = reader.ReadUInt16();
            MsgSn = reader.ReadUInt32();
        }
        else
        {
            WarnSrc = (JT809WarnSrc)reader.ReadByte();
            WarnType = reader.ReadUInt16();
            WarnTime = reader.Read<DateTime>();
            SupervisionID = reader.ReadUInt32();
        }

        SupervisionEndTime = reader.Read<DateTime>();
        SupervisionLevel = (JT809_9401_SupervisionLevel)reader.ReadByte();
        // 督办人与联系电话长度在2019版收紧
        Supervisor = reader.ReadFixedString(j9 ? 16 : 20);
        SupervisorTel = reader.ReadFixedString(j9 ? 20 : 32);
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

        var j9 = writer.Version == "2019";
        if (j9)
        {
            writer.WriteFixedString(SourcePlatformId, 11);
            writer.Write(WarnTime);
            writer.Write(SourceDataType);
            writer.Write(MsgSn);
        }
        else
        {
            writer.Write((Byte)WarnSrc);
            writer.Write(WarnType);
            writer.Write(WarnTime);
            writer.Write(SupervisionID);
        }

        writer.Write(SupervisionEndTime);
        writer.Write((Byte)SupervisionLevel);
        writer.WriteFixedString(Supervisor, j9 ? 16 : 20);
        writer.WriteFixedString(SupervisorTel, j9 ? 20 : 32);
        writer.WriteFixedString(SupervisorEmail, 32);

        return true;
    }
}