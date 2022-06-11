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
            var dt = "5B0000006E00000002140000000000010000000000000014020000004E31323334353637383931320001000000005E7B1E28000000005E7B1E28000000005E7B1E28B4A8413132333435000000000000000000000000000131323334353637383931320000007B000000005A024A5D".ToHex();

            var msg = new JTMessage();
            var rs = msg.Read(dt);

            Assert.True(rs);
            Assert.Equal(1u, msg.Sequence);
            Assert.Equal(MessageKinds.主链路登录请求消息, msg.Kind);
            Assert.Equal(1u, msg.MsgGNSSCenterId);
            Assert.Equal(1u, msg.VersionFlag);
            Assert.Equal(1u, msg.EncryptFlag);
            Assert.Equal(1u, msg.EncryptKey);
            Assert.Equal("".ToDateTime(), msg.Time);

            // 校验写入
            var pk = msg.ToPacket();
            Assert.Equal(dt.ToHex(), pk.ToHex(64));

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
    }
}
