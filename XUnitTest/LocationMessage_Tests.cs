using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using NewLife.JT809;
using NewLife.JT809.Models;
using Xunit;

namespace XUnitTest;

[DisplayName("位置信息模型")]
public class LocationMessage_Tests
{
    [Fact]
    [DisplayName("位置信息读写往返，附加信息按类型还原")]
    public void Location_Roundtrip()
    {
        var loc = new LocationMessage
        {
            Alarm = (LocationAlarms)0,
            Status = 0,
            Latitude = 31876543,
            Longitude = 117123456,
            Altitude = 50,
            Speed = 600,
            Direction = 180,
            GPSTime = new DateTime(2024, 5, 6, 7, 8, 9),
            Additionals = new Dictionary<AdditionalKinds, Object>
            {
                [AdditionalKinds.卫星数] = (Byte)12,
                [AdditionalKinds.信号强度] = (UInt16)25,
                [AdditionalKinds.里程] = (UInt32)123456,
            },
        };

        var ms = new MemoryStream();
        Assert.True(loc.Write(ms, null));

        ms.Position = 0;
        var rs = new LocationMessage();
        Assert.True(rs.Read(ms, null));

        Assert.Equal(loc.Alarm, rs.Alarm);
        Assert.Equal(loc.Status, rs.Status);
        Assert.Equal(loc.Latitude, rs.Latitude);
        Assert.Equal(loc.Longitude, rs.Longitude);
        Assert.Equal(loc.Altitude, rs.Altitude);
        Assert.Equal(loc.Speed, rs.Speed);
        Assert.Equal(loc.Direction, rs.Direction);
        Assert.Equal(loc.GPSTime, rs.GPSTime);

        Assert.Equal(3, rs.Additionals.Count);
        Assert.Equal((Byte)12, rs.Additionals[AdditionalKinds.卫星数]);
        Assert.Equal((UInt16)25, rs.Additionals[AdditionalKinds.信号强度]);
        Assert.Equal(123456u, rs.Additionals[AdditionalKinds.里程]);
    }

    [Fact]
    [DisplayName("附加信息被截断时不抛异常并保留已解析项")]
    public void Additionals_Truncated()
    {
        // 卫星数(1字节) + 下一项声明5字节但只给2字节
        var ms = new MemoryStream(new Byte[] { 0x07, 0x01, 0x0C, 0x0A, 0x05, 0x01, 0x02 });

        var reader = Helper.CreateReader(ms, null);
        var dic = reader.ReadAdditional();

        Assert.Single(dic);
        Assert.Equal((Byte)12, dic[AdditionalKinds.卫星状态]);
    }

    [Fact]
    [DisplayName("附加信息超过255字节时拒绝写入")]
    public void Additionals_TooLong()
    {
        var loc = new LocationMessage
        {
            Additionals = new Dictionary<AdditionalKinds, Object>
            {
                [AdditionalKinds.胎压] = new Byte[256],
            },
        };

        var ms = new MemoryStream();
        Assert.Throws<ArgumentOutOfRangeException>(() => loc.Write(ms, null));
    }

    [Fact]
    [DisplayName("静态信息解析空文本不抛异常")]
    public void CarInfo_Empty()
    {
        var inf = new CarInfo();

        inf.Parse(null);
        inf.Parse("");

        Assert.Null(inf.Vin);
        Assert.Equal(0, inf.VehicleType);
    }
}
