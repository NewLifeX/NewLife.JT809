using NewLife.JT809;
using NewLife.JT809.Handlers;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Net;

namespace JT809Client.Controllers;

internal class J9200Controller : IJT809Handler
{
    [MessageKind(MessageKinds.从链路车辆动态信息交换业务)]
    public Object J9200(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x9200>();
        XTrace.WriteLine(msg.ToJsonJT809());

        XTrace.WriteLine("子业务[{0:X4}]：{1}", msg.DataType, (MessageKinds)msg.DataType);

        return null;
    }
}