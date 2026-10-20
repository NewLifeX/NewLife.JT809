using System.Runtime.Serialization;
using System.Xml.Serialization;
using NewLife.Collections;
using NewLife.Data;
using NewLife.Log;
using NewLife.Messaging;
using NewLife.Security;
using NewLife.Serialization;

namespace NewLife.JT809.Protocols;

/// <summary>JT809报文消息</summary>
/// <remarks>
/// 同时实现 <see cref="IAccessor"/>（承载业务消息体读写）与 <see cref="IMessage"/>（供 Core 消息泵按帧交付）。
/// 由于本类型的 <see cref="Kind"/> 表示 JT809 业务数据类型，与 <see cref="IMessage.Kind"/> 的消息方向语义不同，
/// 接口成员采用显式实现，方向由 <see cref="Reply"/> 决定。
/// </remarks>
public class JTMessage : IAccessor, IMessage
{
    #region 属性
    ///// <summary>数据长度。包括头标识、数据头、数据体、CRC校验码和尾标识</summary>
    //public UInt32 MsgLength { get; set; }

    /// <summary>报文序列号</summary>
    public UInt32 Sequence { get; set; }

    /// <summary>业务数据类型</summary>
    public MessageKinds Kind { get; set; }

    /// <summary>下级平台接入码。上级平台给下级平台分配的唯一标识号</summary>
    public UInt32 CenterId { get; set; }

    /// <summary>协议版本号标识。上下级平台之间采用的标准协议版本编号，长度3字节，0x01020F表示v1.2.15</summary>
    public UInt32 VersionFlag { get; set; }

    /// <summary>报文加密标识。1表示加密</summary>
    public Byte EncryptFlag { get; set; }

    /// <summary>数据加密的密钥</summary>
    public UInt32 EncryptKey { get; set; }

    /// <summary>发送消息时的系统UTC时间，长度为8个字节，Unix毫秒。2019新增</summary>
    public DateTime Time { get; set; }

    ///// <summary>协议配置，包括版本和密钥</summary>
    //[XmlIgnore, IgnoreDataMember]
    //public JTConfig Config { get; set; }
    #endregion

    #region 接口属性
    /// <summary>是否响应</summary>
    /// <remarks>所有终端主发的消息均应要求平台应答</remarks>
    public Boolean Reply { get; set; }

    /// <summary>负载数据</summary>
    [IgnoreDataMember]
    public IPacket Payload { get; set; }

    ///// <summary>是否2019版</summary>
    //public Boolean JTT2019 { get; set; }

    /// <summary>协议版本</summary>
    public JTVersion Version { get; set; } = JTVersion.JT2011;
    #endregion

    #region 接口实现
    /// <summary>消息方向。JT809 无方向位，由 <see cref="Reply"/> 映射为请求/响应</summary>
    NewLife.Messaging.MessageKinds IMessage.Kind
    {
        get => Reply ? NewLife.Messaging.MessageKinds.Response : NewLife.Messaging.MessageKinds.Request;
        set => Reply = value is NewLife.Messaging.MessageKinds.Response or NewLife.Messaging.MessageKinds.Error;
    }

    /// <summary>是否为应答消息</summary>
    Boolean IMessage.Reply => Reply;

    /// <summary>消息体。JT809 按整帧交付，负载统一经 <see cref="Payload"/> 读取</summary>
    LimitedReader IMessage.Body => null;

    /// <summary>根据请求创建配对的响应消息</summary>
    /// <returns></returns>
    IMessage IMessage.CreateReply() => CreateReply();

    /// <summary>设置消息体，用于整帧/发送路径</summary>
    /// <param name="packet">消息体数据包</param>
    void IMessage.SetBody(IPacket packet) => Payload = packet;

    /// <summary>绑定流式消息体。JT809 按整帧定界，帧层不会走流式体路径</summary>
    /// <param name="body">限长读取器</param>
    void IMessage.BindBody(LimitedReader body) { }

    /// <summary>销毁。归还负载数据包</summary>
    public void Dispose()
    {
        Payload.TryDispose();
        Payload = null;
    }
    #endregion

