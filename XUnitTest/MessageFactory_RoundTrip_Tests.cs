using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.Serialization;
using Xunit;

namespace XUnitTest;

[DisplayName("全部已注册指令冒烟测试")]
public class MessageFactory_RoundTrip_Tests
{
    private static Byte[] WriteModel(Object model, JTVersion version)
    {
        var header = new JTMessage { Version = version };
        var ms = new MemoryStream();
        if (model is IAccessor accessor)
            Assert.True(accessor.Write(ms, header));
        else
            Helper.CreateWriter(ms, header).Write(model);

        return ms.ToArray();
    }

    private static Boolean TryReadModel(Object model, Byte[] data, JTVersion version)
    {
        var header = new JTMessage { Version = version };
        if (model is IAccessor accessor) return accessor.Read(new MemoryStream(data), header);

        return Helper.CreateReader(new MemoryStream(data), header).TryRead(null, ref model);
    }

    [Fact]
    [DisplayName("已注册指令在2011与2019下写出再读回不抛异常")]
    public void TestAllRegisteredRoundTrip()
    {
        var factory = MessageFactory.Instance;
        factory.Init();

        var failures = new List<String>();
        foreach (var kind in (MessageKinds[])Enum.GetValues(typeof(MessageKinds)))
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
                    data = WriteModel(model, version);
                }
                catch (Exception ex)
                {
                    failures.Add($"{kind}({(Int32)kind:X4}) {version} 写入异常：{ex.GetType().Name} {ex.Message}");
                    continue;
                }

                // 空数据体（无字段指令）无需回读
                if (data.Length == 0) continue;

                var target = factory.Create(kind);
                try
                {
                    if (!TryReadModel(target, data, version))
                        failures.Add($"{kind}({(Int32)kind:X4}) {version} 回读失败，长度{data.Length}");
                }
                catch (Exception ex)
                {
                    failures.Add($"{kind}({(Int32)kind:X4}) {version} 回读异常：{ex.GetType().Name} {ex.Message}");
                }
            }
        }

        Assert.True(failures.Count == 0, String.Join(Environment.NewLine, failures));
    }
}
