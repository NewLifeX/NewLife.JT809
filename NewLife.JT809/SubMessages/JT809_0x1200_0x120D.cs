using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 主动上报车辆电子运单信息
/// <para>子业务类型标识:UP_EXG_MSG_REPORT_EWAYBILL_INFO</para>
/// <para>描述：下级平台“主动上报车辆电子运单信息”，向上级平台上报车辆当前电子运单</para>
/// </summary>
[MessageKind(MessageKinds.主动上报车辆电子运单信息)]
public class JT809_0x1200_0x120D
{
    /// <summary>
    /// 电子运单数据体长度
    /// </summary>
    public UInt32 EwaybillLength { get; set; }
    /// <summary>
    /// 电子运单数据内容
    /// </summary>
    [FieldSize(nameof(EwaybillLength))]
    public String EwaybillInfo { get; set; }
}