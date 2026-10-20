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

    private static String Zeros(Int32 count) => new('0', count * 2);

    private static Byte[] WriteAuto(Object body, JTVersion version)
    {
        var ms = new MemoryStream();
        Helper.CreateWriter(ms, new JTMessage { Version = version }).Write(body);

        return ms.ToArray();
    }

    [Fact]
    [DisplayName("0x1201 上传车辆注册：两版字段宽度与补0")]
    public void TestUploadRegisterGolden()
    {
        var body = new JT809_0x1200_0x1201
        {
            PlateformId = "1",
            ProducerId = "2",
            TerminalModelType = "3",
            TerminalId = "4",
            TerminalSimCode = "5",
        };

        // 2011：11 + 11 + 8 + 7 + 12
        var b11 = WriteAuto(body, JTVersion.JT2011);
        Assert.Equal(49, b11.Length);
        Assert.Equal("31" + Zeros(10), b11[..11].ToHex());
        Assert.Equal("32" + Zeros(10), b11[11..22].ToHex());
        Assert.Equal("33" + Zeros(7), b11[22..30].ToHex());
        Assert.Equal("34" + Zeros(6), b11[30..37].ToHex());
        Assert.Equal("35" + Zeros(11), b11[37..].ToHex());

        // 2019：11 + 11 + 30 + 15 + 30 + 13
        var b19 = WriteAuto(new JT809_0x1200_0x1201
        {
            PlateformId = "1",
            ProducerId = "2",
            TerminalModelType = "3",
            ImeiId = "6",
            TerminalId = "4",
            TerminalSimCode = "5",
        }, JTVersion.JT2019);
        Assert.Equal(110, b19.Length);
        Assert.Equal("31" + Zeros(10), b19[..11].ToHex());
        Assert.Equal("32" + Zeros(10), b19[11..22].ToHex());
        Assert.Equal("33" + Zeros(29), b19[22..52].ToHex());
        Assert.Equal("36" + Zeros(14), b19[52..67].ToHex());
        Assert.Equal("34" + Zeros(29), b19[67..97].ToHex());
        Assert.Equal("35" + Zeros(12), b19[97..].ToHex());
    }

    [Fact]
    [DisplayName("0x1202 2019定位数据体：加密+长度+数据+三级平台")]
    public void TestLocationBodyGolden()
    {
        var buf = WriteBody(new JT809_0x1200_0x1202
        {
            Encrypt = JT809_VehiclePositionEncrypt.未加密,
            GnssData = [0xAA, 0xBB],
            PlatformId1 = "1",
        }, JTVersion.JT2019);

        // 加密(1) + 长度(4) + 数据(2) + 平台1(11) + 报警1(4) + 平台2(11) + 报警2(4) + 平台3(11) + 报警3(4)
        Assert.Equal(52, buf.Length);
        Assert.Equal("00" + "00000002" + "AABB" + "31" + Zeros(10) + "00000000" + Zeros(11) + "00000000" + Zeros(11) + "00000000", buf.ToHex());
    }

    [Fact]
    [DisplayName("0x1203 自动补报：个数前缀与首条定位时间")]
    public void TestHistoryLocationGolden()
    {
        var body = new JT809_0x1200_0x1203
        {
            GNSS = [new JT809_0x1200_0x1202 { Time = new DateTime(2020, 6, 13, 13, 34, 47) }],
        };
        var buf = WriteBody(body, JTVersion.JT2011);

        Assert.Equal(1 + 36, buf.Length);
        Assert.Equal("01", buf[..1].ToHex());
        Assert.Equal("000D0607E40D222F", buf[1..9].ToHex());
    }

    [Fact]
    [DisplayName("0x1207/0x1209 申请与补发：两版均为两个UTC秒")]
    public void TestApplyTimeGolden()
    {
        var start = 1263085674L.ToDateTime();
        var end = 1263089274L.ToDateTime();

        Assert.Equal("000000004B49286A" + "000000004B49367A", WriteAuto(new JT809_0x1200_0x1207 { StartTime = start, EndTime = end }, JTVersion.JT2019).ToHex());
        Assert.Equal("000000004B49286A" + "000000004B49367A", WriteAuto(new JT809_0x1200_0x1209 { StartTime = start, EndTime = end }, JTVersion.JT2011).ToHex());
    }

    [Fact]
    [DisplayName("0x120C 主动上报驾驶员身份：2019尾部为UTC有效期")]
    public void TestDriverReportGolden()
    {
        var buf = WriteBody(new JT809_0x1200_0x120C
        {
            DriverName = "AB",
            ValidDate = 1263085674L.ToDateTime(),
        }, JTVersion.JT2019);

        Assert.Equal(284, buf.Length);
        Assert.Equal("4142", buf[..2].ToHex());
        for (var i = 2; i < 276; i++) Assert.Equal(0, (Int32)buf[i]);
        Assert.Equal("000000004B49286A", buf[276..].ToHex());
    }

    [Fact]
    [DisplayName("0x120D/0x120E 主动上报运单与路线：长度前缀")]
    public void TestReportLengthPrefixGolden()
    {
        Assert.Equal("000000024142", WriteBody(new JT809_0x1200_0x120D { EwaybillInfo = "AB" }, JTVersion.JT2019).ToHex());
        Assert.Equal("000000020102", WriteBody(new JT809_0x1200_0x120E { DRVLine = [0x01, 0x02] }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x9201 车辆注册应答：源类型+序列号+结果")]
    public void TestRegisterAckGolden()
    {
        Assert.Equal("12010000000301", WriteAuto(new JT809_0x9200_0x9201
        {
            DataType = 0x1201,
            MsgSn = 3,
            Result = JT809_0x9201_Result.审核通过_完成注册,
        }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x9203 补发定位：个数前缀与定长记录")]
    public void TestHistoryAcrossAreaGolden()
    {
        var body = new JT809_0x9200_0x9203
        {
            GNSS = [new JT809_0x9200_0x9202 { Time = new DateTime(2020, 6, 13, 13, 34, 47) }],
        };
        var buf = WriteBody(body, JTVersion.JT2011);

        Assert.Equal(1 + 36, buf.Length);
        Assert.Equal("01", buf[..1].ToHex());
        Assert.Equal("000D0607E40D222F", buf[1..9].ToHex());
    }

    [Fact]
    [DisplayName("0x9205/0x9206 启动与结束原因单字节")]
    public void TestReturnReasonGolden()
    {
        Assert.Equal("01", WriteAuto(new JT809_0x9200_0x9205 { ReasonCode = JT809_0x9205_ReasonCode.人工指定交换 }, JTVersion.JT2019).ToHex());
        Assert.Equal("03", WriteAuto(new JT809_0x9200_0x9206 { ReasonCode = JT809_0x9206_ReasonCode.车辆离线 }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x9207/0x9208/0x9209 应答：2019源字段在结果之前")]
    public void TestExchangeAckGolden()
    {
        Assert.Equal("12070000000900", WriteBody(new JT809_0x9200_0x9207
        {
            SourceDataType = 0x1207,
            SourceMsgSn = 9,
            Result = JT809_0x9207_Result.申请成功,
        }, JTVersion.JT2019).ToHex());

        Assert.Equal("02", WriteBody(new JT809_0x9200_0x9208 { Result = JT809_0x9208_Result.其它 }, JTVersion.JT2011).ToHex());

        Assert.Equal("12090000000104", WriteBody(new JT809_0x9200_0x9209
        {
            SourceDataType = 0x1209,
            SourceMsgSn = 1,
            Result = JT809_0x9209_Result.其它原因,
        }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x920A 上报驾驶员身份请求：2011为空，2019一字节")]
    public void TestDriverInfoReqGolden()
    {
        Assert.Empty(WriteBody(new JT809_0x9200_0x920A(), JTVersion.JT2011));
        Assert.Equal("01", WriteBody(new JT809_0x9200_0x920A { Flag = JT809_0x920A_UploadFlag.从终端获取 }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x920D 行驶线路应答：ID+结果+原因长度+GBK原因")]
    public void TestDrvLineAckGolden()
    {
        var buf = WriteBody(new JT809_0x9200_0x920D
        {
            DRVLineID = 0x1234,
            Result = JT809_0x920D_Result.审核通过_完成记录,
            Reason = "A",
        }, JTVersion.JT2019);

        Assert.Equal("00001234" + "01" + "0001" + "41", buf.ToHex());
    }

    [Fact]
    [DisplayName("0x1301 平台查岗应答：两版对象ID与源字段位置")]
    public void TestPostQueryAckGolden()
    {
        // 2011：对象类型(1) + 对象ID(12) + 信息ID(4) + 长度(4) + 内容
        var b11 = WriteBody(new JT809_0x1300_0x1301
        {
            ObjectType = JT809_0x1301_ObjectType.当前连接的下级平台,
            ObjectID = "A",
            InfoID = 5,
            InfoContent = "B",
        }, JTVersion.JT2011);
        Assert.Equal("01" + "41" + Zeros(11) + "00000005" + "00000001" + "42", b11.ToHex());

        // 2019：对象类型(1) + 应答人(16) + 电话(20) + 对象ID(20) + 源子业务(2) + 源序列号(4) + 长度(4) + 内容
        var b19 = WriteBody(new JT809_0x1300_0x1301
        {
            ObjectType = JT809_0x1301_ObjectType.下级平台所属单一业户,
            Responder = "A",
            ResponderTel = "B",
            ObjectID = "C",
            SourceDataType = 0x9301,
            SourceMsgSn = 2,
            InfoContent = "D",
        }, JTVersion.JT2019);
        Assert.Equal("02" + "41" + Zeros(15) + "42" + Zeros(19) + "43" + Zeros(19) + "9301" + "00000002" + "00000001" + "44", b19.ToHex());
    }

    [Fact]
    [DisplayName("0x1303/0x9303 消息补传请求：两版同构")]
    public void TestRetranGolden()
    {
        var time = 1263085674L.ToDateTime();
        const String expected = "1402" + "01" + "00000001" + "00000002" + "000000004B49286A";

        Assert.Equal(expected, WriteAuto(new JT809_0x1300_0x1303
        {
            RetranDataType = 0x1402,
            SerialCount = 1,
            BeginMessageNumber = 1,
            EndMessageNumber = 2,
            Time = time,
        }, JTVersion.JT2019).ToHex());

        Assert.Equal(expected, WriteBody(new JT809_0x9300_0x9303
        {
            RetranDataType = 0x1402,
            SerialCount = 1,
            BeginMessageNumber = 1,
            EndMessageNumber = 2,
            Time = time,
        }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x9302 下发报文请求：2011信息ID在前，2019对象字段在前")]
    public void TestPlatformInfoReqGolden()
    {
        Assert.Equal("00000007" + "00000001" + "41", WriteBody(new JT809_0x9300_0x9302 { InfoID = 7, InfoContent = "A" }, JTVersion.JT2011).ToHex());

        var b19 = WriteBody(new JT809_0x9300_0x9302
        {
            ObjectType = JT809_0x9302_ObjectType.当前连接的下级平台,
            ObjectID = "A",
            InfoID = 7,
            InfoContent = "B",
        }, JTVersion.JT2019);
        Assert.Equal("01" + "41" + Zeros(19) + "00000007" + "00000001" + "42", b19.ToHex());
    }

    [Fact]
    [DisplayName("0x1401 报警督办应答：2019报文序列号在结果之前")]
    public void TestUrgeAckGolden()
    {
        Assert.Equal("00001234" + "01", WriteBody(new JT809_0x1400_0x1401
        {
            SupervisionID = 0x1234,
            Result = JT809_0x1401_Result.已处理完毕,
        }, JTVersion.JT2011).ToHex());

        Assert.Equal("00000007" + "1401" + "00000009" + "01", WriteBody(new JT809_0x1400_0x1401
        {
            MsgSn = 7,
            SourceDataType = 0x1401,
            SourceMsgSn = 9,
            Result = JT809_0x1401_Result.已处理完毕,
        }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x1501 单向监听应答：2019源字段")]
    public void TestMonitorAckGolden()
    {
        Assert.Equal("9501" + "00000001" + "01", WriteBody(new JT809_0x1500_0x1501
        {
            SourceDataType = 0x9501,
            SourceMsgSn = 1,
            Result = JT809_0x1501_Result.监听失败,
        }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x1502 拍照应答：2011应答标识+定位体+镜头+长度+大小+格式+图片")]
    public void TestPhotoAckGolden()
    {
        var buf = WriteBody(new JT809_0x1500_0x1502
        {
            PhotoRspFlag = JT809_0x1502_PhotoRspFlag.完成拍照,
            Gnss = new JTGnssLocationBody { Time = new DateTime(2020, 6, 13, 13, 34, 47) },
            LensID = 2,
            SizeType = JT809_0x9502_SizeType._640x480,
            Type = JT809_0x9502_ImageType.jpg,
            Photo = [0xFF],
        }, JTVersion.JT2011);

        var expected = "01" + "000D0607E40D222F" + Zeros(4) + Zeros(4) + Zeros(2) + Zeros(2) + Zeros(4) + Zeros(2) + Zeros(2) + Zeros(4) + Zeros(4) + "02" + "00000001" + "02" + "01" + "FF";
        Assert.Equal(45, buf.Length);
        Assert.Equal(expected, buf.ToHex());
    }

    [Fact]
    [DisplayName("0x1504 行驶记录应答：2011长度+数据，2019源字段+命令字")]
    public void TestTravelAckGolden()
    {
        Assert.Equal("00000001" + "AA", WriteBody(new JT809_0x1500_0x1504 { TraveldataInfo = [0xAA] }, JTVersion.JT2011).ToHex());

        Assert.Equal("9504" + "00000003" + "08" + "00000001" + "AA", WriteBody(new JT809_0x1500_0x1504
        {
            SourceDataType = 0x9504,
            SourceMsgSn = 3,
            CommandType = JT809CommandType.采集记录仪行驶记录,
            TraveldataInfo = [0xAA],
        }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x1505 应急接入应答：2019源字段")]
    public void TestEmergencyAckGolden()
    {
        Assert.Equal("9505" + "00000002" + "01", WriteBody(new JT809_0x1500_0x1505
        {
            SourceDataType = 0x9505,
            SourceMsgSn = 2,
            Result = JT809_0x1505_Result.无该车辆,
        }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x9501 单向监听请求：20字节右补0")]
    public void TestMonitorReqGolden()
    {
        Assert.Equal("31" + Zeros(19), WriteAuto(new JT809_0x9500_0x9501 { MonitorTel = "1" }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x9502 拍照请求：镜头+大小两字节")]
    public void TestPhotoReqGolden()
    {
        Assert.Equal("0102", WriteAuto(new JT809_0x9500_0x9502
        {
            LensID = 1,
            SizeType = JT809_0x9502_SizeType._640x480,
        }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x9503 下发车辆报文请求：2019去掉消息序号")]
    public void TestTextReqGolden()
    {
        Assert.Equal("00000001" + "00" + "00000001" + "41", WriteBody(new JT809_0x9500_0x9503
        {
            MsgSequence = 1,
            MsgPriority = JT809_0x9503_MsgPriority.紧急,
            MsgContent = "A",
        }, JTVersion.JT2011).ToHex());

        Assert.Equal("00" + "00000001" + "41", WriteBody(new JT809_0x9500_0x9503
        {
            MsgPriority = JT809_0x9503_MsgPriority.紧急,
            MsgContent = "A",
        }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x9504 行驶记录请求：2019末尾追加命令字")]
    public void TestTravelReqGolden()
    {
        var start = 1263085674L.ToDateTime();
        var end = 1263089274L.ToDateTime();

        Assert.Equal("000000004B49286A" + "000000004B49367A", WriteBody(new JT809_0x9500_0x9504 { StartTime = start, EndTime = end }, JTVersion.JT2011).ToHex());
        Assert.Equal("000000004B49286A" + "000000004B49367A" + "02", WriteBody(new JT809_0x9500_0x9504
        {
            StartTime = start,
            EndTime = end,
            Command = JT809CommandType.记录仪时间,
        }, JTVersion.JT2019).ToHex());
    }

    [Fact]
    [DisplayName("0x9505 应急接入请求：BCD鉴权码+定长字段+UTC结束时间")]
    public void TestEmergencyReqGolden()
    {
        var buf = WriteBody(new JT809_0x9500_0x9505
        {
            AuthenticationCode = "12",
            AccessPointName = "APN",
            UserName = "U",
            Password = "P",
            ServerIP = "1.2.3.4",
            TcpPort = 2809,
            UdpPort = 2810,
            EndTime = 1263085674L.ToDateTime(),
        }, JTVersion.JT2011);

        Assert.Equal(145, buf.Length);
        Assert.Equal("12000000000000000000", buf[..10].ToHex());
        Assert.Equal("41504E" + Zeros(17), buf[10..30].ToHex());
        Assert.Equal("55" + Zeros(48), buf[30..79].ToHex());
        Assert.Equal("50" + Zeros(21), buf[79..101].ToHex());
        Assert.Equal("312E322E332E34" + Zeros(25), buf[101..133].ToHex());
        Assert.Equal("0AF9", buf[133..135].ToHex());
        Assert.Equal("0AFA", buf[135..137].ToHex());
        Assert.Equal("000000004B49286A", buf[137..].ToHex());
    }

    [Fact]
    [DisplayName("0x1602/0x9601 静态信息：原始路线与空请求")]
    public void TestStaticInfoGolden()
    {
        Assert.Equal("AABB", WriteBody(new JT809_0x1600_0x1602 { DRVLine = [0xAA, 0xBB] }, JTVersion.JT2019).ToHex());
        Assert.Empty(WriteAuto(new JT809_0x9600_0x9601(), JTVersion.JT2019));
    }
}
