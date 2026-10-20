using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using NewLife;
using NewLife.Data;
using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;
using Xunit;

namespace XUnitTest;

[DisplayName("0x1200 车辆动态信息交换")]
public class JT809_0x1200_Tests
{
    private readonly MessageFactory _factory;
    public JT809_0x1200_Tests()
    {
        _factory = MessageFactory.Instance;
        _factory.Init();
    }

    /// <summary>2019帧头 + 0x1205 子业务，校验长度字段、CRC 与子消息解析</summary>
    [Fact]
    [DisplayName("解析2019版0x1200并读取0x1205子业务")]
    public void TestJT20192()
    {
        // 由消息头 + 车牌(21) + 颜色(1) + 子业务类型(2) + 子业务长度(4) + 子业务体(10) 组成，
        // 长度字段与 CRC 均按实际内容重新计算；时间字段含 0x5D，写入时被转义为 5E01
        var str =
            "5B" +
            "00000048" +
            "00000004" +
            "1200" +
            "03938706" +
            "010000" +
            "00" +
            "00989681" +
            "000001815E014624D8" +

            "BBA641413532303030000000000000000000000000" +
            "01" +
            "1205" +
            "0000000A" +
            "0001" +
            "00000000" +
            "00000000" +

            "660B" +
            "5D";
        var dt = str.ToHex();

        var msg = new JTMessage { Version = JTVersion.JT2019 };
        var rs = msg.Read(dt);

        Assert.True(rs);
        Assert.Equal(4u, msg.Sequence);
        Assert.Equal(MessageKinds.主链路车辆动态信息交换业务, msg.Kind);
        Assert.Equal(60000006u, msg.CenterId);
        Assert.Equal(0x010000u, msg.VersionFlag);
        Assert.Equal(0u, msg.EncryptFlag);
        Assert.Equal(10000001u, msg.EncryptKey);
        Assert.Equal("2022-06-13T13:34:47".ToDateTime(), msg.Time);

        Assert.Equal(JTVersion.JT2019, msg.Version);

        var body = msg.ReadModel<JT809_0x1200>();
        Assert.NotNull(body);
        Assert.Equal("沪AA52000", body.VehicleNo);
        Assert.Equal(1, (Int32)body.VehicleColor);
        Assert.Equal(0x1205, (Int32)body.DataType);

        var sub = Assert.IsType<JT809_0x1200_0x1205>(body.Body);
        Assert.Equal(1, (Int32)sub.SourceDataType);
        Assert.Equal(0u, sub.SourceMsgSN);
        Assert.Equal(0u, sub.DataLength);

        // 重新写入后除帧头UTC时间精度（规范为Unix秒）外，其余字段应与原始报文一致
        var pk = msg.ToPacket();
        var msg2 = new JTMessage { Version = JTVersion.JT2019 };
        Assert.True(msg2.Read(pk.ToArray()));
        Assert.Equal(msg.Sequence, msg2.Sequence);
        Assert.Equal(msg.Kind, msg2.Kind);
        Assert.Equal(msg.CenterId, msg2.CenterId);
        Assert.Equal(msg.VersionFlag, msg2.VersionFlag);
        Assert.Equal(msg.EncryptKey, msg2.EncryptKey);
        Assert.Equal(msg.Payload.ToHex(), msg2.Payload.ToHex());

        var body2 = msg2.ReadModel<JT809_0x1200>();
        Assert.Equal("沪AA52000", body2.VehicleNo);
        Assert.Equal(0x1205, (Int32)body2.DataType);
    }

