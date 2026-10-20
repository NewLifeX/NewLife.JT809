using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

public enum JT809_0x920A_UploadFlag : Byte
{
    最近收到的消息 = 0x00,
    从终端获取 = 0x01
}

/// <summary>
/// 上报车辆驾驶员身份识别信息请求
/// <para>子业务类型标识:DOWN_EXG_MSG_REPORT_DRIVER_INFO</para>
/// <para>2011版数据体见JT/T 809-2011 表38（无数据部分），2019版数据体见JT/T 809-2019 表50，2019版新增上传标志</para>
/// </summary>
[MessageKind(MessageKinds.上报车辆驾驶员身份识别信息请求)]
public class JT809_0x9200_0x920A : IAccessor
{
    /// <summary>
    /// 上传标志。2019版新增
    /// </summary>
    public JT809_0x920A_UploadFlag Flag { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        // 2011版没有数据部分，剩余长度为0
        if (reader.Version == "2019" && stream.Position < stream.Length)
            Flag = (JT809_0x920A_UploadFlag)reader.ReadByte();

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

        if (writer.Version == "2019") writer.Write((Byte)Flag);

        return true;
    }
}