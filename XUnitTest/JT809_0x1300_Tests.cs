using System;
using System.ComponentModel;
using System.IO;
using NewLife;
using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;
using NewLife.Serialization;
using Xunit;

namespace XUnitTest;

[DisplayName("平台间信息交互消息")]
public class JT809_0x1300_Tests
{
    private static Byte[] WriteBody(Object body, JTVersion version)
    {
        var ms = new MemoryStream();
        Assert.True(((IAccessor)body).Write(ms, new JTMessage { Version = version }));

        return ms.ToArray();
    }

    private static T ReadBody<T>(Byte[] data, JTVersion version) where T : IAccessor, new()
    {
        var rs = new T();
        Assert.True(rs.Read(new MemoryStream(data), new JTMessage { Version = version }));

        return rs;
    }

    private static Byte[] WriteAuto(Object body, JTVersion version)
    {
        var ms = new MemoryStream();
        Helper.CreateWriter(ms, new JTMessage { Version = version }).Write(body);

        return ms.ToArray();
    }

    [Fact]
    [DisplayName("0x1301 平台查岗应答按版本读写")]
    public void TestPostQueryAck()
    {
        var content = "在岗";

        // 2011：对象类型(1) + 对象ID(12) + 信息ID(4) + 长度(4) + 内容
        var body = new JT809_0x1300_0x1301
        {
            ObjectType = JT809_0x1301_ObjectType.当前连接的下级平台,
            ObjectID = "340100000001",
            InfoID = 7,
            InfoContent = content,
        };
        var buf = WriteBody(body, JTVersion.JT2011);
        Assert.Equal(1 + 12 + 4 + 4 + Helper.Encoding.GetByteCount(content), buf.Length);

        var rs = ReadBody<JT809_0x1300_0x1301>(buf, JTVersion.JT2011);
        Assert.Equal(JT809_0x1301_ObjectType.当前连接的下级平台, rs.ObjectType);
        Assert.Equal("340100000001", rs.ObjectID);
        Assert.Equal(7u, rs.InfoID);
        Assert.Equal(content, rs.InfoContent);

        // 2019：对象类型(1) + 应答人(16) + 电话(20) + 对象ID(20) + 源子业务(2) + 源序列号(4) + 长度(4) + 内容
        var body9 = new JT809_0x1300_0x1301
        {
            ObjectType = JT809_0x1301_ObjectType.下级平台所属单一业户,
            Responder = "张三",
            ResponderTel = "13800138000",
            ObjectID = "34010000001123456789",
            SourceDataType = 0x9301,
            SourceMsgSn = 11,
            InfoContent = content,
        };
        var buf9 = WriteBody(body9, JTVersion.JT2019);
        Assert.Equal(1 + 16 + 20 + 20 + 2 + 4 + 4 + Helper.Encoding.GetByteCount(content), buf9.Length);

        var rs9 = ReadBody<JT809_0x1300_0x1301>(buf9, JTVersion.JT2019);
        Assert.Equal("张三", rs9.Responder);
        Assert.Equal("13800138000", rs9.ResponderTel);
        Assert.Equal("34010000001123456789", rs9.ObjectID);
        Assert.Equal((UInt16)0x9301, rs9.SourceDataType);
        Assert.Equal(11u, rs9.SourceMsgSn);
        Assert.Equal(content, rs9.InfoContent);
    }

    [Fact]
    [DisplayName("0x1302 下发报文应答按版本区分")]
    public void TestPlatformInfoAck()
    {
        // 2011：信息ID(4)
        var buf = WriteBody(new JT809_0x1300_0x1302 { InfoID = 3 }, JTVersion.JT2011);
        Assert.Equal(4, buf.Length);
        Assert.Equal(3u, ReadBody<JT809_0x1300_0x1302>(buf, JTVersion.JT2011).InfoID);

        // 2019：源子业务类型(2) + 源报文序列号(4)
        var buf9 = WriteBody(new JT809_0x1300_0x1302 { SourceDataType = 0x9302, SourceMsgSn = 5, InfoID = 3 }, JTVersion.JT2019);
        Assert.Equal(6, buf9.Length);

        var rs9 = ReadBody<JT809_0x1300_0x1302>(buf9, JTVersion.JT2019);
        Assert.Equal((UInt16)0x9302, rs9.SourceDataType);
        Assert.Equal(5u, rs9.SourceMsgSn);
    }

    [Fact]
    [DisplayName("0x9301 平台查岗请求按版本读写")]
    public void TestPostQueryReq()
    {
        var content = "请查岗";

        // 2011：信息ID(4) + 长度(4) + 内容
        var body = new JT809_0x9300_0x9301 { InfoID = 1, InfoContent = content };
        var buf = WriteBody(body, JTVersion.JT2011);
        Assert.Equal(4 + 4 + Helper.Encoding.GetByteCount(content), buf.Length);

        var rs = ReadBody<JT809_0x9300_0x9301>(buf, JTVersion.JT2011);
        Assert.Equal(1u, rs.InfoID);
        Assert.Equal(content, rs.InfoContent);

        // 2019：对象类型(1) + 对象ID(20) + 应答时限(1) + 信息ID(4) + 长度(4) + 内容
        var body9 = new JT809_0x9300_0x9301
        {
            ObjectType = JT809_0x9301_ObjectType.下级平台所属所有业户,
            ObjectID = "34010000001123456789",
            AnswerTime = 10,
            InfoID = 2,
            InfoContent = content,
        };
        var buf9 = WriteBody(body9, JTVersion.JT2019);
        Assert.Equal(1 + 20 + 1 + 4 + 4 + Helper.Encoding.GetByteCount(content), buf9.Length);

        var rs9 = ReadBody<JT809_0x9300_0x9301>(buf9, JTVersion.JT2019);
        Assert.Equal(JT809_0x9301_ObjectType.下级平台所属所有业户, rs9.ObjectType);
        Assert.Equal((Byte)10, rs9.AnswerTime);
        Assert.Equal(2u, rs9.InfoID);
        Assert.Equal(content, rs9.InfoContent);
    }

