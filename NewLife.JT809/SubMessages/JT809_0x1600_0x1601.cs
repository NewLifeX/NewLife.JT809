using System.Text;
using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 补报车辆静态信息应答
/// <para>子业务类型标识:UP_BASE_MSG_VEHICLE_ADDED_ACK</para>
/// <para>描述:上级平台应答下级平台发送的"补报车辆静态信息清求"消息</para>
/// <para>2011版数据体见JT/T 809-2011 表66，2019版数据体见JT/T 809-2019 表87，2019版新增源子业务类型与源报文序列号</para>
/// </summary>
[MessageKind(MessageKinds.补报车辆静态信息应答)]
public class JT809_0x1600_0x1601 : IAccessor
{
    /// <summary>
    /// 对应补报车辆静态信息请求消息源子业务类型标识。2019版新增
    /// </summary>
    public UInt16 SourceDataType { get; set; }

    /// <summary>
    ///  对应补报车辆静态信息请求消息源报文序列号。2019版新增
    /// </summary>
    public UInt32 SourceMsgSn { get; set; }

    /// <summary>
    /// 车辆信息
    /// </summary>
    public String CarInfo { get; set; }

    /// <summary>序列化信息</summary>
    public CarInfo Info { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public virtual Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);
        //reader.Encoding = Encoding.GetEncoding("gb2313");

        if (reader.Version == "2019")
        {
            SourceDataType = reader.ReadUInt16();
            SourceMsgSn = reader.ReadUInt32();
        }

        CarInfo = reader.ReadAllGBK();

        var inf = new CarInfo();
        inf.Parse(CarInfo);
        Info = inf;

        return true;
    }

    /// <summary>写入</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual Boolean Write(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var writer = Helper.CreateWriter(stream, header);

        if (writer.Version == "2019")
        {
            writer.Write(SourceDataType);
            writer.Write(SourceMsgSn);
        }

        var inf = Info;
        if (inf != null) CarInfo = inf.ToStr();

        //var buf = CarInfo?.GetBytes();
        //if (buf != null && buf.Length > 0) writer.Write(buf, 0, buf.Length);
        if (!CarInfo.IsNullOrEmpty()) writer.WriteAllGBK(CarInfo);

        return true;
    }
}