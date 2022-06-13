using NewLife.JT809;
using NewLife.JT809.Handlers;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Net;
using NewLife.Security;

namespace JT809Server.Controllers;

internal class InfoController : IJT809Handler
{
    [MessageKind(MessageKinds.接收车辆定位信息数量通知消息)]
    public Object PositionCount(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x9101>();
        XTrace.WriteLine(msg.ToJsonJT809());

        var client = (session as SlaveSession)?.Slave;
        if (client != null)
        {
            Task.Run(async () =>
            {
                var platformId = Rand.NextString(8);
                var start = DateTime.Now.AddSeconds(-Rand.Next(24 * 3600));
                var end = start.AddHours(1);

                await client.Send9102(platformId, start, end);

                var sn = (UInt32)Rand.Next(65536);

                await client.Send9103(02, sn, start);
            });
        }

        return null;
    }
}