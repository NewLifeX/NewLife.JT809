using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 命令字ID
/// </summary>
public enum JT809CommandType : Byte
{
    记录仪标准版本 = 0x00,
    当前驾驶人信息 = 0x01,
    记录仪时间 = 0x02,
    记录仪累计行驶里程 = 0x03,
    记录仪脉冲系数 = 0x04,
    车辆信息 = 0x05,
    记录仪状态信号配置信息 = 0x06,
    记录仪唯一性编号 = 0x07,
    采集记录仪行驶记录 = 0x08,
    采集记录仪位置信息记录 = 0x09,
    采集记录仪事故疑点记录 = 0x10,
    采集记录仪超时驾驶记录 = 0x11,
    采集记录仪驾驶人身份记录 = 0x12,
    采集记录仪外部供电记录 = 0x13,
    采集记录仪参数修改记录 = 0x14,
    采集记录仪速度状态日志 = 0x15,
}

/// <summary>
/// 上报车辆行驶记录请求消息
/// <para>子业务类型标识:DOVJN_CTRL_MSG_TAKE_TRAVEL_REQ</para>
/// <para>描述:上级平台向下级平台下发上报车辆行驶记录请求消息</para>
/// <para>2011版数据体见JT/T 809-2011 表63，2019版数据体见JT/T 809-2019 表83，2019版新增命令字</para>
/// </summary>
[MessageKind(MessageKinds.上报车辆行驶记录请求消息)]
public class JT809_0x9500_0x9504 : IAccessor
{
    /// <summary>
    /// 开始时间，用 UTC 时间表示
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 结束时间，用 UTC 时间表示
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 命令字ID。2019版新增
    /// </summary>
    public JT809CommandType Command { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        StartTime = reader.Read<DateTime>();
        EndTime = reader.Read<DateTime>();

        // 2019版在时间之后追加命令字
        if (reader.Version == "2019" && stream.Position < stream.Length)
            Command = (JT809CommandType)reader.ReadByte();

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

        writer.Write(StartTime);
        writer.Write(EndTime);

        if (writer.Version == "2019") writer.Write((Byte)Command);

        return true;
    }
}