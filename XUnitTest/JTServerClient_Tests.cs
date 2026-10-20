using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.Net;
using Xunit;

namespace XUnitTest;

[DisplayName("服务端与客户端联调")]
public class JTServerClient_Tests
{
    /// <summary>取一个空闲端口，避免与固定端口冲突</summary>
    private static Int32 GetFreePort()
    {
        using var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        return ((IPEndPoint)listener.LocalEndpoint).Port;
    }

    [Fact]
    [DisplayName("客户端登录服务端并完成心跳往返")]
    public async Task EndToEnd_LoginAndPing()
    {
        var port = GetFreePort();
        using var server = new JTServer { Port = port };
        server.AddHandler(new EchoController());

        var client = new JTClient
        {
            Name = "master",
            Server = new NetUri($"tcp://127.0.0.1:{port}"),
            Config = new JTConfig { Version = JTVersion.JT2019 },
        };

        try
        {
            server.Start();

            client.Open();
            var verify = await client.LoginAsync(9528, "stone", 123654, "127.0.0.1", 2809);
            Assert.Equal(0x4321u, verify);

            var pong = await client.PingAsync();
            Assert.NotNull(pong);
        }
        finally
        {
            client.Dispose();
            server.Stop("test done");
        }
    }

    /// <summary>登录与心跳处理器</summary>
    private class EchoController
    {
        [MessageKind(MessageKinds.主链路登录请求消息)]
        public Object Login(IJTSession session, JTMessage message)
        {
            var model = message.ReadModel<JT809_0x1001>();
            Assert.Equal(9528u, model.UserId);
            Assert.Equal(123654u, message.CenterId);

            return new JT809_0x1002 { Result = JT809_0x1002_Result.成功, VerifyCode = 0x4321 };
        }

        [MessageKind(MessageKinds.主链路连接保持请求消息)]
        public Object KeepAlive(IJTSession session, JTMessage message) => new JT809_0x1006();
    }
}
