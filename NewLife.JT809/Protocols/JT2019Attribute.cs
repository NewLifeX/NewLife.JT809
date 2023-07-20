using NewLife.Serialization;

namespace NewLife.JT809.Protocols;

/// <summary>仅JT/T2019使用的字段</summary>
/// <remarks>版本不是2019时，返回true告知上层处理已完成，即跳过了该字段的处理</remarks>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class JT2019Attribute : AccessorAttribute
{
    /// <summary>从数据流中读取消息</summary>
    /// <param name="formatter">序列化</param>
    /// <param name="context">上下文</param>
    /// <returns>是否成功</returns>
    public override Boolean Read(IFormatterX formatter, AccessorContext context) => formatter is Binary bn && bn.Version != "2019";

    /// <summary>把消息写入到数据流中</summary>
    /// <param name="formatter">序列化</param>
    /// <param name="context">上下文</param>
    public override Boolean Write(IFormatterX formatter, AccessorContext context) => formatter is Binary bn && bn.Version != "2019";
}