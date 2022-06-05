using System.Runtime.Serialization;
using NewLife.Collections;
using NewLife.Data;
using NewLife.Log;
using NewLife.Messaging;
using NewLife.Serialization;

namespace NewLife.JT809.Protocols;

/// <summary>JT808报文消息</summary>
public class JTMessage : IMessage, IAccessor
{
    #region 属性
    /// <summary>数据长度。包括头标识、数据头、数据体、CRC校验码和尾标识</summary>
    public UInt32 MsgLength { get; set; }

    /// <summary>报文序列号</summary>
    public UInt32 MsgSn { get; set; }

    /// <summary>业务数据类型</summary>
    public UInt16 MsgId { get; set; }

    /// <summary>下级平台接入码。上级平台给下级平台分配的唯一标识号</summary>
    public UInt32 MsgGNSSCenterId { get; set; }

    /// <summary>协议版本号标识。上下级平台之间采用的标准协议版本编号，长度3字节，0x01020F表示v1.2.15</summary>
    public UInt32 VersionFlag { get; set; }

    /// <summary>报文加密标识。1表示加密</summary>
    public Byte EncryptFlag { get; set; }

    /// <summary>数据加密的密钥</summary>
    public UInt32 EncryptKey { get; set; }

    /// <summary>发送消息时的系统UTC时间，长度为8个字节，Unix毫秒。2019新增</summary>
    public DateTime Time { get; set; }
    #endregion

    #region 接口属性
    /// <summary>是否响应</summary>
    /// <remarks>所有终端主发的消息均应要求平台应答</remarks>
    public Boolean Reply { get; set; }

    /// <summary>是否错误响应</summary>
    [IgnoreDataMember]
    Boolean IMessage.Error { get; set; }

    /// <summary>是否单向请求。单向请求无需等待响应</summary>
    [IgnoreDataMember]
    Boolean IMessage.OneWay { get; }

    /// <summary>负载数据</summary>
    [IgnoreDataMember]
    public Packet Payload { get; set; }
    #endregion

    #region 构造
    #endregion

    #region 读写
    const Int32 HEADER_LENGTH = 1 + 4 + 4 + 2 + 4 + 3 + 1 + 4 + 8 + 1;

    /// <summary>解析数据流</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        if (stream == null && context == null) return false;

        // 头部标识探测
        if (stream.ReadByte() != 0x5b) return false;

        // 转义处理，0x5b->0x5a01，0x5a->0x5a02
        // 转义处理，0x5d->0x5e01，0x5e->0x5e02
        var ms = new MemoryStream();
        while (true)
        {
            var b = stream.ReadByte();
            if (b < 0) break;

            if (b == 0x5a)
            {
                var b2 = stream.ReadByte();
                if (b2 < 0)
                {
                    ms.WriteByte((Byte)b);
                    break;
                }

                if (b2 == 0x01)
                    ms.Write(0x5b);
                else if (b2 == 0x02)
                    ms.Write(0x5a);
                else
                    ms.Write((Byte)b, (Byte)b2);
            }
            else if (b == 0x5e)
            {
                var b2 = stream.ReadByte();
                if (b2 < 0)
                {
                    ms.WriteByte((Byte)b);
                    break;
                }

                if (b2 == 0x01)
                    ms.Write(0x5d);
                else if (b2 == 0x02)
                    ms.Write(0x5e);
                else
                    ms.Write((Byte)b, (Byte)b2);
            }
            else
                ms.WriteByte((Byte)b);
        }
        ms.Position = 0;

        var reader = new Binary { Stream = ms, IsLittleEndian = false };

        MsgLength = reader.Read<UInt32>();
        MsgSn = reader.Read<UInt32>();
        MsgId = reader.Read<UInt16>();
        MsgGNSSCenterId = reader.Read<UInt32>();

        var flag = reader.Read<UInt32>();
        VersionFlag = flag >> 8;
        EncryptFlag = (Byte)(flag & 0xFF);
        EncryptKey = reader.Read<UInt32>();

        // 2019新增
        Time = reader.Read<UInt64>().ToDateTime();

        // 负载数据
        var len = MsgLength - HEADER_LENGTH;
        if (len > 0) Payload = reader.ReadBytes((Int32)len);

        // 异或校验。从消息头首字节开始。考虑到TCP/UDP本身有校验，这里不再判断
        var checkSum2 = GetCheckSum(ms, 0);
        var checkSum = reader.ReadByte();
        //if (checkSum != checkSum2) return false;
        if (checkSum != checkSum2) XTrace.WriteLine("checkSum Error {0:X2}!={1:X2}", checkSum, checkSum2);

