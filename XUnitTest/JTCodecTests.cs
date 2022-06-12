using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewLife;
using NewLife.JT809;
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

            Assert.Equal(dt.Length - 1 - 2 - 1, len);
        }
    }
}
