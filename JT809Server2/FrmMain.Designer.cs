namespace JT809Server2
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
            btnClear = new Button();
            btnDecode = new Button();
            groupBox1 = new GroupBox();
            txtCommand = new TextBox();
            btnPing = new Button();
            gbAuth = new GroupBox();
            numMyPort = new NumericUpDown();
            txtPass = new TextBox();
            txtCode = new TextBox();
            label5 = new Label();
            btnLogin = new Button();
            label4 = new Label();
            txtUser = new TextBox();
            label6 = new Label();
            label3 = new Label();
            label7 = new Label();
            txtMyIP = new TextBox();
            btnAlarm = new Button();
            btnQuery = new Button();
            btnPositionCount = new Button();
            gbInfo = new GroupBox();
            rtbLog = new RichTextBox();
            label8 = new Label();
            cbVersion = new ComboBox();
            gbConnect = new GroupBox();
            label2 = new Label();
            numPort = new NumericUpDown();
            btnOpen = new Button();
            label11 = new Label();
            txtIC1 = new TextBox();
            label10 = new Label();
            txtIA1 = new TextBox();
            label9 = new Label();
            txtM1 = new TextBox();
            groupBox1.SuspendLayout();
            gbAuth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMyPort).BeginInit();
            gbInfo.SuspendLayout();
            gbConnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numPort).BeginInit();
            SuspendLayout();
            // 
            // btnClear
            // 
            btnClear.Location = new Point(1034, 29);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(100, 50);
            btnClear.TabIndex = 23;
            btnClear.Text = "清空";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // btnDecode
            // 
            btnDecode.Location = new Point(910, 29);
            btnDecode.Name = "btnDecode";
            btnDecode.Size = new Size(100, 50);
            btnDecode.TabIndex = 21;
            btnDecode.Text = "解码";
            btnDecode.UseVisualStyleBackColor = true;
            btnDecode.Click += btnDecode_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnClear);
            groupBox1.Controls.Add(txtCommand);
            groupBox1.Controls.Add(btnDecode);
            groupBox1.Location = new Point(12, 407);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1152, 96);
            groupBox1.TabIndex = 28;
            groupBox1.TabStop = false;
            groupBox1.Text = "辅助功能";
            // 
            // txtCommand
            // 
            txtCommand.Location = new Point(21, 39);
            txtCommand.Name = "txtCommand";
            txtCommand.Size = new Size(852, 30);
            txtCommand.TabIndex = 22;
            // 
            // btnPing
            // 
            btnPing.Enabled = false;
            btnPing.Location = new Point(1034, 47);
            btnPing.Name = "btnPing";
            btnPing.Size = new Size(100, 50);
            btnPing.TabIndex = 20;
            btnPing.Text = "心跳";
            btnPing.UseVisualStyleBackColor = true;
            btnPing.Click += btnPing_Click;
            // 
            // gbAuth
            // 
            gbAuth.Controls.Add(label11);
            gbAuth.Controls.Add(txtIC1);
            gbAuth.Controls.Add(label10);
            gbAuth.Controls.Add(txtIA1);
            gbAuth.Controls.Add(label9);
            gbAuth.Controls.Add(txtM1);
            gbAuth.Controls.Add(btnPing);
            gbAuth.Controls.Add(numMyPort);
            gbAuth.Controls.Add(txtPass);
            gbAuth.Controls.Add(txtCode);
            gbAuth.Controls.Add(label5);
            gbAuth.Controls.Add(btnLogin);
            gbAuth.Controls.Add(label4);
            gbAuth.Controls.Add(txtUser);
            gbAuth.Controls.Add(label6);
            gbAuth.Controls.Add(label3);
            gbAuth.Controls.Add(label7);
            gbAuth.Controls.Add(txtMyIP);
            gbAuth.Enabled = false;
            gbAuth.Location = new Point(12, 104);
            gbAuth.Name = "gbAuth";
            gbAuth.Size = new Size(1152, 195);
            gbAuth.TabIndex = 27;
            gbAuth.TabStop = false;
            gbAuth.Text = "从链路信息";
            // 
            // numMyPort
            // 
            numMyPort.Location = new Point(426, 87);
            numMyPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numMyPort.Name = "numMyPort";
            numMyPort.Size = new Size(108, 30);
            numMyPort.TabIndex = 13;
            numMyPort.TextAlign = HorizontalAlignment.Right;
            numMyPort.Value = new decimal(new int[] { 1809, 0, 0, 0 });
            // 
            // txtPass
            // 
            txtPass.Location = new Point(426, 31);
            txtPass.Name = "txtPass";
            txtPass.Size = new Size(150, 30);
            txtPass.TabIndex = 7;
            txtPass.Text = "pass";
            // 
            // txtCode
            // 
            txtCode.Location = new Point(723, 31);
            txtCode.Name = "txtCode";
            txtCode.Size = new Size(150, 30);
            txtCode.TabIndex = 9;
            txtCode.Text = "12345";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(624, 34);
            label5.Name = "label5";
            label5.Size = new Size(82, 24);
            label5.TabIndex = 8;
            label5.Text = "接入码：";
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(910, 47);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(100, 50);
            btnLogin.TabIndex = 15;
            btnLogin.Text = "登录";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(316, 34);
            label4.Name = "label4";
            label4.Size = new Size(64, 24);
            label4.TabIndex = 6;
            label4.Text = "密码：";
            // 
            // txtUser
            // 
            txtUser.Location = new Point(128, 31);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(150, 30);
            txtUser.TabIndex = 5;
            txtUser.Text = "9527";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(32, 90);
            label6.Name = "label6";
            label6.Size = new Size(80, 24);
            label6.TabIndex = 10;
            label6.Text = "下级IP：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(32, 34);
            label3.Name = "label3";
            label3.Size = new Size(64, 24);
            label3.TabIndex = 4;
            label3.Text = "用户：";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(316, 90);
            label7.Name = "label7";
            label7.Size = new Size(100, 24);
            label7.TabIndex = 12;
            label7.Text = "下级端口：";
            // 
            // txtMyIP
            // 
            txtMyIP.Location = new Point(128, 87);
            txtMyIP.Name = "txtMyIP";
            txtMyIP.Size = new Size(150, 30);
            txtMyIP.TabIndex = 11;
            // 
            // btnAlarm
            // 
            btnAlarm.Location = new Point(329, 29);
            btnAlarm.Name = "btnAlarm";
            btnAlarm.Size = new Size(137, 50);
            btnAlarm.TabIndex = 23;
            btnAlarm.Text = "报警信息交换";
            btnAlarm.UseVisualStyleBackColor = true;
            // 
            // btnQuery
            // 
            btnQuery.Location = new Point(175, 29);
            btnQuery.Name = "btnQuery";
            btnQuery.Size = new Size(137, 50);
            btnQuery.TabIndex = 22;
            btnQuery.Text = "政府查岗";
            btnQuery.UseVisualStyleBackColor = true;
            btnQuery.Click += btnQuery_Click;
            // 
            // btnPositionCount
            // 
            btnPositionCount.Location = new Point(21, 29);
            btnPositionCount.Name = "btnPositionCount";
            btnPositionCount.Size = new Size(137, 50);
            btnPositionCount.TabIndex = 21;
            btnPositionCount.Text = "定位数量通知";
            btnPositionCount.UseVisualStyleBackColor = true;
            // 
            // gbInfo
            // 
            gbInfo.Controls.Add(btnAlarm);
            gbInfo.Controls.Add(btnQuery);
            gbInfo.Controls.Add(btnPositionCount);
            gbInfo.Location = new Point(12, 305);
            gbInfo.Name = "gbInfo";
            gbInfo.Size = new Size(1152, 96);
            gbInfo.TabIndex = 26;
            gbInfo.TabStop = false;
            gbInfo.Text = "链路业务";
            // 
            // rtbLog
            // 
            rtbLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtbLog.Location = new Point(12, 509);
            rtbLog.Name = "rtbLog";
            rtbLog.Size = new Size(1152, 489);
            rtbLog.TabIndex = 25;
            rtbLog.Text = "";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(327, 37);
            label8.Name = "label8";
            label8.Size = new Size(64, 24);
            label8.TabIndex = 15;
            label8.Text = "协议：";
            // 
            // cbVersion
            // 
            cbVersion.DropDownStyle = ComboBoxStyle.DropDownList;
            cbVersion.FormattingEnabled = true;
            cbVersion.Items.AddRange(new object[] { "JTT2011", "JTT2019" });
            cbVersion.Location = new Point(426, 33);
            cbVersion.Name = "cbVersion";
            cbVersion.Size = new Size(150, 32);
            cbVersion.TabIndex = 14;
            // 
            // gbConnect
            // 
            gbConnect.Controls.Add(label8);
            gbConnect.Controls.Add(cbVersion);
            gbConnect.Controls.Add(label2);
            gbConnect.Controls.Add(numPort);
            gbConnect.Location = new Point(12, 18);
            gbConnect.Name = "gbConnect";
            gbConnect.Size = new Size(889, 80);
            gbConnect.TabIndex = 24;
            gbConnect.TabStop = false;
            gbConnect.Text = "连接信息";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(19, 37);
            label2.Name = "label2";
            label2.Size = new Size(64, 24);
            label2.TabIndex = 2;
            label2.Text = "端口：";
            // 
            // numPort
            // 
            numPort.Location = new Point(129, 34);
            numPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            numPort.Name = "numPort";
            numPort.Size = new Size(108, 30);
            numPort.TabIndex = 3;
            numPort.TextAlign = HorizontalAlignment.Right;
            numPort.Value = new decimal(new int[] { 809, 0, 0, 0 });
            // 
            // btnOpen
            // 
            btnOpen.Location = new Point(955, 33);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(100, 50);
            btnOpen.TabIndex = 23;
            btnOpen.Text = "开始";
            btnOpen.UseVisualStyleBackColor = true;
            btnOpen.Click += btnOpen_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(627, 147);
            label11.Name = "label11";
            label11.Size = new Size(56, 24);
            label11.TabIndex = 31;
            label11.Text = "IC1：";
            // 
            // txtIC1
            // 
            txtIC1.Location = new Point(723, 144);
            txtIC1.Name = "txtIC1";
            txtIC1.Size = new Size(150, 30);
            txtIC1.TabIndex = 32;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(330, 147);
            label10.Name = "label10";
            label10.Size = new Size(57, 24);
            label10.TabIndex = 29;
            label10.Text = "IA1：";
            // 
            // txtIA1
            // 
            txtIA1.Location = new Point(426, 144);
            txtIA1.Name = "txtIA1";
            txtIA1.Size = new Size(150, 30);
            txtIA1.TabIndex = 30;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(32, 150);
            label9.Name = "label9";
            label9.Size = new Size(57, 24);
            label9.TabIndex = 27;
            label9.Text = "M1：";
            // 
            // txtM1
            // 
            txtM1.Location = new Point(128, 147);
            txtM1.Name = "txtM1";
            txtM1.Size = new Size(150, 30);
            txtM1.TabIndex = 28;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1176, 1016);
            Controls.Add(groupBox1);
            Controls.Add(gbAuth);
            Controls.Add(gbInfo);
            Controls.Add(rtbLog);
            Controls.Add(gbConnect);
            Controls.Add(btnOpen);
            Name = "FrmMain";
            Text = "JT/T809政府监管平台";
            Load += FrmMain_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            gbAuth.ResumeLayout(false);
            gbAuth.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMyPort).EndInit();
            gbInfo.ResumeLayout(false);
            gbConnect.ResumeLayout(false);
            gbConnect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numPort).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnClear;
        private Button btnDecode;
        private GroupBox groupBox1;
        private TextBox txtCommand;
        private Button btnPing;
        private GroupBox gbAuth;
        private NumericUpDown numMyPort;
        private TextBox txtPass;
        private TextBox txtCode;
        private Label label5;
        private Button btnLogin;
        private Label label4;
        private TextBox txtUser;
        private Label label6;
        private Label label3;
        private Label label7;
        private TextBox txtMyIP;
        private Button btnAlarm;
        private Button btnQuery;
        private Button btnPositionCount;
        private GroupBox gbInfo;
        private RichTextBox rtbLog;
        private Label label8;
        private ComboBox cbVersion;
        private GroupBox gbConnect;
        private Label label2;
        private NumericUpDown numPort;
        private Button btnOpen;
        private Label label11;
        private TextBox txtIC1;
        private Label label10;
        private TextBox txtIA1;
        private Label label9;
        private TextBox txtM1;
    }
}