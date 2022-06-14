using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

/// <summary>
/// 取消交换指定车辆定位信息请求
/// <para>子业务类型标识：UP_EXG_MSG_APPLY_F0R_MONIOR_END</para>
/// <para>描述：下级平台上传该命令给上级平台，取消之前申请监控的特殊车辆</para>
/// </summary>
[MessageKind(MessageKinds.取消交换指定车辆定位信息请求)]
public class JT809_0x1200_0x1208
{
}