    /// <summary>0x1202 实时定位子消息的读写往返</summary>
    [Fact]
    [DisplayName("0x1202实时定位子消息读写往返")]
    public void TestLocationBody()
    {
        var body = new JT809_0x1200_0x1202
        {
            Encrypt = JT809_VehiclePositionEncrypt.未加密,
            Time = new DateTime(2024, 5, 6, 7, 8, 9),
            Longitude = 117123456,
            Latitude = 31876543,
            Speed = 60,
            Speed2 = 58,
            Mileage = 123456,
            Direction = 180,
            Altitude = 50,
            Status = (LocationStatus)0x00000003,
            Alarm = (LocationAlarms)0x00000004,
        };
        var model = new JT809_0x1200
        {
            VehicleNo = "皖AT0011",
            VehicleColor = JT809VehicleColorType.黄色,
            Body = body,
        };

        var msg = new JTMessage
        {
            Sequence = 1,
            Kind = MessageKinds.主链路车辆动态信息交换业务,
            CenterId = 123456,
        };
        Assert.True(msg.WriteModel(model));

        var read = msg.ReadModel<JT809_0x1200>();
        Assert.Equal("皖AT0011", read.VehicleNo);
        Assert.Equal(JT809VehicleColorType.黄色, read.VehicleColor);
        Assert.Equal(0x1202, (Int32)read.DataType);

        var rs = Assert.IsType<JT809_0x1200_0x1202>(read.Body);
        Assert.Equal(JT809_VehiclePositionEncrypt.未加密, rs.Encrypt);
        Assert.Equal(body.Time, rs.Time);
        Assert.Equal(body.Longitude, rs.Longitude);
        Assert.Equal(body.Latitude, rs.Latitude);
        Assert.Equal(body.Speed, rs.Speed);
        Assert.Equal(body.Speed2, rs.Speed2);
        Assert.Equal(body.Mileage, rs.Mileage);
        Assert.Equal(body.Direction, rs.Direction);
        Assert.Equal(body.Altitude, rs.Altitude);
        Assert.Equal(body.Status, rs.Status);
        Assert.Equal(body.Alarm, rs.Alarm);
    }

    /// <summary>809-2019 表90：加密标识 + 数据长度 + 定位数据(808-2019 8.12) + 三级平台编码与报警状态</summary>
    [Fact]
    [DisplayName("0x1202 2019版定位信息数据体读写往返")]
    public void TestLocationBody2019()
    {
        var location = new LocationMessage
        {
            Alarm = (LocationAlarms)0x00000004,
            Status = (LocationStatus)0x00000003,
            Latitude = 31876543,
            Longitude = 117123456,
            Altitude = 50,
            Speed = 600,
            Direction = 180,
            GPSTime = new DateTime(2024, 5, 6, 7, 8, 9),
        };
        var body = new JT809_0x1200_0x1202
        {
            Encrypt = JT809_VehiclePositionEncrypt.未加密,
            Location = location,
            PlatformId1 = "34010000001",
            Alarm1 = (LocationAlarms)0x01,
            PlatformId2 = "34010000002",
            Alarm2 = (LocationAlarms)0x02,
            PlatformId3 = "34010000003",
            Alarm3 = (LocationAlarms)0x04,
        };

        var header = new JTMessage { Version = JTVersion.JT2019 };

        var ms = new MemoryStream();
        Assert.True(body.Write(ms, header));

        // 加密标识(1) + 数据长度(4) + 定位数据(28) + 三级平台编码与报警状态(3*(11+4))
        Assert.Equal(78L, ms.Length);
        Assert.Equal(28, body.DataLength);

        // 2019版数据体不再携带2011版的定长定位字段
        Assert.Equal(DateTime.MinValue, body.Time);

        ms.Position = 0;
        var rs = new JT809_0x1200_0x1202();
        Assert.True(rs.Read(ms, header));

        Assert.Equal(JT809_VehiclePositionEncrypt.未加密, rs.Encrypt);
        Assert.Equal(28, rs.DataLength);

        var loc = Assert.IsType<LocationMessage>(rs.Location);
        Assert.Equal(location.Alarm, loc.Alarm);
        Assert.Equal(location.Status, loc.Status);
        Assert.Equal(location.Latitude, loc.Latitude);
        Assert.Equal(location.Longitude, loc.Longitude);
        Assert.Equal(location.Altitude, loc.Altitude);
        Assert.Equal(location.Speed, loc.Speed);
        Assert.Equal(location.Direction, loc.Direction);
        Assert.Equal(location.GPSTime, loc.GPSTime);

        Assert.Equal("34010000001", rs.PlatformId1);
        Assert.Equal((LocationAlarms)0x01, rs.Alarm1);
        Assert.Equal("34010000002", rs.PlatformId2);
        Assert.Equal((LocationAlarms)0x02, rs.Alarm2);
        Assert.Equal("34010000003", rs.PlatformId3);
        Assert.Equal((LocationAlarms)0x04, rs.Alarm3);
    }

