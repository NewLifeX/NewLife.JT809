using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

public enum JT809_0x920D_Result : Byte
{
    完成记录 = 0x00,
    审核通过_完成记录 = 0x01,
    信息错误_未完成记录 = 0x02,
    审核未通过_未完成记录 = 0x03
}

/// <summary>
/// 车辆行驶线路请求应答
/// <para>子业务类型标识:DOWN_BASE_MSG_DRVLINE_ACK</para>
/// <para>上级平台应答下级平台发送地车辆行驶路线信息</para>
/// </summary>
[MessageKind(MessageKinds.车辆行驶线路请求应答)]
public class JT809_0x9200_0x920D
{
    /// <summary>
    /// 线路ＩＤ，按照８０８－２０１９中0x8606规定的报文中的线路ID
    /// </summary>
    public UInt32 DRVLineID { get; set; }

    /// <summary>
    /// 处理结果 0x00 完成记录，0x01 审核通过，完成记录 0x02信息错误，未完成记录0x03 审核未通过，未完成记录
    /// </summary>
    public JT809_0x920D_Result Result { get; set; }

    /// <summary>
    /// 未通过原因内容长度
    /// </summary>
    public UInt16 ReasonLength { get; set; }

    /// <summary>
    /// 经过GBK编码后的未通过原因
    /// </summary>
    [FieldSize(nameof(ReasonLength))]
    public String Reason { get; set; }
}
