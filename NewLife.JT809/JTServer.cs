using System.Reflection;
using NewLife.JT809.Handlers;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Net;
using NewLife.Reflection;

namespace NewLife.JT809;

/// <summary>JT809服务器</summary>
public class JTServer : NetServer<JTSession>
{
    /// <summary>配置信息</summary>
    public JTConfig Config { get; set; } = new JTConfig { Version = JTVersion.JTT2019 };

    /// <summary>
    /// 消息处理器集合
    /// </summary>
    public Dictionary<MessageKinds, MessageHandler> Handlers = new();

    /// <summary>
    /// 实例化JT809服务器
    /// </summary>
    public JTServer()
    {
        Port = 809;
        Add<JTCodec>();
    }

    /// <summary>注册类型处理器</summary>
    /// <typeparam name="T"></typeparam>
    public void AddHandler<T>(T handler)
    {
        var type = handler.GetType();
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
                Handlers[att.Kind] = item.As<MessageHandler>(handler);

                WriteLog("注册[{0}]：{1}.{2}", att.Kind, item.DeclaringType.FullName, item.Name);
            }
        }
    }

    /// <summary>
    /// 启动时扫描注册所有消息
    /// </summary>
    protected override void OnStart()
    {
        WriteLog("支持根据登录请求识别协议版本 JTT809-2011/2019");

        MessageFactory.Instance.Init();

        base.OnStart();
    }
}

/// <summary>JT809网络会话</summary>
public class JTSession : NetSession<JTServer>
{
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
            if (debug && !_nologs.Contains(msg.Kind)) WriteLog("<= {0}", msg);

            // 根据登录请求猜测协议版本，后续指令自适应
            if (Config == null)
            {
                Config = Host.Config.Clone();
                Config.Version = msg.JTT2019 ? JTVersion.JTT2019 : JTVersion.JTT2011;
                this["Config"] = Config;

                WriteLog("自动识别[{0}]协议版本：{1}", msg.CenterId, Config.Version);
            }

            Object result = null;
            using var span = (this as INetSession).Host.Tracer?.NewSpan($"jt809:{msg.Kind}", e.Packet);
            try
            {
                result = Process(msg);
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);

                var hex = e.Packet?.ToHex(1024);
                span?.SetError(ex, hex);

                XTrace.WriteLine(hex);
            }

            // 处理响应
            if (result != null)
            {
                if (debug) WriteLog("响应 {0}", result.ToJsonJT809());

                var rs = msg.CreateReply();
                rs.Sequence = (UInt16)Interlocked.Increment(ref _gid);
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
                throw new NotSupportedException($"未支持指令[{message.Kind}]");

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
}