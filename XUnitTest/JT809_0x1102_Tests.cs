using System;
using System.IO;
using NewLife;
using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using Xunit;

namespace XUnitTest
{
    public class JT809_0x1102_Tests
    {
        MessageFactory _factory;
        public JT809_0x1102_Tests()
        {
            _factory = MessageFactory.Instance;
            _factory.Init();
        }

        [Fact]
        public void Test2()
        {
            var str =
                "5B" +
                "0000004C" +
                "00000002" +
                "1102" +
                "03938706" +
                "010000" +
                "00" +
                "009896810000000062A6FA562766000000003130303130000000000000000001815C405CE2000001815C41E382" +

                "4A54" +
                "5D";
            var dt = str.ToHex();

            var msg = new JTMessage { JTT2019 = false };
            var rs = msg.Read(dt);

            Assert.True(rs);
            Assert.Equal(1u, msg.Sequence);
            Assert.Equal(MessageKinds.主链路登录请求消息, msg.Kind);
            Assert.Equal(123456u, msg.CenterId);
            Assert.Equal(0x010000u, msg.VersionFlag);
            Assert.Equal(0u, msg.EncryptFlag);
            Assert.Equal(10000001u, msg.EncryptKey);
            //Assert.Equal("2022-06-12T11:31:23.6110000".ToDateTime(), msg.Time);

            // 校验写入
            var pk = msg.ToPacket();
            Assert.Equal(dt.ToHex(), pk.ToHex(256));

            var body = msg.ReadModel<JT809_0x1001>();

            Assert.NotNull(body);
            Assert.Equal(1234u, body.UserId);
            Assert.Equal("1234", body.Password);
            Assert.Equal(0u, body.MsgGNSSCENTERID);
            Assert.Equal("127.0.0.1", body.DownLinkIP.TrimEnd('\0'));
            Assert.Equal(7000, body.DownLinkPort);

            // 序列化
            var ms = new MemoryStream();
            var writer = Helper.CreateWriter(ms);
            writer.Write(body);

            var hex = ms.ToArray().ToHex();
            Assert.Equal(msg.Payload.ToHex(256), hex);
        }
    }
}
