using NewLife.JT809;
using NewLife.JT809.Handlers;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;
using NewLife.Log;
using NewLife.Net;

namespace JT809Client.Controllers;

internal class J9600Controller : IJT809Handler
{
    [MessageKind(MessageKinds.补报车辆静态信息请求消息)]
    public Object J9600(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x9600>();
        XTrace.WriteLine(msg.ToJsonJT809());

        var model = msg.Body as JT809_0x9600_0x9601;

        return new JT809_0x1600_0x1601
        {
            SourceDataType = msg.DataType,
            SourceMsgSn = message.Sequence,
        };
    }
}