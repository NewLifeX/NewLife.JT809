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

[DisplayName("车辆监管消息")]
public class JT809_0x1500_Tests
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

    [Fact]
    [DisplayName("0x9502 拍照请求为镜头与大小两字节")]
    public void TestPhotoReq()
    {
        var buf = WriteAuto(new JT809_0x9500_0x9502 { LensID = 2, SizeType = JT809_0x9502_SizeType._1024x768 }, JTVersion.JT2019);
        Assert.Equal(2, buf.Length);

        var rs = ReadAuto<JT809_0x9500_0x9502>(buf, JTVersion.JT2019);
        Assert.Equal((Byte)2, rs.LensID);
        Assert.Equal(JT809_0x9502_SizeType._1024x768, rs.SizeType);
    }

    [Fact]
    [DisplayName("0x1501 单向监听应答按版本区分")]
    public void TestMonitorAck()
    {
        Assert.Equal(1, WriteBody(new JT809_0x1500_0x1501 { Result = JT809_0x1501_Result.监听成功 }, JTVersion.JT2011).Length);

        var buf9 = WriteBody(new JT809_0x1500_0x1501 { SourceDataType = 0x9501, SourceMsgSn = 3, Result = JT809_0x1501_Result.监听失败 }, JTVersion.JT2019);
        Assert.Equal(7, buf9.Length);

        var rs9 = ReadBody<JT809_0x1500_0x1501>(buf9, JTVersion.JT2019);
        Assert.Equal((UInt16)0x9501, rs9.SourceDataType);
        Assert.Equal(3u, rs9.SourceMsgSn);
        Assert.Equal(JT809_0x1501_Result.监听失败, rs9.Result);
    }

    [Fact]
    [DisplayName("0x1502 拍照应答按版本读写")]
    public void TestPhotoAck()
    {
        var photo = new Byte[] { 0xFF, 0xD8, 0xFF, 0xE0 };

        // 2011：应答标识(1) + 拍照位置36字节 + 镜头(1) + 图片长度(4) + 大小(1) + 格式(1) + 图片
        var body = new JT809_0x1500_0x1502
        {
            PhotoRspFlag = JT809_0x1502_PhotoRspFlag.完成拍照,
            Gnss = new JTGnssLocationBody { Longitude = 117123456, Latitude = 31876543 },
            LensID = 1,
            SizeType = JT809_0x9502_SizeType._640x480,
            Type = JT809_0x9502_ImageType.jpg,
            Photo = photo,
        };
        var buf = WriteBody(body, JTVersion.JT2011);
        Assert.Equal(1 + 36 + 1 + 4 + 1 + 1 + photo.Length, buf.Length);

        var rs = ReadBody<JT809_0x1500_0x1502>(buf, JTVersion.JT2011);
        Assert.Equal(JT809_0x1502_PhotoRspFlag.完成拍照, rs.PhotoRspFlag);
        Assert.Equal(117123456, rs.Gnss.Longitude);
        Assert.Equal(31876543, rs.Gnss.Latitude);
        Assert.Equal((Byte)1, rs.LensID);
        Assert.Equal(JT809_0x9502_SizeType._640x480, rs.SizeType);
        Assert.Equal(JT809_0x9502_ImageType.jpg, rs.Type);
        Assert.Equal(photo, rs.Photo);

        // 2019：源子业务(2) + 源序列号(4) + 应答标识(1) + 定位数据体(表90) + 镜头/长度/大小/格式 + 图片
        var body9 = new JT809_0x1500_0x1502
        {
            SourceDataType = 0x9502,
            SourceMsgSn = 6,
            PhotoRspFlag = JT809_0x1502_PhotoRspFlag.完成拍照,
            Gnss = new JTGnssLocationBody { GnssData = [0x01, 0x02, 0x03], PlatformId1 = "34010000001" },
            LensID = 2,
            SizeType = JT809_0x9502_SizeType._800x600,
            Type = JT809_0x9502_ImageType.png,
            Photo = photo,
        };
        var gnssLength = 1 + 4 + 3 + 3 * (11 + 4);
        var buf9 = WriteBody(body9, JTVersion.JT2019);
        Assert.Equal(6 + 1 + gnssLength + 1 + 4 + 1 + 1 + photo.Length, buf9.Length);

        var rs9 = ReadBody<JT809_0x1500_0x1502>(buf9, JTVersion.JT2019);
        Assert.Equal((UInt16)0x9502, rs9.SourceDataType);
        Assert.Equal(6u, rs9.SourceMsgSn);
        Assert.Equal(body9.Gnss.GnssData, rs9.Gnss.GnssData);
        Assert.Equal("34010000001", rs9.Gnss.PlatformId1);
        Assert.Equal(photo, rs9.Photo);
    }

    [Fact]
    [DisplayName("0x1503 下发报文应答按版本区分")]
    public void TestTextInfoAck()
    {
        var buf = WriteBody(new JT809_0x1500_0x1503 { MsgID = 12, Result = JT809_0x1503_Result.下发成功 }, JTVersion.JT2011);
        Assert.Equal(5, buf.Length);

        var rs = ReadBody<JT809_0x1500_0x1503>(buf, JTVersion.JT2011);
        Assert.Equal(12u, rs.MsgID);
        Assert.Equal(JT809_0x1503_Result.下发成功, rs.Result);

        var buf9 = WriteBody(new JT809_0x1500_0x1503 { SourceDataType = 0x9503, SourceMsgSn = 13, Result = JT809_0x1503_Result.下发失败 }, JTVersion.JT2019);
        Assert.Equal(7, buf9.Length);

        var rs9 = ReadBody<JT809_0x1500_0x1503>(buf9, JTVersion.JT2019);
        Assert.Equal((UInt16)0x9503, rs9.SourceDataType);
        Assert.Equal(13u, rs9.SourceMsgSn);
        Assert.Equal(JT809_0x1503_Result.下发失败, rs9.Result);
    }

    [Fact]
    [DisplayName("0x1504 行驶记录应答按版本读写")]
    public void TestTravelAck()
    {
        var info = new Byte[] { 0x01, 0x02, 0x03 };

        // 2011：数据长度(4) + 数据
        var buf = WriteBody(new JT809_0x1500_0x1504 { TraveldataInfo = info }, JTVersion.JT2011);
        Assert.Equal(4 + info.Length, buf.Length);
        Assert.Equal(info, ReadBody<JT809_0x1500_0x1504>(buf, JTVersion.JT2011).TraveldataInfo);

        // 2019：源子业务(2) + 源序列号(4) + 命令字(1) + 数据长度(4) + 数据
        var buf9 = WriteBody(new JT809_0x1500_0x1504
        {
            SourceDataType = 0x9504,
            SourceMsgSn = 21,
            CommandType = JT809CommandType.采集记录仪行驶记录,
            TraveldataInfo = info,
        }, JTVersion.JT2019);
        Assert.Equal(6 + 1 + 4 + info.Length, buf9.Length);

        var rs9 = ReadBody<JT809_0x1500_0x1504>(buf9, JTVersion.JT2019);
        Assert.Equal((UInt16)0x9504, rs9.SourceDataType);
        Assert.Equal(JT809CommandType.采集记录仪行驶记录, rs9.CommandType);
        Assert.Equal(info, rs9.TraveldataInfo);
    }

    [Fact]
    [DisplayName("0x1505 应急接入应答按版本区分")]
    public void TestEmergencyAck()
    {
        Assert.Equal(1, WriteBody(new JT809_0x1500_0x1505 { Result = JT809_0x1505_Result.无该车辆 }, JTVersion.JT2011).Length);

        var buf9 = WriteBody(new JT809_0x1500_0x1505 { SourceDataType = 0x9505, SourceMsgSn = 2, Result = JT809_0x1505_Result.其它原因失败 }, JTVersion.JT2019);
        Assert.Equal(7, buf9.Length);

        var rs9 = ReadBody<JT809_0x1500_0x1505>(buf9, JTVersion.JT2019);
        Assert.Equal((UInt16)0x9505, rs9.SourceDataType);
        Assert.Equal(JT809_0x1505_Result.其它原因失败, rs9.Result);
    }

    [Fact]
    [DisplayName("0x9501 单向监听请求定长20字节")]
    public void TestMonitorReq()
    {
        var buf = WriteAuto(new JT809_0x9500_0x9501 { MonitorTel = "13800138000" }, JTVersion.JT2019);
        Assert.Equal(20, buf.Length);
        Assert.Equal("13800138000", ReadAuto<JT809_0x9500_0x9501>(buf, JTVersion.JT2019).MonitorTel);
    }

    [Fact]
    [DisplayName("0x9503 下发车辆报文请求按版本区分")]
    public void TestTextReq()
    {
        var content = "请靠边停车";

        // 2011：序号(4) + 优先级(1) + 长度(4) + 内容
        var body = new JT809_0x9500_0x9503 { MsgSequence = 9, MsgPriority = JT809_0x9503_MsgPriority.紧急, MsgContent = content };
        var buf = WriteBody(body, JTVersion.JT2011);
        Assert.Equal(4 + 1 + 4 + Helper.Encoding.GetByteCount(content), buf.Length);

        var rs = ReadBody<JT809_0x9500_0x9503>(buf, JTVersion.JT2011);
        Assert.Equal(9u, rs.MsgSequence);
        Assert.Equal(JT809_0x9503_MsgPriority.紧急, rs.MsgPriority);
        Assert.Equal(content, rs.MsgContent);

        // 2019：优先级(1) + 长度(4) + 内容
        var body9 = new JT809_0x9500_0x9503 { MsgPriority = JT809_0x9503_MsgPriority.一般, MsgContent = content };
        var buf9 = WriteBody(body9, JTVersion.JT2019);
        Assert.Equal(1 + 4 + Helper.Encoding.GetByteCount(content), buf9.Length);

        var rs9 = ReadBody<JT809_0x9500_0x9503>(buf9, JTVersion.JT2019);
        Assert.Equal(JT809_0x9503_MsgPriority.一般, rs9.MsgPriority);
        Assert.Equal(content, rs9.MsgContent);
    }

    [Fact]
    [DisplayName("0x9504 行驶记录请求两版结构")]
    public void TestTravelReq()
    {
        var start = 1263085674L.ToDateTime();
        var end = 1263089274L.ToDateTime();

        // 2011：开始(8) + 结束(8)
        var buf = WriteBody(new JT809_0x9500_0x9504 { StartTime = start, EndTime = end }, JTVersion.JT2011);
        Assert.Equal(16, buf.Length);

        var rs = ReadBody<JT809_0x9500_0x9504>(buf, JTVersion.JT2011);
        Assert.Equal(start, rs.StartTime);
        Assert.Equal(end, rs.EndTime);

        // 2019：开始(8) + 结束(8) + 命令字(1)
        var buf9 = WriteBody(new JT809_0x9500_0x9504 { StartTime = start, EndTime = end, Command = JT809CommandType.记录仪时间 }, JTVersion.JT2019);
        Assert.Equal(17, buf9.Length);

        var rs9 = ReadBody<JT809_0x9500_0x9504>(buf9, JTVersion.JT2019);
        Assert.Equal(start, rs9.StartTime);
        Assert.Equal(JT809CommandType.记录仪时间, rs9.Command);
    }

    [Fact]
    [DisplayName("0x9505 应急接入请求结束时间为UTC秒")]
    public void TestEmergencyReq()
    {
        var time = 1263085674L.ToDateTime();
        var body = new JT809_0x9500_0x9505
        {
            AuthenticationCode = "12345678901234567890",
            AccessPointName = "APN",
            UserName = "user",
            Password = "pwd",
            ServerIP = "127.0.0.1",
            TcpPort = 2809,
            UdpPort = 2810,
            EndTime = time,
        };
        var buf = WriteBody(body, JTVersion.JT2011);

        // 10 + 20 + 49 + 22 + 32 + 2 + 2 + 8
        Assert.Equal(145, buf.Length);

        var rs = ReadBody<JT809_0x9500_0x9505>(buf, JTVersion.JT2011);
        Assert.Equal("12345678901234567890", rs.AuthenticationCode);
        Assert.Equal("APN", rs.AccessPointName);
        Assert.Equal("user", rs.UserName);
        Assert.Equal("pwd", rs.Password);
        Assert.Equal("127.0.0.1", rs.ServerIP);
        Assert.Equal((UInt16)2809, rs.TcpPort);
        Assert.Equal((UInt16)2810, rs.UdpPort);
        Assert.Equal(time, rs.EndTime);
    }
}
