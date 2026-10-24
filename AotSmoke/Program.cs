using System.Buffers;
using System.Globalization;
using System.Reflection;
using NewLife;
using NewLife.Data;
using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;
using NewLife.Serialization;

var failed = 0;
var passed = 0;

void Run(String name, Action action)
{
    try
    {
        action();
        Console.WriteLine($"  OK   {name}");
        passed++;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  FAIL {name}: {ex.GetType().Name} {ex.Message}");
        failed++;
    }
}

static Byte[] WriteBody(Object model, JTVersion version)
{
    var header = new JTMessage { Version = version };
    var ms = new MemoryStream();
    if (model is IAccessor accessor)
    {
        if (!accessor.Write(ms, header)) throw new InvalidOperationException($"{model.GetType().Name} 写入返回 false");
    }
    else
    {
        Helper.CreateWriter(ms, header).Write(model);
    }

    return ms.ToArray();
}

static Object ReadBody(Object model, Byte[] data, JTVersion version)
{
    var header = new JTMessage { Version = version };
    if (model is IAccessor accessor)
    {
        if (!accessor.Read(new MemoryStream(data), header)) throw new InvalidOperationException($"{model.GetType().Name} 回读返回 false");

        return model;
    }

    var target = model;
    if (!Helper.CreateReader(new MemoryStream(data), header).TryRead(null, ref target))
        throw new InvalidOperationException($"{model.GetType().Name} 回读失败，长度 {data.Length}");

    return target;
}

static Byte[] BuildFrame(JTCodec codec, JTMessage message)
{
    using var pk = codec.Build(message) ?? throw new InvalidOperationException("帧构建失败");

    return pk.ToArray();
}

static JTMessage ParseFrame(JTCodec codec, Byte[] frame)
{
    var rs = codec.TryParse(new ReadOnlySequence<Byte>(frame)) ?? throw new InvalidOperationException("帧未解析出结果");
    if (rs.Invalid) throw new InvalidOperationException("帧被判定为非法");
    if (rs.Message is not JTMessage message) throw new InvalidOperationException("解析结果不是 JT809 消息");

    return message;
}

// 单文件/原生 AOT 下 Assembly.Location 恒为空，正是这里区分运行时的依据；
// IL3000 针对的是“依赖 Location 定位文件”的场景，此处只做判空，故局部抑制
#pragma warning disable IL3000
static String DetectRuntime() => String.IsNullOrEmpty(typeof(Object).Assembly.Location) ? "NativeAOT" : "CoreCLR（JIT 基线）";
#pragma warning restore IL3000

Console.WriteLine("==== NewLife.JT809 NativeAOT 冒烟 ====");
Console.WriteLine($"运行时 {Environment.Version}，{DetectRuntime()}");

Run("消息工厂注册全部内置指令", () =>
{
    var factory = MessageFactory.Instance;
    factory.Init();

    var count = factory.Maps.Count;
    if (count < 80) throw new InvalidOperationException($"已注册指令不足：{count}");

    // 业务方常用 Register<T> 与特性驱动分派，特性元数据在裁剪后必须仍然可用
    var att = typeof(JT809_0x1001).GetCustomAttribute<MessageKindAttribute>();
    if (att == null || att.Kind != MessageKinds.主链路登录请求消息) throw new InvalidOperationException("MessageKindAttribute 元数据已被裁剪");
});

Run("反射模型黄金字节（0x1002 登录应答）", () =>
{
    var buf = WriteBody(new JT809_0x1002
    {
        Result = JT809_0x1002_Result.成功,
        VerifyCode = 0x11223344,
    }, JTVersion.JT2011);

    if (buf.Length != 5) throw new InvalidOperationException($"长度应为5，实际{buf.Length}，反射元数据疑似被裁剪");
    if (buf.ToHex() != "0011223344") throw new InvalidOperationException($"黄金字节不符：{buf.ToHex()}");
});

Run("反射模型定长字段往返（0x1003 注销请求）", () =>
{
    var buf = WriteBody(new JT809_0x1003 { UserId = 7, Password = "123456" }, JTVersion.JT2011);
    if (buf.Length != 12) throw new InvalidOperationException($"长度应为12，实际{buf.Length}，反射元数据疑似被裁剪");

    var model = (JT809_0x1003)ReadBody(new JT809_0x1003(), buf, JTVersion.JT2011);
    if (model.UserId != 7) throw new InvalidOperationException($"用户ID错误：{model.UserId}");
    if (model.Password?.Trim() != "123456") throw new InvalidOperationException($"密码错误：{model.Password}");
});

Run("定位体黄金字节（0x9202 2011）", () =>
{
    var buf = WriteBody(new JT809_0x9200_0x9202
    {
        Encrypt = JT809_VehiclePositionEncrypt.未加密,
        Time = new DateTime(2020, 6, 13, 13, 34, 47),
    }, JTVersion.JT2011);

    if (buf.Length != 36) throw new InvalidOperationException($"长度应为36，实际{buf.Length}");
    if (buf[..8].ToHex() != "000D0607E40D222F") throw new InvalidOperationException($"时间字段不符：{buf[..8].ToHex()}");
});

