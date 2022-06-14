using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

public enum JT809_0x920A_UploadFlag : Byte
{
    最近收到的消息 = 0x00,
    从终端获取 = 0x01
}

/// <summary>
/// 上报车辆驾驶员身份识别信息请求
/// <para>子业务类型标识:DOWN_EXG_MSG_REPORT_DRIVER_INFO</para>
/// </summary>
[MessageKind(MessageKinds.上报车辆驾驶员身份识别信息请求)]
public class JT809_0x9200_0x920A
{
    /// <summary>
    /// 上传标志
    /// </summary>
    public JT809_0x920A_UploadFlag Flag { get; set; }
}