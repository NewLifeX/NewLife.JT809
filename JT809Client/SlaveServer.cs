using NewLife.JT809;

namespace JT809Client;

internal class SlaveServer : JTServer
{
    public SlaveServer() => Port = 1809;
}