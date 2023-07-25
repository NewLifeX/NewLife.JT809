using NewLife.JT809;
using NewLife.JT809.Handlers;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Net;

namespace JT809Server.Controllers;

internal class J1600Controller : IJT809Handler
{
    [MessageKind(MessageKinds.主链路静态信息交换消息)]
    public Object J1600(IJTSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x1600>();
        XTrace.WriteLine(msg.ToJsonJT809());

        XTrace.WriteLine("子业务[{0:X4}]：{1}", msg.DataType, (MessageKinds)msg.DataType);

        return null;
    }
}