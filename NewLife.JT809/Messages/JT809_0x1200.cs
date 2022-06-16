using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;

namespace NewLife.JT809.Messages;

/// <summary>
/// 主链路车辆动态信息交换业务
/// <para>链路类型:主链路</para>
/// <para>消息方向:下级平台往上级平台</para>
/// <para>业务数据类型标识:UP_EXG_MSG</para>
/// <para>描述:下级平台向上级平台发送车辆动态信息交换业务数据包</para>
/// </summary>
[MessageKind(MessageKinds.主链路车辆动态信息交换业务)]
public class JT809_0x1200 : JT809ExchangeMessageBodies
{
    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override Boolean Read(Stream stream, Object context)
    {
        if (!base.Read(stream, context)) return false;

        // 解析卫星数据
        if (Body is JT809_0x1200_0x1202 j1202)
            j1202.Decode();
        else if (Body is JT809_0x1200_0x1203 j1203 && j1203.GNSS != null)
        {
            foreach (var item in j1203.GNSS)
            {
                item.Decode();
            }
        }

        return true;
    }

    /// <summary>写入</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override Boolean Write(Stream stream, Object context)
    {
        // 提前写入卫星数据
        if (Body is JT809_0x1200_0x1202 j1202)
            j1202.Encode();
        else if (Body is JT809_0x1200_0x1203 j1203 && j1203.GNSS != null)
        {
            foreach (var item in j1203.GNSS)
            {
                item.Encode();
            }
        }

        return base.Write(stream, context);
    }
}