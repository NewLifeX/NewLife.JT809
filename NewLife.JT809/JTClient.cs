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

        client.Open();

        _client = client;
    }

    private void Send(Object body)
    {
        var msg = new JTMessage();

        msg.WriteModel(body);

        _client.SendMessage(msg);
    }
    #endregion

    #region 链路管理
    public void Connect(Int32 user, String pass, Int32 code, String downIp, Int32 downPort)
    {
        var model = new JT809_0x1001
        {
            UserId = (UInt32)user,
            Password = pass,
            MsgGNSSCENTERID = (UInt32)code,

            DownLinkIP = downIp,
            DownLinkPort = (UInt16)downPort,
        };

        Send(model);
    }
    #endregion

    #region 日志
    public ILog Log { get; set; }
    #endregion
}