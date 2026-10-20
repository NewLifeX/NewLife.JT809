using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 下发平台间报文应答消息
/// <para>子业务类型标识:UP_PLATFORM_MSG_INFO_ACK</para>
/// <para>描述:下级平台收到上级平台发送的"下发平台间报文请求"消息后，根据相应的下发报文对象类型，进行转发，并向上级平台发送应答消息</para>
/// <para>2011版数据体见JT/T 809-2011 表42，2019版数据体见JT/T 809-2019 表56，2019版改为源子业务类型与源报文序列号</para>
/// </summary>
[MessageKind(MessageKinds.下发平台间报文应答消息)]
public class JT809_0x1300_0x1302 : IAccessor
{
    /// <summary>
    /// 对应下发平台间报文请求消息源子业务类型标识。2019版使用
    /// </summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>
    ///  对应下发平台间报文请求消息源报文序列号。2019版使用
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }

    /// <summary>
    /// 信息ID。2011版使用
    /// </summary>
    public UInt32 InfoID { get; set; }

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
        else
        {
            InfoID = reader.ReadUInt32();
        }

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
        else
        {
            writer.Write(InfoID);
        }

        return true;
    }

    //public JT809_0x1300_0x1302 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x1300_0x1302();
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.SourceDataType = reader.ReadUInt16();
    //        value.SourceMsgSn = reader.ReadUInt32();
    //    }
    //    else
    //    {
    //        value.InfoID = reader.ReadUInt32();
    //    }
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1300_0x1302 value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteUInt16(value.SourceDataType);
    //        writer.WriteUInt32(value.SourceMsgSn);
    //    }
    //    else
    //    {
    //        writer.WriteUInt32(value.InfoID);
    //    }
    //}
}