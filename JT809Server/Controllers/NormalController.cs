using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Net;
using NewLife.Security;

namespace JT809Server.Controllers;

internal class NormalController : IJT809Handler
{
    [MessageKind(MessageKinds.主链路登录请求消息)]
    public Object Login(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x1001>();
        XTrace.WriteLine(msg.ToJsonJT809());

        return new JT809_0x1002 { Result = JT809_0x1002_Result.成功, VerifyCode = (UInt32)Rand.Next() };
    }

    [MessageKind(MessageKinds.主链路注销请求消息)]
    public Object Logout(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x1003>();
        XTrace.WriteLine(msg.ToJsonJT809());

        return new JT809_0x1004();
    }

    [MessageKind(MessageKinds.主链路连接保持请求消息)]
    public Object Ping(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x1005>();
        XTrace.WriteLine(msg.ToJsonJT809());

        return new JT809_0x1006();
    }
}