using NewLife.Log;
using NewLife.Net;

namespace NewLife.JT809;

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
        client.Log = Log;

        client.Open();

        _client = client;
    }
    #endregion

    #region 日志
    public ILog Log { get; set; }
    #endregion
}