    #region 构造
    #endregion

    #region 读写
    const Int32 HEADER_LENGTH = 26; //1 + 4 + 4 + 2 + 4 + 3 + 1 + 4 + 2 + 1;
    const Int32 HEADER_LENGTH_2019 = 34; //1 + 4 + 4 + 2 + 4 + 3 + 1 + 4 + 8 + 2 + 1;

    /// <summary>解析数据流</summary>
    /// <param name="stream">完整帧数据（以0x5b开始、0x5d结束）</param>
    /// <param name="context">JTConfig 上下文</param>
    /// <returns></returns>
    public Boolean Read(Stream stream, Object context)
    {
        if (stream == null && context == null) return false;
        if (stream == null) return false;

        var frame = stream.ReadBytes(-1);
        var data = Unescape(frame);
        if (data == null) return false;

        return ReadContent(data, context as JTConfig);
    }

    /// <summary>解除转义。0x5b->0x5a01，0x5a->0x5a02，0x5d->0x5e01，0x5e->0x5e02</summary>
    /// <param name="frame">完整帧数据（以0x5b开始）</param>
    /// <returns>反转义后的帧内容（不含帧头标识），帧头不合法时返回 null</returns>
    public static Byte[] Unescape(Byte[] frame)
    {
        // 头部标识探测
        if (frame == null || frame.Length == 0 || frame[0] != 0x5b) return null;

        var ms = new MemoryStream(frame.Length);
        for (var i = 1; i < frame.Length; i++)
        {
            var b = frame[i];
            if (b == 0x5a || b == 0x5e)
            {
                if (i + 1 >= frame.Length)
                {
                    ms.WriteByte(b);
                    break;
                }

                var b2 = frame[++i];
                if (b == 0x5a && b2 == 0x01)
                    ms.WriteByte(0x5b);
                else if (b == 0x5a && b2 == 0x02)
                    ms.WriteByte(0x5a);
                else if (b == 0x5e && b2 == 0x01)
                    ms.WriteByte(0x5d);
                else if (b == 0x5e && b2 == 0x02)
                    ms.WriteByte(0x5e);
                else
                {
                    ms.WriteByte(b);
                    ms.WriteByte(b2);
                }
            }
            else
                ms.WriteByte(b);
        }

        return ms.ToArray();
    }

