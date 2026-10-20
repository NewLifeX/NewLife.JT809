using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 上报驾驶员身份识别信息应答消息
/// <para>子业务类型标识:UP_EXG_MSG_REPORT_DRIVER_INFO_ACK</para>
/// <para>描述:下级平台应答上级平台发送的上报驾驶员身份识别信息请求消息，上传指定车辆的驾驶员身份识别信息数据</para>
/// <para>2011版数据体见JT/T 809-2011 表27，2019版数据体见JT/T 809-2019 表34，2019版新增源子业务类型、源报文序列号与证件有效期</para>
/// </summary>
[MessageKind(MessageKinds.上报驾驶员身份识别信息应答消息)]
public class JT809_0x1200_0x120A : IAccessor
{
    /// <summary>
    /// 对应上报驾驶员身份请求信息源子业务类型标识。2019版新增
    /// </summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>
    /// 对应上报驾驶员身份请求消息源报文序列号。2019版新增
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }

    /// <summary>
    /// 驾驶员姓名
    /// </summary>
    [FieldSize(16)]
    public String DriverName { get; set; }

    /// <summary>
    /// 身份证编号
    /// </summary>
    [FieldSize(20)]
    public String DriverID { get; set; }

    /// <summary>
    /// 从业资格证（备用）
    /// </summary>
    [FieldSize(40)]
    public String Licence { get; set; }

    /// <summary>
    /// 发证机构名称（备用）
    /// </summary>
    [FieldSize(200)]
    public String OrgName { get; set; }

    /// <summary>
    /// 证件有效期，时分秒均用0表示。2019版新增
    /// </summary>
    public DateTime ValidDate { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        var is2019 = reader.Version == "2019";
        if (is2019)
        {
            SourceDataType = reader.ReadUInt16();
            SourceMsgSn = reader.ReadUInt32();
        }

        DriverName = reader.ReadFixedString(16);
        DriverID = reader.ReadFixedString(20);
        Licence = reader.ReadFixedString(40);
        OrgName = reader.ReadFixedString(200);

        if (is2019) ValidDate = reader.Read<DateTime>();

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

        var is2019 = writer.Version == "2019";
        if (is2019)
        {
            writer.Write(SourceDataType);
            writer.Write(SourceMsgSn);
        }

        writer.WriteFixedString(DriverName, 16);
        writer.WriteFixedString(DriverID, 20);
        writer.WriteFixedString(Licence, 40);
        writer.WriteFixedString(OrgName, 200);

        if (is2019) writer.Write(ValidDate);

        return true;
    }

    //public JT809_0x1200_0x120A Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x1200_0x120A();
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.SourceDataType = reader.ReadUInt16();
    //        value.SourceMsgSn = reader.ReadUInt32();
    //    }
    //    value.DriverName = reader.ReadString(16);
    //    value.DriverID = reader.ReadString(20);
    //    value.Licence = reader.ReadString(40);
    //    value.OrgName = reader.ReadString(200);
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.ValidDate = reader.ReadUTCDateTime();
    //    }
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1200_0x120A value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteUInt16(value.SourceDataType);
    //        writer.WriteUInt32(value.SourceMsgSn);
    //    }
    //    writer.WriteStringPadRight(value.DriverName, 16);
    //    writer.WriteStringPadRight(value.DriverID, 20);
    //    writer.WriteStringPadRight(value.Licence, 40);
    //    writer.WriteStringPadRight(value.OrgName, 200);
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        writer.WriteUTCDateTime(value.ValidDate);
    //    }
    //}
}