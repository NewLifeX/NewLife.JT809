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
/// </summary>
[MessageKind(MessageKinds.车辆定位信息自动补报请求消息)]
public class JT809_0x1200_0x1203
{
    ///// <summary>
    ///// 卫星定位数据个数 1大于GNSS_CNT小于5
    ///// </summary>
    //public Byte Count { get; set; }

    /// <summary>
    /// 卫星定位数据集合
    /// </summary>
    //[FieldSize(nameof(Count))]
    public List<JT809_0x1200_0x1202> GNSS { get; set; }
}