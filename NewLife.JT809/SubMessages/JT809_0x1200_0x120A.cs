using NewLife.JT809.Protocols;
using NewLife.JT809.Serialization;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 上报驾驶员身份识别信息应答消息
/// <para>子业务类型标识:UP_EXG_MSG_REPORT_DRIVER_INFO_ACK</para>
/// <para>描述:下级平台应答上级平台发送的上报驾驶员身份识别信息请求消息，上传指定车辆的驾驶员身份识别信息数据</para>
/// </summary>
[MessageKind(MessageKinds.上报驾驶员身份识别信息应答消息)]
public class JT809_0x1200_0x120A
{
    /// <summary>
    /// 对应上报驾驶员身份请求信息源子业务类型标识
    /// </summary>
    public UInt16 SourceDataType { get; set; }
    /// <summary>
    /// 对应上报驾驶员身份请求消息源报文序列号
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
    /// 证件有效期，时分秒均用0表示
    /// </summary>
    [BCDTime]
    public DateTime ValidDate { get; set; }

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