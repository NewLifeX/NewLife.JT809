using NewLife.JT809.Protocols;
using NewLife.Net;

namespace NewLife.JT809.Handlers;

/// <summary>消息处理委托</summary>
/// <param name="session"></param>
/// <param name="message"></param>
/// <returns></returns>
public delegate Object MessageHandler(INetSession session, JTMessage message);
