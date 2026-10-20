using System;
using System.ComponentModel;
using System.IO;
using NewLife;
using NewLife.Data;
using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using Xunit;

namespace XUnitTest;

[DisplayName("0x9102 链路与定位传输情况上报")]
public class JT809_0x9102_Tests
{
    private readonly MessageFactory _factory;
    public JT809_0x9102_Tests()
    {
        _factory = MessageFactory.Instance;
        _factory.Init();
    }

    [Fact]
    [DisplayName("2011报文字段读写往返")]
    public void Test9102()
    {
        var header = new JTMessage { Version = JTVersion.JT2011 };
        var msg = new JT809_0x9102
        {
            DataType = 0x9102,
            DataLength = 27,
            PlatformId = "10010",
            StartTime = 1263085674L.ToDateTime(),
            EndTime = 1263085674L.ToDateTime(),
        };

        var ms = new MemoryStream();
        Assert.True(Helper.CreateWriter(ms, header).Write(msg));

        ms.Position = 0;
        Object obj = new JT809_0x9102();
        Assert.True(Helper.CreateReader(ms, header).TryRead(null, ref obj));

        var rs = Assert.IsType<JT809_0x9102>(obj);
        Assert.Equal(msg.DataType, rs.DataType);
        Assert.Equal(msg.DataLength, rs.DataLength);
        Assert.Equal(msg.PlatformId, rs.PlatformId);
        Assert.Equal(msg.StartTime, rs.StartTime);
        Assert.Equal(msg.EndTime, rs.EndTime);
    }

    /// <summary>协议示例：2010-1-10 9:7:54 的 UTC 值为 1263085674，协议中表示为 0x000000004B49286A</summary>
    [Fact]
    [DisplayName("UTC时间字段按Unix秒写入")]
    public void Test9102_UtcSeconds()
    {
        var start = 1263085674L.ToDateTime();
        var msg = new JT809_0x9102
        {
            PlatformId = "10010",
            StartTime = start,
            EndTime = start,
        };

        var header = new JTMessage { Version = JTVersion.JT2019 };
        var ms = new MemoryStream();
        Assert.True(Helper.CreateWriter(ms, header).Write(msg));

        var hex = ms.ToArray().ToHex();
        Assert.Contains("000000004B49286A", hex);

        ms.Position = 0;
        Object obj = new JT809_0x9102();
        Assert.True(Helper.CreateReader(ms, header).TryRead(null, ref obj));

        var rs = Assert.IsType<JT809_0x9102>(obj);
        Assert.Equal(start, rs.StartTime);
        Assert.Equal(start, rs.EndTime);
    }

    [Fact]
    [DisplayName("2019报文按秒解析对端数据")]
    public void Test222()
    {
        // DataType(2) + DataLength(4) + PlatformId(11) + StartTime(8) + EndTime(8)
        var str = "27660000001B3130303130000000000000000000004EBC924F000000004EBC924F";
        var dt = str.ToHex();

        var header = new JTMessage { Version = JTVersion.JT2019 };
        var bn = Helper.CreateReader(new MemoryStream(dt), header);

        var body = bn.Read<JT809_0x9102>();

        Assert.NotNull(body);
        Assert.Equal(10086, (Int32)body.DataType);
        Assert.Equal("10010", body.PlatformId);
        Assert.Equal("2011-11-11 03:11:11".ToDateTime(), body.StartTime);
        Assert.Equal("2011-11-11 03:11:11".ToDateTime(), body.EndTime);
    }
}
