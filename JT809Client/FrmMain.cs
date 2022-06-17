using System.Reflection;
using NewLife;
using NewLife.JT809;
using NewLife.JT809.Messages;
using NewLife.JT809.Protocols;
using NewLife.JT809.SubMessages;
using NewLife.Log;
using NewLife.Net;
using NewLife.Reflection;
using NewLife.Security;
using NewLife.Serialization;

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
            var asm = AssemblyX.Create(Assembly.GetExecutingAssembly());
            Text = String.Format("{2} v{0} {1:HH:mm:ss}", asm.FileVersion, asm.Compile, Text);

            _config = new ControlConfig { Control = this, FileName = "jt809.json" };
            _config.Load();

            rtbLog.Clear();
            rtbLog.SetDefaultStyle(12);
            //_log = new TextControlLog { Control = rtbLog };
            rtbLog.UseWinFormControl();
            _log = XTrace.Log;

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
                btnLogin.Text = "登录";
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
            var count = Rand.Next(10000);
            var start = DateTime.Now.AddSeconds(-Rand.Next(24 * 3600));
            var end = start.AddHours(1);

            _client.PositionCount(count, start, end);
        }

        private void btnDecode_Click(Object sender, EventArgs e)
        {
            var hex = txtCommand.Text;
            if (hex.IsNullOrEmpty()) return;

            _config.Save();

            var buf = hex.ToHex();
            _log.Info("报文长度[{0}]字节", buf.Length);

            var msg = new JTMessage();
            //if (_client?.Config != null) msg.JTT2019 = _client.Config.Version == JTVersion.JTT2019;
            if (Enum.TryParse<JTVersion>(cbVersion.SelectedItem + "", true, out var version))
                msg.JTT2019 = version == JTVersion.JTT2019;

            if (!msg.Read(hex.ToHex()))
            {
                _log.Info("解码失败！");
                //return;
            }

            _log.Info("");
            _log.Info("指令头部[{0:X4}]：", (Int32)msg.Kind);
            _log.Info(msg.ToJsonJT809(true));

            _log.Info("");
            _log.Info("主体数据[{0}]：{1}", msg.Payload?.Total, msg.Payload?.ToHex(1024));
            //var model = MessageFactory.Instance.Parse(msg);
            //_log.Info(model.ToJsonJT809(true));

            // 分开解码，即使出错也能输出部分结果
            var model = MessageFactory.Instance.Create(msg.Kind);
            try
            {
                var pk = msg.Payload;
                if (pk != null && pk.Total > 0)
                {
                    var ms = pk.GetStream();
                    if (model is IAccessor accessor)
                        accessor.Read(ms, msg);
                    else
                        Helper.CreateReader(ms).TryRead(null, ref model);

                    if (ms.Position < ms.Length) XTrace.WriteLine($"解析未完成，可用[{ms.Length}]字节，实际解析仅使用[{ms.Position}]字节");
                }
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
            }

            if (model is ISubMessage sub) _log.Info("子业务编码：[{0:X4}]{1}", sub.DataType, (MessageKinds)sub.DataType);

            _log.Info(model.ToJsonJT809(true));

            if (model is JT809_0x1200 j1200 && j1200.Body is JT809_0x1200_0x1202 j1202)
            {
                _log.Info("定位数据：");
                _log.Info(j1202.Location.ToJsonJT809(true));
            }
        }

        private void btnClear_Click(Object sender, EventArgs e) => rtbLog.Clear();

        private void btnExchange_Click(Object sender, EventArgs e)
        {
            var frm = new FrmExchange
            {
                Client = _client,
                Log = _log
            };

            frm.Show();
        }

        private void btnAlarm_Click(Object sender, EventArgs e)
        {
            var frm = new FrmAlarm
            {
                Client = _client,
                Log = _log
            };

            frm.Show();
        }
    }
}