using System;
using System.ComponentModel;
using NewLife;
using NewLife.Data;
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

        // 重新写入后帧内容应与原始报文一致
        var pk = msg.ToPacket();
        Assert.Equal(dt.ToHex(), pk.ToHex(256));
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
}
