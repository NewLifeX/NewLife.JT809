using System.Collections.Concurrent;
using System.Reflection;
using NewLife.Data;
using NewLife.JT809.Messages;
using NewLife.JT809.Models;
using NewLife.JT809.SubMessages;
using NewLife.Log;
using NewLife.Reflection;
using NewLife.Serialization;

namespace NewLife.JT809.Protocols;

/// <summary>消息工厂</summary>
/// <remarks>
/// 内置消息以显式清单注册，不使用反射扫描程序集，既避免 AOT/裁剪下的类型丢失，也让启动更快、失败更早。
/// 业务方通过 <see cref="Register(MessageKinds, Type, Func{Object})"/> 追加自定义指令。
/// </remarks>
public class MessageFactory
{
    #region 静态
    /// <summary>静态实例</summary>
    public static MessageFactory Instance { get; set; } = new();
    #endregion

    #region 属性
    private readonly ConcurrentDictionary<MessageKinds, Func<Object>> _factories = new();
    private readonly ConcurrentDictionary<MessageKinds, Type> _map1 = new();
    private readonly ConcurrentDictionary<Type, MessageKinds> _map2 = new();
    private Boolean _inited;

    /// <summary>类型映射</summary>
    public IDictionary<MessageKinds, Type> Maps => _map1;

    /// <summary>类型映射</summary>
    public IDictionary<Type, MessageKinds> Types => _map2;
    #endregion

