using NewLife.JT809;
using NewLife.JT809.Handlers;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Net;

namespace JT809Client.Controllers;

internal class InfoController : IJT809Handler
{
    [MessageKind(MessageKinds.平台链路连接情况与车辆定位消息传输情况上报请求消息_2019)]
    public Object Inf_9102(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x9102>();
        XTrace.WriteLine(msg.ToJsonJT809());

        return new JT809_0x1102
        {
            PlateformId = msg.PlatformId,
            StartTime = msg.StartTime,
            EndTime = msg.EndTime,
        };
    }

    [MessageKind(MessageKinds.下发平台间消息序列号通知消息_2019)]
    public Object Info_9103(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x9103>();
        XTrace.WriteLine(msg.ToJsonJT809());

        return new JT809_0x1103
        {
            ManageMsgSNInform = msg.ManageMsgSNInform,
        };
    }
}