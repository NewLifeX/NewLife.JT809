using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.Messages;

/// <summary>
///  平台链路连接情况与车辆定位消息传输情况上报应答消息
/// <para>链路类型：主链路</para>
/// <para>消息方向:下级平台往上级平台</para>
/// <para>业务类型标识:UP_MANAGE_MSG_RSP</para>
/// <para>下级平台收到上级平台下发的“平台链路连接情况与车辆定位消息传输情况上报请求消息”后，上报指定平台，指定时间段内的平台链路连接情况与车辆定位消息传输情况</para>
/// </summary>
[MessageKind(MessageKinds.平台链路连接情况与车辆定位消息传输情况上报应答消息_2019)]
public class JT809_0x1102 : ISubMessage
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
    public String PlateformId { get; set; }
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
    /// <summary>
    /// START_TIME~END_TIME期间下级平台丢失得车辆定位消息总数
    /// </summary>
    public UInt32 LoseDymamicSum { get; set; }
    /// <summary>
    /// START_TIME~END_TIME期间下级监控平台链路断开次数
    /// </summary>
    public Byte DisconnectNum { get; set; }
    /// <summary>
    /// START_TIME~END_TIME期间下级监控平台链路断开总时长，用秒表示
    /// </summary>
    public UInt32 DisconnectTime { get; set; }

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
    //    PlateformId = reader.ReadFixedString(11);
    //    StartTime = reader.Read<Int64>().ToDateTime();
    //    EndTime = reader.Read<Int64>().ToDateTime();
    //    LoseDymamicSum = reader.Read<UInt32>();
    //    DisconnectNum = reader.ReadByte();
    //    DisconnectTime = reader.ReadUInt32();

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
    //    var ms = new MemoryStream();
    //    var writer = Helper.CreateWriter(ms);

    //    writer.WriteFixedString(PlateformId, 11);
    //    writer.Write(StartTime.ToLong());
    //    writer.Write(EndTime.ToLong());
    //    writer.Write(LoseDymamicSum);
    //    writer.Write(DisconnectNum);
    //    writer.Write(DisconnectTime);

    //    Payload = ms.ToArray();

    //    return base.Write(stream, context);
    //}
}