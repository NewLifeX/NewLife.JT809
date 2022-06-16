using System;

namespace NewLife.JT809.Models;

/// <summary>位置报文附加信息种类</summary>
public enum AdditionalKinds : Byte
{
    /// <summary>里程，数据类型为 DWORD,单位为 1/10km,对应车上里程表读数</summary>
    里程 = 0x01,

    /// <summary>油量，数据类型为 WORD,单位为 1/10L,对应车上油量表读数</summary>
    油量 = 0x02,

    /// <summary>行驶记录功能获取的速度，数据类型为 WORD,单位为 1/10km/h</summary>
    速度 = 0x03,

    /// <summary>需要人工确认报警事件的 ID,数据类型为 WORD,从 1 开始计数</summary>
    报警事件 = 0x04,

    /// <summary>胎压，单位为Pa，30字节，标定轮子的顺序为从车头开始从左到右顺序排列，例如 :前 左 1,前左2,前右1,前右2,中左1,中左2,中左3,中右1,中右2,中右3,后左 1， 后左2,后左3……,以此类推，多余的字节为0xF,表示无效数据</summary>
    胎压 = 0x05,

    /// <summary>车厢温度，单位为摄氏度，取值范围为 -32767 ~ +32767,最高位为 1 表示 负数</summary>
    车厢温度 = 0x06,

    /// <summary>卫星状态数据。2019版</summary>
    卫星状态 = 0x07,

    /// <summary>超速报警</summary>
    超速报警 = 0x11,

    /// <summary>进出区域/路线报警</summary>
    路线报警 = 0x12,

    /// <summary>路段行驶时间不足/过长报警</summary>
    行驶时间报警 = 0x13,

    /// <summary>视频相关报警，DWORD，按位设置</summary>
    视频报警 = 0x14,

    /// <summary>视频信号丢失报警状态，DWORD，按位设置，bit0～bit31 分别表示第 1～32 个逻辑通道，相应位为 1 则表示该逻辑通道发生视频信号丢失。</summary>
    视频丢失 = 0x15,

    /// <summary>视频信号遮挡报警状态，DWORD，按位设置，bit0～bit31 分别表示第 1～32 个逻辑通道，相应位为 1 则表示该逻辑通道发生视频信号遮挡。</summary>
    视频遮挡 = 0x16,

    /// <summary>存储器故障报警状态，WORD，按位设置，bit0～bit11 分别表示第 1～12 个主存储器，bit12～bit15 分别表示第 1～4 个灾备存储装置，相应位为 1 则表示该存储器发生故障。</summary>
    存储故障 = 0x17,

    /// <summary>异常驾驶行为报警详细描述，WORD</summary>
    异常驾驶 = 0x18,

    /// <summary>扩展车辆信号状态位</summary>
    车辆信号 = 0x25,

    /// <summary>I0状态位</summary>
    IO状态 = 0x2A,

    /// <summary>模拟量，bit0-15,AD0;bit16 -31，AD1</summary>
    模拟量 = 0x2B,

    /// <summary>数据类型为 BYTE,无线通信网络信号强度</summary>
    信号强度 = 0x30,

    /// <summary>数据类型为 BYTE,GNss 定位卫星数</summary>
    卫星数 = 0x31,

    /// <summary>高级驾驶辅助系统报警信息</summary>
    ADAS = 0x64,

    /// <summary>驾驶员状态监测系统报警信息</summary>
    DSM = 0x65,

    /// <summary>胎压监测系统报警信息</summary>
    TPMS = 0x66,

    /// <summary>盲区监测系统报警信息</summary>
    BSD = 0x67,

    /// <summary>厂商自定义</summary>
    EB = 0xEB,

    /// <summary>安装异常信息</summary>
    安装异常 = 0xF1,

    /// <summary>算法异常信息</summary>
    算法异常 = 0xF2,
}