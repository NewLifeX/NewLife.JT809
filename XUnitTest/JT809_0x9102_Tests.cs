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
            //msg.Write(ms, null);
            var bn = Helper.CreateWriter(ms);
            bn.Write(msg);

            var buf = ms.ToArray();

            ms = new MemoryStream(buf);
            var msg2 = new JT809_0x9102();
            //var rs = msg2.Read(ms, null);
            var bn2=Helper.CreateWriter(ms);
            Object obj = msg2;
            var rs = bn2.TryRead(null, ref obj);

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
            //msg.Write(ms, header);
            var bn = Helper.CreateWriter(ms);
            bn.Write(msg);

            var buf = ms.ToArray();

            ms = new MemoryStream(buf);
            var msg2 = new JT809_0x9102();
            //var rs = msg2.Read(ms, header);
            var bn2 = Helper.CreateWriter(ms);
            Object obj = msg2;
            var rs = bn2.TryRead(null, ref obj);

            Assert.True(rs);
        }

        [Fact]
        public void TestJT20192()
        {
            var str =
                "5B" +
                "00000043" +
                "00000003" +
                "9102" +
                "00003039" +
                "01020F" +
                "00" +
                "00000000" +
                "0000018163B6BEF3" +

                "0000" +
                "00000000" +
                "36506657-35614F46-000000" +
                "0000018161C0B382" +
                "0000018161F7A202" +

                "3D06" +
                "5D";
            var dt = str.ToHex();

            var msg = new JTMessage { JTT2019 = true };
            var rs = msg.Read(dt);

            //Assert.True(rs);
            Assert.Equal(MessageKinds.平台链路连接情况与车辆定位消息传输情况上报请求消息_2019, msg.Kind);
            Assert.Equal(12345u, msg.CenterId);
            Assert.Equal(0x01020Fu, msg.VersionFlag);
            Assert.Equal(0u, msg.EncryptFlag);
            Assert.Equal(0u, msg.EncryptKey);
            Assert.Equal("2022-06-14T19:35:29.7790000".ToDateTime(), msg.Time);

            Assert.True(msg.JTT2019);

            var body = msg.ReadModel<JT809_0x9102>();
        }

        [Fact]
        public void Test222()
        {
            var str = "27660000001B3130303130000000000000000000004EBC924F000000004EBC924F";
            var dt = str.ToHex();

            var bn = Helper.CreateReader(new MemoryStream(dt));

            var body = bn.Read<JT809_0x9102>();

            Assert.NotNull(body);
            Assert.Equal(10086, body.DataType);
            Assert.Equal("10010", body.PlatformId);
            Assert.Equal("2011-11-11 03:11:11".ToDateTime(), body.StartTime);
            Assert.Equal("2011-11-11 03:11:11".ToDateTime(), body.EndTime);
        }
    }
}