using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 上报车辆电子运单应答消息
/// <para>子业务类型标识:UP_EXG_MSG_TAKE_EWAYBILL_ACK</para>
/// <para>描述:下级平台应答上级平台发送的上报车辆电子运单请求消息，向上级平台上传车辆当前电子运单</para>
/// <para>2011版数据体见JT/T 809-2011 表28，2019版数据体见JT/T 809-2019 表35，2019版新增源子业务类型与源报文序列号</para>
/// </summary>
[MessageKind(MessageKinds.上报车辆电子运单应答消息)]
public class JT809_0x1200_0x120B : IAccessor
{
    /// <summary>
    /// 对应上报车辆电子运单源子业务类型标识。2019版新增
    /// </summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>
    ///  对应上报车辆电子运单源报文序列号。2019版新增
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }

    /// <summary>
    /// 电子运单数据体长度
    /// </summary>
    public UInt32 EwaybillLength { get; set; }

    /// <summary>
    /// 电子运单数据内容
    /// </summary>
    [FieldSize(nameof(EwaybillLength))]
    public String EwaybillInfo { get; set; }

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

        var len = reader.ReadInt32();
        if (len < 0 || len > stream.Length - stream.Position) return false;

        EwaybillLength = (UInt32)len;
        if (len > 0) EwaybillInfo = Helper.Encoding.GetString(reader.ReadBytes(len));

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

        var buf = EwaybillInfo.IsNullOrEmpty() ? Array.Empty<Byte>() : Helper.Encoding.GetBytes(EwaybillInfo);
        EwaybillLength = (UInt32)buf.Length;

        writer.Write(buf.Length);
        writer.Write(buf, 0, buf.Length);

        return true;
    }

    //public JT809_0x1200_0x120B Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x1200_0x120B();
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.SourceDataType = reader.ReadUInt16();
    //        value.SourceMsgSn = reader.ReadUInt32();
    //    }
    //    value.EwaybillLength = reader.ReadUInt32();
    //    value.EwaybillInfo = reader.ReadString((Int32)value.EwaybillLength);
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1200_0x120B value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteUInt16(value.SourceDataType);
    //        writer.WriteUInt32(value.SourceMsgSn);
    //    }
    //    writer.WriteUInt32((UInt32)value.EwaybillInfo.Length);
    //    writer.WriteString(value.EwaybillInfo);
    //}
}