    [Fact]
    [DisplayName("0x9302 下发报文请求按版本读写")]
    public void TestPlatformInfoReq()
    {
        var content = "通知内容";

        // 2011：信息ID(4) + 长度(4) + 内容
        var buf = WriteBody(new JT809_0x9300_0x9302 { InfoID = 8, InfoContent = content }, JTVersion.JT2011);
        Assert.Equal(4 + 4 + Helper.Encoding.GetByteCount(content), buf.Length);
        Assert.Equal(8u, ReadBody<JT809_0x9300_0x9302>(buf, JTVersion.JT2011).InfoID);

        // 2019：对象类型(1) + 对象ID(20) + 信息ID(4) + 长度(4) + 内容
        var body9 = new JT809_0x9300_0x9302
        {
            ObjectType = JT809_0x9302_ObjectType.下级平台所属所有平台,
            ObjectID = "34010000001123456789",
            InfoID = 9,
            InfoContent = content,
        };
        var buf9 = WriteBody(body9, JTVersion.JT2019);
        Assert.Equal(1 + 20 + 4 + 4 + Helper.Encoding.GetByteCount(content), buf9.Length);

        var rs9 = ReadBody<JT809_0x9300_0x9302>(buf9, JTVersion.JT2019);
        Assert.Equal(JT809_0x9302_ObjectType.下级平台所属所有平台, rs9.ObjectType);
        Assert.Equal(9u, rs9.InfoID);
        Assert.Equal(content, rs9.InfoContent);
    }

    [Fact]
    [DisplayName("0x9102/0x1102 链路情况上报请求与应答")]
    public void TestManageReport()
    {
        var start = 1263085674L.ToDateTime();
        var end = 1263089274L.ToDateTime();

        var req = new JT809_0x9102
        {
            DataType = 0x9102,
            PlatformId = "34010000001",
            StartTime = start,
            EndTime = end,
        };
        var reqMs = new MemoryStream();
        Assert.True(req.Write(reqMs, new JTMessage { Version = JTVersion.JT2019 }));

        // 子业务类型(2) + 数据长度(4) + 平台编码(11) + 开始(8) + 结束(8)
        Assert.Equal(33L, reqMs.Length);
        Assert.Equal(27u, req.DataLength);

        reqMs.Position = 0;
        var reqRs = new JT809_0x9102();
        Assert.True(reqRs.Read(reqMs, new JTMessage { Version = JTVersion.JT2019 }));
        Assert.Equal("34010000001", reqRs.PlatformId);
        Assert.Equal(start, reqRs.StartTime);
        Assert.Equal(end, reqRs.EndTime);

        var ack = new JT809_0x1102
        {
            DataType = 0x1102,
            PlateformId = "34010000001",
            StartTime = start,
            EndTime = end,
            LoseDymamicSum = 12,
            DisconnectNum = 2,
            DisconnectTime = 300,
        };
        var ackMs = new MemoryStream();
        Assert.True(ack.Write(ackMs, new JTMessage { Version = JTVersion.JT2019 }));

        // 子业务类型(2) + 数据长度(4) + 平台编码(11) + 开始(8) + 结束(8) + 丢失总数(4) + 断开次数(1) + 断开时长(4)
        Assert.Equal(42L, ackMs.Length);

        ackMs.Position = 0;
        var ackRs = new JT809_0x1102();
        Assert.True(ackRs.Read(ackMs, new JTMessage { Version = JTVersion.JT2019 }));
        Assert.Equal("34010000001", ackRs.PlateformId);
        Assert.Equal(12u, ackRs.LoseDymamicSum);
        Assert.Equal((Byte)2, ackRs.DisconnectNum);
        Assert.Equal(300u, ackRs.DisconnectTime);
    }

    [Fact]
    [DisplayName("0x1303/0x9303 消息补传请求结构一致")]
    public void TestRetran()
    {
        var time = 1263085674L.ToDateTime();

        var up = WriteAuto(new JT809_0x1300_0x1303
        {
            RetranDataType = 0x1402,
            SerialCount = 2,
            BeginMessageNumber = 10,
            EndMessageNumber = 11,
            Time = time,
        }, JTVersion.JT2019);
        Assert.Equal(2 + 1 + 4 + 4 + 8, up.Length);

        var down = WriteBody(new JT809_0x9300_0x9303
        {
            RetranDataType = 0x1402,
            SerialCount = 2,
            BeginMessageNumber = 10,
            EndMessageNumber = 11,
            Time = time,
        }, JTVersion.JT2019);
        Assert.Equal(up.Length, down.Length);

        var rs = ReadBody<JT809_0x9300_0x9303>(down, JTVersion.JT2019);
        Assert.Equal((UInt16)0x1402, rs.RetranDataType);
        Assert.Equal((Byte)2, rs.SerialCount);
        Assert.Equal(10u, rs.BeginMessageNumber);
        Assert.Equal(11u, rs.EndMessageNumber);
        Assert.Equal(time, rs.Time);
    }
}
