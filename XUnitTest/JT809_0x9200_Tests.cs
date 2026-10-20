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

[DisplayName("从链路动态信息交换消息")]
public class JT809_0x9200_Tests
{
    private static Byte[] WriteBody(Object body, JTVersion version)
    {
        var ms = new MemoryStream();
        Assert.True(((IAccessor)body).Write(ms, new JTMessage { Version = version }));

        return ms.ToArray();
    }

    private static T ReadBody<T>(Byte[] data, JTVersion version) where T : IAccessor, new()
    {
        var rs = new T();
        Assert.True(rs.Read(new MemoryStream(data), new JTMessage { Version = version }));

        return rs;
    }

    private static T ReadAuto<T>(Byte[] data, JTVersion version) where T : new()
    {
        Object obj = new T();
        Assert.True(Helper.CreateReader(new MemoryStream(data), new JTMessage { Version = version }).TryRead(null, ref obj));

        return (T)obj;
    }

    private static Byte[] WriteAuto(Object body, JTVersion version)
    {
        var ms = new MemoryStream();
        Helper.CreateWriter(ms, new JTMessage { Version = version }).Write(body);

        return ms.ToArray();
    }

    private static DateTime Utc(Int64 seconds) => seconds.ToDateTime();

    [Fact]
    [DisplayName("0x9202 交换车辆定位信息按版本读写")]
    public void TestCarLocation()
    {
        // 2011：36字节定长定位数据体
        var body = new JT809_0x9200_0x9202
        {
            Time = new DateTime(2020, 6, 13, 13, 34, 47),
            Longitude = 117123456,
            Latitude = 31876543,
            Speed = 600,
            Speed2 = 580,
            Mileage = 123456,
            Direction = 180,
            Altitude = 1200,
        };
        var buf = WriteBody(body, JTVersion.JT2011);
        Assert.Equal(36, buf.Length);

        var rs = ReadBody<JT809_0x9200_0x9202>(buf, JTVersion.JT2011);
        Assert.Equal(body.Time, rs.Time);
        Assert.Equal(117123456, rs.Longitude);
        Assert.Equal(31876543, rs.Latitude);
        Assert.Equal((UInt16)600, rs.Speed);
        Assert.Equal((UInt16)580, rs.Speed2);
        Assert.Equal(123456u, rs.Mileage);
        Assert.Equal((UInt16)180, rs.Direction);
        Assert.Equal((UInt16)1200, rs.Altitude);

        // 2019：加密标识(1) + 定位数据长度(4) + 定位数据 + 三级平台编码与报警状态
        var body9 = new JT809_0x9200_0x9202
        {
            GnssData = [0x01, 0x02, 0x03, 0x04, 0x05],
            PlatformId1 = "34010000001",
            PlatformId2 = "34010000002",
            PlatformId3 = "34010000003",
        };
        var buf9 = WriteBody(body9, JTVersion.JT2019);
        Assert.Equal(1 + 4 + 5 + 3 * (11 + 4), buf9.Length);

        var rs9 = ReadBody<JT809_0x9200_0x9202>(buf9, JTVersion.JT2019);
        Assert.Equal(5, rs9.DataLength);
        Assert.Equal(body9.GnssData, rs9.GnssData);
        Assert.Equal("34010000001", rs9.PlatformId1);
        Assert.Equal("34010000003", rs9.PlatformId3);
    }

    [Fact]
    [DisplayName("0x9203 补发定位信息按版本读写")]
    public void TestHistoryAcrossArea()
    {
        // 2011：个数(1) + N×36字节
        var body = new JT809_0x9200_0x9203
        {
            GNSS =
            [
                new JT809_0x9200_0x9202 { Time = new DateTime(2020, 1, 1, 0, 0, 0), Longitude = 1, Latitude = 2 },
                new JT809_0x9200_0x9202 { Time = new DateTime(2020, 1, 1, 0, 1, 0), Longitude = 3, Latitude = 4 },
            ],
        };
        var buf = WriteBody(body, JTVersion.JT2011);
        Assert.Equal(1 + 2 * 36, buf.Length);

        var rs = ReadBody<JT809_0x9200_0x9203>(buf, JTVersion.JT2011);
        Assert.Equal((Byte)2, rs.Count);
        Assert.Equal(2, rs.GNSS.Count);
        Assert.Equal(1, rs.GNSS[0].Longitude);
        Assert.Equal(4, rs.GNSS[1].Latitude);

        // 2019：个数(1) + N×(加密标识+长度+定位数据+平台编码与报警状态)
        var body9 = new JT809_0x9200_0x9203
        {
            GNSS =
            [
                new JT809_0x9200_0x9202 { GnssData = [0x10, 0x11], PlatformId1 = "34010000001" },
            ],
        };
        var buf9 = WriteBody(body9, JTVersion.JT2019);
        Assert.Equal(1 + (1 + 4 + 2 + 3 * (11 + 4)), buf9.Length);

        var rs9 = ReadBody<JT809_0x9200_0x9203>(buf9, JTVersion.JT2019);
        Assert.Equal((Byte)1, rs9.Count);
        Assert.Equal(body9.GNSS[0].GnssData, rs9.GNSS[0].GnssData);

        // 超过5条时应提示拆包
        var many = new JT809_0x9200_0x9203 { GNSS = [] };
        for (var i = 0; i < 6; i++) many.GNSS.Add(new JT809_0x9200_0x9202());
        Assert.Throws<ArgumentOutOfRangeException>(() => WriteBody(many, JTVersion.JT2019));
    }

