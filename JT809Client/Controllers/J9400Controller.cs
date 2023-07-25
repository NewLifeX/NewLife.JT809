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
    public Object J9400(IJTSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x9400>();
        XTrace.WriteLine(msg.ToJsonJT809());

        XTrace.WriteLine("子业务[{0:X4}]：{1}", msg.DataType, (MessageKinds)msg.DataType);

        switch (msg.DataType)
        {
            case 0x9401:
                var j9401 = msg.Body as JT809_0x9400_0x9401;
                return new JT809_0x1400_0x1401
                {
                    MsgSn = j9401.SupervisionID,
                    Result = JT809_0x1401_Result.已处理完毕
                };
            default:
                break;
        }

        return null;
    }
}