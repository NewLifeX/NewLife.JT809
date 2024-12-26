using System.IO;
using NewLife;
using NewLife.Data;
using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using Xunit;

namespace XUnitTest;

public class JT809_0x1001_Tests
{
    MessageFactory _factory;
    public JT809_0x1001_Tests()
    {
        _factory = MessageFactory.Instance;
        _factory.Init();
    }

    [Fact]
    public void Test1()
    {
        var str =
            "0133EFB8" +
            "3230313830393230" +
            //"0001E240" +
            "3132372E-302E302E-31000000-00000000-00000000-00000000-00000000-00000000" +
            "0329";
        var dt = str.ToHex();

        var message = new JTMessage();

        var reader = Helper.CreateReader(new MemoryStream(dt), message);
        //reader.IgnoreMembers.Add("DownLinkIP");
        //reader.IgnoreMembers.Add("DownLinkPort");
        //reader.EnableTrace();

        var body = reader.Read<JT809_0x1001>();

        Assert.NotNull(body);
        Assert.Equal(20180920u, body.UserId);
        Assert.Equal("20180920", body.Password);
        //Assert.Equal(123456u, body.MsgGNSSCENTERID);
        Assert.Equal("127.0.0.1", body.DownLinkIP.TrimEnd('\0'));
        Assert.Equal(809, body.DownLinkPort);

        // 序列化
        var ms = new MemoryStream();
        var writer = Helper.CreateWriter(ms, message);
        writer.Write(body);

        var hex = ms.ToArray().ToHex();
        Assert.Equal(str.Replace("-", null), hex);
    }

    [Fact]
    public void Test2()
    {
        var str =
            "5B" +
            "00000048" +
            "00000001" +
            "1001" +
            "0001E240" +
            "010000" +
            "00" +
            "00989681" +

            "000004D2" +
            "3132333400000000" +
            "3132372E-302E302E-31000000-00000000-00000000-00000000-00000000-00000000" +
            "1B58" +

            "B085" +
            "5D";
        var dt = str.ToHex();

        var msg = new JTMessage { Version = JTVersion.JT2011 };
        var rs = msg.Read(dt);

        Assert.True(rs);
        Assert.Equal(1u, msg.Sequence);
        Assert.Equal(MessageKinds.主链路登录请求消息, msg.Kind);
        Assert.Equal(123456u, msg.CenterId);
        Assert.Equal(0x010000u, msg.VersionFlag);
        Assert.Equal(0u, msg.EncryptFlag);
        Assert.Equal(10000001u, msg.EncryptKey);
        //Assert.Equal("2022-06-12T11:31:23.6110000".ToDateTime(), msg.Time);

        // 校验写入
        var pk = msg.ToPacket();
        Assert.Equal(dt.ToHex(), pk.ToHex(256));

        var body = msg.ReadModel<JT809_0x1001>();

        Assert.NotNull(body);
        Assert.Equal(1234u, body.UserId);
        Assert.Equal("1234", body.Password);
        Assert.Equal(0u, body.MsgGNSSCENTERID);
        Assert.Equal("127.0.0.1", body.DownLinkIP.TrimEnd('\0'));
        Assert.Equal(7000, body.DownLinkPort);

        // 序列化
        var ms = new MemoryStream();
        var writer = Helper.CreateWriter(ms, msg);
        writer.Write(body);

        var hex = ms.ToArray().ToHex();
        Assert.Equal(msg.Payload.ToHex(256), hex);
    }
}
