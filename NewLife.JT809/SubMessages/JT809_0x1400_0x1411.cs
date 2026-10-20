using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 报警处理结果
/// </summary>
public enum JT809_0x1411_Result : Byte
{
    处理中 = 0x00,
    已处理完毕 = 0x01,
    不做处理 = 0x02,
    将来处理 = 0x03
}

/// <summary>
/// 上报报警督办应答消息
/// <para>子业务类型标识:UP_WARN_MSG_URGE_TODO_ACK_INFO</para>
/// <para>描述:下级监管平台上报上级平台“上报报警督办应答消息”</para>
/// <para>数据体见JT/T 809-2019 表66</para>
/// </summary>
[MessageKind(MessageKinds.上报报警督办应答消息)]
public class JT809_0x1400_0x1411 : IAccessor
{
    /// <summary>对应报警督办请求消息源子业务类型标识</summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>对应报警督办请求消息源报文序列号</summary>
    public UInt32 SourceMsgSn { get; set; }

    /// <summary>报警处理结果</summary>
    public JT809_0x1411_Result Result { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        SourceDataType = reader.ReadUInt16();
        SourceMsgSn = reader.ReadUInt32();
        Result = (JT809_0x1411_Result)reader.ReadByte();

        return true;
    }

    /// <summary>写入</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Write(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var writer = Helper.CreateWriter(stream, header);

        writer.Write(SourceDataType);
        writer.Write(SourceMsgSn);
        writer.Write((Byte)Result);

        return true;
    }
}