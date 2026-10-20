using System;
using System.ComponentModel;
using System.IO;
using NewLife;
using NewLife.JT809;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;
using NewLife.Serialization;
using Xunit;

namespace XUnitTest;

/// <summary>按规范表逐字节核对的黄金字节用例，防止字段顺序或宽度被自洽的往返测试掩盖</summary>
[DisplayName("协议黄金字节")]
public class JT809_GoldenBytes_Tests
{
    private static Byte[] WriteBody(Object body, JTVersion version)
    {
        var ms = new MemoryStream();
        Assert.True(((IAccessor)body).Write(ms, new JTMessage { Version = version }));

        return ms.ToArray();
    }

    [Fact]
    [DisplayName("0x1302 下发报文应答：2011信息ID / 2019源字段")]
    public void TestPlatformInfoAckGolden()
    {
        Assert.Equal("12345678", WriteBody(new JT809_0x1300_0x1302 { InfoID = 0x12345678 }, JTVersion.JT2011).ToHex());
        Assert.Equal("930201020304", WriteBody(new JT809_0x1300_0x1302 { SourceDataType = 0x9302, SourceMsgSn = 0x01020304 }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x1503 下发车辆报文应答：2011消息ID / 2019源字段")]
    public void TestTextInfoAckGolden()
    {
        Assert.Equal("0000006301", WriteBody(new JT809_0x1500_0x1503 { MsgID = 99, Result = JT809_0x1503_Result.下发失败 }, JTVersion.JT2011).ToHex());
        Assert.Equal("95030000000200", WriteBody(new JT809_0x1500_0x1503 { SourceDataType = 0x9503, SourceMsgSn = 2, Result = JT809_0x1503_Result.下发成功 }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x1205 启动交换应答：2019源字段+长度")]
    public void TestReturnStartupAckGolden()
    {
        Assert.Equal("00000000", WriteBody(new JT809_0x1200_0x1205(), JTVersion.JT2011).ToHex());
        Assert.Equal("92051122334400000000", WriteBody(new JT809_0x1200_0x1205 { SourceDataType = 0x9205, SourceMsgSN = 0x11223344 }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x120B 电子运单应答：长度前缀+GBK内容")]
    public void TestEwaybillGolden()
    {
        var buf = WriteBody(new JT809_0x1200_0x120B { EwaybillInfo = "AB" }, JTVersion.JT2011);
        Assert.Equal("000000024142", buf.ToHex());
    }

    [Fact]
    [DisplayName("0x120A 驾驶员身份应答：定长填充与UTC有效期")]
    public void TestDriverInfoGolden()
    {
        var buf = WriteBody(new JT809_0x1200_0x120A
        {
            SourceDataType = 0x920A,
            SourceMsgSn = 7,
            DriverName = "AB",
            ValidDate = 1263085674L.ToDateTime(),
        }, JTVersion.JT2019);

        // 源子业务(2) + 源序列号(4) + 姓名(16) + 身份证(20) + 资格证(40) + 机构(200) + 有效期(8)
        Assert.Equal(290, buf.Length);
        Assert.Equal("920A000000074142", buf[..8].ToHex());

        // 姓名之后全部填0
        for (var i = 8; i < 282; i++) Assert.Equal(0, (Int32)buf[i]);

        // 有效期按8字节UTC秒写出（表7 注3：1263085674 = 0x4B49286A）
        Assert.Equal("000000004B49286A", buf[282..].ToHex());
    }

    [Fact]
    [DisplayName("0x9202 2011定长定位体：时间字段按 日/月/年/时/分/秒 顺序")]
    public void TestLocationTimeGolden()
    {
        var buf = WriteBody(new JT809_0x9200_0x9202
        {
            Encrypt = JT809_VehiclePositionEncrypt.未加密,
            Time = new DateTime(2020, 6, 13, 13, 34, 47),
        }, JTVersion.JT2011);

        Assert.Equal(36, buf.Length);

        // 加密标识(0) + 日(13) + 月(6) + 年(2020=0x07E4) + 时(13) + 分(34) + 秒(47)
        Assert.Equal("000D0607E40D222F", buf[..8].ToHex());
    }

    [Fact]
    [DisplayName("0x9204 车辆静态信息：原始GBK无内层长度")]
    public void TestCarInfoGolden()
    {
        Assert.Equal("413A3D31", WriteBody(new JT809_0x9200_0x9204 { CarInfo = "A:=1" }, JTVersion.JT2011).ToHex());
    }

    [Fact]
    [DisplayName("0x1601 补报静态信息应答：两版前缀")]
    public void TestVehicleAddedAckGolden()
    {
        Assert.Equal("413A3D31", WriteBody(new JT809_0x1600_0x1601 { CarInfo = "A:=1" }, JTVersion.JT2011).ToHex());
        Assert.Equal("960100000001413A3D31", WriteBody(new JT809_0x1600_0x1601 { SourceDataType = 0x9601, SourceMsgSn = 1, CarInfo = "A:=1" }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x9301 平台查岗请求：2011信息ID在前，2019对象类型在前")]
    public void TestPostQueryReqGolden()
    {
        var buf11 = WriteBody(new JT809_0x9300_0x9301 { InfoID = 1 }, JTVersion.JT2011);
        Assert.Equal("0000000100000000", buf11.ToHex());

        var buf19 = WriteBody(new JT809_0x9300_0x9301
        {
            ObjectType = JT809_0x9301_ObjectType.当前连接的下级平台,
            ObjectID = "A",
            AnswerTime = 10,
            InfoID = 2,
        }, JTVersion.JT2019);

        // 对象类型(1) + 对象ID(20，右补0) + 应答时限(1) + 信息ID(4) + 长度(4)
        Assert.Equal(30, buf19.Length);
        Assert.Equal("01", buf19[..1].ToHex());
        Assert.Equal("41", buf19[1..2].ToHex());
        for (var i = 2; i < 21; i++) Assert.Equal(0, (Int32)buf19[i]);
        Assert.Equal("0A", buf19[21..22].ToHex());
        Assert.Equal("00000002", buf19[22..26].ToHex());
        Assert.Equal("00000000", buf19[26..30].ToHex());
    }
}
