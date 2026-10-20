using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.Messages;

/// <summary>
/// 下发平台间消息序列号通知消息
/// </summary>
/// <remarks>
/// 链路类型：从链路
/// 消息方向:上级平台往下级平台
/// 业务类型标识：DOWN_MANAGE_MSG_SN_INFORM
/// 链路登录成功后，平台须发送链路断开之前所有子业务数据类型对应的消息序列号，发送方根据收到的消息序列号，在发送消息时，续编链路中断之前的消息序列号
/// </remarks>
[MessageKind(MessageKinds.下发平台间消息序列号通知消息_2019)]
public class JT809_0x9103 : IAccessor
{
    /// <summary>
    /// 子业务类型标识
    /// </summary>
    public UInt16 DataType { get; set; }

    /// <summary>
    /// 数据长度
    /// </summary>
    public UInt32 DataLength { get; set; }

    /// <summary>列表</summary>
    public List<JT809ManageMsgSNInform> Messages { get; set; } = new List<JT809ManageMsgSNInform>();

    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        DataType = reader.ReadUInt16();
        DataLength = reader.ReadUInt32();

        Messages.Clear();
        var count = reader.ReadByte();
        for (var i = 0; i < count; i++)
        {
            var msg = reader.Read<JT809ManageMsgSNInform>();
            Messages.Add(msg);
        }

        return true;
    }

    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Write(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var writer = Helper.CreateWriter(stream, header);

        writer.Write(DataType);
        //writer.Write(DataLength);

        // 必须先序列化后面，需要用到长度
        var ms = new MemoryStream();
        var writer2 = Helper.CreateWriter(ms, header);

        var count = Messages.Count;
        writer2.Write((Byte)count);
        for (var i = 0; i < count; i++)
        {
            writer2.Write(Messages[i]);
        }

        ms.Position = 0;
        writer.Write((Int32)ms.Length);
        ms.CopyTo(stream);

        return true;
    }
}