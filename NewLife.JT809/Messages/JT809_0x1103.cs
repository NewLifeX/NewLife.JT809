using NewLife.JT809.Models;
using NewLife.JT809.Protocols;

namespace NewLife.JT809.Messages;

/// <summary>
///  上传平台间消息序列号通知消息
/// <para>链路类型：主链路</para>
/// <para>消息方向:上级平台往下级平台</para>
/// <para>业务类型标识:UP_MANAGE_MSG_SN_INFORM</para>
/// <para>描述：链路登录成功后，平台须发送链路断开之前所有子业务数据类型对应的消息序列号，发送方根据收到的消息序列号，在发送消息时，续编链路中断之前的消息序列号</para>
/// </summary>
[MessageKind(MessageKinds.上传平台间消息序列号通知消息_2019)]
public class JT809_0x1103 : JT809ExchangeMessageBodies
{
    /// <summary>信息列表</summary>
    public List<JT809ManageMsgSNInform> ManageMsgSNInform { get; set; } = new List<JT809ManageMsgSNInform>();
    //public byte Count { get; set; }

    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override Boolean Read(Stream stream, Object context)
    {
        if (!base.Read(stream, context)) return false;

        var pk = Payload;
        if (pk == null) return false;

        var reader = Helper.CreateReader(pk.GetStream());
        var count = reader.ReadByte();
        for (var i = 0; i < count; i++)
        {
            var msg = reader.Read<JT809ManageMsgSNInform>();
            ManageMsgSNInform.Add(msg);
        }

        return true;
    }

    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override Boolean Write(Stream stream, Object context)
    {
        var ms = new MemoryStream();
        var writer = Helper.CreateWriter(ms);

        var count = ManageMsgSNInform.Count;
        writer.Write((Byte)count);
        for (var i = 0; i < count; i++)
        {
            writer.Write(ManageMsgSNInform[i]);
        }

        Payload = ms.ToArray();

        return base.Write(stream, context);
    }
}