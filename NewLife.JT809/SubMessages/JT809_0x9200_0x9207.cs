using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 验证结果
/// </summary>
public enum JT809_0x9207_Result : Byte
{
    申请成功 = 0x00,
    上级平台没有该车数据 = 0x01,
    申请时间段错误 = 0x02,
    其它 = 0x03,
    /// <summary>其他。2019版取值</summary>
    其它2019 = 0xFF
}

/// <summary>
/// 申请交换指定车辆定位信息应答
/// <para>子业务类型标识:DOWN_EXG_MSG_APPLY_FOR_MONITOR_STARTUP_ACK</para>
/// <para>描述：应答下级平台申请交换指定车辆定位信息,请求消息."即 UP_EXG_MSG_APPLY_FOR_MONITOR_STARTUP</para>
/// <para>2011版数据体见JT/T 809-2011 表35，2019版数据体见JT/T 809-2019 表47，2019版新增源子业务类型与源报文序列号</para>
/// </summary>
[MessageKind(MessageKinds.申请交换指定车辆定位信息应答)]
public class JT809_0x9200_0x9207 : IAccessor
{
    /// <summary>
    /// 对应申请交换指定车辆定位信息请求消息源子业务类型标识。2019版新增
    /// </summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>
    ///  对应申请交换指定车辆定位信息请求消息源报文序列号。2019版新增
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }

    /// <summary>
    /// 返回结果
    /// </summary>
    public JT809_0x9207_Result Result { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        if (reader.Version == "2019")
        {
            SourceDataType = reader.ReadUInt16();
            SourceMsgSn = reader.ReadUInt32();
        }

        Result = (JT809_0x9207_Result)reader.ReadByte();

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

        if (writer.Version == "2019")
        {
            writer.Write(SourceDataType);
            writer.Write(SourceMsgSn);
        }

        writer.Write((Byte)Result);

        return true;
    }

    //public JT809_0x9200_0x9207 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x9200_0x9207();
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.SourceDataType = reader.ReadUInt16();
    //        value.SourceMsgSn = reader.ReadUInt32();
    //    }
    //    value.Result = (JT809_0x9207_Result)reader.ReadByte();
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x9200_0x9207 value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteUInt16(value.SourceDataType);
    //        writer.WriteUInt32(value.SourceMsgSn);
    //    }
    //    writer.WriteByte((byte)value.Result);
    //}
}