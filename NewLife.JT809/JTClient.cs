using NewLife.JT809.Messages;
using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;
using NewLife.Log;
using NewLife.Net;

namespace NewLife.JT809;

/// <summary>JT809客户端</summary>
public class JTClient : DisposeBase
{
    #region 属性
    /// <summary>名称</summary>
    public String Name { get; set; }

    /// <summary>服务端地址</summary>
    public NetUri Server { get; set; }

    /// <summary>配置信息</summary>
    public JTConfig Config { get; set; } = new JTConfig { Version = JTVersion.JTT2019 };

    /// <summary>下级平台接入码。上级平台给下级平台分配的唯一标识号</summary>
    public UInt32 CenterId { get; set; }

    private ISocketClient _client;
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

        _client.TryDispose();
    }
    #endregion

    #region 方法
    /// <summary>
    /// 打开连接
    /// </summary>
    public void Open()
    {
        MessageFactory.Instance.Init();

        var client = Server.CreateRemote();
        client.Name = Name;
        client.Add<JTCodec>();
        client.Tracer = Tracer;
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

        if (Config != null && Config.Version == JTVersion.JTT2019) msg.JTT2019 = true;

        msg.SetResult(body);

        using var span = Tracer?.NewSpan($"jt809:Send:{msg.Kind}", body);

        WriteLog("=>{0}", body.ToJsonJT809());
        WriteLog("=>{0}", msg);

        _client.SendMessage(msg);
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

        if (Config != null && Config.Version == JTVersion.JTT2019) msg.JTT2019 = true;

        msg.SetResult(body);

        using var span = Tracer?.NewSpan($"jt809:SendAsync:{msg.Kind}", body);

        WriteLog("=>{0}", body.ToJsonJT809());
        WriteLog("=>{0}", msg);

        var rs = await _client.SendMessageAsync(msg) as JTMessage;

        WriteLog("<={0}", rs);

        var result = rs.ReadModel<TResult>();
        WriteLog("<={0}", result.ToJsonJT809());

        return result;
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

        CenterId = model.MsgGNSSCENTERID;

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
        if (rs == null || rs.Result != JT809_0x9002_Result.成功) throw new Exception($"登录失败！{rs?.Result}");

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
    public async Task<JT809_0x9200_0x9201> RegisterAsync(String plate, Int32 color, JT809_0x1200_0x1201 model)
    {
        var req = new JT809_0x1200
        {
            DataType = 0x1201,
            VehicleNo = plate,
            VehicleColor = (JT809VehicleColorType)color,
            Body = model
        };
        return await SendAsync<JT809_0x9200_0x9201>(req);
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