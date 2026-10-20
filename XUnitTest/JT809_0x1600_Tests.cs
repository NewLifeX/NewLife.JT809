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

[DisplayName("0x1600 车辆静态信息交换")]
public class JT809_0x1600_Tests
{
    private readonly MessageFactory _factory;
    public JT809_0x1600_Tests()
    {
        _factory = MessageFactory.Instance;
        _factory.Init();
    }

    [Fact]
    [DisplayName("解析2013版0x1600并读取0x1601车辆静态信息")]
    public void Test1601()
    {
        var str =
            "5B" +
            "000000CD" +
            "00003672" +
            "1600" +
            "78859725" +
            "02010F" +
            "00" +
            "00000000" +

            // 0x1601
            "CDEE41543030313100000000000000000000000000" +
            "02" +
            "1601" +
            "00000097" +

            "56494E3A3DCDEE4154303031313B56454849434C455F434F4C4F523A3D323B56454849434C455F545950453A3D32313B5452414E535F545950453A3D3033313B56454849434C455F4E4154494F4E414C4954593A3D3334303131313B4F574552535F49443A3D333037343B4F574552535F4E414D453A3DBACFB7CACAFDB3B5BFC6BCBCD3D0CFDEB9ABCBBE3B4F574552535F54454C3A3D0000" +
            "5D";
        var dt = str.ToHex();

        var msg = new JTMessage { Version = JTVersion.JT2013 };
        var rs = msg.Read(dt);

        Assert.True(rs);
        Assert.Equal(0x3672u, msg.Sequence);
        Assert.Equal(MessageKinds.主链路静态信息交换消息, msg.Kind);
        Assert.Equal(2022020901u, msg.CenterId);
        Assert.Equal(0x02010Fu, msg.VersionFlag);
        Assert.Equal(0u, msg.EncryptFlag);
        Assert.Equal(0u, msg.EncryptKey);
        Assert.Equal(DateTime.MinValue, msg.Time);

        Assert.Equal(JTVersion.JT2013, msg.Version);

        var model = msg.ReadModel<JT809_0x1600>();
        Assert.NotNull(model);
        Assert.Equal("皖AT0011", model.VehicleNo);
        Assert.Equal(JT809VehicleColorType.黄色, model.VehicleColor);

        var body = model.Body as JT809_0x1600_0x1601;
        Assert.NotNull(body);
        Assert.Equal("VIN:=皖AT0011;VEHICLE_COLOR:=2;VEHICLE_TYPE:=21;TRANS_TYPE:=031;VEHICLE_NATIONALITY:=340111;OWERS_ID:=3074;OWERS_NAME:=合肥数车科技有限公司;OWERS_TEL:=", body.CarInfo);

        var inf = body.Info;
        Assert.NotNull(inf);
        Assert.Equal("皖AT0011", inf.Vin);
        Assert.Equal(JT809VehicleColorType.黄色, inf.VehicleColor);
        Assert.Equal(21, inf.VehicleType);
        Assert.Equal("031", inf.TransType);
        Assert.Equal("340111", inf.VehicleNationality);
        Assert.Equal("3074", inf.OwnersId);
        Assert.Equal("合肥数车科技有限公司", inf.OwnersName);
        Assert.Empty(inf.OwnersTel);
    }

    /// <summary>加密后的负载在转义、CRC 与解密链路中保持不变</summary>
    [Fact]
    [DisplayName("加密帧写入后可用相同密钥解出原始负载")]
    public void TestEncrypted()
    {
        var config = new JTConfig
        {
            Version = JTVersion.JT2011,
            M1 = 10000001,
            IA1 = 20000001,
            IC1 = 30000001,
        };

        // 负载刻意包含全部转义字符，覆盖加密后再转义的组合场景
        var payload = new Byte[] { 0x01, 0x5B, 0x5A, 0x5D, 0x5E, 0x5B, 0x5D, 0x00, 0xFF, 0x7F };
        var msg = new JTMessage
        {
            Sequence = 0x3672,
            Kind = MessageKinds.主链路静态信息交换消息,
            CenterId = 2022020901,
            VersionFlag = 0x02010F,
            Version = JTVersion.JT2011,
            Payload = new ArrayPacket(payload),
        };

        var ms = new MemoryStream();
        Assert.True(msg.Write(ms, config));
        Assert.Equal(1, (Int32)msg.EncryptFlag);
        Assert.NotEqual(0u, msg.EncryptKey);

        var frame = ms.ToArray();
        Assert.Equal(0x5B, (Int32)frame[0]);
        Assert.Equal(0x5D, (Int32)frame[^1]);

        var rs = new JTMessage();
        Assert.True(rs.Read(frame, config));
        Assert.Equal(0x3672u, rs.Sequence);
        Assert.Equal(MessageKinds.主链路静态信息交换消息, rs.Kind);
        Assert.Equal(2022020901u, rs.CenterId);
        Assert.Equal(0x02010Fu, rs.VersionFlag);
        Assert.Equal(1, (Int32)rs.EncryptFlag);
        Assert.Equal(msg.EncryptKey, rs.EncryptKey);
        Assert.Equal(payload, rs.Payload.ReadBytes());
    }

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
    [DisplayName("0x1601 补报静态信息应答按版本读写")]
    public void TestVehicleAddedAckVersion()
    {
        var content = "VIN:=京CJ6789;PLATE_COLOR:=2";

        // 2011：车辆信息直接填满后续数据长度
        var buf = WriteBody(new JT809_0x1600_0x1601 { CarInfo = content }, JTVersion.JT2011);
        Assert.Equal(Helper.Encoding.GetByteCount(content), buf.Length);

        var rs = ReadBody<JT809_0x1600_0x1601>(buf, JTVersion.JT2011);
        Assert.Equal(content, rs.CarInfo);
        Assert.NotNull(rs.Info);

        // 2019：源子业务(2) + 源序列号(4) + 车辆信息
        var buf9 = WriteBody(new JT809_0x1600_0x1601 { SourceDataType = 0x9601, SourceMsgSn = 8, CarInfo = content }, JTVersion.JT2019);
        Assert.Equal(6 + Helper.Encoding.GetByteCount(content), buf9.Length);

        var rs9 = ReadBody<JT809_0x1600_0x1601>(buf9, JTVersion.JT2019);
        Assert.Equal((UInt16)0x9601, rs9.SourceDataType);
        Assert.Equal(8u, rs9.SourceMsgSn);
        Assert.Equal(content, rs9.CarInfo);
    }

    [Fact]
    [DisplayName("0x1602 补报行驶路线应答直接携带路线字节")]
    public void TestDrvLineAdded()
    {
        var line = new Byte[] { 0x86, 0x06, 0x01, 0x02, 0x03 };
        var body = new JT809_0x1600_0x1602 { DRVLine = line };

        var buf = WriteBody(body, JTVersion.JT2019);
        Assert.Equal(line.Length, buf.Length);

        var rs = ReadBody<JT809_0x1600_0x1602>(buf, JTVersion.JT2019);
        Assert.Equal(line, rs.DRVLine);
    }

    [Fact]
    [DisplayName("0x9601 补报静态信息请求无数据部分")]
    public void TestVehicleAddedReq()
    {
        Assert.Empty(WriteAuto(new JT809_0x9600_0x9601(), JTVersion.JT2011));
        Assert.Empty(WriteAuto(new JT809_0x9600_0x9601(), JTVersion.JT2019));
    }
}
