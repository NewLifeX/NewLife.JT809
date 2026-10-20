using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
///  交换车辆静态信息消息
///  <para>子业务类型标识:DOWN_EXG_MSG_CAR_INFO</para>
///  <para>描述:在首次启动跨域车辆定位信息交换，或者以后交换过程中车辆静态信息有更新时，由上级平台向下级平台下发一次车辆静态信息。下.级平台接收后自行更新该车辆静态信息</para>
///  <para>2011版数据体见JT/T 809-2011 表32，2019版数据体见JT/T 809-2019 表44，均为「车辆静态信息内容」直接填满后续数据长度</para>
/// </summary>
[MessageKind(MessageKinds.交换车辆静态信息消息)]
public class JT809_0x9200_0x9204 : IAccessor
{
    /// <summary>
    /// 车辆信息，标识与内容之间用半角“:=”分开，不同标识以半角“;”为分隔符
    /// </summary>
    public String CarInfo { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        CarInfo = reader.ReadAllGBK();

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

        if (!CarInfo.IsNullOrEmpty()) writer.WriteAllGBK(CarInfo);

        return true;
    }
}