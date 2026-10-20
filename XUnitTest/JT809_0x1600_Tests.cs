using System;
using System.ComponentModel;
using System.IO;
using NewLife;
using NewLife.Data;
using NewLife.JT809.Messages;
using NewLife.JT809.Models;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;
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
}
