using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife;
using Xunit;
using NewLife.JT809.Models;
using NewLife.JT809.SubMessages;

namespace XUnitTest;

public class JT809_0x1600_Tests
{
    MessageFactory _factory;
    public JT809_0x1600_Tests()
    {
        _factory = MessageFactory.Instance;
        _factory.Init();
    }

    [Fact]
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
        Assert.Equal(0x78859725u, msg.CenterId);
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

    [Fact]
    public void Test1601_2()
    {
        var str =
            "5B" +
            "000000BE" +
            "0000000A" +
            "1601" +
            "78959A5E" +
            "010102" +
            "0F" +
            "01754FDB" +
            "5465DDD97A001B49B93B3CE336435E024492402543078DD6E269EB9F3B8B58411326BEE94D69CB83B12B5E01D9D5561784DB292230D9900F3EA5B816E896DC4BFAF48BA2F1D027497CD2366104906D3E715856DD23923C7AE9B5E032DD31090EAC11F1E76ACAC9B2A719BD88B281045500746081E2B00404E6C374C67BAF3490BDC88098A42FB4AB3495E35537E3EA3EAF871699267452347833D13839DEBAF1CB8D88BCB865240C22" +
            "5D";
        var dt = str.ToHex();

        var msg = new JTMessage { Version = JTVersion.JT2013 };
        var rs = msg.Read(dt);

        Assert.True(rs);
        Assert.Equal(0x3672u, msg.Sequence);
        Assert.Equal(MessageKinds.主链路静态信息交换消息, msg.Kind);
        Assert.Equal(2022020901u, msg.CenterId);
        Assert.Equal(0x78859725u, msg.CenterId);
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
}