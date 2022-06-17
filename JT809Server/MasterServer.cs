using JT809Server.Controllers;
using NewLife.JT809;
using NewLife.Net;

namespace JT809Server;

internal class MasterServer : JTServer
{
    protected override void OnStart()
    {
        AddHandler(new NormalController());
        AddHandler(new InfoController());
        AddHandler(new J1200Controller());
        AddHandler(new J1300Controller());
        AddHandler(new J1400Controller());
        AddHandler(new J1500Controller());
        AddHandler(new J1600Controller());

        base.OnStart();
    }

    protected override INetSession CreateSession(ISocketSession session) => new SlaveSession();
}

class SlaveSession : JTSession
{
    public JTClient Slave { get; set; }

    //protected override Object Process(JTMessage message)
    //{
    //    var rs = base.Process(message);

    //    // 特殊处理登录指令
    //    if (message.Kind == MessageKinds.主链路登录请求消息)
    //    {
    //        if (rs is JT809_0x1002 result && result.Result == JT809_0x1002_Result.成功)
    //        {
    //            try
    //            {
    //                var model = message.ReadModel<JT809_0x1001>();
    //                var client = new JTClient
    //                {
    //                    Name = "Slave",
    //                    Server = new NetUri(NetType.Tcp, model.DownLinkIP, model.DownLinkPort),
    //                    Config = Config,

    //                    Log = Log
    //                };

    //                client.Open();

    //                Slave = client;
    //            }
    //            catch
    //            {
    //                return new JT809_0x1002 { Result = JT809_0x1002_Result.IP地址不正确 };
    //            }
    //        }
    //    }

    //    return rs;
    //}
}