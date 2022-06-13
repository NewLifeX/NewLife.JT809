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
    public Object Login(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x9001>();
        XTrace.WriteLine(msg.ToJsonJT809());

        return new JT809_0x9002 { Result = JT809_0x9002_Result.成功 };
    }
}