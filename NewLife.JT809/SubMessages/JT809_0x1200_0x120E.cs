using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 主动上报车辆行驶路线信息
/// <para>子业务类型标识:UP_BASE_MSG_DRVLINE_INFO</para>
/// <para>2019版数据体见JT/T 809-2019 表38：路线长度(4) + 路线信息</para>
/// </summary>
[MessageKind(MessageKinds.主动上报车辆行驶路线信息)]
public class JT809_0x1200_0x120E : IAccessor
{
    /// <summary>
    /// 路线信息长度
    /// </summary>
    public UInt32 DRVLineLength { get; set; }

    /// <summary>
    /// 路线信息 按照808-2019中8606规定的报文格式
    /// </summary>
    public Byte[] DRVLine { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        var len = reader.ReadInt32();
        if (len < 0 || len > stream.Length - stream.Position) return false;

        DRVLineLength = (UInt32)len;
        if (len > 0) DRVLine = reader.ReadBytes(len);

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

        var buf = DRVLine ?? Array.Empty<Byte>();
        DRVLineLength = (UInt32)buf.Length;

        writer.Write(buf.Length);
        writer.Write(buf, 0, buf.Length);

        return true;
    }
}