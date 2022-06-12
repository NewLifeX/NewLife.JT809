namespace NewLife.JT809.Models;

/// <summary>
/// 车辆颜色，按照 JT/T 415-2021 中5.4.12 的规定
/// </summary>
public enum JT809VehicleColorType : Byte
{
    /// <summary>蓝色</summary>
    蓝色 = 0x01,

    /// <summary>黄色</summary>
    黄色 = 0x02,

    /// <summary>黑色</summary>
    黑色 = 0x03,

    /// <summary>白色</summary>
    白色 = 0x04,

    /// <summary>其他</summary>
    其他 = 0x09,

    /// <summary>农黄色</summary>
    农黄色 = 91,

    /// <summary>农绿色</summary>
    农绿色 = 92,

    /// <summary>黄绿色</summary>
    黄绿色 = 93,

    /// <summary>渐变绿</summary>
    渐变绿 = 94,
}