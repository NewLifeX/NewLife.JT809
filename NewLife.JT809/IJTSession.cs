using NewLife.JT809.Messages;
using NewLife.JT809.Models;
using NewLife.JT809.Protocols;

namespace NewLife.JT809;

/// <summary>JT809会话，客户端或服务端连接</summary>
public interface IJTSession
{
    /// <summary>下级平台接入码。上级平台给下级平台分配的唯一标识号</summary>
    UInt32 CenterId { get; set; }

    /// <summary>配置信息</summary>
    JTConfig Config { get; set; }

    /// <summary>发送不等待</summary>
    /// <param name="body"></param>
    void Send(Object body);

    /// <summary>发送并等待响应</summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="body"></param>
    /// <returns></returns>
    Task<TResult> SendAsync<TResult>(Object body) where TResult : class, new();

    ///// <summary>发送消息，不等待响应</summary>
    ///// <param name="message"></param>
    ///// <returns></returns>
    //Int32 SendMessage(Object message);

    ///// <summary>异步发送消息并等待响应</summary>
    ///// <param name="message"></param>
    ///// <returns></returns>
    //Task<Object> SendMessageAsync(Object message);
}

/// <summary>JT809会话扩展</summary>
public static class JTSessionExtensions
{
    /// <summary>提交车辆动态信息</summary>
    /// <param name="session"></param>
    /// <param name="plate"></param>
    /// <param name="color"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static void Send1200(this IJTSession session, String plate, Int32 color, Object model)
    {
        var req = new JT809_0x1200
        {
            //DataType = 0x1201,
            VehicleNo = plate,
            VehicleColor = (JT809VehicleColorType)color,
            Body = model
        };

        // 指令表查找替代特性反射，AOT 下无需额外保留特性元数据
        if (!MessageFactory.Instance.Types.TryGetValue(model.GetType(), out var kind))
            throw new ArgumentException(L.T("Unsupported type [{0}]", model.GetType()), nameof(model));

        req.DataType = (UInt16)kind;

        session.Send(req);
    }

    /// <summary>提交车辆静态信息</summary>
    /// <param name="session"></param>
    /// <param name="plate"></param>
    /// <param name="color"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static void Send1600(this IJTSession session, String plate, Int32 color, Object model)
    {
        var req = new JT809_0x1600
        {
            //DataType = 0x1601,
            VehicleNo = plate,
            VehicleColor = (JT809VehicleColorType)color,
            Body = model
        };

        // 指令表查找替代特性反射，AOT 下无需额外保留特性元数据
        if (!MessageFactory.Instance.Types.TryGetValue(model.GetType(), out var kind))
            throw new ArgumentException(L.T("Unsupported type [{0}]", model.GetType()), nameof(model));

        req.DataType = (UInt16)kind;

        session.Send(req);
    }
}