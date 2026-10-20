using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 报警处理结果
/// </summary>
public enum JT809_0x1412_Result : Byte
{
    处理中 = 0x00,
    已处理完毕 = 0x01,
    不做处理 = 0x02,
    将来处理 = 0x03
}

/// <summary>
/// 主动上报报警处理结果消息
/// <para>子业务类型标识:UP_WARN_MSG_ADPT_TODO_INFO</para>
/// <para>描述:下级平台主动向上级平台上报报警处理结果。本条消息上级平台无需应答</para>
/// <para>数据体见JT/T 809-2019 表65</para>
/// </summary>
[MessageKind(MessageKinds.主动上报报警处理结果消息)]
public class JT809_0x1400_0x1412 : IAccessor
{
    /// <summary>对应报警消息源子业务类型标识</summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>对应报警信息消息或车辆定位消息源报文序列号</summary>
    public UInt32 SourceMsgSn { get; set; }

    /// <summary>报警处理结果</summary>
    public JT809_0x1412_Result Result { get; set; }

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
        Result = (JT809_0x1412_Result)reader.ReadByte();

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