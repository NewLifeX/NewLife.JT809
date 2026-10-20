using NewLife.Data;
using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.Messages;

/// <summary>交换信息体。包含车牌和颜色</summary>
public abstract class JT809ExchangeMessageBodies : IAccessor, ISubMessage
{
    #region 属性
    /// <summary>
    /// 车牌号
    /// </summary>
    [FieldSize(21)]
    public String VehicleNo { get; set; }

    /// <summary>
    /// 车辆颜色
    /// </summary>
    public JT809VehicleColorType VehicleColor { get; set; } = JT809VehicleColorType.其他;

    /// <summary>
    /// 子业务类型标识
    /// </summary>
    public UInt16 DataType { get; set; }

    /// <summary>子业务数据体</summary>
    public Object Body { get; set; }
    #endregion

    #region 方法
    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        VehicleNo = reader.ReadFixedString(21);
        VehicleColor = (JT809VehicleColorType)reader.ReadByte();

        return ReadBody(stream, reader, header);
    }

    /// <summary>读取子业务类型标识、数据长度与子业务数据体</summary>
    /// <param name="stream">数据流</param>
    /// <param name="reader">读取器</param>
    /// <param name="header">报文头</param>
    /// <returns></returns>
    protected Boolean ReadBody(Stream stream, Binary reader, JTMessage header)
    {
        DataType = reader.ReadUInt16();

        var len = reader.ReadInt32();
        if (len < 0 || len > stream.Length - stream.Position) return false;
        if (len > 0)
        {
            var model = MessageFactory.Instance.Create((MessageKinds)DataType);
            if (model == null)
                Body = reader.ReadBytes(len);
            else
            {
                // 子消息限定在自身长度内读取，且必须携带报文头，否则拿不到协议版本
                var ms = new MemoryStream(reader.ReadBytes(len));
                if (model is IAccessor accessor)
                {
                    if (!accessor.Read(ms, header)) return false;
                }
                else if (!Helper.CreateReader(ms, header).TryRead(null, ref model)) return false;

                Body = model;
            }
        }

        return true;
    }

    /// <summary>
    /// 写入
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual Boolean Write(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var writer = Helper.CreateWriter(stream, header);

        writer.WriteFixedString(VehicleNo, 21);
        writer.Write(VehicleColor);

        return WriteBody(stream, writer, header);
    }

    /// <summary>写入子业务类型标识、数据长度与子业务数据体</summary>
    /// <param name="stream">数据流</param>
    /// <param name="writer">写入器</param>
    /// <param name="header">报文头</param>
    /// <returns></returns>
    protected Boolean WriteBody(Stream stream, Binary writer, JTMessage header)
    {
        // 修正数据
        if (Body is Byte[] buf)
        {
            writer.Write(DataType);

            writer.Write(buf.Length);
            writer.Write(buf, 0, buf.Length);
        }
        else if (Body is IPacket pk)
        {
            writer.Write(DataType);

            writer.Write(pk.Total);
            pk.CopyTo(stream);
        }
        else if (Body != null)
        {
            if (MessageFactory.Instance.Types.TryGetValue(Body.GetType(), out var kind))
                DataType = (UInt16)kind;

            writer.Write(DataType);

            //writer.Write(Body);
            // 必须先序列化后面，需要用到长度
            var ms = new MemoryStream();
            if (Body is IAccessor accessor)
            {
                if (!accessor.Write(ms, header)) return false;
            }
            else
            {
                var writer2 = Helper.CreateWriter(ms, header);
                writer2.Write(Body);
            }

            ms.Position = 0;
            writer.Write((Int32)ms.Length);
            ms.CopyTo(stream);
        }

        return true;
    }
    #endregion
}
