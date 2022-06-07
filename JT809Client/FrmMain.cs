using NewLife;
using NewLife.JT809;
using NewLife.Log;
using NewLife.Net;

namespace JT809Client
{
    public partial class FrmMain : Form
    {
        private ControlConfig _config;
        private ILog _log;
        private JTClient _client;

        public FrmMain() => InitializeComponent();

        private void FrmMain_Load(Object sender, EventArgs e)
        {
            _config = new ControlConfig { Control = this, FileName = "mqtt.json" };
            _config.Load();

            rtbLog.Clear();
            rtbLog.SetDefaultStyle(12);
            _log = new TextControlLog { Control = rtbLog };

            if (txtMyIP.Text.IsNullOrEmpty()) txtMyIP.Text = NetHelper.MyIP() + "";
        }

        private void btnConnect_Click(Object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn.Text == "连接")
            {
                _client = new JTClient
                {
                    Server = new NetUri(NetType.Tcp, txtServer.Text, (Int32)numPort.Value),

                    Log = _log,
                };
                _client.Open();

                btn.Text = "断开";
            }
            else
            {
                _client.TryDispose();
                _client = null;

                btn.Text = "连接";
            }
        }

        private void btnLogin_Click(Object sender, EventArgs e)
        {

        }
    }
}