    #region 方法
    /// <summary>初始化，加载内部</summary>
    public void Init()
    {
        lock (_factories)
        {
            if (_inited) return;
            _inited = true;
        }

        // 内置指令清单。新增指令时在此登记，务必与 MessageKindAttribute 保持一致
        Register(MessageKinds.主链路登录请求消息, typeof(JT809_0x1001), () => new JT809_0x1001());
        Register(MessageKinds.主链路登录应答消息, typeof(JT809_0x1002), () => new JT809_0x1002());
        Register(MessageKinds.主链路注销请求消息, typeof(JT809_0x1003), () => new JT809_0x1003());
        Register(MessageKinds.主链路注销应答消息, typeof(JT809_0x1004), () => new JT809_0x1004());
        Register(MessageKinds.主链路连接保持请求消息, typeof(JT809_0x1005), () => new JT809_0x1005());
        Register(MessageKinds.主链路连接保持应答消息, typeof(JT809_0x1006), () => new JT809_0x1006());
        Register(MessageKinds.主链路断开通知消息, typeof(JT809_0x1007), () => new JT809_0x1007());
        Register(MessageKinds.下级平台主动关闭主从链路通知消息, typeof(JT809_0x1008), () => new JT809_0x1008());
        Register(MessageKinds.平台链路连接情况与车辆定位消息传输情况上报应答消息_2019, typeof(JT809_0x1102), () => new JT809_0x1102());
        Register(MessageKinds.上传平台间消息序列号通知消息_2019, typeof(JT809_0x1103), () => new JT809_0x1103());
        Register(MessageKinds.主链路车辆动态信息交换业务, typeof(JT809_0x1200), () => new JT809_0x1200());
        Register(MessageKinds.主链路平台间信息交互消息, typeof(JT809_0x1300), () => new JT809_0x1300());
        Register(MessageKinds.主链路报警信息交互消息, typeof(JT809_0x1400), () => new JT809_0x1400());
        Register(MessageKinds.主链路车辆监管消息, typeof(JT809_0x1500), () => new JT809_0x1500());
        Register(MessageKinds.主链路静态信息交换消息, typeof(JT809_0x1600), () => new JT809_0x1600());
        Register(MessageKinds.从链路连接请求消息, typeof(JT809_0x9001), () => new JT809_0x9001());
        Register(MessageKinds.从链路连接应答信息, typeof(JT809_0x9002), () => new JT809_0x9002());
        Register(MessageKinds.从链路注销请求消息, typeof(JT809_0x9003), () => new JT809_0x9003());
        Register(MessageKinds.从链路注销应答消息, typeof(JT809_0x9004), () => new JT809_0x9004());
        Register(MessageKinds.从链路连接保持请求消息, typeof(JT809_0x9005), () => new JT809_0x9005());
        Register(MessageKinds.从链路连接保持应答消息, typeof(JT809_0x9006), () => new JT809_0x9006());
        Register(MessageKinds.从链路断开通知消息, typeof(JT809_0x9007), () => new JT809_0x9007());
        Register(MessageKinds.上级平台主动关闭链路通知消息, typeof(JT809_0x9008), () => new JT809_0x9008());
        Register(MessageKinds.接收车辆定位信息数量通知消息, typeof(JT809_0x9101), () => new JT809_0x9101());
        Register(MessageKinds.平台链路连接情况与车辆定位消息传输情况上报请求消息_2019, typeof(JT809_0x9102), () => new JT809_0x9102());
        Register(MessageKinds.下发平台间消息序列号通知消息_2019, typeof(JT809_0x9103), () => new JT809_0x9103());
        Register(MessageKinds.从链路车辆动态信息交换业务, typeof(JT809_0x9200), () => new JT809_0x9200());
        Register(MessageKinds.从链路平台间信息交互业务, typeof(JT809_0x9300), () => new JT809_0x9300());
        Register(MessageKinds.从链路报警信息交互消息, typeof(JT809_0x9400), () => new JT809_0x9400());
        Register(MessageKinds.从链路车辆监管消息, typeof(JT809_0x9500), () => new JT809_0x9500());
        Register(MessageKinds.从链路静态信息交换消息, typeof(JT809_0x9600), () => new JT809_0x9600());
        Register(MessageKinds.上传车辆注册信息, typeof(JT809_0x1200_0x1201), () => new JT809_0x1200_0x1201());
        Register(MessageKinds.实时上传车辆定位信息, typeof(JT809_0x1200_0x1202), () => new JT809_0x1200_0x1202());
        Register(MessageKinds.车辆定位信息自动补报请求消息, typeof(JT809_0x1200_0x1203), () => new JT809_0x1200_0x1203());
        Register(MessageKinds.启动车辆定位信息交换应答消息, typeof(JT809_0x1200_0x1205), () => new JT809_0x1200_0x1205());
        Register(MessageKinds.结束车辆定位信息交换应答消息, typeof(JT809_0x1200_0x1206), () => new JT809_0x1200_0x1206());
        Register(MessageKinds.申请交换指定车辆定位信息请求消息, typeof(JT809_0x1200_0x1207), () => new JT809_0x1200_0x1207());
        Register(MessageKinds.取消交换指定车辆定位信息请求, typeof(JT809_0x1200_0x1208), () => new JT809_0x1200_0x1208());
        Register(MessageKinds.补发车辆定位信息请求, typeof(JT809_0x1200_0x1209), () => new JT809_0x1200_0x1209());
        Register(MessageKinds.上报驾驶员身份识别信息应答消息, typeof(JT809_0x1200_0x120A), () => new JT809_0x1200_0x120A());
        Register(MessageKinds.上报车辆电子运单应答消息, typeof(JT809_0x1200_0x120B), () => new JT809_0x1200_0x120B());
        Register(MessageKinds.主动上报驾驶员身份信息消息, typeof(JT809_0x1200_0x120C), () => new JT809_0x1200_0x120C());
        Register(MessageKinds.主动上报车辆电子运单信息, typeof(JT809_0x1200_0x120D), () => new JT809_0x1200_0x120D());
        Register(MessageKinds.主动上报车辆行驶路线信息, typeof(JT809_0x1200_0x120E), () => new JT809_0x1200_0x120E());
        Register(MessageKinds.平台查岗应答消息, typeof(JT809_0x1300_0x1301), () => new JT809_0x1300_0x1301());
        Register(MessageKinds.下发平台间报文应答消息, typeof(JT809_0x1300_0x1302), () => new JT809_0x1300_0x1302());
        Register(MessageKinds.上传平台间消息补传请求消息, typeof(JT809_0x1300_0x1303), () => new JT809_0x1300_0x1303());
        Register(MessageKinds.报警督办应答消息, typeof(JT809_0x1400_0x1401), () => new JT809_0x1400_0x1401());
        Register(MessageKinds.上报报警信息消息, typeof(JT809_0x1400_0x1402), () => new JT809_0x1400_0x1402());
        Register(MessageKinds.主动上报报警处理结果信息2013_上报报警预警信息2019, typeof(JT809_0x1400_0x1403), () => new JT809_0x1400_0x1403());
        Register(MessageKinds.上报报警督办应答消息, typeof(JT809_0x1400_0x1411), () => new JT809_0x1400_0x1411());
        Register(MessageKinds.主动上报报警处理结果消息, typeof(JT809_0x1400_0x1412), () => new JT809_0x1400_0x1412());
        Register(MessageKinds.上报报警督办请求消息, typeof(JT809_0x1400_0x1413), () => new JT809_0x1400_0x1413());
        Register(MessageKinds.车辆单向监听应答, typeof(JT809_0x1500_0x1501), () => new JT809_0x1500_0x1501());
        Register(MessageKinds.车辆拍照应答, typeof(JT809_0x1500_0x1502), () => new JT809_0x1500_0x1502());
        Register(MessageKinds.下发车辆报文应答消息, typeof(JT809_0x1500_0x1503), () => new JT809_0x1500_0x1503());
        Register(MessageKinds.上报车辆行驶记录应答消息, typeof(JT809_0x1500_0x1504), () => new JT809_0x1500_0x1504());
        Register(MessageKinds.车辆应急接入监管平台应答消息, typeof(JT809_0x1500_0x1505), () => new JT809_0x1500_0x1505());
        Register(MessageKinds.补报车辆静态信息应答, typeof(JT809_0x1600_0x1601), () => new JT809_0x1600_0x1601());
        Register(MessageKinds.补报车辆行驶路线信息应答消息, typeof(JT809_0x1600_0x1602), () => new JT809_0x1600_0x1602());
        Register(MessageKinds.车辆注册信息应答消息, typeof(JT809_0x9200_0x9201), () => new JT809_0x9200_0x9201());
        Register(MessageKinds.交换车辆定位信息消息, typeof(JT809_0x9200_0x9202), () => new JT809_0x9200_0x9202());
        Register(MessageKinds.车辆定位信息交换补发消息, typeof(JT809_0x9200_0x9203), () => new JT809_0x9200_0x9203());
        Register(MessageKinds.交换车辆静态信息消息, typeof(JT809_0x9200_0x9204), () => new JT809_0x9200_0x9204());
        Register(MessageKinds.启动车辆定位信息交换请求消息, typeof(JT809_0x9200_0x9205), () => new JT809_0x9200_0x9205());
        Register(MessageKinds.结束车辆定位信息交换请求, typeof(JT809_0x9200_0x9206), () => new JT809_0x9200_0x9206());
        Register(MessageKinds.申请交换指定车辆定位信息应答, typeof(JT809_0x9200_0x9207), () => new JT809_0x9200_0x9207());
        Register(MessageKinds.取消交换指定车辆定位信息应答or取消申请交换指定车辆定位信息应答消息, typeof(JT809_0x9200_0x9208), () => new JT809_0x9200_0x9208());
        Register(MessageKinds.补发车辆定位信息应答, typeof(JT809_0x9200_0x9209), () => new JT809_0x9200_0x9209());
        Register(MessageKinds.上报车辆驾驶员身份识别信息请求, typeof(JT809_0x9200_0x920A), () => new JT809_0x9200_0x920A());
        Register(MessageKinds.上报车辆电子运单请求消息, typeof(JT809_0x9200_0x920B), () => new JT809_0x9200_0x920B());
        Register(MessageKinds.上报车辆车辆行驶路线请求, typeof(JT809_0x9200_0x920C), () => new JT809_0x9200_0x920C());
        Register(MessageKinds.车辆行驶线路请求应答, typeof(JT809_0x9200_0x920D), () => new JT809_0x9200_0x920D());
        Register(MessageKinds.平台查岗请求, typeof(JT809_0x9300_0x9301), () => new JT809_0x9300_0x9301());
        Register(MessageKinds.下发平台间报文请求, typeof(JT809_0x9300_0x9302), () => new JT809_0x9300_0x9302());
        Register(MessageKinds.下发平台间消息补传请求消息, typeof(JT809_0x9300_0x9303), () => new JT809_0x9300_0x9303());
        Register(MessageKinds.报警督办请求消息, typeof(JT809_0x9400_0x9401), () => new JT809_0x9400_0x9401());
        Register(MessageKinds.报警预警2013_下发报警预警消息2019, typeof(JT809_0x9400_0x9402), () => new JT809_0x9400_0x9402());
        Register(MessageKinds.实时交换报警信息, typeof(JT809_0x9400_0x9403), () => new JT809_0x9400_0x9403());
        Register(MessageKinds.车辆单向监听请求消息, typeof(JT809_0x9500_0x9501), () => new JT809_0x9500_0x9501());
        Register(MessageKinds.车辆拍照请求消息, typeof(JT809_0x9500_0x9502), () => new JT809_0x9500_0x9502());
        Register(MessageKinds.下发车辆报文请求, typeof(JT809_0x9500_0x9503), () => new JT809_0x9500_0x9503());
        Register(MessageKinds.上报车辆行驶记录请求消息, typeof(JT809_0x9500_0x9504), () => new JT809_0x9500_0x9504());
        Register(MessageKinds.车辆应急接入监管平台请求消息, typeof(JT809_0x9500_0x9505), () => new JT809_0x9500_0x9505());
        Register(MessageKinds.补报车辆静态信息请求消息, typeof(JT809_0x9600_0x9601), () => new JT809_0x9600_0x9601());


    }

