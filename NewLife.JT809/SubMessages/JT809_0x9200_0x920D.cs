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
/// <para>2019版数据体见JT/T 809-2019 表53：线路ID(4) + 处理结果(1) + 原因长度(2) + 原因</para>
/// </summary>
[MessageKind(MessageKinds.车辆行驶线路请求应答)]
public class JT809_0x9200_0x920D : IAccessor
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

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        DRVLineID = reader.ReadUInt32();
        Result = (JT809_0x920D_Result)reader.ReadByte();

        var len = reader.ReadUInt16();
        if (len > stream.Length - stream.Position) return false;

        ReasonLength = len;
        if (len > 0) Reason = Helper.Encoding.GetString(reader.ReadBytes(len));

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

        writer.Write(DRVLineID);
        writer.Write((Byte)Result);

        var buf = Reason.IsNullOrEmpty() ? Array.Empty<Byte>() : Helper.Encoding.GetBytes(Reason);
        ReasonLength = (UInt16)buf.Length;

        writer.Write(ReasonLength);
        writer.Write(buf, 0, buf.Length);

        return true;
    }
}