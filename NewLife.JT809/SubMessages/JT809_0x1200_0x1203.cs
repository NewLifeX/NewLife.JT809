using System.Runtime.Serialization;
using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
///  车辆定位信息自动补报请求消息
///  <para>子业务类型标识:UP_EXG_MSG_HISTORY_LOCATION</para>
///  <para>描述:  a: 如果平台间传输链路中断，下级平台重新登录并与上级平台建立通信链路后，下级平台应将中断期间内车载终端上传的车辆定位信息自动补报到上级平台。
///  b: 如果系统断线期间，该车需发送的数据包条数大于 5，则以每包五条进行补发，直到补发完毕。
///   多条数据以卫星定位时间先后顺序排列。
///  本条消息上级平台采用定量回复，即收到一定数量的数据后，即通过从链路应答数据量。
///  </para>
///  <para>2011版每条为定长定位信息数据体（JT/T 809-2011 表69），2019版每条为自描述长度数据体（JT/T 809-2019 表90）</para>
/// </summary>
[MessageKind(MessageKinds.车辆定位信息自动补报请求消息)]
public class JT809_0x1200_0x1203 : IAccessor
{
    /// <summary>卫星定位数据个数。规范要求 1~5，超出时应拆分为多条消息补报</summary>
    public Byte Count => (Byte)(GNSS?.Count ?? 0);

    /// <summary>
    /// 卫星定位数据集合
    /// </summary>
    [IgnoreDataMember]
    public List<JT809_0x1200_0x1202> GNSS { get; set; }

    /// <summary>读取消息</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        var count = reader.ReadByte();
        var list = new List<JT809_0x1200_0x1202>(count);
        for (var i = 0; i < count; i++)
        {
            var item = new JT809_0x1200_0x1202();
            if (!item.Read(stream, header)) return false;

            list.Add(item);
        }

        GNSS = list;

        return true;
    }

    /// <summary>序列化消息</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Write(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var writer = Helper.CreateWriter(stream, header);

        var list = GNSS;
        if (list != null && list.Count > 5) throw new ArgumentOutOfRangeException(nameof(GNSS), L.T("A maximum of 5 location records can be carried in one message; split the rest into additional messages."));

        writer.WriteByte((Byte)(list?.Count ?? 0));
        if (list != null)
        {
            foreach (var item in list)
            {
                if (!item.Write(stream, header)) return false;
            }
        }

        return true;
    }
}