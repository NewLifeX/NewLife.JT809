using NewLife.JT809;
using NewLife.JT809.Handlers;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;
using NewLife.Log;
using NewLife.Net;

namespace JT809Client.Controllers;

internal class J9300Controller : IJT809Handler
{
    [MessageKind(MessageKinds.从链路平台间信息交互业务)]
    public Object J9300(IJTSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x9300>();
        XTrace.WriteLine(msg.ToJsonJT809());

        XTrace.WriteLine("子业务[{0:X4}]：{1}", msg.DataType, (MessageKinds)msg.DataType);

        switch (msg.DataType)
        {
            case 0x9301:
                var j9301 = msg.Body as JT809_0x9300_0x9301;
                var model = new JT809_0x1300_0x1301
                {
                    ObjectType = (JT809_0x1301_ObjectType)j9301.ObjectType,
                    ObjectID = j9301.ObjectID,

                    Responder = "张三",
                    ResponderTel = "13012345678",

                    InfoContent = $"[{j9301.ObjectID}]不在，我不明白这个问题\"{j9301.InfoContent}\"",
                };
                return new JT809_0x1300
                {
                    DataType = 0x1301,
                    Body = model,
                };
            default:
                break;
        }

        return null;
    }
}