    /// <summary>解析已反转义的帧内容</summary>
    /// <param name="data">帧内容（0x5b之后、含0x5d）</param>
    /// <param name="config">协议配置</param>
    /// <returns></returns>
    public Boolean ReadContent(Byte[] data, JTConfig config)
    {
        if (data == null || data.Length < HEADER_LENGTH - 1) return false;

        var ms = new MemoryStream(data);

        if (config != null && config.Version > 0) Version = config.Version;
        var reader = new Binary { Stream = ms, IsLittleEndian = false };

        //MsgLength = reader.Read<UInt32>();
        var len = reader.Read<Int32>();
        Sequence = reader.Read<UInt32>();
        Kind = reader.Read<MessageKinds>();
        CenterId = reader.Read<UInt32>();

        var flag = reader.Read<UInt32>();
        VersionFlag = flag >> 8;
        EncryptFlag = (Byte)(flag & 0xFF);
        EncryptKey = reader.Read<UInt32>();

#if DEBUG
        var len2 = data.Length + 1;
        if (len != len2) XTrace.WriteLine("Command [{0:X4}] declares {1} bytes but actually has {2} bytes (unescaped)", (Int32)Kind, len, len2);
#endif

        // 根据登录请求，猜测协议版本
        var j9 = Version == JTVersion.JT2019;
        if (!j9 && TryGuessVersion(Kind, len, out var jt2019) && jt2019)
        {
            if (config == null || config.Version != JTVersion.JT2019)
                XTrace.WriteLine("Command [{0:X4}] configured protocol version: {1}, actual protocol version: {2}", (Int32)Kind, config?.Version, JTVersion.JT2019);

            j9 = true;
            Version = JTVersion.JT2019;
        }

        // 2019新增
        if (j9)
        {
            var t = reader.Read<Int64>();
            if (t < 9999L * 365 * 34 * 3600 * 1000)
                Time = t.ToDateTime();
        }

        // 负载数据
        len -= j9 ? HEADER_LENGTH_2019 : HEADER_LENGTH;
        if (len > 0)
        {
            var buf = reader.ReadBytes(len);
            if (EncryptFlag > 0 && config != null && config.M1 > 0)
                Encrypt(buf, EncryptKey, config);
            Payload = (ArrayPacket)buf;
        }

        //if (ms.Position + 2 < ms.Length)
        {
            var checkSum2 = GetCheckSum(ms, 0, 0);
            var checkSum = reader.ReadUInt16();
            //if (checkSum != checkSum2) return false;
            if (checkSum != checkSum2) XTrace.WriteLine("Crc16 Error {0:X4}!={1:X4}", checkSum, checkSum2);
        }

        Reply = MessageFactory.Instance.IsReply(Kind);

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

        var config = context as JTConfig;
        if (config != null && config.Version > 0) Version = config.Version;

        // 使用内存流，后面可能需要转义
        var ms = Pool.MemoryStream.Get();
        var writer = new Binary { Stream = ms, IsLittleEndian = false };

        var pk = Payload;
        var len = Version == JTVersion.JT2019 ? HEADER_LENGTH_2019 : HEADER_LENGTH;
        if (pk != null) len += pk.Total;
        var data = pk == null ? [] : pk.ReadBytes();

        // 加密数据。避免就地修改调用方负载缓冲，先拷贝再加密
        if (config != null && config.M1 > 0)
        {
            EncryptFlag = 1;
            EncryptKey = (UInt32)Rand.Next();

            data = (Byte[])data.Clone();
            Encrypt(data, EncryptKey, config);
        }

        writer.Write(len);
        writer.Write(Sequence);
        writer.Write(Kind);
        writer.Write(CenterId);

        var flag = (VersionFlag << 8) | EncryptFlag;
        writer.Write(flag);

        writer.Write(EncryptKey);

        // 2019新增
        if (Version == JTVersion.JT2019) writer.Write(Time.ToLong());

        //pk?.CopyTo(writer.Stream);
        writer.Write(data, 0, data.Length);

        // 计算校验和
        var checkSum = GetCheckSum(ms, 0, 0);
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

    /// <summary>根据几个重要的固定长度的指令，去猜测协议版本</summary>
    /// <param name="kind"></param>
    /// <param name="len"></param>
    /// <param name="jt2019"></param>
    /// <returns></returns>
    private static Boolean TryGuessVersion(MessageKinds kind, Int32 len, out Boolean jt2019)
    {
        switch (kind)
        {
            case MessageKinds.主链路登录请求消息:
                jt2019 = len >= HEADER_LENGTH_2019 + (4 + 8 + 4 + 32 + 2);
                return true;
            case MessageKinds.主链路登录应答消息:
                jt2019 = len >= HEADER_LENGTH_2019 + (1 + 4);
                return true;
            case MessageKinds.主链路连接保持请求消息:
                jt2019 = len >= HEADER_LENGTH_2019 + (0);
                return true;
            case MessageKinds.从链路连接请求消息:
                jt2019 = len >= HEADER_LENGTH_2019 + (4);
                return true;
            default:
                jt2019 = false;
                break;
        }
        return false;
    }

    private static UInt16 GetCheckSum(Stream stream, Int64 offset, Int32 count)
    {
        var count2 = stream.Position - offset - count;
        stream.Position = offset;

        var buf = stream.ReadBytes(count2);
        if (count > 0) stream.Seek(count, SeekOrigin.Current);

        //var crc = new Crc16();
        //crc.Update(buf);

        //return crc.Value;

        var crc = (UInt16)0xFFFF;
        //crc ^= crc;
        for (var i = 0; i < buf.Length; i++)
        {
            crc = (UInt16)((crc << 8) ^ CrcTable[(crc >> 8 ^ buf[offset + i]) & 0xFF]);
        }
        return crc;
    }

    /// <summary>加解密数据</summary>
    /// <param name="data"></param>
    /// <param name="key"></param>
    /// <param name="config"></param>
    public static void Encrypt(Byte[] data, UInt32 key, JTConfig config)
    {
        if (key == 0) key = 1;
        for (var i = 0; i < data.Length; i++)
        {
            key = config.IA1 * (key % config.M1) + config.IC1;
            data[i] ^= (Byte)((key >> 20) & 0xFF);
        }
    }

    #region CRC16数据表
    /// <summary>CRC16表</summary>
    static readonly UInt16[] CrcTable = new UInt16[256]
    {      
            /* CRC16 余式表 */
            0x0000,0x1021,0x2042,0x3063,0x4084,0x50A5,0x60C6,0x70E7,
            0x8108,0x9129,0xA14A,0xB16B,0xC18C,0xD1AD,0xE1CE,0xF1EF,
            0x1231,0x0210,0x3273,0x2252,0x52B5,0x4294,0x72F7,0x62D6,
            0x9339,0x8318,0xB37B,0xA35A,0xD3BD,0xC39C,0xF3FF,0xE3DE,
            0x2462,0x3443,0x0420,0x1401,0x64E6,0x74C7,0x44A4,0x5485,
            0xA56A,0xB54B,0x8528,0x9509,0xE5EE,0xF5CF,0xC5AC,0xD58D,
            0x3653,0x2672,0x1611,0x0630,0x76D7,0x66F6,0x5695,0x46B4,
            0xB75B,0xA77A,0x9719,0x8738,0xF7DF,0xE7FE,0xD79D,0xC7BC,
            0x48C4,0x58E5,0x6886,0x78A7,0x0840,0x1861,0x2802,0x3823,
            0xC9CC,0xD9ED,0xE98E,0xF9AF,0x8948,0x9969,0xA90A,0xB92B,
            0x5AF5,0x4AD4,0x7AB7,0x6A96,0x1A71,0x0A50,0x3A33,0x2A12,
            0xDBFD,0xCBDC,0xFBBF,0xEB9E,0x9B79,0x8B58,0xBB3B,0xAB1A,
            0x6CA6,0x7C87,0x4CE4,0x5CC5,0x2C22,0x3C03,0x0C60,0x1C41,
            0xEDAE,0xFD8F,0xCDEC,0xDDCD,0xAD2A,0xBD0B,0x8D68,0x9D49,
            0x7E97,0x6EB6,0x5ED5,0x4EF4,0x3E13,0x2E32,0x1E51,0x0E70,
            0xFF9F,0xEFBE,0xDFDD,0xCFFC,0xBF1B,0xAF3A,0x9F59,0x8F78,
            0x9188,0x81A9,0xB1CA,0xA1EB,0xD10C,0xC12D,0xF14E,0xE16F,
            0x1080,0x00A1,0x30C2,0x20E3,0x5004,0x4025,0x7046,0x6067,
            0x83B9,0x9398,0xA3FB,0xB3DA,0xC33D,0xD31C,0xE37F,0xF35E,
            0x02B1,0x1290,0x22F3,0x32D2,0x4235,0x5214,0x6277,0x7256,
            0xB5EA,0xA5CB,0x95A8,0x8589,0xF56E,0xE54F,0xD52C,0xC50D,
            0x34E2,0x24C3,0x14A0,0x0481,0x7466,0x6447,0x5424,0x4405,
            0xA7DB,0xB7FA,0x8799,0x97B8,0xE75F,0xF77E,0xC71D,0xD73C,
            0x26D3,0x36F2,0x0691,0x16B0,0x6657,0x7676,0x4615,0x5634,
            0xD94C,0xC96D,0xF90E,0xE92F,0x99C8,0x89E9,0xB98A,0xA9AB,
            0x5844,0x4865,0x7806,0x6827,0x18C0,0x08E1,0x3882,0x28A3,
            0xCB7D,0xDB5C,0xEB3F,0xFB1E,0x8BF9,0x9BD8,0xABBB,0xBB9A,
            0x4A75,0x5A54,0x6A37,0x7A16,0x0AF1,0x1AD0,0x2AB3,0x3A92,
            0xFD2E,0xED0F,0xDD6C,0xCD4D,0xBDAA,0xAD8B,0x9DE8,0x8DC9,
            0x7C26,0x6C07,0x5C64,0x4C45,0x3CA2,0x2C83,0x1CE0,0x0CC1,
            0xEF1F,0xFF3E,0xCF5D,0xDF7C,0xAF9B,0xBFBA,0x8FD9,0x9FF8,
            0x6E17,0x7E36,0x4E55,0x5E74,0x2E93,0x3EB2,0x0ED1,0x1EF0
    };
    #endregion

    /// <summary>读取数据包</summary>
    /// <param name="pk"></param>
    /// <returns></returns>
    public Boolean Read(IPacket pk) => Read(pk.GetStream(), null);

    /// <summary>读取数据包</summary>
    /// <param name="pk"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public Boolean Read(IPacket pk, JTConfig config) => Read(pk.GetStream(), config);

    /// <summary>读取数据包</summary>
    /// <param name="data"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public Boolean Read(Byte[] data, JTConfig config = null) => Read(new MemoryStream(data), config);

    /// <summary>转数据包</summary>
    /// <returns></returns>
    public IPacket ToPacket()
    {
        try
        {
            var ms = new MemoryStream();
            if (!Write(ms, null)) return null;

            ms.Position = 0;
            return new OwnerPacket(ms, 0);
        }
        catch (Exception ex)
        {
            XTrace.WriteException(ex);
            throw;
        }
    }
    #endregion

    #region 解析负载
    /// <summary>从负载数据解析业务消息模型</summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Boolean TryReadModel(Object model)
    {
        var pk = Payload;
        return pk == null || pk.Total <= 0 || (model is IAccessor accessor
            ? accessor.Read(pk.GetStream(), this)
            : Helper.CreateReader(pk.GetStream(), this).TryRead(null, ref model));
    }

    /// <summary>从负载数据解析业务消息模型</summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    public T ReadModel<T>() where T : new()
    {
        var msg = new T();
        return !TryReadModel(msg) ? throw new InvalidDataException() : msg;
    }

    /// <summary>写入业务消息模型到负载数据</summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Boolean WriteModel(Object model)
    {
        var rs = false;
        var ms = new MemoryStream();
        rs = model is IAccessor accessor ? accessor.Write(ms, this) : Helper.CreateWriter(ms, this).Write(model);

        ms.Position = 0;
        Payload = new OwnerPacket(ms, 0);

        return rs;
    }

    /// <summary>设置结果</summary>
    /// <param name="result"></param>
    public void SetResult(Object result)
    {
        //Reply = true;

        if (MessageFactory.Instance.Types.TryGetValue(result.GetType(), out var kind)) Kind = kind;

        Reply = MessageFactory.Instance.IsReply(Kind);

        WriteModel(result);
    }
    #endregion

    #region 辅助
    /// <summary>创建响应</summary>
    /// <returns></returns>
    public JTMessage CreateReply() => new JTMessage
    {
        Reply = true,
        Sequence = Sequence,
        Kind = Kind,
        VersionFlag = VersionFlag,
        CenterId = CenterId,
    };

    //private static readonly ConcurrentDictionary<Type, MessageKinds> _cache = new();

    //private static MessageKinds GetKind(Type type) => _cache.GetOrAdd(type, k => k.GetCustomAttribute<MessageKindAttribute>()?.Kind ?? MessageKinds.主链路登录请求消息);

    /// <summary>已重写。返回友好文本</summary>
    /// <returns></returns>
    public override String ToString() => $"[{(Int32)Kind:X4}][{Kind}] {CenterId} Seq={Sequence} [{Payload?.Total}]";
    #endregion
}
