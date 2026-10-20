using System;
using System.ComponentModel;
using System.IO;
using NewLife;
using NewLife.Data;
using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;
using NewLife.Serialization;
using Xunit;

namespace XUnitTest;

[DisplayName("报警信息交互")]
public class JT809_0x1400_Tests
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

    [Fact]
    [DisplayName("0x1401 报警督办应答按版本读写")]
    public void TestUrgeAck()
    {
        // 2011：督办ID(4) + 结果(1)
        var body = new JT809_0x1400_0x1401 { SupervisionID = 0x1234, Result = JT809_0x1401_Result.已处理完毕 };
        var buf = WriteBody(body, JTVersion.JT2011);
        Assert.Equal(5, buf.Length);

        var rs = ReadBody<JT809_0x1400_0x1401>(buf, JTVersion.JT2011);
        Assert.Equal(0x1234u, rs.SupervisionID);
        Assert.Equal(JT809_0x1401_Result.已处理完毕, rs.Result);

        // 2019：报文序列号(4) + 源子业务(2) + 源序列号(4) + 结果(1)
        var body9 = new JT809_0x1400_0x1401 { MsgSn = 7, SourceDataType = 0x1401, SourceMsgSn = 9, Result = JT809_0x1401_Result.将来处理 };
        var buf9 = WriteBody(body9, JTVersion.JT2019);
        Assert.Equal(11, buf9.Length);

        var rs9 = ReadBody<JT809_0x1400_0x1401>(buf9, JTVersion.JT2019);
        Assert.Equal(7u, rs9.MsgSn);
        Assert.Equal((UInt16)0x1401, rs9.SourceDataType);
        Assert.Equal(9u, rs9.SourceMsgSn);
        Assert.Equal(JT809_0x1401_Result.将来处理, rs9.Result);
    }

    [Fact]
    [DisplayName("0x1402 上报报警信息按版本读写")]
    public void TestAdptInfo()
    {
        var content = "超速报警";

        // 2011：来源(1) + 类型(2) + 时间(8) + 信息ID(4) + 长度(4) + 内容
        var body = new JT809_0x1400_0x1402
        {
            WarnSrc = JT809WarnSrc.车载终端,
            WarnType = JT809WarnType.超速报警,
            WarnTime = 1263085674L.ToDateTime(),
            InfoID = 3,
            InfoContent = content,
        };
        var buf = WriteBody(body, JTVersion.JT2011);
        Assert.Equal(1 + 2 + 8 + 4 + 4 + content.Length * 2, buf.Length);

        var rs = ReadBody<JT809_0x1400_0x1402>(buf, JTVersion.JT2011);
        Assert.Equal(JT809WarnSrc.车载终端, rs.WarnSrc);
        Assert.Equal(JT809WarnType.超速报警, rs.WarnType);
        Assert.Equal(1263085674L.ToDateTime(), rs.WarnTime);
        Assert.Equal(3u, rs.InfoID);
        Assert.Equal(content, rs.InfoContent);

        // 2019：平台编码(11)+类型(2)+时间(8)+开始(8)+结束(8)+车牌(21)+颜色(1)+平台编码(11)+线路(4)+长度(4)+内容
        var body9 = new JT809_0x1400_0x1402
        {
            SourcePlatformId = "34010000001",
            WarnType = JT809WarnType.疲劳驾驶报警,
            WarnTime = 1263085674L.ToDateTime(),
            StartTime = 1263085600L.ToDateTime(),
            EndTime = 1263085700L.ToDateTime(),
            VehicleNo = "皖AT0011",
            VehicleColor = JT809VehicleColorType.黄色,
            DestinationPlatformId = "34010000002",
            DRVLineId = 0x1234,
            InfoContent = content,
        };
        var buf9 = WriteBody(body9, JTVersion.JT2019);
        Assert.Equal(11 + 2 + 8 + 8 + 8 + 21 + 1 + 11 + 4 + 4 + content.Length * 2, buf9.Length);

        var rs9 = ReadBody<JT809_0x1400_0x1402>(buf9, JTVersion.JT2019);
        Assert.Equal("34010000001", rs9.SourcePlatformId);
        Assert.Equal(JT809WarnType.疲劳驾驶报警, rs9.WarnType);
        Assert.Equal(1263085674L.ToDateTime(), rs9.WarnTime);
        Assert.Equal(1263085600L.ToDateTime(), rs9.StartTime);
        Assert.Equal(1263085700L.ToDateTime(), rs9.EndTime);
        Assert.Equal("皖AT0011", rs9.VehicleNo);
        Assert.Equal(JT809VehicleColorType.黄色, rs9.VehicleColor);
        Assert.Equal("34010000002", rs9.DestinationPlatformId);
        Assert.Equal(0x1234u, rs9.DRVLineId);
        Assert.Equal(content, rs9.InfoContent);
    }

    [Fact]
    [DisplayName("0x9401 报警督办请求按版本读写")]
    public void TestUrgeReq()
    {
        // 2011：来源(1)+类型(2)+时间(8)+督办ID(4)+截止(8)+级别(1)+督办人(20)+电话(32)+邮箱(32)
        var body = new JT809_0x9400_0x9401
        {
            WarnSrc = JT809WarnSrc.企业监控平台,
            WarnType = 0x0002,
            WarnTime = 1263085674L.ToDateTime(),
            SupervisionID = 5,
            SupervisionEndTime = 1263089999L.ToDateTime(),
            SupervisionLevel = JT809_9401_SupervisionLevel.紧急,
            Supervisor = "张三",
            SupervisorTel = "13800138000",
            SupervisorEmail = "a@b.com",
        };
        var buf = WriteBody(body, JTVersion.JT2011);
        Assert.Equal(1 + 2 + 8 + 4 + 8 + 1 + 20 + 32 + 32, buf.Length);

        var rs = ReadBody<JT809_0x9400_0x9401>(buf, JTVersion.JT2011);
        Assert.Equal(JT809WarnSrc.企业监控平台, rs.WarnSrc);
        Assert.Equal(5u, rs.SupervisionID);
        Assert.Equal("张三", rs.Supervisor);
        Assert.Equal("13800138000", rs.SupervisorTel);
        Assert.Equal("a@b.com", rs.SupervisorEmail);
        Assert.Equal(JT809_9401_SupervisionLevel.紧急, rs.SupervisionLevel);

        // 2019：平台编码(11)+时间(8)+源子业务(2)+源序列号(4)+截止(8)+级别(1)+督办人(16)+电话(20)+邮箱(32)
        var body9 = new JT809_0x9400_0x9401
        {
            SourcePlatformId = "34010000001",
            WarnTime = 1263085674L.ToDateTime(),
            SourceDataType = 0x1402,
            MsgSn = 11,
            SupervisionEndTime = 1263089999L.ToDateTime(),
            SupervisionLevel = JT809_9401_SupervisionLevel.一般,
            Supervisor = "李四",
            SupervisorTel = "13900139000",
            SupervisorEmail = "c@d.com",
        };
        var buf9 = WriteBody(body9, JTVersion.JT2019);
        Assert.Equal(11 + 8 + 2 + 4 + 8 + 1 + 16 + 20 + 32, buf9.Length);

        var rs9 = ReadBody<JT809_0x9400_0x9401>(buf9, JTVersion.JT2019);
        Assert.Equal("34010000001", rs9.SourcePlatformId);
        Assert.Equal((UInt16)0x1402, rs9.SourceDataType);
        Assert.Equal(11u, rs9.MsgSn);
        Assert.Equal("李四", rs9.Supervisor);
        Assert.Equal("13900139000", rs9.SupervisorTel);
        Assert.Equal(JT809_9401_SupervisionLevel.一般, rs9.SupervisionLevel);
    }

    [Fact]
    [DisplayName("0x9403 实时交换报警信息按版本读写")]
    public void TestExchangeInform()
    {
        var content = "偏离路线";

        // 2011：来源(1)+类型(2)+时间(8)+长度(4)+内容
        var body = new JT809_0x9400_0x9403
        {
            WarnSrc = JT809WarnSrc.政府监管平台,
            WarnType = JT809WarnType.偏离路线报警,
            WarnTime = 1263085674L.ToDateTime(),
            WarnContent = content,
        };
        var buf = WriteBody(body, JTVersion.JT2011);
        Assert.Equal(1 + 2 + 8 + 4 + content.Length * 2, buf.Length);

        var rs = ReadBody<JT809_0x9400_0x9403>(buf, JTVersion.JT2011);
        Assert.Equal(JT809WarnSrc.政府监管平台, rs.WarnSrc);
        Assert.Equal(JT809WarnType.偏离路线报警, rs.WarnType);
        Assert.Equal(content, rs.WarnContent);

        // 2019：平台编码(11)+类型(2)+时间(8)+开始(8)+结束(8)+车牌(21)+颜色(1)+平台编码(11)+线路(4)+长度(4)+内容
        var body9 = new JT809_0x9400_0x9403
        {
            SourcePlatformId = "34010000001",
            WarnType = JT809WarnType.偏离路线报警,
            WarnTime = 1263085674L.ToDateTime(),
            StartTime = 1263085600L.ToDateTime(),
            EndTime = 1263085700L.ToDateTime(),
            VehicleNo = "皖AT0011",
            VehicleColor = JT809VehicleColorType.黄色,
            DestinationPlatformId = "11000000002",
            DRVLineId = 9,
            WarnContent = content,
        };
        var buf9 = WriteBody(body9, JTVersion.JT2019);
        Assert.Equal(11 + 2 + 8 + 8 + 8 + 21 + 1 + 11 + 4 + 4 + content.Length * 2, buf9.Length);

        var rs9 = ReadBody<JT809_0x9400_0x9403>(buf9, JTVersion.JT2019);
        Assert.Equal("34010000001", rs9.SourcePlatformId);
        Assert.Equal("皖AT0011", rs9.VehicleNo);
        Assert.Equal("11000000002", rs9.DestinationPlatformId);
        Assert.Equal(9u, rs9.DRVLineId);
        Assert.Equal(content, rs9.WarnContent);
    }

    [Fact]
    [DisplayName("0x9400 报警信息体2011带车牌、2019不带")]
    public void TestWarnEnvelope()
    {
        var sub = new JT809_0x9400_0x9403 { WarnSrc = JT809WarnSrc.车载终端, WarnContent = "越界" };

        // 2011：车牌(21)+颜色(1)+类型(2)+长度(4)+子业务
        var msg = new JTMessage { Sequence = 1, Kind = MessageKinds.从链路报警信息交互消息, CenterId = 123456, Version = JTVersion.JT2011 };
        Assert.True(msg.WriteModel(new JT809_0x9400 { VehicleNo = "皖AT0011", VehicleColor = JT809VehicleColorType.黄色, Body = sub }));
        var pk = msg.Payload.ToArray();
        Assert.Equal(21 + 1 + 2 + 4 + 1 + 2 + 8 + 4 + 4, pk.Length);
        Assert.Equal(0x94, (Int32)pk[22]);
        Assert.Equal(0x03, (Int32)pk[23]);

        var rs = msg.ReadModel<JT809_0x9400>();
        Assert.Equal("皖AT0011", rs.VehicleNo);
        Assert.Equal(JT809VehicleColorType.黄色, rs.VehicleColor);
        Assert.Equal(JT809WarnSrc.车载终端, Assert.IsType<JT809_0x9400_0x9403>(rs.Body).WarnSrc);
        Assert.Equal("越界", Assert.IsType<JT809_0x9400_0x9403>(rs.Body).WarnContent);

        // 2019：类型(2)+长度(4)+子业务，车牌在子业务数据体内
        var msg9 = new JTMessage { Sequence = 1, Kind = MessageKinds.从链路报警信息交互消息, CenterId = 123456, Version = JTVersion.JT2019 };
        Assert.True(msg9.WriteModel(new JT809_0x9400 { Body = new JT809_0x9400_0x9403 { SourcePlatformId = "34010000001", WarnContent = "越界" } }));
        var pk9 = msg9.Payload.ToArray();
        Assert.Equal(2 + 4 + 11 + 2 + 8 + 8 + 8 + 21 + 1 + 11 + 4 + 4 + 4, pk9.Length);
        Assert.Equal(0x94, (Int32)pk9[0]);
        Assert.Equal(0x03, (Int32)pk9[1]);

        var rs9 = msg9.ReadModel<JT809_0x9400>();
        var sub9 = Assert.IsType<JT809_0x9400_0x9403>(rs9.Body);
        Assert.Equal("34010000001", sub9.SourcePlatformId);
        Assert.Equal("越界", sub9.WarnContent);
    }

    [Fact]
    [DisplayName("0x1412/0x1413 报警处理结果与督办请求读写")]
    public void TestAdptResultAndUrgeInfo()
    {
        var body = new JT809_0x1400_0x1412 { SourceDataType = 0x1402, SourceMsgSn = 21, Result = JT809_0x1412_Result.不做处理 };
        var buf = WriteBody(body, JTVersion.JT2019);
        Assert.Equal(2 + 4 + 1, buf.Length);

        var rs = ReadBody<JT809_0x1400_0x1412>(buf, JTVersion.JT2019);
        Assert.Equal((UInt16)0x1402, rs.SourceDataType);
        Assert.Equal(21u, rs.SourceMsgSn);
        Assert.Equal(JT809_0x1412_Result.不做处理, rs.Result);

        var urge = new JT809_0x1400_0x1413
        {
            SourcePlatformId = "34010000001",
            WarnTime = 1263085674L.ToDateTime(),
            SourceDataType = 0x1402,
            SourceMsgSn = 22,
            SupervisionEndTime = 1263089999L.ToDateTime(),
            SupervisionLevel = JT809_0x1413_SupervisionLevel.紧急,
            Supervisor = "王五",
        };
        var buf2 = WriteBody(urge, JTVersion.JT2019);
        Assert.Equal(11 + 8 + 2 + 4 + 8 + 1 + 16 + 20 + 32, buf2.Length);

        var rs2 = ReadBody<JT809_0x1400_0x1413>(buf2, JTVersion.JT2019);
        Assert.Equal("34010000001", rs2.SourcePlatformId);
        Assert.Equal(22u, rs2.SourceMsgSn);
        Assert.Equal("王五", rs2.Supervisor);
        Assert.Equal(JT809_0x1413_SupervisionLevel.紧急, rs2.SupervisionLevel);
    }
    [Fact]
    [DisplayName("0x9402/0x1403/0x1411 报警预警与督办应答读写")]
    public void TestInformTips()
    {
        // 0x9402 下发报警预警：2019与2011均按各自布局
        var body = new JT809_0x9400_0x9402
        {
            SourcePlatformId = "34010000001",
            WarnType = JT809WarnType.超时停车,
            WarnTime = 1263085674L.ToDateTime(),
            VehicleNo = "皖AT0011",
            WarnContent = "超时停车",
        };
        var buf = WriteBody(body, JTVersion.JT2019);
        Assert.Equal(11 + 2 + 8 + 8 + 8 + 21 + 1 + 11 + 4 + 4 + 8, buf.Length);

        var rs = ReadBody<JT809_0x9400_0x9402>(buf, JTVersion.JT2019);
        Assert.Equal(JT809WarnType.超时停车, rs.WarnType);
        Assert.Equal("超时停车", rs.WarnContent);

        var body11 = new JT809_0x9400_0x9402 { WarnSrc = JT809WarnSrc.车载终端, WarnType = JT809WarnType.超速报警, WarnContent = "超速" };
        var buf11 = WriteBody(body11, JTVersion.JT2011);
        Assert.Equal(1 + 2 + 8 + 4 + 4, buf11.Length);
        Assert.Equal("超速", ReadBody<JT809_0x9400_0x9402>(buf11, JTVersion.JT2011).WarnContent);

        // 0x1403 上报报警预警
        var tip = new JT809_0x1400_0x1403 { SourcePlatformId = "34010000001", WarnType = JT809WarnType.胎压异常报警, InfoContent = "胎压异常" };
        var buf2 = WriteBody(tip, JTVersion.JT2019);
        var rs2 = ReadBody<JT809_0x1400_0x1403>(buf2, JTVersion.JT2019);
        Assert.Equal(JT809WarnType.胎压异常报警, rs2.WarnType);
        Assert.Equal("胎压异常", rs2.InfoContent);

        // 0x1411 上报报警督办应答
        var ack = new JT809_0x1400_0x1411 { SourceDataType = 0x9401, SourceMsgSn = 31, Result = JT809_0x1411_Result.处理中 };
        var buf3 = WriteBody(ack, JTVersion.JT2019);
        Assert.Equal(2 + 4 + 1, buf3.Length);
        var rs3 = ReadBody<JT809_0x1400_0x1411>(buf3, JTVersion.JT2019);
        Assert.Equal((UInt16)0x9401, rs3.SourceDataType);
        Assert.Equal(31u, rs3.SourceMsgSn);
        Assert.Equal(JT809_0x1411_Result.处理中, rs3.Result);
    }
}