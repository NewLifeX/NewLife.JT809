using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 报警处理结果
/// </summary>
public enum JT809_0x1401_Result : Byte
{
    处理中 = 0x00,
    已处理完毕 = 0x01,
    不做处理 = 0x02,
    将来处理 = 0x03
}

/// <summary>
/// 报警督办应答消息
/// <para>子业务类型标识:UP_WARN_MSG_URGE_TODO_ACK</para>
/// <para>描述:下级平台应答上级平台下发的报警督办请求消息，向上.级平台上报车辆的报瞥处理结果</para>
/// </summary>
[MessageKind(MessageKinds.报警督办应答消息)]
public class JT809_0x1400_0x1401 : IAccessor
{
    /// <summary>请求序号</summary>
    public UInt32 MsgSn { get; set; }

    /// <summary>子业务标识</summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>子业务序号</summary>
    [FieldSize(3)]
    public UInt32 SourceMsgSn { get; set; }

    /// <summary>
    /// 报警处理结果
    /// </summary>
    public JT809_0x1401_Result Result { get; set; }

    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var reader = Helper.CreateReader(stream);

        MsgSn = reader.ReadUInt32();
        SourceDataType = reader.ReadUInt16();

        var flag = reader.ReadUInt32();
        SourceMsgSn = flag >> 8;
        Result = (JT809_0x1401_Result)(flag & 0xFF);

        return true;
    }

    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Write(Stream stream, Object context)
    {
        var writer = Helper.CreateWriter(stream);

        writer.Write(MsgSn);
        writer.Write(SourceDataType);

        var flag = (SourceMsgSn << 8) | (Byte)Result;
        writer.Write(flag);

        return true;
    }
}