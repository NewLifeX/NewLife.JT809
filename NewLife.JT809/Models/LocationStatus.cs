using System;

namespace NewLife.JT809.Models;

/// <summary>位置状态</summary>
[Flags]
public enum LocationStatus : UInt32
{
    /// <summary>电源开关</summary>
    ACC = 1,

    /// <summary>定位</summary>
    定位 = 2,

    /// <summary>南纬</summary>
    Latitude = 4,

    /// <summary>西经</summary>
    Longitude = 8,

    /// <summary>停运状态</summary>
    Operate = 1 << 4,

    /// <summary>经纬度加密</summary>
    Encryption = 1 << 5,

    /// <summary>紧急刹车系统采集的前撞预警</summary>
    前撞预警 = 1 << 6,

    /// <summary>车道偏移预警</summary>
    车道偏移预警 = 1 << 7,

    /// <summary>半载</summary>
    半载 = 1 << 8,

    /// <summary>满载</summary>
    满载 = 1 << 9,

    /// <summary>油路断开</summary>
    OilRoad = 1 << 10,

    /// <summary>电路断开</summary>
    ElectricityRoad = 1 << 11,

    /// <summary>车门加锁</summary>
    DoorLock = 1 << 12,

    /// <summary>前门开</summary>
    Door1 = 1 << 13,

    /// <summary>中门开</summary>
    Door2 = 1 << 14,

    /// <summary>后门开</summary>
    Door3 = 1 << 15,

    /// <summary>驾驶席门开</summary>
    Door4 = 1 << 16,

    /// <summary>自定义门开</summary>
    Door5 = 1 << 17,

    /// <summary>GPS卫星</summary>
    GPS = 1 << 18,

    /// <summary>北斗卫星</summary>
    北斗 = 1 << 19,

    /// <summary>GLONASS卫星</summary>
    GLONASS = 1 << 20,

    /// <summary>GALILEO卫星</summary>
    GALILEO = 1 << 21,

    /// <summary>行驶状态</summary>
    Driving = 1 << 22,

    /// <summary>保留</summary>
    Keep23 = 1 << 23,

    /// <summary>保留</summary>
    Keep24 = 1 << 24,

    /// <summary>保留</summary>
    Keep25 = 1 << 25,

    /// <summary>保留</summary>
    Keep26 = 1 << 26,

    /// <summary>保留</summary>
    Keep27 = 1 << 27,

    /// <summary>保留</summary>
    Keep28 = 1 << 28,

    /// <summary>保留</summary>
    Keep29 = 1 << 29,

    /// <summary>保留</summary>
    Keep30 = 1 << 30,

    /// <summary>保留</summary>
    Keep31 = 0x80000000,
}