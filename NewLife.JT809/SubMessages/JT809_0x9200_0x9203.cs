using System.Runtime.Serialization;
using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.SubMessages;

/// <summary>
///  车辆定位信息交换补发消息
///  <para>子业务类型标识:DOWN_EXG_MSG_HISTORY_ARCOSSAREA</para>
///  <para>描述:本业务在 DOWN_EXG_MSG_APPLY_HISGNSSDATA_ACK 应答成功后，立即开始交换。如果申请失败，则不进行数据转发</para>
///  <para>2011版数据体见JT/T 809-2011 表31，2019版数据体见JT/T 809-2019 表42，均为「个数(1) + N 条定位信息数据体」</para>
/// </summary>
[MessageKind(MessageKinds.车辆定位信息交换补发消息)]
public class JT809_0x9200_0x9203 : IAccessor
{
    ///// <summary>
    ///// 卫星定位数据个数 1大于GNSS_CNT小于5
    ///// </summary>
    //public Byte GNSSCount { get; set; }

    /// <summary>卫星定位数据个数。规范要求 1~5，超出时应拆分为多条消息补发</summary>
    public Byte Count => (Byte)(GNSS?.Count ?? 0);

    /// <summary>
    /// 卫星定位数据集合
    /// </summary>
    [IgnoreDataMember]
    public List<JT809_0x9200_0x9202> GNSS { get; set; }

    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context">报文头，用于取协议版本</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        var count = reader.ReadByte();
        var list = new List<JT809_0x9200_0x9202>(count);
        for (var i = 0; i < count; i++)
        {
            var item = new JT809_0x9200_0x9202();
            if (!item.Read(stream, context)) return false;

            list.Add(item);
        }

        GNSS = list;

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

        var list = GNSS;
        if (list != null && list.Count > 5) throw new ArgumentOutOfRangeException(nameof(GNSS), L.T("A maximum of 5 location records can be carried in one message; split the rest into additional messages."));

        writer.WriteByte((Byte)(list?.Count ?? 0));
        if (list != null)
        {
            foreach (var item in list)
            {
                if (!item.Write(stream, context)) return false;
            }
        }

        return true;
    }
}