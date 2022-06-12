using NewLife;
using NewLife.JT809;
using NewLife.JT809.Protocols;
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
            _config = new ControlConfig { Control = this, FileName = "jt809.json" };
            _config.Load();

            rtbLog.Clear();
            rtbLog.SetDefaultStyle(12);
            _log = new TextControlLog { Control = rtbLog };

            if (txtMyIP.Text.IsNullOrEmpty()) txtMyIP.Text = NetHelper.MyIP() + "";
        }

        private void btnConnect_Click(Object sender, EventArgs e)
        {
            _config.Save();

            var btn = sender as Button;
            if (btn.Text == "连接")
            {
                _client = new JTClient
                {
                    Server = new NetUri(NetType.Tcp, txtServer.Text, (Int32)numPort.Value),

                    Log = _log,
                };

                if (Enum.TryParse<JTVersion>(cbVersion.SelectedText, true, out var version))
                    _client.Config.Version = version;

                _client.Open();

                btn.Text = "断开";
                gbConnect.Enabled = false;
                gbAuth.Enabled = true;
            }
            else
            {
                _client.TryDispose();
                _client = null;

                btn.Text = "连接";
                gbConnect.Enabled = true;
                gbAuth.Enabled = false;
            }
        }

        private async void btnLogin_Click(Object sender, EventArgs e)
        {
            _config.Save();

            var btn = sender as Button;
            if (btn.Text == "登录")
            {
                var user = txtUser.Text.ToInt();
                var pass = txtPass.Text;
                var code = txtCode.Text.ToInt();
                var ip = txtMyIP.Text;
                var port = (Int32)numMyPort.Value;

                var rs = await _client.LoginAsync(user, pass, code, ip, port);

                btn.Text = "注销";
                btnPing.Enabled = true;
            }
            else
            {
                //var rs = await _client.LoginAsync(user, pass, code, ip, port);

                btn.Text = "登录";
                btnPing.Enabled = false;
            }
        }
    }
}