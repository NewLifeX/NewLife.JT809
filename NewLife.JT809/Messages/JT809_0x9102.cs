using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.Messages;

/// <summary>
///  平台链路连接情况与车辆定位消息传输情况上报请求消息
/// </summary>
/// <remarks>
/// 链路类型：从链路
/// 消息方向：上级平台往下级平台
/// 业务类型标识：DOWN_MANAGE_MSG_REQ
/// 上级平台向下级平台发送上报指定下级监控平台一段时间内链路连接情况与车辆定位消息传输情况请求
/// 数据体见 JT/T 809-2019 表22：子业务类型(2) + 数据长度(4) + 平台编码(11) + 开始时间(8) + 结束时间(8)
/// </remarks>
[MessageKind(MessageKinds.平台链路连接情况与车辆定位消息传输情况上报请求消息_2019)]
public class JT809_0x9102 : IAccessor
{
    /// <summary>
    /// 子业务类型标识
    /// </summary>
    public UInt16 DataType { get; set; }

    /// <summary>
    /// 数据长度
    /// </summary>
    public UInt32 DataLength { get; set; }

    /// <summary>
    /// 平台唯一编码
    /// </summary>
    [FieldSize(11)]
    public String PlatformId { get; set; }

    /// <summary>
    /// 开始时间，用 UTC 时间表示
    /// 注：采用 UTC 时间表示，如 2010-1-10 9:7:54 的 UTC 值为 1263085674，其在协议中表示为0x000000004B49286A.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间，用 UTC 时间表示
    /// 注：采用 UTC 时间表示，如 2010-1-10 9:7:54 的 UTC 值为 1263085674，其在协议中表示为0x000000004B49286A.
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

        DataType = reader.ReadUInt16();
        DataLength = reader.ReadUInt32();
        PlatformId = reader.ReadFixedString(11);
        StartTime = reader.Read<DateTime>();
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

        writer.Write(DataType);

        // 数据长度为平台编码与两个时间字段共27字节
        DataLength = 11 + 8 + 8;

        writer.Write(DataLength);
        writer.WriteFixedString(PlatformId, 11);
        writer.Write(StartTime);
        writer.Write(EndTime);

        return true;
    }
}