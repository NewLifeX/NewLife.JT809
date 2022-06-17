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
    public Object J9300(INetSession session, JTMessage message)
    {
        var msg = message.ReadModel<JT809_0x9300>();
        XTrace.WriteLine(msg.ToJsonJT809());

        XTrace.WriteLine("子业务[{0:X4}]：{1}", msg.DataType, (MessageKinds)msg.DataType);

        //return new JT809_0x1400_0x1401
        //{
        //    MsgSn = msg.SupervisionID,
        //    Result = JT809_0x1401_Result.已处理完毕
        //};
        return null;
    }
}