using NewLife.JT809.Protocols;
using NewLife.JT809.Serialization;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 主动上报驾驶员身份信息消息
/// <para>子业务类型标识:UP_EXG_MSG_REPORT_DRIVER_INFO</para>
/// <para>下级平台在接收到车载终端上传的驾驶员身份信息后，主动向上级平台上报该消息</para>
/// </summary>
[MessageKind(MessageKinds.主动上报驾驶员身份信息消息)]
public class JT809_0x1200_0x120C
{
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

    //public JT809_0x1200_0x120C Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x1200_0x120C
    //    {
    //        DriverName = reader.ReadString(16),
    //        DriverID = reader.ReadString(20),
    //        Licence = reader.ReadString(40),
    //        OrgName = reader.ReadString(200)
    //    };
    //    if (config.Version == JT809Version.JTT2019)
    //    {
    //        value.ValidDate = reader.ReadUTCDateTime();
    //    }
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1200_0x120C value, IJT809Config config)
    //{
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