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
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDecode = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.btnPing = new System.Windows.Forms.Button();
            this.gbAuth = new System.Windows.Forms.GroupBox();
            this.numMyPort = new System.Windows.Forms.NumericUpDown();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMyIP = new System.Windows.Forms.TextBox();
            this.btnAlarm = new System.Windows.Forms.Button();
            this.btnExchange = new System.Windows.Forms.Button();
            this.btnPositionCount = new System.Windows.Forms.Button();
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbVersion = new System.Windows.Forms.ComboBox();
            this.gbConnect = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.btnOpen = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.gbAuth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMyPort)).BeginInit();
            this.gbInfo.SuspendLayout();
            this.gbConnect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(1034, 29);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 50);
            this.btnClear.TabIndex = 23;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // btnDecode
            // 
            this.btnDecode.Location = new System.Drawing.Point(910, 29);
            this.btnDecode.Name = "btnDecode";
            this.btnDecode.Size = new System.Drawing.Size(100, 50);
            this.btnDecode.TabIndex = 21;
            this.btnDecode.Text = "解码";
            this.btnDecode.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.txtCommand);
            this.groupBox1.Controls.Add(this.btnDecode);
            this.groupBox1.Location = new System.Drawing.Point(12, 344);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1152, 96);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "辅助功能";
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(21, 39);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(852, 30);
            this.txtCommand.TabIndex = 22;
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
            this.gbAuth.Location = new System.Drawing.Point(12, 104);
            this.gbAuth.Name = "gbAuth";
            this.gbAuth.Size = new System.Drawing.Size(1152, 132);
            this.gbAuth.TabIndex = 27;
            this.gbAuth.TabStop = false;
            this.gbAuth.Text = "基本信息";
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
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(426, 31);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(150, 30);
            this.txtPass.TabIndex = 7;
            this.txtPass.Text = "pass";
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
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(910, 47);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(100, 50);
            this.btnLogin.TabIndex = 15;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
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
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(128, 31);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(150, 30);
            this.txtUser.TabIndex = 5;
            this.txtUser.Text = "9527";
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "用户：";
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
            // txtMyIP
            // 
            this.txtMyIP.Location = new System.Drawing.Point(128, 87);
            this.txtMyIP.Name = "txtMyIP";
            this.txtMyIP.Size = new System.Drawing.Size(150, 30);
            this.txtMyIP.TabIndex = 11;
            // 
            // btnAlarm
            // 
            this.btnAlarm.Location = new System.Drawing.Point(329, 29);
            this.btnAlarm.Name = "btnAlarm";
            this.btnAlarm.Size = new System.Drawing.Size(137, 50);
            this.btnAlarm.TabIndex = 23;
            this.btnAlarm.Text = "报警信息交换";
            this.btnAlarm.UseVisualStyleBackColor = true;
            // 
            // btnExchange
            // 
            this.btnExchange.Location = new System.Drawing.Point(175, 29);
            this.btnExchange.Name = "btnExchange";
            this.btnExchange.Size = new System.Drawing.Size(137, 50);
            this.btnExchange.TabIndex = 22;
            this.btnExchange.Text = "动态信息交换";
            this.btnExchange.UseVisualStyleBackColor = true;
            // 
            // btnPositionCount
            // 
            this.btnPositionCount.Location = new System.Drawing.Point(21, 29);
            this.btnPositionCount.Name = "btnPositionCount";
            this.btnPositionCount.Size = new System.Drawing.Size(137, 50);
            this.btnPositionCount.TabIndex = 21;
            this.btnPositionCount.Text = "定位数量通知";
            this.btnPositionCount.UseVisualStyleBackColor = true;
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.btnAlarm);
            this.gbInfo.Controls.Add(this.btnExchange);
            this.gbInfo.Controls.Add(this.btnPositionCount);
            this.gbInfo.Location = new System.Drawing.Point(12, 242);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(1152, 96);
            this.gbInfo.TabIndex = 26;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "链路业务";
            // 
            // rtbLog
            // 
            this.rtbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbLog.Location = new System.Drawing.Point(12, 446);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(1152, 552);
            this.rtbLog.TabIndex = 25;
            this.rtbLog.Text = "";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(327, 37);
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
            this.cbVersion.Location = new System.Drawing.Point(426, 33);
            this.cbVersion.Name = "cbVersion";
            this.cbVersion.Size = new System.Drawing.Size(150, 32);
            this.cbVersion.TabIndex = 14;
            // 
            // gbConnect
            // 
            this.gbConnect.Controls.Add(this.label8);
            this.gbConnect.Controls.Add(this.cbVersion);
            this.gbConnect.Controls.Add(this.label2);
            this.gbConnect.Controls.Add(this.numPort);
            this.gbConnect.Location = new System.Drawing.Point(12, 18);
            this.gbConnect.Name = "gbConnect";
            this.gbConnect.Size = new System.Drawing.Size(889, 80);
            this.gbConnect.TabIndex = 24;
            this.gbConnect.TabStop = false;
            this.gbConnect.Text = "连接信息";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口：";
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(129, 34);
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
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(955, 33);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(100, 50);
            this.btnOpen.TabIndex = 23;
            this.btnOpen.Text = "开始";
            this.btnOpen.UseVisualStyleBackColor = true;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 1016);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbAuth);
            this.Controls.Add(this.gbInfo);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.gbConnect);
            this.Controls.Add(this.btnOpen);
            this.Name = "FrmMain";
            this.Text = "JT/T809政府监管平台";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbAuth.ResumeLayout(false);
            this.gbAuth.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMyPort)).EndInit();
            this.gbInfo.ResumeLayout(false);
            this.gbConnect.ResumeLayout(false);
            this.gbConnect.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.ResumeLayout(false);

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
        private Button btnExchange;
        private Button btnPositionCount;
        private GroupBox gbInfo;
        private RichTextBox rtbLog;
        private Label label8;
        private ComboBox cbVersion;
        private GroupBox gbConnect;
        private Label label2;
        private NumericUpDown numPort;
        private Button btnOpen;
    }
}