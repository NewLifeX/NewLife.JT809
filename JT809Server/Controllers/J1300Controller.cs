using NewLife.JT809;
using NewLife.JT809.Handlers;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Net;

namespace JT809Server.Controllers;

internal class J1300Controller : IJT809Handler
{
    [MessageKind(MessageKinds.主链路平台间信息交互消息)]
    public Object J1300(IJTSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x1300>();
        XTrace.WriteLine(msg.ToJsonJT809());

        XTrace.WriteLine("子业务[{0:X4}]：{1}", msg.DataType, (MessageKinds)msg.DataType);

        return null;
    }
}