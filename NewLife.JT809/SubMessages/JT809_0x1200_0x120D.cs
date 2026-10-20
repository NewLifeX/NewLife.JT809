using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 主动上报车辆电子运单信息
/// <para>子业务类型标识:UP_EXG_MSG_REPORT_EWAYBILL_INFO</para>
/// <para>描述：下级平台“主动上报车辆电子运单信息”，向上级平台上报车辆当前电子运单</para>
/// <para>2019版数据体见JT/T 809-2019 表37：电子运单长度(4) + 电子运单内容，两版结构一致</para>
/// </summary>
[MessageKind(MessageKinds.主动上报车辆电子运单信息)]
public class JT809_0x1200_0x120D : IAccessor
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

        EwaybillLength = (UInt32)len;
        if (len > 0) EwaybillInfo = Helper.Encoding.GetString(reader.ReadBytes(len));

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

        var buf = EwaybillInfo.IsNullOrEmpty() ? Array.Empty<Byte>() : Helper.Encoding.GetBytes(EwaybillInfo);
        EwaybillLength = (UInt32)buf.Length;

        writer.Write(buf.Length);
        writer.Write(buf, 0, buf.Length);

        return true;
    }
}