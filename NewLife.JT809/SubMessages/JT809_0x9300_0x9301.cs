using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 查岗对象的类型
/// </summary>
public enum JT809_0x9301_ObjectType : Byte
{
    当前连接的下级平台 = 0x01,
    下级平台所属单一业户 = 0x02,
    下级平台所属所有业户 = 0x03
}

/// <summary>
/// 平台查岗请求
/// <para>子业务类型标识:DOWN_PLATFORM-MSG_POST_QUERY_REQ</para>
/// <para>描述:上级平台不定期向下级平台发送平台查岗信息</para>
/// <para>2011版数据体见JT/T 809-2011 表44，2019版数据体见JT/T 809-2019 表59</para>
/// </summary>
[MessageKind(MessageKinds.平台查岗请求)]
public class JT809_0x9300_0x9301 : IAccessor
{
    /// <summary>
    /// 查岗对象的类型。2019版新增
    /// </summary>
    public JT809_0x9301_ObjectType ObjectType { get; set; }

    /// <summary>
    /// 查岗对象的ID。2019版新增
    /// </summary>
    [FieldSize(20)]
    public String ObjectID { get; set; }

    /// <summary>
    /// 查岗应答时限。2019版新增
    /// </summary>
    public Byte AnswerTime { get; set; }

    /// <summary>
    /// 信息ID
    /// </summary>
    public UInt32 InfoID { get; set; }

    /// <summary>
    /// 数据长度
    /// </summary>
    public UInt32 InfoLength { get; set; }

    /// <summary>
    /// 应答内容
    /// </summary>
    [FieldSize(nameof(InfoLength))]
    public String InfoContent { get; set; }

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
            ObjectType = (JT809_0x9301_ObjectType)reader.ReadByte();
            ObjectID = reader.ReadFixedString(20);
            AnswerTime = reader.ReadByte();
        }

        InfoID = reader.ReadUInt32();

        var len = reader.ReadInt32();
        if (len < 0 || len > stream.Length - stream.Position) return false;

        InfoLength = (UInt32)len;
        if (len > 0) InfoContent = Helper.Encoding.GetString(reader.ReadBytes(len));

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
            writer.Write((Byte)ObjectType);
            writer.WriteFixedString(ObjectID, 20);
            writer.Write(AnswerTime);
        }

        writer.Write(InfoID);

        var buf = InfoContent.IsNullOrEmpty() ? Array.Empty<Byte>() : Helper.Encoding.GetBytes(InfoContent);
        InfoLength = (UInt32)buf.Length;

        writer.Write(buf.Length);
        writer.Write(buf, 0, buf.Length);

        return true;
    }

    //public JT809_0x9300_0x9301 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x9300_0x9301();
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.ObjectType = (JT809_0x9301_ObjectType)reader.ReadByte();
    //        value.ObjectID = reader.ReadString(20);
    //        value.AnswerTime = reader.ReadByte();
    //    }
    //    value.InfoID = reader.ReadUInt32();
    //    value.InfoLength = reader.ReadUInt32();
    //    value.InfoContent = reader.ReadString((Int32)value.InfoLength);
    //    return value;
    //}


    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x9300_0x9301 value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteByte((Byte)value.ObjectType);
    //        writer.WriteStringPadRight(value.ObjectID, 20);
    //        writer.WriteByte(value.AnswerTime);
    //    }
    //    writer.WriteUInt32(value.InfoID);
    //    // 先计算内容长度（汉字为两个字节）
    //    writer.Skip(4, out Int32 lengthPosition);
    //    writer.WriteString(value.InfoContent);
    //    writer.WriteInt32Return(writer.GetCurrentPosition() - lengthPosition - 4, lengthPosition);
    //}
}