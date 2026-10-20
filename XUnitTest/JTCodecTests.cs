using System;
using System.Buffers;
using System.ComponentModel;
using NewLife;
using NewLife.Data;
using NewLife.JT809.Protocols;
using Message = NewLife.Messaging.Message;
using Xunit;

namespace XUnitTest;

[DisplayName("JT809 帧编解码器")]
public class JTCodecTests
{
    /// <summary>0x1200 车辆动态信息交换帧，含 2019 帧头与 0x1205 子业务</summary>
    private static Byte[] SampleFrame() =>
        ("5B" +
        "00000048" +
        "00000004" +
        "1200" +
        "03938706" +
        "010000" +
        "00" +
        "00989681" +
        "000001815E014624D8" +
        "BBA641413532303030000000000000000000000000" +
        "01" +
        "1205" +
        "0000000A" +
        "0001" +
        "00000000" +
        "00000000" +
        "660B" +
        "5D").ToHex();

    [Fact]
    [DisplayName("帧长扫描：完整帧返回整帧长度")]
    public void FindFrameLength_Complete()
    {
        var dt = SampleFrame();
        var len = JTCodec.FindFrameLength(new ReadOnlySequence<Byte>(dt));

        Assert.Equal(dt.Length, len);
    }

    [Fact]
    [DisplayName("帧长扫描：半帧返回-1等待更多数据")]
    public void FindFrameLength_Partial()
    {
        var dt = SampleFrame();

        Assert.Equal(-1, JTCodec.FindFrameLength(new ReadOnlySequence<Byte>(dt, 0, dt.Length - 4)));
        Assert.Equal(-1, JTCodec.FindFrameLength(new ReadOnlySequence<Byte>(dt, 0, 1)));
    }

    [Fact]
    [DisplayName("帧长扫描：帧头非法返回0")]
    public void FindFrameLength_InvalidHead()
    {
        Assert.Equal(0, JTCodec.FindFrameLength(new ReadOnlySequence<Byte>(new Byte[] { 0x00, 0x01, 0x02 })));
    }

    [Fact]
    [DisplayName("定界解析：完整帧解析出消息与帧长")]
    public void TryParse_Success()
    {
        var dt = SampleFrame();
        var codec = new JTCodec { Config = new JTConfig { Version = JTVersion.JT2019 } };

        var rs = codec.TryParse(new ReadOnlySequence<Byte>(dt));
        Assert.NotNull(rs);
        Assert.False(rs.Value.Invalid);
        Assert.Equal(dt.Length, rs.Value.HeaderSize);
        Assert.Equal(0L, rs.Value.BodyLength);

        var msg = Assert.IsType<JTMessage>(rs.Value.Message);
        Assert.Equal(4u, msg.Sequence);
        Assert.Equal(MessageKinds.主链路车辆动态信息交换业务, msg.Kind);
        Assert.Equal(60000006u, msg.CenterId);
        Assert.Equal(JTVersion.JT2019, msg.Version);
        Assert.Equal("2022-06-13T13:34:47".ToDateTime(), msg.Time);
        Assert.NotNull(msg.Payload);
    }

    [Fact]
    [DisplayName("定界解析：数据不足返回null")]
    public void TryParse_Partial()
    {
        var dt = SampleFrame();
        var codec = new JTCodec();

        Assert.Null(codec.TryParse(new ReadOnlySequence<Byte>(dt, 0, dt.Length - 6)));
        Assert.Null(codec.TryParse(ReadOnlySequence<Byte>.Empty));
    }

    [Fact]
    [DisplayName("定界解析：帧头非法判定为损坏帧")]
    public void TryParse_InvalidHead()
    {
        var codec = new JTCodec();
        var rs = codec.TryParse(new ReadOnlySequence<Byte>(new Byte[] { 0x00, 0x01, 0x02, 0x03 }));

        Assert.NotNull(rs);
        Assert.True(rs.Value.Invalid);
        Assert.Equal(1, rs.Value.HeaderSize);
    }

