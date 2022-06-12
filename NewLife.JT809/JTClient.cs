using System.Reflection;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Net;

namespace NewLife.JT809;

/// <summary>JT809客户端</summary>
public class JTClient : DisposeBase
{
    #region 属性
    public NetUri Server { get; set; }

    public JTConfig Config { get; set; } = new JTConfig { Version = JTVersion.JTT2019 };

    private ISocketClient _client;
    #endregion

    #region 构造
    protected override void Dispose(Boolean disposing)
    {
        base.Dispose(disposing);

        _client.TryDispose();
    }
    #endregion

    #region 方法
    public void Open()
    {
        var client = Server.CreateRemote();
        client.Add<JTCodec>();
        client.Log = Log;

        // 指定协议配置，包括协议版本等信息
        client["Config"] = Config;

        client.Open();

        _client = client;
    }

    private void Send(Object body)
    {
        var msg = new JTMessage();

        msg.WriteModel(body);

        _client.SendMessage(msg);
    }

    private async Task<TResult> SendAsync<TResult>(Object body) where TResult : class
    {
        var att = body.GetType().GetCustomAttribute<MessageKindAttribute>();

        var msg = new JTMessage
        {
            Kind = att.Kind,
            Time = DateTime.Now,
        };

        msg.WriteModel(body);

        return await _client.SendMessageAsync(msg) as TResult;
    }
    #endregion

    #region 链路管理
    public async Task<UInt32> ConnectAsync(Int32 user, String pass, Int32 code, String downIp, Int32 downPort)
    {
        var model = new JT809_0x1001
        {
            UserId = (UInt32)user,
            Password = pass,
            MsgGNSSCENTERID = (UInt32)code,

            DownLinkIP = downIp,
            DownLinkPort = (UInt16)downPort,
        };

        //Send(model);

        var rs = await SendAsync<JT809_0x1002>(model);
        if (rs == null || rs.Result != JT809_0x1002_Result.成功) throw new Exception($"登录失败！{rs?.Result}");

        return rs.VerifyCode;
    }
    #endregion

    #region 日志
    public ILog Log { get; set; }
    #endregion
}