using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using NewLife;
using NewLife.Data;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;
using Xunit;

namespace XUnitTest;

[DisplayName("消息工厂")]
public class MessageFactory_Tests
{
    [Fact]
    [DisplayName("内置指令在初始化后全部注册")]
    public void Init_RegistersBuiltIn()
    {
        var factory = new MessageFactory();
        factory.Init();

        Assert.IsType<JT809_0x1001>(factory.Create(MessageKinds.主链路登录请求消息));
        Assert.IsType<JT809_0x1002>(factory.Create(MessageKinds.主链路登录应答消息));
        Assert.IsType<JT809_0x1200>(factory.Create(MessageKinds.主链路车辆动态信息交换业务));
        Assert.IsType<JT809_0x1200_0x1202>(factory.Create(MessageKinds.实时上传车辆定位信息));
        Assert.IsType<JT809_0x1600_0x1601>(factory.Create(MessageKinds.补报车辆静态信息应答));

        // 已剔除的指令不应被注册
        Assert.Null(factory.Create((MessageKinds)0x7FFF));

        // 类型映射双向一致
        Assert.Equal(typeof(JT809_0x1001), factory.Maps[MessageKinds.主链路登录请求消息]);
        Assert.Equal(MessageKinds.主链路登录请求消息, factory.Types[typeof(JT809_0x1001)]);
    }

    [Fact]
    [DisplayName("重复初始化保持幂等")]
    public void Init_Idempotent()
    {
        var factory = new MessageFactory();
        factory.Init();
        var count = factory.Maps.Count;
        factory.Init();

        Assert.Equal(count, factory.Maps.Count);
    }

    [Fact]
    [DisplayName("并发初始化返回时指令表已完整")]
    public void Init_Concurrent()
    {
        var factory = new MessageFactory();
        var errors = 0;

        Parallel.For(0, 32, _ =>
        {
            factory.Init();

            // Init 返回即代表整张表可用，不能容忍半注册状态
            if (factory.Create(MessageKinds.主链路登录请求消息) == null) Interlocked.Increment(ref errors);
            if (factory.Create(MessageKinds.补报车辆静态信息请求消息) == null) Interlocked.Increment(ref errors);
            if (factory.Create(MessageKinds.实时上传车辆定位信息) == null) Interlocked.Increment(ref errors);
        });

        Assert.Equal(0, errors);
    }

    [Fact]
    [DisplayName("泛型注册可扩展自定义指令")]
    public void Register_Generic()
    {
        var factory = new MessageFactory();
        factory.Init();
        factory.Register<CustomMessage>();

        var obj = factory.Create(MessageKinds.主链路断开通知消息);

        Assert.IsType<CustomMessage>(obj);
        Assert.Equal(MessageKinds.主链路断开通知消息, factory.Types[typeof(CustomMessage)]);
    }

    [Fact]
    [DisplayName("应答判断覆盖标准应答指令")]
    public void IsReply_Kinds()
    {
        var factory = new MessageFactory();

        Assert.True(factory.IsReply(MessageKinds.主链路登录应答消息));
        Assert.True(factory.IsReply(MessageKinds.从链路连接应答信息));
        Assert.True(factory.IsReply(MessageKinds.车辆注册信息应答消息));
        Assert.True(factory.IsReply(MessageKinds.报警督办应答消息));

        Assert.False(factory.IsReply(MessageKinds.主链路登录请求消息));
        Assert.False(factory.IsReply(MessageKinds.主链路车辆动态信息交换业务));
    }

    [Fact]
    [DisplayName("解析报文负载得到业务模型")]
    public void Parse_LoginMessage()
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
        var msg = new JTMessage { Version = JTVersion.JT2011 };
        Assert.True(msg.Read(str.ToHex()));

        var factory = new MessageFactory();
        factory.Init();
        var model = Assert.IsType<JT809_0x1001>(factory.Parse(msg));

        Assert.Equal(1234u, model.UserId);
        Assert.Equal("1234", model.Password);
        Assert.Equal("127.0.0.1", model.DownLinkIP.TrimEnd('\0'));
        Assert.Equal(7000, (Int32)model.DownLinkPort);
    }

    [Fact]
    [DisplayName("解析未注册指令返回null")]
    public void Parse_Unknown()
    {
        var factory = new MessageFactory();
        factory.Init();

        var msg = new JTMessage { Kind = (MessageKinds)0x7FFF, Payload = new ArrayPacket(new Byte[] { 1, 2, 3 }) };

        Assert.Null(factory.Parse(msg));
    }

    [MessageKind(MessageKinds.主链路断开通知消息)]
    private class CustomMessage
    {
        public Int32 Value { get; set; }
    }
}
