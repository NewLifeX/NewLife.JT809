using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 应答结果
/// </summary>
public enum JT809_0x1501_Result : Byte
{
    监听成功 = 0x00,
    监听失败 = 0x01
}

/// <summary>
/// 车辆单向监听应答
/// <para>子业务类型标识:UP_CTRL_MSG_MONITOR_VEHTCLE_ACK</para>
/// <para>描述:下级平台向上级平台上传车辆单向监听请求消息的应答</para>
/// <para>2011版数据体见JT/T 809-2011 表54，2019版数据体见JT/T 809-2019 表74，2019版新增源子业务类型与源报文序列号</para>
/// </summary>
[MessageKind(MessageKinds.车辆单向监听应答)]
public class JT809_0x1500_0x1501 : IAccessor
{
    /// <summary>
    /// 对应车辆单向监听请求消息源子业务类型标识。2019版新增
    /// </summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>
    /// 对应车辆单向监听请求消息源报文序列号。2019版新增
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }

    /// <summary>
    /// 应答结果
    /// </summary>
    public JT809_0x1501_Result Result { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        if (reader.Version == "2019")
        {
            SourceDataType = reader.ReadUInt16();
            SourceMsgSn = reader.ReadUInt32();
        }

        Result = (JT809_0x1501_Result)reader.ReadByte();

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

        if (writer.Version == "2019")
        {
            writer.Write(SourceDataType);
            writer.Write(SourceMsgSn);
        }

        writer.Write((Byte)Result);

        return true;
    }
}