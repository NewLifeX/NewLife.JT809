using System.ComponentModel;
using NewLife.JT809;
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
}
