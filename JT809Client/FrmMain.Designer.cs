namespace JT809Client
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMyIP = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numMyPort = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.gbConnect = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbVersion = new System.Windows.Forms.ComboBox();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.btnPositionCount = new System.Windows.Forms.Button();
            this.gbAuth = new System.Windows.Forms.GroupBox();
            this.btnPing = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMyPort)).BeginInit();
            this.gbConnect.SuspendLayout();
            this.gbInfo.SuspendLayout();
            this.gbAuth.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务端：";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(128, 33);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(150, 30);
            this.txtServer.TabIndex = 1;
            this.txtServer.Text = "127.0.0.1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(316, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口：";
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(426, 33);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(108, 30);
            this.numPort.TabIndex = 3;
            this.numPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPort.Value = new decimal(new int[] {
            809,
            0,
            0,
            0});
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(128, 31);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(150, 30);
            this.txtUser.TabIndex = 5;
            this.txtUser.Text = "9527";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "用户：";
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(426, 31);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(150, 30);
            this.txtPass.TabIndex = 7;
            this.txtPass.Text = "pass";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(316, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 24);
            this.label4.TabIndex = 6;
            this.label4.Text = "密码：";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(723, 31);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(150, 30);
            this.txtCode.TabIndex = 9;
            this.txtCode.Text = "12345";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(624, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 24);
            this.label5.TabIndex = 8;
            this.label5.Text = "接入码：";
            // 
            // txtMyIP
            // 
            this.txtMyIP.Location = new System.Drawing.Point(128, 87);
            this.txtMyIP.Name = "txtMyIP";
            this.txtMyIP.Size = new System.Drawing.Size(150, 30);
            this.txtMyIP.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 24);
            this.label6.TabIndex = 10;
            this.label6.Text = "下级IP：";
            // 
            // numMyPort
            // 
            this.numMyPort.Location = new System.Drawing.Point(426, 87);
            this.numMyPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numMyPort.Name = "numMyPort";
            this.numMyPort.Size = new System.Drawing.Size(108, 30);
            this.numMyPort.TabIndex = 13;
            this.numMyPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numMyPort.Value = new decimal(new int[] {
            1809,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(316, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 24);
            this.label7.TabIndex = 12;
            this.label7.Text = "下级端口：";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(955, 39);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 50);
            this.btnConnect.TabIndex = 14;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(910, 47);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(100, 50);
            this.btnLogin.TabIndex = 15;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // gbConnect
            // 
            this.gbConnect.Controls.Add(this.label8);
            this.gbConnect.Controls.Add(this.cbVersion);
            this.gbConnect.Controls.Add(this.label1);
            this.gbConnect.Controls.Add(this.txtServer);
            this.gbConnect.Controls.Add(this.label2);
            this.gbConnect.Controls.Add(this.numPort);
            this.gbConnect.Location = new System.Drawing.Point(12, 24);
            this.gbConnect.Name = "gbConnect";
            this.gbConnect.Size = new System.Drawing.Size(889, 80);
            this.gbConnect.TabIndex = 16;
            this.gbConnect.TabStop = false;
            this.gbConnect.Text = "连接信息";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(624, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 24);
            this.label8.TabIndex = 15;
            this.label8.Text = "协议：";
            // 
            // cbVersion
            // 
            this.cbVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVersion.FormattingEnabled = true;
            this.cbVersion.Items.AddRange(new object[] {
            "JTT2011",
            "JTT2019"});
            this.cbVersion.Location = new System.Drawing.Point(723, 32);
            this.cbVersion.Name = "cbVersion";
            this.cbVersion.Size = new System.Drawing.Size(150, 32);
            this.cbVersion.TabIndex = 14;
            // 
            // rtbLog
            // 
            this.rtbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLog.Location = new System.Drawing.Point(12, 385);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(1152, 619);
            this.rtbLog.TabIndex = 17;
            this.rtbLog.Text = "";
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.btnPositionCount);
            this.gbInfo.Location = new System.Drawing.Point(12, 248);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(1152, 131);
            this.gbInfo.TabIndex = 18;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "链路业务";
            // 
            // btnPositionCount
            // 
            this.btnPositionCount.Location = new System.Drawing.Point(21, 29);
            this.btnPositionCount.Name = "btnPositionCount";
            this.btnPositionCount.Size = new System.Drawing.Size(137, 50);
            this.btnPositionCount.TabIndex = 21;
            this.btnPositionCount.Text = "定位数量通知";
            this.btnPositionCount.UseVisualStyleBackColor = true;
            this.btnPositionCount.Click += new System.EventHandler(this.btnPositionCount_Click);
            // 
            // gbAuth
            // 
            this.gbAuth.Controls.Add(this.btnPing);
            this.gbAuth.Controls.Add(this.numMyPort);
            this.gbAuth.Controls.Add(this.txtPass);
            this.gbAuth.Controls.Add(this.txtCode);
            this.gbAuth.Controls.Add(this.label5);
            this.gbAuth.Controls.Add(this.btnLogin);
            this.gbAuth.Controls.Add(this.label4);
            this.gbAuth.Controls.Add(this.txtUser);
            this.gbAuth.Controls.Add(this.label6);
            this.gbAuth.Controls.Add(this.label3);
            this.gbAuth.Controls.Add(this.label7);
            this.gbAuth.Controls.Add(this.txtMyIP);
            this.gbAuth.Enabled = false;
            this.gbAuth.Location = new System.Drawing.Point(12, 110);
            this.gbAuth.Name = "gbAuth";
            this.gbAuth.Size = new System.Drawing.Size(1152, 132);
            this.gbAuth.TabIndex = 19;
            this.gbAuth.TabStop = false;
            this.gbAuth.Text = "基本信息";
            // 
            // btnPing
            // 
            this.btnPing.Enabled = false;
            this.btnPing.Location = new System.Drawing.Point(1034, 47);
            this.btnPing.Name = "btnPing";
            this.btnPing.Size = new System.Drawing.Size(100, 50);
            this.btnPing.TabIndex = 20;
            this.btnPing.Text = "心跳";
            this.btnPing.UseVisualStyleBackColor = true;
            this.btnPing.Click += new System.EventHandler(this.btnPing_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 1016);
            this.Controls.Add(this.gbAuth);
            this.Controls.Add(this.gbInfo);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.gbConnect);
            this.Controls.Add(this.btnConnect);
            this.Name = "FrmMain";
            this.Text = "JT/T809企业平台";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMyPort)).EndInit();
            this.gbConnect.ResumeLayout(false);
            this.gbConnect.PerformLayout();
            this.gbInfo.ResumeLayout(false);
            this.gbAuth.ResumeLayout(false);
            this.gbAuth.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Label label1;
        private TextBox txtServer;
        private Label label2;
        private NumericUpDown numPort;
        private TextBox txtUser;
        private Label label3;
        private TextBox txtPass;
        private Label label4;
        private TextBox txtCode;
        private Label label5;
        private TextBox txtMyIP;
        private Label label6;
        private NumericUpDown numMyPort;
        private Label label7;
        private Button btnConnect;
        private Button btnLogin;
        private GroupBox gbConnect;
        private RichTextBox rtbLog;
        private GroupBox gbInfo;
        private Label label8;
        private ComboBox cbVersion;
        private GroupBox gbAuth;
        private Button btnPing;
        private Button btnPositionCount;
    }
}