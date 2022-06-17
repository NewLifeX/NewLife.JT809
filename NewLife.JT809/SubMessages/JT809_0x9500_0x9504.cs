using NewLife.JT809.Protocols;
using NewLife.JT809.Serialization;
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
/// </summary>
[MessageKind(MessageKinds.上报车辆行驶记录请求消息)]
public class JT809_0x9500_0x9504 : IAccessor
{
    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 命令字ID
    /// </summary>
    public JT809CommandType Command { get; set; }

    /// <summary>
    /// 最大数据数
    /// </summary>
    public UInt16 Max { get; set; }

    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var jt2019 = context is JTMessage msg && msg.JTT2019;
        var reader = Helper.CreateReader(stream);

        if (jt2019)
        {
            StartTime = reader.ReadBCDTime();
            EndTime = reader.ReadBCDTime();
            Command = (JT809CommandType)reader.ReadByte();
        }
        else
        {
            Command = (JT809CommandType)reader.ReadByte();
            switch (Command)
            {
                case JT809CommandType.记录仪标准版本:
                case JT809CommandType.当前驾驶人信息:
                case JT809CommandType.记录仪时间:
                case JT809CommandType.记录仪累计行驶里程:
                case JT809CommandType.记录仪脉冲系数:
                case JT809CommandType.车辆信息:
                case JT809CommandType.记录仪状态信号配置信息:
                case JT809CommandType.记录仪唯一性编号:
                    break;
                case JT809CommandType.采集记录仪行驶记录:
                case JT809CommandType.采集记录仪位置信息记录:
                case JT809CommandType.采集记录仪事故疑点记录:
                case JT809CommandType.采集记录仪超时驾驶记录:
                case JT809CommandType.采集记录仪驾驶人身份记录:
                case JT809CommandType.采集记录仪外部供电记录:
                case JT809CommandType.采集记录仪参数修改记录:
                case JT809CommandType.采集记录仪速度状态日志:
                    StartTime = reader.ReadBCDTime();
                    EndTime = reader.ReadBCDTime();
                    Max = reader.ReadUInt16();
                    break;
            }
        }

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
        var jt2019 = context is JTMessage msg && msg.JTT2019;
        var binary = Helper.CreateWriter(stream);

        if (jt2019)
        {
            binary.WriteBCDTime(StartTime);
            binary.WriteBCDTime(EndTime);
            binary.Write(Command);
        }
        else
        {
            binary.Write(Command);
            switch (Command)
            {
                case JT809CommandType.记录仪标准版本:
                case JT809CommandType.当前驾驶人信息:
                case JT809CommandType.记录仪时间:
                case JT809CommandType.记录仪累计行驶里程:
                case JT809CommandType.记录仪脉冲系数:
                case JT809CommandType.车辆信息:
                case JT809CommandType.记录仪状态信号配置信息:
                case JT809CommandType.记录仪唯一性编号:
                    break;
                case JT809CommandType.采集记录仪行驶记录:
                case JT809CommandType.采集记录仪位置信息记录:
                case JT809CommandType.采集记录仪事故疑点记录:
                case JT809CommandType.采集记录仪超时驾驶记录:
                case JT809CommandType.采集记录仪驾驶人身份记录:
                case JT809CommandType.采集记录仪外部供电记录:
                case JT809CommandType.采集记录仪参数修改记录:
                case JT809CommandType.采集记录仪速度状态日志:
                    binary.WriteBCDTime(StartTime);
                    binary.WriteBCDTime(EndTime);
                    binary.WriteUInt16(Max);
                    break;
            }
        }

        return true;
    }
}
