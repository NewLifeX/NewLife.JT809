using NewLife.JT809;
using NewLife.JT809.Models;
using NewLife.JT809.SubMessages;
using NewLife.Log;
using NewLife.Security;

namespace JT809Client;

public partial class FrmQuery : Form
{
    public JTClient Client { get; set; }

    public ILog Log { get; set; }

    public FrmQuery()
    {
        InitializeComponent();
    }

    private void FrmExchange_Load(Object sender, EventArgs e)
    {
        var dic = Enum.GetValues<JT809_0x9301_ObjectType>();
        cbObjectType.DataSource = dic;
    }

    private async void btnQuery_Click(Object sender, EventArgs e)
    {
        var model = new JT809_0x9300_0x9301
        {
            ObjectType = (JT809_0x9301_ObjectType)cbObjectType.SelectedItem,
            ObjectID = "stone",
            AnswerTime = 15,
            InfoContent = txtQuestion.Text,
        };

        //await Client.RegisterAsync(plate, color, model);
        var rs = await Client.Send9300Async<JT809_0x1300_0x1301>(model);
        if (rs != null)
        {
            Log.Info("收到{0}[{1}]答复：{2}", rs.Responder, rs.ResponderTel, rs.InfoContent);
        }
    }

    private void btnLocation_Click(Object sender, EventArgs e)
    {

    }

    private void btnLocation2_Click(Object sender, EventArgs e)
    {

    }

    private void btnMonitorStart_Click(Object sender, EventArgs e)
    {

    }

    private void btnMonitorEnd_Click(Object sender, EventArgs e)
    {

    }

    private void btnHistory_Click(Object sender, EventArgs e)
    {

    }

    private void btnDriver_Click(Object sender, EventArgs e)
    {

    }

    private void btnWaybill_Click(Object sender, EventArgs e)
    {

    }

    private void btnDriveLine_Click(Object sender, EventArgs e)
    {

    }
}
