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
/// </remarks>
[MessageKind(MessageKinds.平台链路连接情况与车辆定位消息传输情况上报请求消息_2019)]
public class JT809_0x9102 //: IAccessor
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

    ///// <summary>
    ///// 读取
    ///// </summary>
    ///// <param name="stream"></param>
    ///// <param name="context"></param>
    ///// <returns></returns>
    //public override Boolean Read(Stream stream, Object context)
    //{
    //    if (!base.Read(stream, context)) return false;

    //    var pk = Payload;
    //    if (pk == null) return false;

    //    var reader = Helper.CreateReader(pk.GetStream());
    //    PlatformId = reader.ReadFixedString(11);
    //    StartTime = reader.Read<Int64>().ToDateTime();
    //    EndTime = reader.Read<Int64>().ToDateTime();

    //    return true;
    //}

    ///// <summary>
    ///// 写入
    ///// </summary>
    ///// <param name="stream"></param>
    ///// <param name="context"></param>
    ///// <returns></returns>
    //public override Boolean Write(Stream stream, Object context)
    //{
    //    //if (!base.Write(stream, context)) return false;

    //    var ms = new MemoryStream();
    //    var writer = Helper.CreateWriter(ms);

    //    writer.WriteFixedString(PlatformId, 11);
    //    writer.Write(StartTime.ToLong());
    //    writer.Write(EndTime.ToLong());

    //    Payload = ms.ToArray();

    //    //return true;
    //    return base.Write(stream, context);
    //}
}