    /// <summary>注册新类型</summary>
    /// <param name="kind">业务数据类型</param>
    /// <param name="type">消息模型类型</param>
    /// <param name="factory">实例工厂，为空时使用无参构造反射创建</param>
    public void Register(MessageKinds kind, Type type, Func<Object> factory = null)
    {
        if (_map1.TryGetValue(kind, out var t) && t != type)
            XTrace.WriteLine("Command [{0:X4}] is already registered to {1}; re-registering to {2}", (Int32)kind, t.Name, type.Name);

        _map1[kind] = type;
        _map2[type] = kind;
        _factories[kind] = factory ?? NewFactory(type);
    }

    /// <summary>注册新类型</summary>
    /// <typeparam name="T">消息模型类型</typeparam>
    public void Register<T>() where T : new()
    {
        var type = typeof(T);
        var att = type.GetCustomAttribute<MessageKindAttribute>();
        if (att != null) Register(att.Kind, type, () => new T());
    }

    /// <summary>创建指定类型的模型对象</summary>
    /// <param name="kind">业务数据类型</param>
    /// <returns>消息模型，未注册时返回 null</returns>
    public Object Create(MessageKinds kind) => _factories.TryGetValue(kind, out var factory) ? factory() : null;

    /// <summary>解析消息包，得到业务模型</summary>
    /// <param name="message">报文消息</param>
    /// <returns>业务模型，未注册时返回 null</returns>
    public Object Parse(JTMessage message)
    {
        var msg = Create(message.Kind);
        if (msg == null) return null;

        var pk = message.Payload;
        if (pk != null && pk.Total > 0)
        {
            var ms = pk.GetStream();
            if (msg is IAccessor accessor)
                accessor.Read(ms, message);
            else
                Helper.CreateReader(ms, message).TryRead(null, ref msg);

#if DEBUG
            if (ms.Position < ms.Length) XTrace.WriteLine("Parsing incomplete: {0} bytes available but only {1} bytes were used", ms.Length, ms.Position);
#endif
        }

        return msg;
    }

    /// <summary>是否响应消息</summary>
    /// <param name="kind">业务数据类型</param>
    /// <returns></returns>
    public Boolean IsReply(MessageKinds kind)
    {
        return kind switch
        {
            MessageKinds.主链路登录应答消息 or
            MessageKinds.主链路注销应答消息 or
            MessageKinds.主链路连接保持应答消息 or
            MessageKinds.从链路连接应答信息 or
            MessageKinds.从链路注销应答消息 or
            MessageKinds.从链路连接保持应答消息 or
            MessageKinds.平台链路连接情况与车辆定位消息传输情况上报应答消息_2019 => true,
            MessageKinds.上传平台间消息序列号通知消息_2019 => true,
            MessageKinds.报警督办应答消息 => true,
            MessageKinds.上报报警督办应答消息 => true,
            MessageKinds.车辆注册信息应答消息 => true,
            _ => false,
        };
    }

    private static Func<Object> NewFactory(Type type) => () => type.CreateInstance();
    #endregion
}