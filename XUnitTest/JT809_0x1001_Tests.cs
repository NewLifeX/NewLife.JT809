using System.IO;
using NewLife;
using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using Xunit;

namespace XUnitTest
{
    public class JT809_0x1001_Tests
    {
        MessageFactory _factory;
        public JT809_0x1001_Tests()
        {
            _factory = MessageFactory.Instance;
            _factory.Init();
        }

        [Fact]
        public void Test1()
        {
            var str =
                "0133EFB8" +
                "3230313830393230" +
                "0001E240" +
                "3132372E-302E302E-31000000-00000000-00000000-00000000-00000000-00000000" +
                "0329";
            var dt = str.ToHex();

            var reader = Helper.CreateReader(new MemoryStream(dt));
            //reader.IgnoreMembers.Add("DownLinkIP");
            //reader.IgnoreMembers.Add("DownLinkPort");
            reader.EnableTrace();

            var body = reader.Read<JT809_0x1001>();

            Assert.NotNull(body);
            Assert.Equal(20180920u, body.UserId);
            Assert.Equal("20180920", body.Password);
            Assert.Equal(123456u, body.MsgGNSSCENTERID);
            Assert.Equal("127.0.0.1", body.DownLinkIP.TrimEnd('\0'));
            Assert.Equal(809, body.DownLinkPort);
        }
    }
}
