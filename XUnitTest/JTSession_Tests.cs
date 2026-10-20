using System;
using System.ComponentModel;
using System.Threading.Tasks;
using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Models;
using NewLife.JT809.SubMessages;
using NewLife.JT809.Protocols;
using Xunit;

namespace XUnitTest;

[DisplayName("服务端与客户端会话配置")]
public class JTSession_Tests
{
    [Fact]
    [DisplayName("服务端默认端口、定界缓存与配置引用稳定")]
    public void Server_Defaults()
    {
        using var server = new JTServer();

        Assert.Equal(809, server.Port);
        Assert.NotNull(server.Config);
        Assert.Equal(JTVersion.JT2011, server.Config.Version);

        // 批量车辆数据需要更大的定界缓存
        Assert.Equal(4 * 1024 * 1024, server.MaxCache);

        // 配置在读取之间保持同一实例，替换后立即生效
        var config = new JTConfig { Version = JTVersion.JT2019 };
        server.Config = config;
        Assert.Same(config, server.Config);

        // 置空时回落到默认配置，保持非空不变式
        server.Config = null;
        Assert.NotNull(server.Config);
    }

    [Fact]
    [DisplayName("客户端默认配置可替换且引用稳定")]
    public void Client_Defaults()
    {
        using var client = new JTClient();

        Assert.NotNull(client.Config);
        Assert.Equal(JTVersion.JT2011, client.Config.Version);
        Assert.Same(client.Config, client.Config);

        var config = new JTConfig { Version = JTVersion.JT2019 };
        client.Config = config;
        Assert.Same(config, client.Config);

        client.Config = null;
        Assert.NotNull(client.Config);
    }

    [Fact]
    [DisplayName("程序集内嵌链接器保留清单，AOT 下模型成员不被裁剪")]
    public void Assembly_KeepsLinkerDescriptor()
    {
        var asm = typeof(JTMessage).Assembly;

        Assert.Contains("ILLink.Descriptors.xml", asm.GetManifestResourceNames());
    }

    [Fact]
    [DisplayName("发送位置消息按指令表回填子业务类型")]
    public void Send1200_UsesRegistry()
    {
        var session = new FakeSession();
        var body = new JT809_0x1200_0x1202
        {
            Time = new DateTime(2024, 5, 6, 7, 8, 9),
            Longitude = 117123456,
            Latitude = 31876543,
            Speed = 600,
        };

        session.Send1200("皖AT0011", 2, body);

        var msg = Assert.IsType<JT809_0x1200>(session.Sent);
        Assert.Equal(0x1202, (Int32)msg.DataType);
        Assert.Equal("皖AT0011", msg.VehicleNo);
        Assert.Equal(2, (Int32)msg.VehicleColor);
        Assert.Same(body, msg.Body);
    }

    [Fact]
    [DisplayName("发送未注册类型的位置消息抛出参数异常")]
    public void Send1200_Unregistered()
    {
        var session = new FakeSession();

        Assert.Throws<ArgumentException>(() => session.Send1200("皖AT0011", 2, new Object()));
    }

    /// <summary>记录发送内容的会话替身</summary>
    private class FakeSession : IJTSession
    {
        public UInt32 CenterId { get; set; }

        public JTConfig Config { get; set; } = new JTConfig();

        public Object Sent { get; private set; }

        public void Send(Object body) => Sent = body;

        public Task<TResult> SendAsync<TResult>(Object body) where TResult : class, new() => throw new NotSupportedException();
    }
}
