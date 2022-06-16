namespace JT809Client
{
    partial class FrmAlarm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.cbColor = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPlate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnJ1401 = new System.Windows.Forms.Button();
            this.btnLocation = new System.Windows.Forms.Button();
            this.btnLocation2 = new System.Windows.Forms.Button();
            this.btnMonitorStart = new System.Windows.Forms.Button();
            this.btnMonitorEnd = new System.Windows.Forms.Button();
            this.btnHistory = new System.Windows.Forms.Button();
            this.btnDriver = new System.Windows.Forms.Button();
            this.btnWaybill = new System.Windows.Forms.Button();
            this.btnDriveLine = new System.Windows.Forms.Button();
            this.gbInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.cbColor);
            this.gbInfo.Controls.Add(this.label4);
            this.gbInfo.Controls.Add(this.txtPlate);
            this.gbInfo.Controls.Add(this.label3);
            this.gbInfo.Location = new System.Drawing.Point(12, 12);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(728, 79);
            this.gbInfo.TabIndex = 20;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "基本信息";
            // 
            // cbColor
            // 
            this.cbColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColor.FormattingEnabled = true;
            this.cbColor.Location = new System.Drawing.Point(391, 30);
            this.cbColor.Name = "cbColor";
            this.cbColor.Size = new System.Drawing.Size(100, 32);
            this.cbColor.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(316, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 24);
            this.label4.TabIndex = 6;
            this.label4.Text = "颜色：";
            // 
            // txtPlate
            // 
            this.txtPlate.Location = new System.Drawing.Point(128, 31);
            this.txtPlate.Name = "txtPlate";
            this.txtPlate.Size = new System.Drawing.Size(150, 30);
            this.txtPlate.TabIndex = 5;
            this.txtPlate.Text = "沪A12345";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "车牌号：";
            // 
            // btnJ1401
            // 
            this.btnJ1401.Location = new System.Drawing.Point(44, 121);
            this.btnJ1401.Name = "btnJ1401";
            this.btnJ1401.Size = new System.Drawing.Size(124, 50);
            this.btnJ1401.TabIndex = 15;
            this.btnJ1401.Text = "督办应答";
            this.btnJ1401.UseVisualStyleBackColor = true;
            this.btnJ1401.Click += new System.EventHandler(this.btnJ1201_Click);
            // 
            // btnLocation
            // 
            this.btnLocation.Location = new System.Drawing.Point(203, 121);
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Size = new System.Drawing.Size(124, 50);
            this.btnLocation.TabIndex = 21;
            this.btnLocation.Text = "车辆定位";
            this.btnLocation.UseVisualStyleBackColor = true;
            this.btnLocation.Click += new System.EventHandler(this.btnLocation_Click);
            // 
            // btnLocation2
            // 
            this.btnLocation2.Location = new System.Drawing.Point(358, 121);
            this.btnLocation2.Name = "btnLocation2";
            this.btnLocation2.Size = new System.Drawing.Size(124, 50);
            this.btnLocation2.TabIndex = 22;
            this.btnLocation2.Text = "定位补传";
            this.btnLocation2.UseVisualStyleBackColor = true;
            this.btnLocation2.Click += new System.EventHandler(this.btnLocation2_Click);
            // 
            // btnMonitorStart
            // 
            this.btnMonitorStart.Location = new System.Drawing.Point(44, 201);
            this.btnMonitorStart.Name = "btnMonitorStart";
            this.btnMonitorStart.Size = new System.Drawing.Size(124, 50);
            this.btnMonitorStart.TabIndex = 23;
            this.btnMonitorStart.Text = "申请定位";
            this.btnMonitorStart.UseVisualStyleBackColor = true;
            this.btnMonitorStart.Click += new System.EventHandler(this.btnMonitorStart_Click);
            // 
            // btnMonitorEnd
            // 
            this.btnMonitorEnd.Location = new System.Drawing.Point(203, 201);
            this.btnMonitorEnd.Name = "btnMonitorEnd";
            this.btnMonitorEnd.Size = new System.Drawing.Size(124, 50);
            this.btnMonitorEnd.TabIndex = 24;
            this.btnMonitorEnd.Text = "取消定位";
            this.btnMonitorEnd.UseVisualStyleBackColor = true;
            this.btnMonitorEnd.Click += new System.EventHandler(this.btnMonitorEnd_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.Location = new System.Drawing.Point(358, 201);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(124, 50);
            this.btnHistory.TabIndex = 25;
            this.btnHistory.Text = "补发定位";
            this.btnHistory.UseVisualStyleBackColor = true;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // btnDriver
            // 
            this.btnDriver.Location = new System.Drawing.Point(44, 286);
            this.btnDriver.Name = "btnDriver";
            this.btnDriver.Size = new System.Drawing.Size(124, 50);
            this.btnDriver.TabIndex = 26;
            this.btnDriver.Text = "上报驾驶员";
            this.btnDriver.UseVisualStyleBackColor = true;
            this.btnDriver.Click += new System.EventHandler(this.btnDriver_Click);
            // 
            // btnWaybill
            // 
            this.btnWaybill.Location = new System.Drawing.Point(203, 286);
            this.btnWaybill.Name = "btnWaybill";
            this.btnWaybill.Size = new System.Drawing.Size(124, 50);
            this.btnWaybill.TabIndex = 27;
            this.btnWaybill.Text = "电子运单";
            this.btnWaybill.UseVisualStyleBackColor = true;
            this.btnWaybill.Click += new System.EventHandler(this.btnWaybill_Click);
            // 
            // btnDriveLine
            // 
            this.btnDriveLine.Location = new System.Drawing.Point(358, 286);
            this.btnDriveLine.Name = "btnDriveLine";
            this.btnDriveLine.Size = new System.Drawing.Size(124, 50);
            this.btnDriveLine.TabIndex = 28;
            this.btnDriveLine.Text = "驾驶路线";
            this.btnDriveLine.UseVisualStyleBackColor = true;
            this.btnDriveLine.Click += new System.EventHandler(this.btnDriveLine_Click);
            // 
            // FrmAlarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 373);
            this.Controls.Add(this.btnDriveLine);
            this.Controls.Add(this.btnWaybill);
            this.Controls.Add(this.btnDriver);
            this.Controls.Add(this.btnHistory);
            this.Controls.Add(this.btnMonitorEnd);
            this.Controls.Add(this.btnMonitorStart);
            this.Controls.Add(this.btnLocation2);
            this.Controls.Add(this.btnLocation);
            this.Controls.Add(this.gbInfo);
            this.Controls.Add(this.btnJ1401);
            this.Name = "FrmAlarm";
            this.Text = "FrmExchange";
            this.Load += new System.EventHandler(this.FrmExchange_Load);
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox gbInfo;
        private Button btnJ1401;
        private Label label4;
        private TextBox txtPlate;
        private Label label3;
        private ComboBox cbColor;
        private Button btnLocation;
        private Button btnLocation2;
        private Button btnMonitorStart;
        private Button btnMonitorEnd;
        private Button btnHistory;
        private Button btnDriver;
        private Button btnWaybill;
        private Button btnDriveLine;
    }
}