    /// <summary>0x1202 2019版经0x1200外层封装后的完整往返</summary>
    [Fact]
    [DisplayName("0x1202 2019版经0x1200封装往返")]
    public void TestLocationBody2019InEnvelope()
    {
        var body = new JT809_0x1200_0x1202
        {
            Encrypt = JT809_VehiclePositionEncrypt.未加密,
            Location = new LocationMessage
            {
                Latitude = 31876543,
                Longitude = 117123456,
                Speed = 600,
                GPSTime = new DateTime(2024, 5, 6, 7, 8, 9),
            },
            PlatformId1 = "34010000001",
            Alarm1 = (LocationAlarms)0x01,
        };
        var model = new JT809_0x1200
        {
            VehicleNo = "皖AT0011",
            VehicleColor = JT809VehicleColorType.黄色,
            Body = body,
        };

        var msg = new JTMessage
        {
            Sequence = 3,
            Kind = MessageKinds.主链路车辆动态信息交换业务,
            CenterId = 123456,
            Version = JTVersion.JT2019,
            VersionFlag = 0x010000,
        };
        Assert.True(msg.WriteModel(model));

        var read = msg.ReadModel<JT809_0x1200>();
        Assert.Equal("皖AT0011", read.VehicleNo);
        Assert.Equal(0x1202, (Int32)read.DataType);

        var rs = Assert.IsType<JT809_0x1200_0x1202>(read.Body);
        Assert.Equal(28, rs.DataLength);
        Assert.Equal(31876543, rs.Location.Latitude);
        Assert.Equal(117123456, rs.Location.Longitude);
        Assert.Equal("34010000001", rs.PlatformId1);
        Assert.Equal((LocationAlarms)0x01, rs.Alarm1);
        Assert.Equal("", rs.PlatformId2 + "");
        Assert.Equal((LocationAlarms)0, rs.Alarm2);
    }

    /// <summary>0x1203 自动补报：逐个定位数据体顺序排列，2011/2019 两种长度</summary>
    [Fact]
    [DisplayName("0x1203 自动补报定位数据读写往返")]
    public void TestHistoryLocation()
    {
        var items = new List<JT809_0x1200_0x1202>
        {
            new()
            {
                Encrypt = JT809_VehiclePositionEncrypt.未加密,
                Time = new DateTime(2024, 5, 6, 7, 8, 9),
                Longitude = 117123456,
                Latitude = 31876543,
                Speed = 60,
            },
            new()
            {
                Encrypt = JT809_VehiclePositionEncrypt.未加密,
                Time = new DateTime(2024, 5, 6, 7, 8, 59),
                Longitude = 117123457,
                Latitude = 31876544,
                Speed = 61,
            },
        };
        var body = new JT809_0x1200_0x1203 { GNSS = items };

        // 2011版：个数(1) + 2 * 定长定位数据体(36)
        var header2011 = new JTMessage { Version = JTVersion.JT2011 };
        var ms = new MemoryStream();
        Assert.True(body.Write(ms, header2011));
        Assert.Equal(73L, ms.Length);

        ms.Position = 0;
        var rs = new JT809_0x1200_0x1203();
        Assert.True(rs.Read(ms, header2011));
        Assert.Equal((Byte)2, rs.Count);
        Assert.Equal(items[0].Time, rs.GNSS[0].Time);
        Assert.Equal(items[0].Longitude, rs.GNSS[0].Longitude);
        Assert.Equal(items[1].Latitude, rs.GNSS[1].Latitude);
        Assert.Equal(items[1].Speed, rs.GNSS[1].Speed);

        // 2019版：个数(1) + 2 * 自描述长度定位数据体(78)
        var items2019 = new List<JT809_0x1200_0x1202>
        {
            new()
            {
                Encrypt = JT809_VehiclePositionEncrypt.未加密,
                Location = new LocationMessage { Longitude = 117123456, Latitude = 31876543, Speed = 600 },
                PlatformId1 = "34010000001",
                Alarm1 = (LocationAlarms)0x01,
            },
            new()
            {
                Encrypt = JT809_VehiclePositionEncrypt.未加密,
                Location = new LocationMessage { Longitude = 117123457, Latitude = 31876544, Speed = 610 },
            },
        };
        var body2019 = new JT809_0x1200_0x1203 { GNSS = items2019 };

        var header2019 = new JTMessage { Version = JTVersion.JT2019 };
        var ms2 = new MemoryStream();
        Assert.True(body2019.Write(ms2, header2019));
        Assert.Equal(157L, ms2.Length);

        ms2.Position = 0;
        var rs2 = new JT809_0x1200_0x1203();
        Assert.True(rs2.Read(ms2, header2019));
        Assert.Equal((Byte)2, rs2.Count);
        Assert.Equal(117123456, rs2.GNSS[0].Location.Longitude);
        Assert.Equal("34010000001", rs2.GNSS[0].PlatformId1);
        Assert.Equal((LocationAlarms)0x01, rs2.GNSS[0].Alarm1);
        Assert.Equal(117123457, rs2.GNSS[1].Location.Longitude);
    }