Run("帧编解码往返（2011/2019，含转义与 CRC）", () =>
{
    var payload = new Byte[] { 0x5b, 0x5a, 0x5d, 0x5e, 0x00, 0xff };
    foreach (var version in new[] { JTVersion.JT2011, JTVersion.JT2019 })
    {
        var codec = new JTCodec { Config = new JTConfig { Version = version } };
        var message = new JTMessage
        {
            Kind = MessageKinds.主链路连接保持请求消息,
            Sequence = 0x01020304,
            CenterId = 0x03938706,
            Version = version,
            VersionFlag = 0x01020F,
            Time = new DateTime(2024, 1, 2, 3, 4, 5),
            Payload = new ArrayPacket(payload),
        };

        var frame = BuildFrame(codec, message);
        if (frame[0] != 0x5b || frame[^1] != 0x5d) throw new InvalidOperationException("帧首尾标识错误");

        // 帧内除首尾标识外不得再出现 0x5b/0x5d，否则对端会提前定界
        var inner = frame.AsSpan(1, frame.Length - 2);
        if (inner.IndexOfAny((Byte)0x5b, (Byte)0x5d) >= 0) throw new InvalidOperationException("帧内敏感字节未被转义");

        var reply = ParseFrame(codec, frame);
        if (reply.Kind != MessageKinds.主链路连接保持请求消息) throw new InvalidOperationException($"指令错误：{reply.Kind}");
        if (reply.Sequence != 0x01020304) throw new InvalidOperationException($"序列号错误：{reply.Sequence}");
        if (reply.CenterId != 0x03938706) throw new InvalidOperationException($"接入码错误：{reply.CenterId}");
        if (reply.Version != version) throw new InvalidOperationException($"协议版本错误：{reply.Version}");

        var data = reply.Payload?.ToArray();
        if (data == null || !data.SequenceEqual(payload)) throw new InvalidOperationException("负载不一致");
    }
});

Run("链路加密往返", () =>
{
    var codec = new JTCodec
    {
        Config = new JTConfig
        {
            Version = JTVersion.JT2011,
            M1 = 0x01234567,
            IA1 = 0x07654321,
            IC1 = 0x01357246,
        },
    };
    var payload = new Byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
    var message = new JTMessage
    {
        Kind = MessageKinds.主链路车辆动态信息交换业务,
        Sequence = 9,
        CenterId = 60000006,
        Payload = new ArrayPacket(payload),
    };

    var frame = BuildFrame(codec, message);
    var reply = ParseFrame(codec, frame);
    var data = reply.Payload?.ToArray();
    if (data == null || !data.SequenceEqual(payload)) throw new InvalidOperationException("解密后的负载不一致");
});

Run("全部已注册指令在 2011/2019 下往返", () =>
{
    var factory = MessageFactory.Instance;
    factory.Init();

    var failures = new List<String>();
    var checkedCount = 0;
    foreach (var kind in Enum.GetValues<MessageKinds>())
    {
        var model = factory.Create(kind);
        if (model == null) continue;

        // 交换类信封默认不带子消息，写出后读回必然失败，由各自的专项用例覆盖
        if (model is JT809ExchangeMessageBodies || model is JT809ExchangeMessage) continue;

        foreach (var version in new[] { JTVersion.JT2011, JTVersion.JT2019 })
        {
            Byte[] data;
            try
            {
                data = WriteBody(model, version);
            }
            catch (Exception ex)
            {
                failures.Add($"{kind}({(Int32)kind:X4}) {version} 写入异常：{ex.GetType().Name} {ex.Message}");
                continue;
            }

            // 空数据体（无字段指令）无需回读
            if (data.Length == 0) continue;

            checkedCount++;
            try
            {
                ReadBody(factory.Create(kind), data, version);
            }
            catch (Exception ex)
            {
                failures.Add($"{kind}({(Int32)kind:X4}) {version} 回读异常：{ex.GetType().Name} {ex.Message}");
            }
        }
    }

    if (checkedCount == 0) throw new InvalidOperationException("没有任何指令产生数据体，反射元数据疑似被裁剪");
    if (failures.Count > 0) throw new InvalidOperationException(String.Join(Environment.NewLine + "    ", failures));
});

Run("本地化词表命中", () =>
{
    var old = Localization.Culture;
    try
    {
        Localization.Culture = new CultureInfo("zh-CN");

        var text = Localization.Translate("Login failed! {0}", 7);
        if (text != "登录失败！7") throw new InvalidOperationException($"译文不符：{text}");
    }
    finally
    {
        Localization.Culture = old;
    }
});

Run("反射 JSON 序列化", () =>
{
    var message = new JTMessage
    {
        Sequence = 12,
        Kind = MessageKinds.主链路登录请求消息,
        CenterId = 123456,
    };

    var json = message.ToJsonJT809();
    if (!json.Contains("主链路登录请求消息")) throw new InvalidOperationException($"JSON 缺少枚举字符串：{json}");
    if (!json.Replace(" ", "").Contains("\"Sequence\":12")) throw new InvalidOperationException($"JSON 缺少序列号：{json}");
});

Console.WriteLine($"==== 通过 {passed}，失败 {failed} ====");

return failed == 0 ? 0 : 1;
