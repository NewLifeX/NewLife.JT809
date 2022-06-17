using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 查岗对象的类型
/// </summary>
public enum JT809_0x1301_ObjectType : Byte
{
    当前连接的下级平台 = 0x01,
    下级平台所属单一业户 = 0x02,
    下级平台所属所有业户 = 0x03
}

/// <summary>
/// 平台查岗应答消息
/// <para>子业务类型标识:UP_PLATFORM_MSG_POST_QUERY_ACK</para>
/// <para>2011 描述:下级平台应答上级平台发送的不定期平台查岗消息</para>
/// <para>2019 描述:下级平台根据查岗对象地类型将上级平台发送地不定期平台查岗消息发送到不同地查岗对象，并将不同地查岗对象地应答分别转发给上级平台</para>
/// </summary>
[MessageKind(MessageKinds.平台查岗应答消息)]
public class JT809_0x1300_0x1301
{
    /// <summary>
    /// 查岗对象的类型
    /// </summary>
    public JT809_0x1301_ObjectType ObjectType { get; set; }

    /// <summary>
    /// 查岗应答人姓名
    /// </summary>
    [FieldSize(16)]
    public String Responder { get; set; }

    /// <summary>
    /// 查岗应答人联系电话
    /// </summary>
    [FieldSize(20)]
    public String ResponderTel { get; set; }

    /// <summary>
    /// 查岗对象的ID
    /// 2013:12位
    /// 2019:20位
    /// </summary>
    [FieldSize(20)]
    public String ObjectID { get; set; }

    /// <summary>
    /// 对应平台查岗请求消息源子业务类型标识
    /// </summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>
    /// 对应平台查岗请求消息源报文序列号
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }

    ///// <summary>
    ///// 信息ID
    ///// </summary>
    //public UInt32 InfoID { get; set; }

    /// <summary>
    /// 数据长度
    /// </summary>
    public UInt32 InfoLength { get; set; }

    /// <summary>
    /// 应答内容
    /// </summary>
    [FieldSize(nameof(InfoLength))]
    public String InfoContent { get; set; }

    //public JT809_0x1300_0x1301 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x1300_0x1301
    //    {
    //        ObjectType = (JT809_0x1301_ObjectType)reader.ReadByte()
    //    };
    //    if (config.Version == JT809Version.JTT2011)
    //    {
    //        value.ObjectID = reader.ReadString(12);
    //        value.InfoID = reader.ReadUInt32();
    //    }
    //    else
    //    {
    //        value.Responder = reader.ReadString(16);
    //        value.ResponderTel = reader.ReadString(20);
    //        value.ObjectID = reader.ReadString(20);
    //        value.SourceDataType = reader.ReadUInt16();
    //        value.SourceMsgSn = reader.ReadUInt32();
    //    }
    //    value.InfoLength = reader.ReadUInt32();
    //    value.InfoContent = reader.ReadString((Int32)value.InfoLength);
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1300_0x1301 value, IJT809Config config)
    //{
    //    writer.WriteByte((Byte)value.ObjectType);
    //    if (config.Version == JT809Version.JTT2011)
    //    {
    //        writer.WriteStringPadRight(value.ObjectID, 12);
    //        writer.WriteUInt32(value.InfoID);
    //    }
    //    else
    //    {
    //        writer.WriteStringPadRight(value.Responder, 16);
    //        writer.WriteStringPadRight(value.ResponderTel, 20);
    //        writer.WriteStringPadRight(value.ObjectID, 20);
    //        writer.WriteUInt16(value.SourceDataType);
    //        writer.WriteUInt32(value.SourceMsgSn);
    //    }
    //    // 先计算内容长度（汉字为两个字节）
    //    writer.Skip(4, out Int32 lengthPosition);
    //    writer.WriteString(value.InfoContent);
    //    writer.WriteInt32Return(writer.GetCurrentPosition() - lengthPosition - 4, lengthPosition);
    //}
}