    [Fact]
    [DisplayName("定界解析：帧尾缺失时继续等待")]
    public void TryParse_MissingTail()
    {
        var dt = SampleFrame();
        var codec = new JTCodec();

        Assert.Null(codec.TryParse(new ReadOnlySequence<Byte>(dt, 0, dt.Length - 1)));
    }

    [Fact]
    [DisplayName("定界解析：长度字段越界判定为损坏帧")]
    public void TryParse_InvalidLength()
    {
        var codec = new JTCodec { Config = new JTConfig { Version = JTVersion.JT2019 } };

        // 声明长度远大于实际帧长
        var over = SampleFrame();
        over[1] = 0x00; over[2] = 0x00; over[3] = 0xFF; over[4] = 0xFF;
        var rs1 = codec.TryParse(new ReadOnlySequence<Byte>(over));
        Assert.NotNull(rs1);
        Assert.True(rs1.Value.Invalid);

        // 声明长度小于头部
        var under = SampleFrame();
        under[1] = 0x00; under[2] = 0x00; under[3] = 0x00; under[4] = 0x10;
        var rs2 = codec.TryParse(new ReadOnlySequence<Byte>(under));
        Assert.NotNull(rs2);
        Assert.True(rs2.Value.Invalid);
    }

    [Fact]
    [DisplayName("整帧构建：写入结果可被自身解析")]
    public void Build_Roundtrip()
    {
        var codec = new JTCodec { Config = new JTConfig { Version = JTVersion.JT2011 } };
        var msg = new JTMessage
        {
            Kind = MessageKinds.主链路连接保持请求消息,
            CenterId = 123456,
            VersionFlag = 0x010000,
            Version = JTVersion.JT2011,
            Payload = new ArrayPacket(new Byte[] { 0x11, 0x22, 0x33 }),
        };

        var pk = codec.Build(msg);
        Assert.NotNull(pk);

        var bytes = pk.ToArray();
        Assert.Equal(0x5B, (Int32)bytes[0]);
        Assert.Equal(0x5D, (Int32)bytes[^1]);
        Assert.NotEqual(0u, msg.Sequence);

        var rs = codec.TryParse(new ReadOnlySequence<Byte>(bytes));
        Assert.NotNull(rs);
        var parsed = Assert.IsType<JTMessage>(rs.Value.Message);
        Assert.Equal(msg.Sequence, parsed.Sequence);
        Assert.Equal(msg.Kind, parsed.Kind);
        Assert.Equal(msg.CenterId, parsed.CenterId);
        Assert.Equal(new Byte[] { 0x11, 0x22, 0x33 }, parsed.Payload.ReadBytes());
    }

    [Fact]
    [DisplayName("整帧构建：非JT809消息抛出参数异常")]
    public void Build_WrongType()
    {
        var codec = new JTCodec();

        Assert.Throws<ArgumentException>(() => codec.Build(new Message()));
    }

    [Fact]
    [DisplayName("头部构建：JT809不支持头部先行发送")]
    public void BuildHeader_NotSupported()
    {
        var codec = new JTCodec();
        var msg = new JTMessage();

        Assert.Throws<NotSupportedException>(() => codec.BuildHeader(msg, 10));
    }

    [Fact]
    [DisplayName("请求响应配对：仅应答消息匹配挂起请求")]
    public void Match_Pairs()
    {
        var codec = new JTCodec();
        var request = new JTMessage { Kind = MessageKinds.主链路登录请求消息 };
        var response = new JTMessage { Kind = MessageKinds.主链路登录应答消息, Reply = true };
        var another = new JTMessage { Kind = MessageKinds.主链路登录请求消息 };

        Assert.True(codec.Match(request, response));
        Assert.False(codec.Match(request, another));
        Assert.False(codec.Match(response, response));
    }
}
