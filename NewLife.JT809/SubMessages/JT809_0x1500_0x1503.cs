using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 应答结果
/// </summary>
public enum JT809_0x1503_Result : Byte
{
    下发成功 = 0x00,
    下发失败 = 0x01,
}

/// <summary>
/// 下发车辆报文应答消息
/// <para>子业务类型标识:UP_CTRL_MSG_TEXT_INFO_ACK</para>
/// <para>描述:下级平台应答上级平台下发的报文是否成功到达指定车辆</para>
/// <para>2011版数据体见JT/T 809-2011 表56，2019版数据体见JT/T 809-2019 表76，2019版以源子业务类型与源报文序列号取代消息ID</para>
/// </summary>
[MessageKind(MessageKinds.下发车辆报文应答消息)]
public class JT809_0x1500_0x1503 : IAccessor
{
    /// <summary>
    /// 对应下发车辆报文请求消息源子业务类型标识。2019版新增
    /// </summary>
    public UInt16 SourceDataType { get; set; }
    /// <summary>
    ///  对应下发车辆报文请求消息源报文序列号。2019版新增
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }
    /// <summary>
    /// 消息ID
    /// 对应“下发车辆报文请求消息”中的MSG_ID。2011版使用
    /// </summary>
    public UInt32 MsgID { get; set; }
    /// <summary>
    /// 应答结果
    /// </summary>
    public JT809_0x1503_Result Result { get; set; }

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
            MsgID = reader.ReadUInt32();
        }

        Result = (JT809_0x1503_Result)reader.ReadByte();

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
            writer.Write(MsgID);
        }

        writer.Write((Byte)Result);

        return true;
    }

    //public JT809_0x1500_0x1503 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x1500_0x1503();
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.SourceDataType = reader.ReadUInt16();
    //        value.SourceMsgSn = reader.ReadUInt32();
    //    }
    //    else
    //    {
    //        value.MsgID = reader.ReadUInt32();
    //    }
    //    value.Result = (JT809_0x1503_Result)reader.ReadByte();
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1500_0x1503 value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteUInt16(value.SourceDataType);
    //        writer.WriteUInt32(value.SourceMsgSn);
    //    }
    //    else
    //    {
    //        writer.WriteUInt32(value.MsgID);
    //    }
    //    writer.WriteByte((Byte)value.Result);
    //}
}