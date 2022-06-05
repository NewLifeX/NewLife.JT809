namespace NewLife.JT809.Protocols;

/// <summary>
/// 类型、长度、数值结构
/// </summary>
public class TLV
{
    /// <summary>
    /// 类型
    /// </summary>
    public UInt32 Type { get; set; }

    /// <summary>
    /// 长度
    /// </summary>
    public Int32 Length { get; set; }

    /// <summary>
    /// 数值
    /// </summary>
    public Byte[] Value { get; set; }
}