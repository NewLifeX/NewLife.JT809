using System;
using System.ComponentModel;
using NewLife;
using NewLife.Data;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using Xunit;

namespace XUnitTest;

public class JTMessage_Tests
{
    MessageFactory _factory;
    public JTMessage_Tests()
    {
        _factory = MessageFactory.Instance;
        _factory.Init();
    }

    [Fact]
    public void Test1()
    {
        var str =
            "5B" +
            "00000076" +
            "00000002" +
            "1400" +
            "00000000" +
            "010000" +
            "00" +
            "00000000" +
            "0000018157AED15A01" +
            "14020000004E31323334353637383931320001000000005E027B1E28000000005E027B1E28000000005E027B1E28B4A8413132333435000000000000000000000000000131323334353637383931320000007B00000000" +
            "F654" +
            "5D";
        var dt = str.ToHex();

        var config = new JTConfig { Version = JTVersion.JT2019 };
        var msg = new JTMessage();
        var rs = msg.Read(dt, config);

        Assert.True(rs);
        Assert.Equal(2u, msg.Sequence);
        Assert.Equal(MessageKinds.主链路报警信息交互消息, msg.Kind);
        Assert.Equal(0u, msg.CenterId);
        Assert.Equal(0x010000u, msg.VersionFlag);
        Assert.Equal(0u, msg.EncryptFlag);
        Assert.Equal(0u, msg.EncryptKey);
        Assert.Equal("2022-06-12T11:31:23.6110000".ToDateTime(), msg.Time);

        // 校验写入
        var pk = msg.ToPacket();
        Assert.Equal(dt.ToHex(), pk.ToHex(256));

        //var rm = _factory.Parse(msg) as RegisterMessage;
        //Assert.NotNull(rm);

        //Assert.Equal(0, rm.Province);
        //Assert.Equal(0, rm.City);
        //Assert.Equal("44030072104", rm.Provider);
        //Assert.Equal("P9", rm.Model);
        //Assert.Equal("5976616", rm.DeviceId);
        //Assert.Equal(CarColors.黄色, rm.Color);
        //Assert.Equal("粤ABB280", rm.PlateNo);
    }

    [Fact]
    public void Test2()
    {
        var str =
            "5B" +
            "00000092" +
            "00000682" +
            "9400" +
            "0133EFB8" +
            "010000" +
            "00" +
            "0000270F" +
            "D4C1413132333435000000000000000000000000000294010000005C010002000000005A01AC3F40123FFAA1000000005A01AC4D5001736D616C6C636869000000000000000031323334353637383930310000000000000000003132333435364071712E636F6D00000000000000000000000000000000000000" +
            "7AEA" +
            "5D";
        var dt = str.ToHex();

        var config = new JTConfig { Version = JTVersion.JT2011 };
        var msg = new JTMessage();
        var rs = msg.Read(dt, config);

        Assert.True(rs);
        Assert.Equal(1666u, msg.Sequence);
        Assert.Equal(MessageKinds.从链路报警信息交互消息, msg.Kind);
        Assert.Equal(20180920u, msg.CenterId);
        Assert.Equal(0x010000u, msg.VersionFlag);
        Assert.Equal(0u, msg.EncryptFlag);
        Assert.Equal(9999u, msg.EncryptKey);
        Assert.Equal("".ToDateTime(), msg.Time);

        // 校验写入
        var pk = msg.ToPacket();
        Assert.Equal(dt.ToHex(), pk.ToHex(256));

        //var rm = _factory.Parse(msg) as RegisterMessage;
        //Assert.NotNull(rm);

        //Assert.Equal(0, rm.Province);
        //Assert.Equal(0, rm.City);
        //Assert.Equal("44030072104", rm.Provider);
        //Assert.Equal("P9", rm.Model);
        //Assert.Equal("5976616", rm.DeviceId);
        //Assert.Equal(CarColors.黄色, rm.Color);
        //Assert.Equal("粤ABB280", rm.PlateNo);
    }

    [Fact]
    public void TestJT2019()
    {
        var str =
            "5B" +
            "00000054" +
            "00000002" +
            "1001" +
            "0133EFB8" +
            "010000" +
            "00" +
            "0000270F" +
            "0000018157AED15A01" +

            "0133EFB8" +
            "3230313830393230" +
            "0001E240" +
            "3132372E-302E302E-31000000-00000000-00000000-00000000-00000000-00000000" +
            "0329" +

            "024A" +
            "5D";
        var dt = str.ToHex();

        var msg = new JTMessage();
        var rs = msg.Read(dt);

        Assert.True(rs);
        Assert.Equal(2u, msg.Sequence);
        Assert.Equal(MessageKinds.主链路登录请求消息, msg.Kind);
        Assert.Equal(20180920u, msg.CenterId);
        Assert.Equal(0x010000u, msg.VersionFlag);
        Assert.Equal(0u, msg.EncryptFlag);
        Assert.Equal(9999u, msg.EncryptKey);
        Assert.Equal("2022-06-12T11:31:23.6110000".ToDateTime(), msg.Time);

        Assert.Equal(JTVersion.JT2019, msg.Version);
    }

    [Fact]
    [DisplayName("转义字符在写入与解析间保持不变")]
    public void TestEscape()
    {
        var payload = new Byte[] { 0x5B, 0x5A, 0x5D, 0x5E, 0x5B, 0x5A, 0x5D, 0x5E };
        var msg = new JTMessage
        {
            Sequence = 7,
            Kind = MessageKinds.主链路车辆动态信息交换业务,
            CenterId = 123456,
            VersionFlag = 0x010000,
            Version = JTVersion.JT2011,
            Payload = new ArrayPacket(payload),
        };

        var pk = msg.ToPacket();
        var frame = pk.ToArray();

        // 帧内不得出现未转义的 0x5D（除帧尾）与 0x5B（除帧头）
        for (var i = 1; i < frame.Length - 1; i++)
        {
            Assert.NotEqual(0x5B, (Int32)frame[i]);
            Assert.NotEqual(0x5D, (Int32)frame[i]);
        }

        var rs = new JTMessage();
        Assert.True(rs.Read(frame));
        Assert.Equal(payload, rs.Payload.ReadBytes());
    }

