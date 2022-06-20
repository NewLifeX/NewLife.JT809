using JT809Client.Controllers;
using NewLife.JT809;

namespace JT809Client;

internal class SlaveServer : JTServer
{
    public SlaveServer() => Port = 1809;

    protected override void OnStart()
    {
        AddHandler(new NormalController());
        AddHandler(new InfoController());
        AddHandler(new J9200Controller());
        AddHandler(new J9300Controller());
        AddHandler(new J9400Controller());
        AddHandler(new J9500Controller());
        AddHandler(new J9600Controller());

        base.OnStart();
    }
}