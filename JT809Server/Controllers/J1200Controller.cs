using NewLife.JT809;
using NewLife.JT809.Handlers;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;
using NewLife.Log;
using NewLife.Net;

namespace JT809Server.Controllers;

internal class J1200Controller : IJT809Handler
{
    [MessageKind(MessageKinds.主链路车辆动态信息交换业务)]
    public Object J1200(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x1200>();
        XTrace.WriteLine(msg.ToJsonJT809());

        XTrace.WriteLine("子业务[{0:X4}]：{1}", msg.DataType, (MessageKinds)msg.DataType);

        switch (msg.DataType)
        {
            case 0x1201:
                var j1201 = msg.Body as JT809_0x1200_0x1201;
                return new JT809_0x9200_0x9201
                {
                    DataType = 0x9201,
                    MsgSn = message.Sequence,
                    Result = JT809_0x9201_Result.审核通过_完成注册
                };
            case 0x1202:
                var j1202 = msg.Body as JT809_0x1200_0x1202;
                XTrace.WriteLine(msg.ToJsonJT809());
                break;
            default:
                break;
        }

        return null;
    }
}