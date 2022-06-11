using System.Collections.Concurrent;
using System.Reflection;
using NewLife.JT809;
using NewLife.Reflection;
using NewLife.Serialization;

namespace NewLife.JT809.Protocols;

/// <summary>消息工厂</summary>
public class MessageFactory
{
    #region 静态
    /// <summary>静态实例</summary>
    public static MessageFactory Instance { get; set; } = new();
    #endregion

    #region 属性
    ConcurrentDictionary<MessageKinds, Type> _map1 = new();
    ConcurrentDictionary<Type, MessageKinds> _map2 = new();

    /// <summary>类型映射</summary>
    public IDictionary<MessageKinds, Type> Maps => _map1;

    /// <summary>类型映射</summary>
    public IDictionary<Type, MessageKinds> Types => _map2;
    #endregion

    #region 方法
    /// <summary>初始化，加载内部</summary>
    public void Init()
    {
        //var ns = typeof(PingMessage).Namespace;
        //foreach (var item in GetType().Assembly.GetTypes())
        //{
        //    if (item.Namespace == ns)
        //    {
        //        var att = item.GetCustomAttribute<MessageKindAttribute>();
        //        if (att != null) Register(att.Kind, item);
        //    }
        //}
    }

    /// <summary>注册新类型</summary>
    /// <param name="kind"></param>
    /// <param name="type"></param>
    public void Register(MessageKinds kind, Type type)
    {
        _map1[kind] = type;
        _map2[type] = kind;
    }

    /// <summary>注册新类型</summary>
    /// <typeparam name="T"></typeparam>
    public void Register<T>()
    {
        var type = typeof(T);
        var att = type.GetCustomAttribute<MessageKindAttribute>();
        if (att != null) Register(att.Kind, type);
    }

    /// <summary>创建指定类型的模型对象</summary>
    /// <param name="kind"></param>
    /// <returns></returns>
    public Object Create(MessageKinds kind)
    {
        if (!_map1.TryGetValue(kind, out var type)) return null;

        return type.CreateInstance();
    }

    /// <summary>解析消息包，得到业务模型</summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public Object Parse(JTMessage message)
    {
        var msg = Create((MessageKinds)message.Kind);
        if (msg == null) return null;

        var pk = message.Payload;
        if (pk != null && pk.Total > 0)
        {
            var ms = pk.GetStream();
            if (msg is IAccessor accessor)
                accessor.Read(ms, message);
            else
                Helper.CreateReader(ms).TryRead(null, ref msg);

#if DEBUG
            if (ms.Position < ms.Length) throw new Exception($"解析未完成，可用[{ms.Length}]，实际[{ms.Position}]");
#endif
        }

        return msg;
    }
    #endregion
}