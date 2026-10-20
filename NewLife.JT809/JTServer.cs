using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using NewLife.Data;
using NewLife.JT809.Handlers;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Net;
using NewLife.Reflection;

namespace NewLife.JT809;

/// <summary>JT809服务器</summary>
public class JTServer : NetServer<JTSession>
{
    private readonly JTCodec _codec = new();

    /// <summary>配置信息</summary>
    public JTConfig Config
    {
        get => _codec.Config ??= new JTConfig { Version = JTVersion.JT2011 };
        set => _codec.Config = value;
    }

    /// <summary>
    /// 消息处理器集合
    /// </summary>
    public Dictionary<MessageKinds, MessageHandler> Handlers { get; set; } = new();

    /// <summary>
    /// 实例化JT809服务器
    /// </summary>
    public JTServer()
    {
        Port = 809;
        Config = new JTConfig { Version = JTVersion.JT2011 };
        Protocol = _codec;
    }

    /// <summary>注册类型处理器</summary>
    /// <typeparam name="T"></typeparam>
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

    /// <summary>
    /// 启动时扫描注册所有消息
    /// </summary>
    protected override void OnStart()
    {
        WriteLog("Supports protocol version detection (JTT809-2011/2019) from the login request");

        MessageFactory.Instance.Init();

        base.OnStart();
    }
}

/// <summary>JT809网络会话</summary>
public class JTSession : NetSession<JTServer>, IJTSession
{
    /// <summary>下级平台接入码。上级平台给下级平台分配的唯一标识号</summary>
    public UInt32 CenterId { get; set; }

    /// <summary>配置信息。记录协议版本</summary>
    public JTConfig Config { get; set; }

    private Int32 _gid;
    //private readonly MessageKinds[] _nologs = new[] { MessageKinds.主链路连接保持请求消息 };
    private readonly MessageKinds[] _nologs = new MessageKinds[] { };
    /// <summary>接收指令</summary>
    /// <param name="e"></param>
    protected override void OnReceive(ReceivedEventArgs e)
    {
        var debug = XTrace.Log.Level <= LogLevel.Debug;
#if DEBUG
        debug = true;
#endif

        if (e.Message is JTMessage msg)
        {
            if (msg.CenterId > 0) CenterId = msg.CenterId;

            if (debug && !_nologs.Contains(msg.Kind)) WriteLog("<= {0}", msg);

            // 根据登录请求猜测协议版本，后续指令自适应
            if (Config == null)
            {
                Config = Host.Config.Clone();
                //Config.Version = msg.JTT2019 ? JTVersion.JTT2019 : JTVersion.JTT2011;
                if (msg.Version >= JTVersion.JT2019)
                {
                    Config.Version = msg.Version;

                    WriteLog("Detected protocol version {1} of center [{0}]", msg.CenterId, Config.Version);
                }

                this["Config"] = Config;
            }

            Object result = null;
            using var span = (this as INetSession).Host.Tracer?.NewSpan($"jt809:Receive:{msg.Kind}", e.Packet);
            try
            {
                result = Process(msg);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);

                var hex = e.Packet?.ToHex(1024);
                span?.SetError(ex, hex);

                XTrace.WriteLine("{0}", hex);
            }

            // 处理响应
            if (result != null)
            {
                if (debug) WriteLog("Reply {0}", result.ToJsonJT809());

                var rs = msg.CreateReply();
                rs.Sequence = (UInt32)Interlocked.Increment(ref _gid);
                rs.SetResult(result);
                if (Config != null) rs.VersionFlag = Config.VersionFlag;

                if (debug && !_nologs.Contains(msg.Kind)) WriteLog("=> {0}", rs);

                Session.SendMessage(rs);
            }
        }

        // 父级 OnReceive 触发事件，调用 NetServer.OnReceive
        base.OnReceive(e);
    }

    /// <summary>
    /// 处理消息
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    protected virtual Object Process(JTMessage message)
    {
        if (this is INetSession ss && ss.Host is JTServer js)
        {
            // 找到处理器
            if (!js.Handlers.TryGetValue(message.Kind, out var handler))
                throw new NotSupportedException(L.T("Unsupported command [{0}]", message.Kind));

            // 执行处理器
            return handler(this, message);
        }

        return null;
    }

    /// <summary>
    /// 处理错误
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void OnError(Object sender, ExceptionEventArgs e) => XTrace.WriteException(e.Exception);

    #region 发送
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

        using var span = Host.Tracer?.NewSpan($"jt809:Send:{msg.Kind}", body);

        WriteLog("=>{0}", body.ToJsonJT809());
        WriteLog("=>{0}", msg);

        SendMessage(msg);
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

        using var span = Host.Tracer?.NewSpan($"jt809:SendAsync:{msg.Kind}", body);

        WriteLog("=>{0}", body.ToJsonJT809());
        WriteLog("=>{0}", msg);

        var rs = await SendMessageAsync(msg) as JTMessage;

        WriteLog("<={0}", rs);

        var result = rs.ReadModel<TResult>();
        WriteLog("<={0}", result.ToJsonJT809());

        return result;
    }
    #endregion
}