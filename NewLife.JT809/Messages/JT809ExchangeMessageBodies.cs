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
        var reader = Helper.CreateReader(stream);

        VehicleNo = reader.ReadFixedString(21);
        VehicleColor = (JT809VehicleColorType)reader.ReadByte();
        DataType = reader.ReadUInt16();

        var len = reader.ReadInt32();
        if (len > 0)
        {
            var model = MessageFactory.Instance.Create((MessageKinds)DataType);
            if (model == null)
                Body = reader.ReadBytes(len);
            else
            {
                if (!reader.TryRead(null, ref model)) return false;
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
        var writer = Helper.CreateWriter(stream);

        writer.WriteFixedString(VehicleNo, 21);
        writer.Write(VehicleColor);

        // 修正数据
        if (Body is Byte[] buf)
        {
            writer.Write(DataType);

            writer.Write(buf.Length);
            writer.Write(buf, 0, buf.Length);
        }
        else if (Body is Packet pk)
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
            var writer2 = Helper.CreateWriter(ms);
            writer2.Write(Body);

            ms.Position = 0;
            writer.Write((Int32)ms.Length);
            ms.CopyTo(stream);
        }

        return true;
    }
    #endregion
}
