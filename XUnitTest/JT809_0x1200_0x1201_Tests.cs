using System;
using System.ComponentModel;
using System.IO;
using NewLife.JT809;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;
using Xunit;

namespace XUnitTest;

[DisplayName("0x1201 车辆注册信息")]
public class JT809_0x1200_0x1201_Tests
{
    private static JT809_0x1200_0x1201 CreateModel() => new()
    {
        PlateformId = "10010",
        ProducerId = "P1",
        TerminalModelType = "M1",
        ImeiId = "860000000000001",
        TerminalId = "1234567",
        TerminalSimCode = "13800138000",
    };

    [Fact]
    [DisplayName("2011版字段长度固定，2019专有字段被跳过")]
    public void Test2011()
    {
        var header = new JTMessage { Version = JTVersion.JT2011 };
        var msg = CreateModel();

        var ms = new MemoryStream();
        Assert.True(Helper.CreateWriter(ms, header).Write(msg));

        // 11 + 11 + 8 + 7 + 12
        Assert.Equal(49L, ms.Length);

        ms.Position = 0;
        Object obj = new JT809_0x1200_0x1201();
        Assert.True(Helper.CreateReader(ms, header).TryRead(null, ref obj));

        var rs = Assert.IsType<JT809_0x1200_0x1201>(obj);
        Assert.Equal("10010", rs.PlateformId);
        Assert.Equal("P1", rs.ProducerId);
        Assert.Equal("M1", rs.TerminalModelType);
        Assert.Equal("1234567", rs.TerminalId);
        Assert.Equal("13800138000", rs.TerminalSimCode);
        Assert.Null(rs.ImeiId);
    }

    [Fact]
    [DisplayName("2019版启用IMEI字段并使用加长长度")]
    public void Test2019()
    {
        var header = new JTMessage { Version = JTVersion.JT2019 };
        var msg = CreateModel();

        var ms = new MemoryStream();
        Assert.True(Helper.CreateWriter(ms, header).Write(msg));

        // 11 + 11 + 30 + 15 + 30 + 13
        Assert.Equal(110L, ms.Length);

        ms.Position = 0;
        Object obj = new JT809_0x1200_0x1201();
        Assert.True(Helper.CreateReader(ms, header).TryRead(null, ref obj));

        var rs = Assert.IsType<JT809_0x1200_0x1201>(obj);
        Assert.Equal("10010", rs.PlateformId);
        Assert.Equal("P1", rs.ProducerId);
        Assert.Equal("M1", rs.TerminalModelType);
        Assert.Equal("860000000000001", rs.ImeiId);
        Assert.Equal("1234567", rs.TerminalId);
        Assert.Equal("13800138000", rs.TerminalSimCode);
    }
}
