using System;
using NewLife;
using NewLife.JT809.Protocols;
using Xunit;

namespace XUnitTest
{
    public class JTMessage_Tests
    {
        MessageFactory _factory;
        public JTMessage_Tests()
        {
            _factory = MessageFactory.Instance;
            _factory.Init();
        }

        [Fact]
        public void Test1()
        {
            var str =
                "5B" +
                "00000076" +
                "00000002" +
                "1400" +
                "00000000" +
                "010000" +
                "00" +
                "00000000" +
                "0000018157AED15A01" +
                "14020000004E31323334353637383931320001000000005E7B1E28000000005E7B1E28000000005E7B1E28B4A8413132333435000000000000000000000000000131323334353637383931320000007B00000000" +
                "F654" +
                "5D";
            var dt = str.ToHex();

            var config = new JTConfig { Version = JTVersion.JTT2019 };
            var msg = new JTMessage();
            var rs = msg.Read(dt, config);

            Assert.True(rs);
            Assert.Equal(2u, msg.Sequence);
            Assert.Equal(MessageKinds.主链路报警信息交互消息, msg.Kind);
            Assert.Equal(0u, msg.MsgGNSSCenterId);
            Assert.Equal(0x010000u, msg.VersionFlag);
            Assert.Equal(0u, msg.EncryptFlag);
            Assert.Equal(0u, msg.EncryptKey);
            Assert.Equal("2022-06-12T11:31:23.6110000".ToDateTime(), msg.Time);

            // 校验写入
            var pk = msg.ToPacket();
            //Assert.Equal(dt.ToHex(), pk.ToHex(256));

            //var rm = _factory.Parse(msg) as RegisterMessage;
            //Assert.NotNull(rm);

            //Assert.Equal(0, rm.Province);
            //Assert.Equal(0, rm.City);
            //Assert.Equal("44030072104", rm.Provider);
            //Assert.Equal("P9", rm.Model);
            //Assert.Equal("5976616", rm.DeviceId);
            //Assert.Equal(CarColors.黄色, rm.Color);
            //Assert.Equal("粤ABB280", rm.PlateNo);
        }

        [Fact]
        public void Test2()
        {
            var str =
                "5B" +
                "00000092" +
                "00000682" +
                "9400" +
                "0133EFB8" +
                "010000" +
                "00" +
                "0000270F" +
                "D4C1413132333435000000000000000000000000000294010000005C010002000000005A01AC3F40123FFAA1000000005A01AC4D5001736D616C6C636869000000000000000031323334353637383930310000000000000000003132333435364071712E636F6D00000000000000000000000000000000000000" +
                "7AEA" +
                "5D";
            var dt = str.ToHex();

            var config = new JTConfig { Version = JTVersion.JTT2011 };
            var msg = new JTMessage();
            var rs = msg.Read(dt, config);

            Assert.True(rs);
            Assert.Equal(1666u, msg.Sequence);
            Assert.Equal(MessageKinds.从链路报警信息交互消息, msg.Kind);
            Assert.Equal(20180920u, msg.MsgGNSSCenterId);
            Assert.Equal(0x010000u, msg.VersionFlag);
            Assert.Equal(0u, msg.EncryptFlag);
            Assert.Equal(9999u, msg.EncryptKey);
            Assert.Equal("".ToDateTime(), msg.Time);

            // 校验写入
            var pk = msg.ToPacket();
            Assert.Equal(dt.ToHex(), pk.ToHex(256));

            //var rm = _factory.Parse(msg) as RegisterMessage;
            //Assert.NotNull(rm);

            //Assert.Equal(0, rm.Province);
            //Assert.Equal(0, rm.City);
            //Assert.Equal("44030072104", rm.Provider);
            //Assert.Equal("P9", rm.Model);
            //Assert.Equal("5976616", rm.DeviceId);
            //Assert.Equal(CarColors.黄色, rm.Color);
            //Assert.Equal("粤ABB280", rm.PlateNo);
        }

        [Fact]
        public void TestJT2019()
        {
            var str =
                "5B" +
                "00000054" +
                "00000002" +
                "1001" +
                "0133EFB8" +
                "010000" +
                "00" +
                "0000270F" +
                "0000018157AED15A01" +

                "0133EFB8" +
                "3230313830393230" +
                "0001E240" +
                "3132372E-302E302E-31000000-00000000-00000000-00000000-00000000-00000000" +
                "0329" +

                "024A" +
                "5D";
            var dt = str.ToHex();

            var msg = new JTMessage();
            var rs = msg.Read(dt);

            Assert.True(rs);
            Assert.Equal(2u, msg.Sequence);
            Assert.Equal(MessageKinds.主链路登录请求消息, msg.Kind);
            Assert.Equal(20180920u, msg.MsgGNSSCenterId);
            Assert.Equal(0x010000u, msg.VersionFlag);
            Assert.Equal(0u, msg.EncryptFlag);
            Assert.Equal(9999u, msg.EncryptKey);
            Assert.Equal("2022-06-12T11:31:23.6110000".ToDateTime(), msg.Time);

            Assert.True(msg.JTT2019);
        }
    }
}
