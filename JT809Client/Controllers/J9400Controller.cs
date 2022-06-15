using NewLife.JT809;
using NewLife.JT809.Handlers;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;
using NewLife.Log;
using NewLife.Net;

namespace JT809Client.Controllers;

internal class J9400Controller : IJT809Handler
{
    [MessageKind(MessageKinds.从链路报警信息交互消息)]
    public Object J9400(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x9400_0x9401>();
        XTrace.WriteLine(msg.ToJsonJT809());

        return new JT809_0x1400_0x1401
        {
            SupervisionID = msg.SupervisionID,
            Result = JT809_0x1401_Result.已处理完毕
        };
    }
}