using NewLife.JT809;
using NewLife.Net;

namespace JT809Server
{
    internal class JT809Server : NetServer<JT809Session>
    {
        public JT809Server()
        {
            Port = 809;
            Add<JTCodec>();
        }
    }

    internal class JT809Session : NetSession
    {

    }
}