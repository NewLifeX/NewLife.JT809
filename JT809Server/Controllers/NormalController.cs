using NewLife.JT809;
using NewLife.JT809.Handlers;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Net;
using NewLife.Security;

namespace JT809Server.Controllers;

internal class NormalController : IJT809Handler
{
    [MessageKind(MessageKinds.主链路登录请求消息)]
    public Object Login(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x1001>();
        XTrace.WriteLine(msg.ToJsonJT809());

        var verifyCode = (UInt32)Rand.Next();
        var ss = session as SlaveSession;
        try
        {
            // 建立从链路的连接
            var client = new JTClient
            {
                Name = "Slave",
                Server = new NetUri(NetType.Tcp, msg.DownLinkIP, msg.DownLinkPort),
                Config = ss.Config,
                CenterId = verifyCode,

                Log = ss.Log
            };

            //client.Open();

            //var rs = client.DownLoginAsync(verifyCode).Result;
            Task.Run(() => BuildSlave(client, verifyCode));

            ss.Slave = client;
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);

            return new JT809_0x1002 { Result = JT809_0x1002_Result.IP地址不正确 };
        }

        return new JT809_0x1002 { Result = JT809_0x1002_Result.成功, VerifyCode = verifyCode };
    }

    async Task BuildSlave(JTClient client, UInt32 verifyCode)
    {
        // 延迟建立建立从链路登录请求，得等主链路验证码先回去
        await Task.Delay(1000);

        client.Open();

        var rs = await client.DownLoginAsync(verifyCode);

        client.WriteLog("建立从链路：{0}", rs?.Result);

        await Task.Delay(1000);

        await client.DownPingAsync();

        client.WriteLog("从链路心跳");
    }

    [MessageKind(MessageKinds.主链路注销请求消息)]
    public Object Logout(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x1003>();
        XTrace.WriteLine(msg.ToJsonJT809());

        return new JT809_0x1004();
    }

    [MessageKind(MessageKinds.主链路连接保持请求消息)]
    public Object Ping(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x1005>();
        XTrace.WriteLine(msg.ToJsonJT809());

        return new JT809_0x1006();
    }
}