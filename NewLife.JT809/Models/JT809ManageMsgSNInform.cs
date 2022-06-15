namespace NewLife.JT809.Models;

/// <summary>
/// 子业务消息
/// </summary>
public class JT809ManageMsgSNInform
{
    /// <summary>
    /// 子业务类型标识
    /// </summary>
    public UInt16 Kind { get; set; }

    /// <summary>
    /// 对应得子业务数据类型报文序列号
    /// </summary>
    public UInt32 MsgSN { get; set; }

    /// <summary>
    /// 系统UTC时间
    /// </summary>
    public DateTime Time { get; set; }
}