    [Fact]
    [DisplayName("0x9204 交换车辆静态信息不带内层长度")]
    public void TestCarInfo()
    {
        var content = "VIN:=京CJ6789;PLATE_COLOR:=2";

        var buf = WriteBody(new JT809_0x9200_0x9204 { CarInfo = content }, JTVersion.JT2011);
        Assert.Equal(Helper.Encoding.GetByteCount(content), buf.Length);

        var rs = ReadBody<JT809_0x9200_0x9204>(buf, JTVersion.JT2011);
        Assert.Equal(content, rs.CarInfo);

        // 2019结构一致
        var rs9 = ReadBody<JT809_0x9200_0x9204>(WriteBody(new JT809_0x9200_0x9204 { CarInfo = content }, JTVersion.JT2019), JTVersion.JT2019);
        Assert.Equal(content, rs9.CarInfo);
    }

    [Fact]
    [DisplayName("0x9207/0x9208/0x9209 应答消息按版本区分源字段")]
    public void TestExchangeAcks()
    {
        // 2011只有结果，2019增加源子业务类型与源报文序列号
        var buf7 = WriteBody(new JT809_0x9200_0x9207 { Result = JT809_0x9207_Result.申请成功 }, JTVersion.JT2011);
        Assert.Equal(1, buf7.Length);

        var buf79 = WriteBody(new JT809_0x9200_0x9207 { SourceDataType = 0x1207, SourceMsgSn = 9, Result = JT809_0x9207_Result.其它2019 }, JTVersion.JT2019);
        Assert.Equal(7, buf79.Length);

        var rs79 = ReadBody<JT809_0x9200_0x9207>(buf79, JTVersion.JT2019);
        Assert.Equal((UInt16)0x1207, rs79.SourceDataType);
        Assert.Equal(9u, rs79.SourceMsgSn);
        Assert.Equal(JT809_0x9207_Result.其它2019, rs79.Result);

        Assert.Equal(1, WriteBody(new JT809_0x9200_0x9208 { Result = JT809_0x9208_Result.取消申请成功 }, JTVersion.JT2011).Length);
        Assert.Equal(7, WriteBody(new JT809_0x9200_0x9208 { SourceDataType = 0x1208, SourceMsgSn = 1 }, JTVersion.JT2019).Length);

        Assert.Equal(1, WriteBody(new JT809_0x9200_0x9209 { Result = JT809_0x9209_Result.成功_上级平台即刻补发 }, JTVersion.JT2011).Length);
        Assert.Equal(7, WriteBody(new JT809_0x9200_0x9209 { SourceDataType = 0x1209, SourceMsgSn = 2 }, JTVersion.JT2019).Length);
    }

    [Fact]
    [DisplayName("0x920A 驾驶员身份请求2019才带上传标志")]
    public void TestDriverInfoReq()
    {
        Assert.Empty(WriteBody(new JT809_0x9200_0x920A(), JTVersion.JT2011));

        var buf = WriteBody(new JT809_0x9200_0x920A { Flag = JT809_0x920A_UploadFlag.从终端获取 }, JTVersion.JT2019);
        Assert.Equal(1, buf.Length);
        Assert.Equal(JT809_0x920A_UploadFlag.从终端获取, ReadBody<JT809_0x9200_0x920A>(buf, JTVersion.JT2019).Flag);
    }

    [Fact]
    [DisplayName("0x920D 车辆行驶线路应答自动计算原因长度")]
    public void TestDrvLineAck()
    {
        var body = new JT809_0x9200_0x920D
        {
            DRVLineID = 0x1234,
            Result = JT809_0x920D_Result.审核未通过_未完成记录,
            Reason = "线路不存在",
        };
        var buf = WriteBody(body, JTVersion.JT2019);
        Assert.Equal(4 + 1 + 2 + Helper.Encoding.GetByteCount("线路不存在"), buf.Length);

        var rs = ReadBody<JT809_0x9200_0x920D>(buf, JTVersion.JT2019);
        Assert.Equal(0x1234u, rs.DRVLineID);
        Assert.Equal(JT809_0x920D_Result.审核未通过_未完成记录, rs.Result);
        Assert.Equal("线路不存在", rs.Reason);

        // 空原因时长度为零
        var empty = WriteBody(new JT809_0x9200_0x920D(), JTVersion.JT2019);
        Assert.Equal(7, empty.Length);
    }

    [Fact]
    [DisplayName("0x9201 车辆注册应答两版结构一致")]
    public void TestRegisterAck()
    {
        var body = new JT809_0x9200_0x9201 { DataType = 0x1201, MsgSn = 3, Result = JT809_0x9201_Result.审核通过_完成注册 };

        var buf = WriteAuto(body, JTVersion.JT2011);
        Assert.Equal(7, buf.Length);

        var rs = ReadAuto<JT809_0x9200_0x9201>(buf, JTVersion.JT2011);
        Assert.Equal((UInt16)0x1201, rs.DataType);
        Assert.Equal(3u, rs.MsgSn);
        Assert.Equal(JT809_0x9201_Result.审核通过_完成注册, rs.Result);
    }

    [Fact]
    [DisplayName("UTC时间字段按 Unix 秒写出")]
    public void TestUtcTime()
    {
        var body = new JT809_0x9200_0x9202 { Time = Utc(1263085674) };
        var buf = WriteBody(body, JTVersion.JT2011);

        // 定长时间自带时区无关性，跨平台一致
        var rs = ReadBody<JT809_0x9200_0x9202>(buf, JTVersion.JT2011);
        Assert.Equal(Utc(1263085674), rs.Time);
    }
}