    [Fact]
    [DisplayName("帧头非法或数据截断时解析失败")]
    public void TestInvalidFrame()
    {
        var msg = new JTMessage();

        // 帧头不是0x5B
        Assert.False(msg.Read(new Byte[] { 0x00, 0x01, 0x02 }));

        // 截断的帧
        var str = "5B000000480000000110010001E2400100000000989681";
        Assert.False(new JTMessage().Read(str.ToHex()));

        // 空数据
        Assert.False(new JTMessage().Read(Array.Empty<Byte>()));
    }

    [Fact]
    [DisplayName("登录请求可自动识别2019协议版本")]
    public void TestGuessVersion()
    {
        var str =
            "5B" +
            "00000054" +
            "00000002" +
            "1001" +
            "0133EFB8" +
            "010000" +
            "00" +
            "0000270F" +
            "0000018157AED15A01" +
            "0133EFB8" +
            "3230313830393230" +
            "0001E240" +
            "3132372E-302E302E-31000000-00000000-00000000-00000000-00000000-00000000" +
            "0329" +
            "024A" +
            "5D";

        // 未指定协议版本时，按帧长自动识别为2019
        var msg = new JTMessage();
        Assert.True(msg.Read(str.ToHex()));
        Assert.Equal(JTVersion.JT2019, msg.Version);
        Assert.Equal("2022-06-12T11:31:23.6110000".ToDateTime(), msg.Time);

        // 登录帧按帧长自适应识别，配置为2011也会纠正为2019
        var msg2 = new JTMessage();
        Assert.True(msg2.Read(str.ToHex(), new JTConfig { Version = JTVersion.JT2011 }));
        Assert.Equal(JTVersion.JT2019, msg2.Version);
        Assert.Equal("2022-06-12T11:31:23.6110000".ToDateTime(), msg2.Time);
    }

    [Fact]
    [DisplayName("创建应答继承序列号与接入码")]
    public void TestCreateReply()
    {
        var msg = new JTMessage
        {
            Sequence = 9,
            Kind = MessageKinds.主链路登录请求消息,
            CenterId = 123456,
            VersionFlag = 0x01020F,
            Version = JTVersion.JT2019,
        };

        var rs = msg.CreateReply();
        Assert.True(rs.Reply);
        Assert.Equal(msg.Sequence, rs.Sequence);
        Assert.Equal(msg.Kind, rs.Kind);
        Assert.Equal(msg.CenterId, rs.CenterId);
        Assert.Equal(msg.VersionFlag, rs.VersionFlag);

        // 2019 帧头需要写发送时间，应答必须继承协议版本并带当前时间
        Assert.Equal(JTVersion.JT2019, rs.Version);
        Assert.True(rs.Time > DateTime.Now.AddMinutes(-1));

        // 2011 帧头不写时间字段，但接口语义保持一致
        var msg2 = new JTMessage { Version = JTVersion.JT2011 };
        Assert.True(msg2.CreateReply().Time > DateTime.Now.AddMinutes(-1));
    }

    [Fact]
    [DisplayName("设置结果按模型类型回填指令并写入负载")]
    public void TestSetResult()
    {
        var msg = new JTMessage { Sequence = 3, CenterId = 123456 };
        var body = new JT809_0x1002
        {
            Result = JT809_0x1002_Result.成功,
            VerifyCode = 0x1234,
        };

        msg.SetResult(body);

        Assert.Equal(MessageKinds.主链路登录应答消息, msg.Kind);
        Assert.True(msg.Reply);
        Assert.NotNull(msg.Payload);
        Assert.True(msg.Payload.Total > 0);

        var rs = msg.ReadModel<JT809_0x1002>();
        Assert.Equal(JT809_0x1002_Result.成功, rs.Result);
        Assert.Equal(0x1234u, rs.VerifyCode);
    }

    [Fact]
    [DisplayName("消息实现IMessage契约并按应答映射方向")]
    public void TestIMessage()
    {
        var msg = new JTMessage { Kind = MessageKinds.主链路登录应答消息, Reply = true, Payload = new ArrayPacket(new Byte[] { 1, 2 }) };

        var im = msg as NewLife.Messaging.IMessage;
        Assert.NotNull(im);
        Assert.Equal(NewLife.Messaging.MessageKinds.Response, im.Kind);
        Assert.True(im.Reply);
        Assert.Same(msg.Payload, im.Payload);
        var reply = im.CreateReply();
        Assert.NotNull(reply);
        Assert.True(reply.Reply);
        Assert.Equal(msg.Sequence, ((JTMessage)reply).Sequence);

        // 释放后负载归还
        im.Dispose();
        Assert.Null(msg.Payload);
    }

    [Fact]
    [DisplayName("CRC校验错误的帧仍可解析但日志告警")]
    public void TestCrcMismatch()
    {
        // 将正确帧的 CRC 改为错误值，解析不应抛出异常
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
            "FFFF" +
            "5D";

        var msg = new JTMessage { Version = JTVersion.JT2011 };
        Assert.True(msg.Read(str.ToHex()));
        Assert.Equal(MessageKinds.主链路登录请求消息, msg.Kind);
    }
}
