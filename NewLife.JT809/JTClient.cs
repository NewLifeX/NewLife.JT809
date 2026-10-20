using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using NewLife.JT809.Handlers;
using NewLife.JT809.Messages;
using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;
using NewLife.Log;
using NewLife.Net;
using NewLife.Reflection;

namespace NewLife.JT809;

/// <summary>JT809客户端</summary>
public class JTClient : DisposeBase, IJTSession
{
    #region 属性
    private readonly JTCodec _codec = new() { Config = new JTConfig { Version = JTVersion.JT2011 } };

    /// <summary>名称</summary>
    public String Name { get; set; }

    /// <summary>服务端地址</summary>
    public NetUri Server { get; set; }

    /// <summary>配置信息</summary>
    public JTConfig Config
    {
        get => _codec.Config;
        set => _codec.Config = value ?? new JTConfig { Version = JTVersion.JT2011 };
    }

    /// <summary>下级平台接入码。上级平台给下级平台分配的唯一标识号</summary>
    public UInt32 CenterId { get; set; }

    /// <summary>接收事件</summary>
    public event EventHandler<ReceivedEventArgs> Received;

    /// <summary>
    /// 消息处理器集合
    /// </summary>
    public Dictionary<MessageKinds, MessageHandler> Handlers { get; set; } = new();

    private ISocketClient _client;
    private readonly Object _lock = new();
    /// <summary>网络连接</summary>
    public ISocketClient Client => _client;
    #endregion

    #region 构造
    static JTClient() => MessageFactory.Instance.Init();

    /// <summary>
    /// 销毁
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(Boolean disposing)
    {
        base.Dispose(disposing);

        lock (_lock)
        {
            if (_client != null)
            {
                _client.Received -= OnReceive;
                _client.TryDispose();
                _client = null;
            }
        }
    }
    #endregion

    #region 方法
    /// <summary>
    /// 打开连接
    /// </summary>
    public void Open()
    {
        MessageFactory.Instance.Init();

        GetClient();
    }

    ISocketClient GetClient()
    {
        var client = _client;
        if (client != null && !client.Disposed) return client;

        // 并发发送时只建立一条连接，避免重复连接与句柄泄漏
        lock (_lock)
        {
            client = _client;
            if (client != null && !client.Disposed) return client;

            if (client != null) client.Received -= OnReceive;

            client = Server.CreateRemote();
            client.Name = Name;
            if (client is SessionBase session) session.Protocol = _codec;
            // 批量车辆数据可能超过默认1M定界缓存，放宽到4M，避免正常大帧被当作无法定界的残余而断链
            if (client is TcpSession tcp) tcp.MaxCache = 4 * 1024 * 1024;
            client.Tracer = Tracer;
            client.Log = Log;

            client.Received += OnReceive;

            //if (client is TcpSession tcp) tcp.DisconnectWhenEmptyData = false;

#if DEBUG
            client.LogSend = true;
            client.LogReceive = true;
#endif

            client.Open();

            return _client = client;
        }
    }

    void OnReceive(Object sender, ReceivedEventArgs e)
    {
        if (e.Message is not JTMessage msg) return;

        using var span = Tracer?.NewSpan($"jt809:Receive:{msg.Kind}", msg);
        span?.AppendTag(e.Packet);
        //WriteLog("<={0}", msg);

        Received?.Invoke(this, e);

        // 找到处理器
        if (Handlers.TryGetValue(msg.Kind, out var handler))
            handler(this, msg);
    }

