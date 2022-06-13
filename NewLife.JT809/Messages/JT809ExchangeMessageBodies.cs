using NewLife.Data;
using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.Messages;

/// <summary>
/// 交换信息体
/// </summary>
public abstract class JT809ExchangeMessageBodies : IAccessor
{
    #region 属性
    /// <summary>
    /// 车牌号
    /// </summary>
    public String VehicleNo { get; set; }

    /// <summary>
    /// 车辆颜色
    /// </summary>
    public JT809VehicleColorType VehicleColor { get; set; } = JT809VehicleColorType.其他;

    /// <summary>
    /// 子业务类型标识
    /// </summary>
    public UInt16 DataType { get; set; }

    ///// <summary>
    ///// 后续数据长度
    ///// </summary>
    //public UInt32 DataLength { get; set; }

    ///// <summary>
    ///// 子业务数据体
    ///// </summary>
    //public JT809SubBodies SubBodies { get; set; }

    /// <summary>子业务数据体</summary>
    public Packet Payload { get; set; }
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

        var jt2019 = context is JTMessage header && header.JTT2019;

        if (!jt2019)
        {
            VehicleNo = reader.ReadFixedString(21);
            VehicleColor = (JT809VehicleColorType)reader.ReadByte();
        }

        DataType = reader.ReadUInt16();

        var len = reader.ReadUInt32();
        if (len > 0) Payload = reader.ReadBytes((Int32)len);

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

        var jt2019 = context is JTMessage header && header.JTT2019;

        if (!jt2019)
        {
            writer.WriteFixedString(VehicleNo, 21);
            writer.Write(VehicleColor);
        }

        writer.Write(DataType);

        var pk = Payload;
        if (pk == null)
            writer.Write((Byte)0);
        else
        {
            writer.Write((Byte)pk.Total);
            pk.CopyTo(stream);
        }

        return true;
    }
    #endregion
}
