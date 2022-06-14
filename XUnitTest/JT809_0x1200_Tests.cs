using System;
using System.IO;
using NewLife;
using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using Xunit;

namespace XUnitTest
{
    public class JT809_0x1200_Tests
    {
        MessageFactory _factory;
        public JT809_0x1200_Tests()
        {
            _factory = MessageFactory.Instance;
            _factory.Init();
        }

        [Fact]
        public void TestJT20192()
        {
            var str =
                "5B" +
                "0000003E" +
                "00000004" +
                "1200" +
                "03938706" +
                "010000" +
                "00" +
                "00989681" +
                "0000000062A73CF7" +

                "BBA641413532303030000000000000000000000000" +
                "01" +
                "1205" +
                "0000" +
                "00220001" +
                "00000000" +

                "B1B7" +
                "5D";
            var dt = str.ToHex();

            var msg = new JTMessage { JTT2019 = true };
            var rs = msg.Read(dt);

            //Assert.True(rs);
            Assert.Equal(4u, msg.Sequence);
            Assert.Equal(MessageKinds.主链路车辆动态信息交换业务, msg.Kind);
            Assert.Equal(60000006u, msg.CenterId);
            Assert.Equal(0x010000u, msg.VersionFlag);
            Assert.Equal(0u, msg.EncryptFlag);
            Assert.Equal(10000001u, msg.EncryptKey);
            Assert.Equal("2022-06-13T13:34:47".ToDateTime(), msg.Time);

            Assert.True(msg.JTT2019);

            var body = msg.ReadModel<JT809_0x1200>();
        }
    }
}