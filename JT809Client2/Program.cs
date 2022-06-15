using JT809Client;
using NewLife;
using NewLife.JT809;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Net;

XTrace.UseConsole();

// 加大网络HEX日志长度
var set = NewLife.Net.Setting.Current;
if (set.LogDataLength <= 64)
{
    set.LogDataLength = 1024;
    set.Save();
}

MessageFactory.Instance.Init();

var log = XTrace.Log;
var client = new JTClient
{
    Name = "主链路",
    Server = new NetUri("tcp://127.0.0.1:809"),

    Log = log,
};

client.Config.Version = JTVersion.JTT2019;
client.Open();

var ip = NetHelper.MyIP() + "";
var port = 2809;
var server = new SlaveServer
{
    Port = port,

    Log = log,
    SessionLog = log
};

#if DEBUG
server.SocketLog = log;
server.LogSend = true;
server.LogReceive = true;
#endif

server.Start();

var rs = await client.LoginAsync(9528, "stone", 123654, ip, port);
log.Info("授权码：{0}", rs);

Console.ReadKey();