        // 尾部标识探测
        return reader.ReadByte() == 0x5d;
    }

    /// <summary>写入数据</summary>
    /// <param name="stream"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Boolean Write(Stream stream, Object context)
    {
        if (stream == null && context == null) return false;

        // 使用内存流，后面可能需要转义
        var ms = Pool.MemoryStream.Get();
        var writer = new Binary { Stream = ms, IsLittleEndian = false };

        var pk = Payload;
        var len = HEADER_LENGTH;
        if (pk != null) len += pk.Total;

        writer.Write(len);
        writer.Write(MsgSn);
        writer.Write(MsgId);
        writer.Write(MsgGNSSCenterId);

        var flag = (VersionFlag << 8) | EncryptFlag;
        writer.Write(flag);

        writer.Write(EncryptKey);

        // 2019新增
        writer.Write(Time.ToLong());

        if (pk != null) pk.CopyTo(writer.Stream);

        // 计算校验和
        var checkSum = GetCheckSum(ms, 0);
        writer.Write(checkSum);

        // 转义处理，0x5b->0x5a01，0x5a->0x5a02
        // 转义处理，0x5d->0x5e01，0x5e->0x5e02
        var ms2 = new MemoryStream();
        ms2.WriteByte(0x5b);

        ms.Position = 0;
        while (ms.Position < ms.Length)
        {
            var b = ms.ReadByte();
            if (b == 0x5b)
                ms2.Write(0x5a, 0x01);
            else if (b == 0x5a)
                ms2.Write(0x5a, 0x02);
            else if (b == 0x5d)
                ms2.Write(0x5e, 0x01);
            else if (b == 0x5e)
                ms2.Write(0x5e, 0x02);
            else
                ms2.WriteByte((Byte)b);
        }
        ms.Put(false);
        ms2.WriteByte(0x5d);

        // 一次性写入结果
        ms2.Position = 0;
        ms2.CopyTo(stream);

        return true;
    }

    private static Byte GetCheckSum(Stream stream, Int64 offset)
    {
        var count = stream.Position - offset;
        stream.Position = offset;
        var crc = (Byte)stream.ReadByte();
        for (var i = 1; i < count; i++)
        {
            var b = (Byte)stream.ReadByte();
            crc = (Byte)(crc ^ b);
        }
        return crc;
    }

    /// <summary>读取数据包</summary>
    /// <param name="pk"></param>
    /// <returns></returns>
    public Boolean Read(Packet pk) => Read(pk.GetStream(), null);

    /// <summary>转数据包</summary>
    /// <returns></returns>
    public Packet ToPacket()
    {
        try
        {
            var ms = new MemoryStream();
            if (!Write(ms, null)) return null;

            ms.Position = 0;
            return new Packet(ms);
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
            throw;
        }
    }
    #endregion

    #region 解析负载
    ///// <summary>从负载数据解析业务消息模型</summary>
    ///// <param name="model"></param>
    ///// <returns></returns>
    //public Boolean TryReadModel(Object model)
    //{
    //    var pk = Payload;
    //    return pk == null || pk.Total <= 0 || (model is IAccessor accessor
    //        ? accessor.Read(pk.GetStream(), this)
    //        : Helper.CreateReader(pk.GetStream()).TryRead(null, ref model));
    //}

    ///// <summary>从负载数据解析业务消息模型</summary>
    ///// <typeparam name="T"></typeparam>
    ///// <returns></returns>
    ///// <exception cref="InvalidDataException"></exception>
    //public T ReadModel<T>() where T : new()
    //{
    //    var msg = new T();
    //    return !TryReadModel(msg) ? throw new InvalidDataException() : msg;
    //}

    ///// <summary>写入业务消息模型到负载数据</summary>
    ///// <param name="model"></param>
    ///// <returns></returns>
    //public Boolean WriteModel(Object model)
    //{
    //    var rs = false;
    //    var ms = new MemoryStream();
    //    rs = model is IAccessor accessor ? accessor.Write(ms, this) : Helper.CreateWriter(ms).Write(model);

    //    ms.Position = 0;
    //    Payload = new Packet(ms);

    //    return rs;
    //}

    ///// <summary>设置结果</summary>
    ///// <param name="result"></param>
    //public void SetResult(Object result)
    //{
    //    Reply = true;

    //    if (MessageFactory.Instance.Types.TryGetValue(result.GetType(), out var kind)) MsgId = kind;

    //    WriteModel(result);
    //}
    #endregion

    #region 辅助
    /// <summary>创建响应</summary>
    /// <returns></returns>
    public IMessage CreateReply() => new JTMessage { Reply = true, MsgSn = MsgSn, MsgId = MsgId, VersionFlag = VersionFlag };

    /// <summary>已重写。返回友好文本</summary>
    /// <returns></returns>
    public override String ToString() => $"[{MsgId}] {MsgGNSSCenterId} Seq={MsgSn} [{Payload?.Total}]";
    #endregion
}
