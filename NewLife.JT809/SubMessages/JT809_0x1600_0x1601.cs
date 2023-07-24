using System.Text;
using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 补报车辆静态信息应答
/// <para>子业务类型标识:UP_BASE_MSG_VEHICLE_ADDED_ACK</para>
/// <para>描述:上级平台应答下级平台发送的"补报车辆静态信息清求"消息</para>
/// </summary>
[MessageKind(MessageKinds.补报车辆静态信息应答)]
public class JT809_0x1600_0x1601 : IAccessor
{
    ///// <summary>
    ///// 对应补报车辆静态信息请求消息源子业务类型标识
    ///// </summary>
    //public UInt16 SourceDataType { get; set; }

    ///// <summary>
    /////  对应补报车辆静态信息请求消息源报文序列号
    ///// </summary>
    //public UInt32 SourceMsgSn { get; set; }

    /// <summary>
    /// 车辆信息
    /// </summary>
    public String CarInfo { get; set; }

    /// <summary>序列化信息</summary>
    public CarInfo Info { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);
        //reader.Encoding = Encoding.GetEncoding("gb2313");

        //SourceDataType = reader.ReadUInt16();
        //SourceMsgSn = reader.ReadUInt32();
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

        //writer.Write(SourceDataType);
        //writer.Write(SourceMsgSn);

        var inf = Info;
        if (inf != null) CarInfo = inf.ToStr();

        //var buf = CarInfo?.GetBytes();
        //if (buf != null && buf.Length > 0) writer.Write(buf, 0, buf.Length);
        if (!CarInfo.IsNullOrEmpty()) writer.WriteAllGBK(CarInfo);

        return true;
    }
}