    /// <summary>注册类型处理器</summary>
    /// <typeparam name="T">处理器类型，需为具体类型（泛型推断即满足），AOT/裁剪下其公共方法才会被保留</typeparam>
    public void AddHandler<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] T>(T handler)
    {
        Type type = typeof(T);
        foreach (var item in type.GetMethods())
        {
            if (item.IsStatic) continue;
            if (item.ReturnType != typeof(Object)) continue;

            // 参数匹配
            var pis = item.GetParameters();
            if (pis.Length != 2) continue;

            // 获取类型
            var att = item.GetCustomAttribute<MessageKindAttribute>();
            if (att != null)
            {
                Handlers[att.Kind] = (MessageHandler)Delegate.CreateDelegate(typeof(MessageHandler), handler, item);

                WriteLog("Registered [{0}]: {1}.{2}", att.Kind, item.DeclaringType.FullName, item.Name);
            }
        }
    }

    /// <summary>发送不等待</summary>
    /// <param name="body"></param>
    public void Send(Object body)
    {
        var msg = new JTMessage
        {
            CenterId = CenterId,
            VersionFlag = Config.VersionFlag,
            Time = DateTime.Now,
        };

        if (Config != null) msg.Version = Config.Version;

        msg.SetResult(body);

        using var span = Tracer?.NewSpan($"jt809:Send:{msg.Kind}", body);

        WriteLog("=>{0}", body.ToJsonJT809());
        WriteLog("=>{0}", msg);

        var client = GetClient();
        client.SendMessage(msg);
    }

    /// <summary>发送并等待响应</summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="body"></param>
    /// <returns></returns>
    public async Task<TResult> SendAsync<TResult>(Object body) where TResult : class, new()
    {
        var msg = new JTMessage
        {
            CenterId = CenterId,
            VersionFlag = Config.VersionFlag,
            Time = DateTime.Now,
        };

        if (Config != null) msg.Version = Config.Version;

        msg.SetResult(body);

        using var span = Tracer?.NewSpan($"jt809:SendAsync:{msg.Kind}", body);

        WriteLog("=>{0}", body.ToJsonJT809());
        WriteLog("=>{0}", msg);

        var client = GetClient();
        var rs = await client.SendMessageAsync(msg) as JTMessage;

        WriteLog("<={0}", rs);

        var result = rs.ReadModel<TResult>();
        WriteLog("<={0}", result.ToJsonJT809());

        return result;
    }

    //Int32 IJTSession.SendMessage(Object message) => _client.SendMessage(message);
    //Task<Object> IJTSession.SendMessageAsync(Object message) => _client.SendMessageAsync(message);
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

        CenterId = model.MsgGNSSCENTERID;

        var rs = await SendAsync<JT809_0x1002>(model);
        if (rs == null || rs.Result != JT809_0x1002_Result.成功) throw new Exception(L.T("Login failed! {0}", rs?.Result));

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

        return await SendAsync<JT809_0x1004>(model);
    }

    /// <summary>心跳</summary>
    /// <returns></returns>
    public async Task<JT809_0x1006> PingAsync()
    {
        var model = new JT809_0x1005();

        return await SendAsync<JT809_0x1006>(model);
    }

    /// <summary>从链路登录</summary>
    /// <param name="verifyCode"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<JT809_0x9002> DownLoginAsync(UInt32 verifyCode)
    {
        var model = new JT809_0x9001
        {
            VerifyCode = verifyCode,
        };

        var rs = await SendAsync<JT809_0x9002>(model);
        if (rs == null || rs.Result != JT809_0x9002_Result.成功) throw new Exception(L.T("Login failed! {0}", rs?.Result));

        return rs;
    }

    /// <summary>从链路注销</summary>
    /// <param name="verifyCode"></param>
    /// <returns></returns>
    public async Task<JT809_0x9004> DownLogoutAsync(UInt32 verifyCode)
    {
        var model = new JT809_0x9003
        {
            VerifyCode = verifyCode,
        };

        return await SendAsync<JT809_0x9004>(model);
    }

    /// <summary>从链路心跳</summary>
    /// <returns></returns>
    public async Task<JT809_0x9006> DownPingAsync()
    {
        var model = new JT809_0x9005();

        return await SendAsync<JT809_0x9006>(model);
    }
    #endregion

    #region 信息统计
    /// <summary>
    /// 上报接收车辆定位信息数量通知消息
    /// </summary>
    /// <param name="count"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void PositionCount(Int32 count, DateTime start, DateTime end)
    {
        var model = new JT809_0x9101
        {
            DynamicInfoTotal = (UInt32)count,
            StartTime = start,
            EndTime = end
        };

        Send(model);
    }

    public async Task<JT809_0x1102> Send9102(String platformId, DateTime start, DateTime end)
    {
        var model = new JT809_0x9102
        {
            DataType = 0x9102,
            DataLength = 27,
            PlatformId = platformId,
            StartTime = start,
            EndTime = end
        };

        return await SendAsync<JT809_0x1102>(model);
    }

    public async Task<JT809_0x1103> Send9103(UInt16 subType, UInt32 sn, DateTime time)
    {
        var model = new JT809_0x9103
        {
            DataType = subType,
        };
        model.Messages = new List<JT809ManageMsgSNInform>
        {
            new JT809ManageMsgSNInform
            {
                Kind = subType,
                MsgSN = sn,
                Time = time,
            }
        };
        return await SendAsync<JT809_0x1103>(model);
    }
    #endregion

    #region 信息交互
    /// <summary>提交车辆注册信息</summary>
    /// <param name="plate"></param>
    /// <param name="color"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public void RegisterAsync(String plate, Int32 color, JT809_0x1200_0x1201 model)
    {
        var req = new JT809_0x1200
        {
            DataType = 0x1201,
            VehicleNo = plate,
            VehicleColor = (JT809VehicleColorType)color,
            Body = model
        };
        //return await SendAsync<JT809_0x9200_0x9201>(req);
        Send(req);
    }

    /// <summary>提交定位信息</summary>
    /// <param name="plate"></param>
    /// <param name="color"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public void LocationAsync(String plate, Int32 color, JT809_0x1200_0x1202 model)
    {
        var req = new JT809_0x1200
        {
            DataType = 0x1202,
            VehicleNo = plate,
            VehicleColor = (JT809VehicleColorType)color,
            Body = model
        };
        Send(req);
    }

    /// <summary>
    /// 在从链路上发送9300系列指令
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<TResult> Send9300Async<TResult>(Object model) where TResult : class, new()
    {
        var req = new JT809_0x9300
        {
            DataType = 0x1201,
            Body = model
        };
        var rs = await SendAsync<JT809_0x1300>(req);

        return rs.Body as TResult;
    }
    #endregion

    #region 日志
    /// <summary>性能追踪</summary>
    public ITracer Tracer { get; set; }

    /// <summary>日志</summary>
    public ILog Log { get; set; }

    /// <summary>写日志</summary>
    /// <param name="format"></param>
    /// <param name="args"></param>
    public void WriteLog(String format, params Object[] args) => Log?.Info(format, args);
    #endregion
}