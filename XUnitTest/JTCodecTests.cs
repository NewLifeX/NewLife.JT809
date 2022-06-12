using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife;
using NewLife.JT809;
using NewLife.JT809.Protocols;
using Xunit;

namespace XUnitTest
{
    public class JTCodecTests
    {
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
                "14020000004E31323334353637383931320001000000005E027B1E28000000005E027B1E28000000005E027B1E28B4A8413132333435000000000000000000000000000131323334353637383931320000007B00000000" +
                "F654" +
                "5D";
            var dt = str.ToHex();

            var codec = new JTCodec();
            var len = codec.GetLength(dt);

            Assert.Equal(dt.Length, len);

            var msg = new JTMessage { JTT2019 = true };
            var rs = msg.Read(dt);

            Assert.True(rs);
            Assert.Equal(2u, msg.Sequence);
            Assert.Equal(MessageKinds.主链路报警信息交互消息, msg.Kind);
            Assert.Equal(0u, msg.CenterId);
            Assert.Equal(0x010000u, msg.VersionFlag);
            Assert.Equal(0u, msg.EncryptFlag);
            Assert.Equal(0u, msg.EncryptKey);
            Assert.Equal("2022-06-12T11:31:23.6110000".ToDateTime(), msg.Time);

            // 校验写入
            var pk = msg.ToPacket();
            Assert.Equal(dt.ToHex(), pk.ToHex(256));
        }
    }
}
