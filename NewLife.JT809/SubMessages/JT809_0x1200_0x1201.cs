using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 上传车辆注册信息消息
/// <para>子业务类型标识:UP_EXG_MSG_REGISTER</para>
/// <para>描述:监控平台收到车载终端鉴权信息后，启动本命令向上级监管平台上传该车辆注册信息.各级监管平台再逐级向上级平台上传该信息</para>
/// <para>本条消息服务端无需应答</para>
/// </summary>
[MessageKind(MessageKinds.上传车辆注册信息)]
public class JT809_0x1200_0x1201
{
    /// <summary>
    /// 平台唯一编码
    /// </summary>
    [FieldSize(11)]
    public String PlateformId { get; set; }
    /// <summary>
    /// 车载终端厂商唯一编码
    /// </summary>
    [FieldSize(11)]
    public String ProducerId { get; set; }
    /// <summary>
    /// 车载终端型号，不是 8 位时以“\0”终结
    /// </summary>
    [FieldSize(20, Version = "2011")]
    [FieldSize(30, Version = "2019")]
    public String TerminalModelType { get; set; }
    /// <summary>
    /// 车载终端通讯模块IMEI码
    /// 2019版本
    /// </summary>
    //[FieldSize(-1, Version = "2011")]
    [JT2019]
    [FieldSize(15, Version = "2019")]
    public String ImeiId { get; set; }
    /// <summary>
    /// 车载终端编号，大写字母和数字组成
    /// </summary>
    [FieldSize(7, Version = "2011")]
    [FieldSize(30, Version = "2019")]
    public String TerminalId { get; set; }
    /// <summary>
    /// 车载终端 SIM 卡电话号码。号码不是12 位，则在前补充数字 0.
    /// </summary>
    [FieldSize(12, Version = "2011")]
    [FieldSize(13, Version = "2019")]
    public String TerminalSimCode { get; set; }

    //public JT809_0x1200_0x1201 Deserialize(ref JT809MessagePackReader reader, IJT809Config config)
    //{
    //    var value = new JT809_0x1200_0x1201();
    //    if (config.Version == JT809Version.JTT2011)
    //    {
    //        value.PlateformId = reader.ReadString(11);
    //        value.ProducerId = reader.ReadString(11);
    //        value.TerminalModelType = reader.ReadString(20);
    //        value.TerminalId = reader.ReadString(7);
    //        value.TerminalId = value.TerminalId.ToUpper();
    //        value.TerminalSimCode = reader.ReadString(12);
    //    }
    //    else
    //    {
    //        value.PlateformId = reader.ReadString(11);
    //        value.ProducerId = reader.ReadString(11);
    //        value.TerminalModelType = reader.ReadString(30);
    //        value.IMIEId = reader.ReadString(15);
    //        value.TerminalId = reader.ReadString(30);
    //        value.TerminalId = value.TerminalId.ToUpper();
    //        value.TerminalSimCode = reader.ReadString(13);
    //    }
    //    return value;
    //}

    //public void Serialize(ref JT809MessagePackWriter writer, JT809_0x1200_0x1201 value, IJT809Config config)
    //{
    //    if (config.Version == JT809Version.JTT2011)
    //    {
    //        writer.WriteStringPadRight(value.PlateformId, 11);
    //        writer.WriteStringPadRight(value.ProducerId, 11);
    //        writer.WriteStringPadRight(value.TerminalModelType, 20);
    //        writer.WriteStringPadRight(value.TerminalId.ToUpper(), 7);
    //        writer.WriteStringPadLeft(value.TerminalSimCode, 12);
    //    }
    //    else
    //    {
    //        writer.WriteStringPadRight(value.PlateformId, 11);
    //        writer.WriteStringPadRight(value.ProducerId, 11);
    //        writer.WriteStringPadRight(value.TerminalModelType, 30);
    //        writer.WriteStringPadRight(value.IMIEId, 15);
    //        writer.WriteStringPadRight(value.TerminalId.ToUpper(), 30);
    //        writer.WriteStringPadRight(value.TerminalSimCode, 13);
    //    }
    //}
}