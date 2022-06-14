using System;
using System.IO;
using NewLife;
using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using Xunit;

namespace XUnitTest
{
    public class JT809_0x9102_Tests
    {
        private readonly MessageFactory _factory;
        public JT809_0x9102_Tests()
        {
            _factory = MessageFactory.Instance;
            _factory.Init();
        }

        [Fact]
        public void Test9102()
        {
            var msg = new JT809_0x9102
            {
                PlatformId = "XXX",
                StartTime = DateTime.Today,
                EndTime = DateTime.Now,
            };

            var ms = new MemoryStream();
            msg.Write(ms, null);

            var buf = ms.ToArray();

            ms = new MemoryStream(buf);
            var msg2 = new JT809_0x9102();
            var rs = msg2.Read(ms, null);

            Assert.True(rs);
        }

        [Fact]
        public void Test9102_2019()
        {
            var header = new JTMessage { JTT2019 = true };

            var msg = new JT809_0x9102
            {
                PlatformId = "XXX",
                StartTime = DateTime.Today,
                EndTime = DateTime.Now,
            };

            var ms = new MemoryStream();
            msg.Write(ms, header);

            var buf = ms.ToArray();

            ms = new MemoryStream(buf);
            var msg2 = new JT809_0x9102();
            var rs = msg2.Read(ms, header);

            Assert.True(rs);
        }

        [Fact]
        public void TestJT20192()
        {
            var str =
                "5B" +
                "00000040" +
                "00000003" +
                "9102" +
                "7AF8FE92" +
                "01020F" +
                "00" +
                "00000000" +
                "0000018163522FFB" +

                "00001B4D4648475A024D5351000000000001816160EF1B000001816197DD9B" +

                "FA4E" +
                "5D";
            var dt = str.ToHex();

            var msg = new JTMessage { JTT2019 = true };
            var rs = msg.Read(dt);

            //Assert.True(rs);
            Assert.Equal(MessageKinds.平台链路连接情况与车辆定位消息传输情况上报请求消息_2019, msg.Kind);
            Assert.Equal(2063138450u, msg.CenterId);
            Assert.Equal(0x01020Fu, msg.VersionFlag);
            Assert.Equal(0u, msg.EncryptFlag);
            Assert.Equal(0u, msg.EncryptKey);
            Assert.Equal("2022-06-14T17:45:39.5790000".ToDateTime(), msg.Time);

            Assert.True(msg.JTT2019);

            var body = msg.ReadModel<JT809_0x9102>();
        }
    }
}