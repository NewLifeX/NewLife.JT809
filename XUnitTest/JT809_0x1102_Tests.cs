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
                "00000040" +
                "00000004" +
                "9102" +
                "7BD4D0EB" +
                "01020F" +
                "00" +
                "00000000" +

                "0000" +
                "01815E02" +
                "55B68B00001B766245754A473869000000000001815A013A5E012B000001815A01714BAB" +

                "AA87" +
                "5D";
            var dt = str.ToHex();

            var msg = new JTMessage { JTT2019 = false };
            var rs = msg.Read(dt);

            Assert.True(rs);
            //Assert.Equal(1u, msg.Sequence);
            Assert.Equal(MessageKinds.平台链路连接情况与车辆定位消息传输情况上报请求消息_2019, msg.Kind);
            //Assert.Equal(123456u, msg.CenterId);
            //Assert.Equal(0x010000u, msg.VersionFlag);
            //Assert.Equal(0u, msg.EncryptFlag);
            //Assert.Equal(10000001u, msg.EncryptKey);
            //Assert.Equal("2022-06-12T11:31:23.6110000".ToDateTime(), msg.Time);

            // 校验写入
            var pk = msg.ToPacket();
            Assert.Equal(dt.ToHex(), pk.ToHex(256));

            var body = msg.ReadModel<JT809_0x1102>();

            Assert.NotNull(body);
            //Assert.Equal(1234u, body.UserId);
            //Assert.Equal("1234", body.Password);
            //Assert.Equal(0u, body.MsgGNSSCENTERID);
            //Assert.Equal("127.0.0.1", body.DownLinkIP.TrimEnd('\0'));
            //Assert.Equal(7000, body.DownLinkPort);

            //// 序列化
            //var ms = new MemoryStream();
            //var writer = Helper.CreateWriter(ms, msg);
            //writer.Write(body);

            //var hex = ms.ToArray().ToHex();
            //Assert.Equal(msg.Payload.ToHex(256), hex);
        }
    }
}
