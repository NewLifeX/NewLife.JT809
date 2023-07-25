using NewLife.JT809;
using NewLife.JT809.Handlers;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Net;

namespace JT809Client.Controllers;

internal class NormalController : IJT809Handler
{
    [MessageKind(MessageKinds.从链路连接请求消息)]
    public Object Login(IJTSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x9001>();
        XTrace.WriteLine(msg.ToJsonJT809());

        return new JT809_0x9002 { Result = JT809_0x9002_Result.成功 };
    }

    [MessageKind(MessageKinds.从链路注销请求消息)]
    public Object Logout(IJTSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x9003>();
        XTrace.WriteLine(msg.ToJsonJT809());

        return new JT809_0x9004();
    }

    [MessageKind(MessageKinds.从链路连接保持请求消息)]
    public Object Ping(IJTSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x9005>();
        XTrace.WriteLine(msg.ToJsonJT809());

        return new JT809_0x9006();
    }
}