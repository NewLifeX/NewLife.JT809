using JT809Client.Controllers;
using NewLife;
using NewLife.JT809;
using NewLife.JT809.Protocols;
using NewLife.Log;
using NewLife.Net;
using NewLife.Security;

namespace JT809Client
{
    public partial class FrmMain : Form
    {
        private ControlConfig _config;
        private ILog _log;
        private JTClient _client;
        private SlaveServer _slave;

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
                    Name = "主链路",
                    Server = new NetUri(NetType.Tcp, txtServer.Text, (Int32)numPort.Value),

                    Log = _log,
                };

                if (Enum.TryParse<JTVersion>(cbVersion.SelectedItem + "", true, out var version))
                    _client.Config.Version = version;

                _client.Open();

                txtCode.Enabled = _client.Config.Version == JTVersion.JTT2019;

                btn.Text = "断开";
                gbConnect.Enabled = false;
                gbAuth.Enabled = true;
                gbInfo.Enabled = true;
            }
            else
            {
                _slave.TryDispose();
                _slave = null;

                _client.TryDispose();
                _client = null;

                btn.Text = "连接";
                gbConnect.Enabled = true;
                gbAuth.Enabled = false;
                gbInfo.Enabled = false;
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

                var server = new SlaveServer
                {
                    Port = port,

                    Log = _log,
                    SessionLog = _log
                };

#if DEBUG
                server.SocketLog = _log;
                server.LogSend = true;
                server.LogReceive = true;
#endif

                server.AddHandler(new NormalController());

                server.Start();

                _slave = server;

                var rs = await _client.LoginAsync(user, pass, code, ip, port);
                _log.Info("授权码：{0}", rs);

                btn.Text = "注销";
                btnPing.Enabled = true;
            }
            else
            {
                var user = txtUser.Text.ToInt();
                var pass = txtPass.Text;

                await _client.LogoutAsync(user, pass);

                _slave.TryDispose();
                _slave = null;

                btn.Text = "登录";
                btnPing.Enabled = false;
            }
        }

        private async void btnPing_Click(Object sender, EventArgs e) => await _client.PingAsync();

        private void btnPositionCount_Click(Object sender, EventArgs e)
        {
            var count = Rand.Next();
            var start = DateTime.Now.AddSeconds(-Rand.Next());
            var end = start.AddHours(1);

            _client.PositionCount(count, start, end);
        }
    }
}