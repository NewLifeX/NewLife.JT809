using System;

namespace NewLife.JT809.Models;

/// <summary>位置报警</summary>
[Flags]
public enum LocationAlarms : UInt32
{
    /// <summary>
    /// 紧急报瞥触动报警开关后触发
    /// </summary>
    紧急报瞥 = 1 << 0,

    /// <summary>
    /// 超速报警
    /// </summary>
    超速报警 = 1 << 1,

    /// <summary>
    /// 疲劳驾驶报警
    /// </summary>
    疲劳驾驶报警 = 1 << 2,

    /// <summary>
    /// 危险驾驶行为报警
    /// </summary>
    危险驾驶行为报警 = 1 << 3,

    /// <summary>
    /// GNSS模块发生故障
    /// </summary>
    GNSS模块故障 = 1 << 4,

    /// <summary>
    /// GNSS天线未接或被剪断
    /// </summary>
    GNSS天线未接 = 1 << 5,

    /// <summary>
    /// GNSS天线短路
    /// </summary>
    GNSS天线短路 = 1 << 6,

    /// <summary>
    /// 终端主电源欠压
    /// </summary>
    电源欠压 = 1 << 7,

    /// <summary>
    /// 终端主电源掉电
    /// </summary>
    电源掉电 = 1 << 8,

    /// <summary>
    /// 终端LCD或显示器故障
    /// </summary>
    显示器故障 = 1 << 9,

    /// <summary>
    /// TTS模块故障
    /// </summary>
    TTS故障 = 1 << 10,

    /// <summary>
    /// 摄像头故障
    /// </summary>
    摄像头故障 = 1 << 11,

    /// <summary>IC卡故障</summary>
    IC卡故障 = 1 << 12,

    /// <summary>超速预警</summary>
    超速预警 = 1 << 13,

    /// <summary>疲劳驾驶预警</summary>
    疲劳驾驶预警 = 1 << 14,

    /// <summary>违规行驶报警</summary>
    违规行驶报警 = 1 << 15,

    /// <summary>胎压预警</summary>
    胎压预警 = 1 << 16,

    /// <summary>右转盲区异常报警</summary>
    右转盲区异常报警 = 1 << 17,

    /// <summary>当天累计驾驶超时报警</summary>
    当天累计驾驶超时报警 = 1 << 18,

    /// <summary>超时停车报警</summary>
    超时停车报警 = 1 << 19,

    /// <summary>进出区域报警</summary>
    进出区域报警 = 1 << 20,

    /// <summary>进出路线报警</summary>
    进出路线报警 = 1 << 21,

    /// <summary>路段行驶时间报警，不足/过长</summary>
    路段行驶时间报警 = 1 << 22,

    /// <summary>路线偏离报警</summary>
    路线偏离报警 = 1 << 23,

    /// <summary>VSS故障</summary>
    VSS故障 = 1 << 24,

    /// <summary>油量异常报警</summary>
    油量异常报警 = 1 << 25,

    /// <summary>被盗报警</summary>
    被盗报警 = 1 << 26,

    /// <summary>非法点火报警</summary>
    非法点火报警 = 1 << 27,

    /// <summary>非法位移报警</summary>
    非法位移报警 = 1 << 28,

    /// <summary>碰撞侧翻报警</summary>
    碰撞侧翻报警 = 1 << 29,

    /// <summary>侧翻预警</summary>
    侧翻预警 = 1 << 30,

    ///// <summary>保留</summary>
    //Keep31 = 1 << 31,
}