    /// <summary>规范要求单条消息最多携带5条定位记录</summary>
    [Fact]
    [DisplayName("0x1203 超过5条定位记录时拒绝写入")]
    public void TestHistoryLocationTooMany()
    {
        var items = new List<JT809_0x1200_0x1202>();
        for (var i = 0; i < 6; i++) items.Add(new JT809_0x1200_0x1202());

        var body = new JT809_0x1200_0x1203 { GNSS = items };
        Assert.Throws<ArgumentOutOfRangeException>(() => body.Write(new MemoryStream(), new JTMessage { Version = JTVersion.JT2019 }));
    }

    /// <summary>加密的定位数据不解析，原样保留密文</summary>
    [Fact]
    [DisplayName("0x1202 2019版加密定位数据保留原始字节")]
    public void TestLocationBody2019Encrypted()
    {
        var gnss = new Byte[] { 0x01, 0x02, 0x5B, 0x5A, 0x5D, 0x5E };

        var body = new JT809_0x1200_0x1202
        {
            Encrypt = JT809_VehiclePositionEncrypt.已加密,
            GnssData = gnss,
        };

        var header = new JTMessage { Version = JTVersion.JT2019 };

        var ms = new MemoryStream();
        Assert.True(body.Write(ms, header));

        // 加密标识(1) + 数据长度(4) + 密文(6) + 三级平台编码与报警状态(45)
        Assert.Equal(56L, ms.Length);

        ms.Position = 0;
        var rs = new JT809_0x1200_0x1202();
        Assert.True(rs.Read(ms, header));

        Assert.Equal(JT809_VehiclePositionEncrypt.已加密, rs.Encrypt);
        Assert.Equal(gnss, rs.GnssData);
        Assert.Null(rs.Location);
    }

    /// <summary>部分平台不携带三级平台编码，长度不足时应跳过而不是解析失败</summary>
    [Fact]
    [DisplayName("0x1202 2019版缺少平台编码时仍可解析")]
    public void TestLocationBody2019NoPlatforms()
    {
        // 加密标识(0) + 数据长度(4) + 4字节定位数据，不含三级平台编码
        var data = new Byte[] { 0x00, 0x00, 0x00, 0x00, 0x04, 0x01, 0x02, 0x03, 0x04 };

        var header = new JTMessage { Version = JTVersion.JT2019 };
        var ms = new MemoryStream(data);
        var rs = new JT809_0x1200_0x1202();
        Assert.True(rs.Read(ms, header));

        Assert.Equal(JT809_VehiclePositionEncrypt.未加密, rs.Encrypt);
        Assert.Equal(4, rs.DataLength);
        Assert.Null(rs.PlatformId1);
        Assert.Equal((LocationAlarms)0, rs.Alarm1);
    }
}
