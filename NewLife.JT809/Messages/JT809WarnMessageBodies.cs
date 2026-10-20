using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.Serialization;

namespace NewLife.JT809.Messages;

/// <summary>报警信息体。2011版数据体含车牌，2019版起车牌移入各子业务数据体</summary>
/// <remarks>
/// JT/T 809-2011 表46/表49：车牌号(21) + 车牌颜色(1) + 子业务类型(2) + 后续数据长度(4) + 数据；
/// JT/T 809-2019 表62/表67：子业务类型(2) + 后续数据长度(4) + 数据。
/// </remarks>
public abstract class JT809WarnMessageBodies : JT809ExchangeMessageBodies
{
    #region 方法
    /// <summary>读取</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override Boolean Read(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var reader = Helper.CreateReader(stream, header);

        // 2019起报警业务数据体不再携带车牌
        if (reader.Version != "2019")
        {
            // 车牌(21)+颜色(1)都读不到时判定为无效数据体，避免在流末尾读取抛异常
            if (stream.Length - stream.Position < 22) return false;

            VehicleNo = reader.ReadFixedString(21);
            VehicleColor = (JT809VehicleColorType)reader.ReadByte();
        }

        return ReadBody(stream, reader, header);
    }

    /// <summary>写入</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override Boolean Write(Stream stream, Object context)
    {
        var header = context as JTMessage;
        var writer = Helper.CreateWriter(stream, header);

        // 2011版报警业务数据体以车牌开头
        if (writer.Version != "2019")
        {
            writer.WriteFixedString(VehicleNo, 21);
            writer.Write(VehicleColor);
        }

        return WriteBody(stream, writer, header);
    }
    #endregion
}