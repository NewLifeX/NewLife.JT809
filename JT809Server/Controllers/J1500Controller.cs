using NewLife.JT809;
using NewLife.JT809.Handlers;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Net;

namespace JT809Server.Controllers;

internal class J1500Controller : IJT809Handler
{
    [MessageKind(MessageKinds.主链路车辆监管消息)]
    public Object J1500(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x1500>();
        XTrace.WriteLine(msg.ToJsonJT809());

        XTrace.WriteLine("子业务[{0:X4}]：{1}", msg.DataType, (MessageKinds)msg.DataType);

        return null;
    }
}