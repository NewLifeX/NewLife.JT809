using NewLife.JT809.Protocols;

namespace NewLife.JT809.SubMessages;

public enum JT809_0x9502_SizeType
{
    _320x240 = 0x01,
    _640x480 = 0x02,
    _800x600 = 0x03,
    _1024x768 = 0x04,
    _176x144_QCIF = 0x05,
    _704x288_CIF = 0x06,
    _704x288_HALFD = 0x07,
    _704x576_DI = 0x08
}

/// <summary>
/// 车辆拍照请求消息
/// <para>子业务类型标识:DOWN_CTRL_MSG_TAKE_PHOTO_REQ</para>
/// <para>描述:上级平台向下级平台下发对某指定车辆的拍照请求消息</para>
/// </summary>
[MessageKind(MessageKinds.车辆拍照请求消息)]
public class JT809_0x9500_0x9502
{
    /// <summary>
    /// 镜头ID
    /// </summary>
    public Byte LensID { get; set; }
    /// <summary>
    /// 图片大小
    /// Ox01:320x240:
    /// Ox02:640x480:
    /// Ox03;:800x600:
    /// Ox04:1024x768:
    /// Ox05:176x 144[QCIF];
    /// 0x06:704x288[CIF];
    /// 0x07:704x288[HALF D];
    /// Ox08:704x576[DI]
    /// </summary>
    public JT809_0x9502_SizeType SizeType { get; set; }
}