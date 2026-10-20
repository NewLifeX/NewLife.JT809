using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using NewLife;
using NewLife.JT809;
using NewLife.JT809.Protocols;
using Xunit;

namespace XUnitTest;

[DisplayName("序列化助手")]
public class Helper_Tests
{
    [Fact]
    [DisplayName("GBK字符串按长度字节写入并读回")]
    public void WriteString_Roundtrip()
    {
        var ms = new MemoryStream();
        var writer = Helper.CreateWriter(ms, null);
        writer.WriteString("皖AT0011");
        ms.Position = 0;

        var reader = Helper.CreateReader(ms, null);
        var str = reader.ReadString();

        Assert.Equal("皖AT0011", str);
    }

    [Fact]
    [DisplayName("读取定长字符串时剔除头尾零与0xFF")]
    public void ReadString_TrimInvalid()
    {
        var ms = new MemoryStream(new Byte[] { 0x00, 0xFF, (Byte)'A', (Byte)'B', 0x00, 0xFF });
        var reader = Helper.CreateReader(ms, null);

        Assert.Equal("AB", reader.ReadString(6));
    }

    [Fact]
    [DisplayName("长度为零时返回null")]
    public void ReadString_Empty()
    {
        var ms = new MemoryStream(new Byte[] { 0x00 });
        var reader = Helper.CreateReader(ms, null);

        Assert.Null(reader.ReadString(0));
    }

    [Fact]
    [DisplayName("按GBK写入全部剩余字符串")]
    public void WriteAllGBK_Roundtrip()
    {
        var text = "合肥数车科技有限公司";

        var ms = new MemoryStream();
        var writer = Helper.CreateWriter(ms, null);
        writer.WriteAllGBK(text);
        ms.Position = 0;

        var reader = Helper.CreateReader(ms, null);
        Assert.Equal(text, reader.ReadAllGBK());
    }

    [Fact]
    [DisplayName("BCD时间写入后可原样读回")]
    public void BCDTime_Roundtrip()
    {
        var dt = new DateTime(2024, 5, 6, 7, 8, 9);

        var ms = new MemoryStream();
        var writer = Helper.CreateWriter(ms, null);
        writer.WriteBCDTime(dt);
        ms.Position = 0;

        var reader = Helper.CreateReader(ms, null);
        Assert.Equal(dt, reader.ReadBCDTime());
    }

    [Fact]
    [DisplayName("非法BCD时间返回最小值")]
    public void ReadBCDTime_Invalid()
    {
        var ms = new MemoryStream(new Byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF });
        var reader = Helper.CreateReader(ms, null);

        Assert.Equal(DateTime.MinValue, reader.ReadBCDTime());
    }

    [Fact]
    [DisplayName("报文序列化为枚举字符串")]
    public void ToJsonJT809_EnumString()
    {
        var msg = new JTMessage
        {
            Sequence = 12,
            Kind = MessageKinds.主链路登录请求消息,
            CenterId = 123456,
        };

        var json = msg.ToJsonJT809();

        Assert.Contains("主链路登录请求消息", json);
        Assert.Contains("\"Sequence\":12", json.Replace(" ", ""));
    }

    [Fact]
    [DisplayName("分帧失败不产生业务模型")]
    public void CreateReader_WithoutMessage()
    {
        var ms = new MemoryStream(new Byte[] { 1, 2, 3 });
        var reader = Helper.CreateReader(ms, null);

        Assert.Null(reader.Version);
        Assert.Equal(3, reader.ReadBytes(3).Length);
    }
}
