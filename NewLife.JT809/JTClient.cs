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
    /// <summary>服务端地址</summary>
    public NetUri Server { get; set; }

    /// <summary>配置信息</summary>
    public JTConfig Config { get; set; } = new JTConfig { Version = JTVersion.JTT2019 };

    private ISocketClient _client;
    #endregion

    #region 构造
    /// <summary>
    /// 销毁
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(Boolean disposing)
    {
        base.Dispose(disposing);

        _client.TryDispose();
    }
    #endregion

    #region 方法
    /// <summary>
    /// 打开连接
    /// </summary>
    public void Open()
    {
        var client = Server.CreateRemote();
        client.Add<JTCodec>();
        client.Log = Log;

#if DEBUG
        client.LogSend = true;
        client.LogReceive = true;
#endif

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

        WriteLog("=>{0}", msg);

        var rs = await _client.SendMessageAsync(msg) as TResult;

        WriteLog("<={0}", rs);

        return rs;
    }
    #endregion

    #region 链路管理
    /// <summary>登录</summary>
    /// <param name="user"></param>
    /// <param name="pass"></param>
    /// <param name="code"></param>
    /// <param name="downIp"></param>
    /// <param name="downPort"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<UInt32> LoginAsync(Int32 user, String pass, Int32 code, String downIp, Int32 downPort)
    {
        var model = new JT809_0x1001
        {
            UserId = (UInt32)user,
            Password = pass,
            MsgGNSSCENTERID = (UInt32)code,

            DownLinkIP = downIp,
            DownLinkPort = (UInt16)downPort,
        };

        var rs = await SendAsync<JT809_0x1002>(model);
        if (rs == null || rs.Result != JT809_0x1002_Result.成功) throw new Exception($"登录失败！{rs?.Result}");

        return rs.VerifyCode;
    }

    /// <summary>注销</summary>
    /// <param name="user"></param>
    /// <param name="pass"></param>
    /// <returns></returns>
    public async Task<JT809_0x1004> LogoutAsync(Int32 user, String pass)
    {
        var model = new JT809_0x1003
        {
            UserId = (UInt32)user,
            Password = pass,
        };

        var rs = await SendAsync<JT809_0x1004>(model);

        return rs;
    }

    /// <summary>心跳</summary>
    /// <returns></returns>
    public async Task<JT809_0x1006> PingAsync()
    {
        var model = new JT809_0x1005();

        var rs = await SendAsync<JT809_0x1006>(model);

        return rs;
    }
    #endregion

    #region 日志
    /// <summary>日志</summary>
    public ILog Log { get; set; }

    /// <summary>写日志</summary>
    /// <param name="format"></param>
    /// <param name="args"></param>
    public void WriteLog(String format, params Object[] args) => Log?.Info(format, args);
    #endregion
}