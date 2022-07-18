namespace JT809Client
{
    partial class FrmQuery
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
            this.cbObjectType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtQuestion = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            this.btnMonitorStart = new System.Windows.Forms.Button();
            this.btnMonitorEnd = new System.Windows.Forms.Button();
            this.btnHistory = new System.Windows.Forms.Button();
            this.btnDriver = new System.Windows.Forms.Button();
            this.btnWaybill = new System.Windows.Forms.Button();
            this.btnDriveLine = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtObjectId = new System.Windows.Forms.TextBox();
            this.gbInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.cbObjectType);
            this.gbInfo.Controls.Add(this.label4);
            this.gbInfo.Controls.Add(this.txtQuestion);
            this.gbInfo.Controls.Add(this.label3);
            this.gbInfo.Location = new System.Drawing.Point(12, 12);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(728, 79);
            this.gbInfo.TabIndex = 20;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "基本信息";
            // 
            // cbObjectType
            // 
            this.cbObjectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbObjectType.FormattingEnabled = true;
            this.cbObjectType.Location = new System.Drawing.Point(91, 29);
            this.cbObjectType.Name = "cbObjectType";
            this.cbObjectType.Size = new System.Drawing.Size(254, 32);
            this.cbObjectType.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 24);
            this.label4.TabIndex = 6;
            this.label4.Text = "类型：";
            // 
            // txtQuestion
            // 
            this.txtQuestion.Location = new System.Drawing.Point(461, 27);
            this.txtQuestion.Name = "txtQuestion";
            this.txtQuestion.Size = new System.Drawing.Size(251, 30);
            this.txtQuestion.TabIndex = 5;
            this.txtQuestion.Text = "孙悟空的师父是谁？";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(391, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 24);
            this.label3.TabIndex = 4;
            this.label3.Text = "问题：";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(44, 121);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(124, 50);
            this.btnQuery.TabIndex = 15;
            this.btnQuery.Text = "政府查岗";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(214, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 24);
            this.label1.TabIndex = 29;
            this.label1.Text = "查岗对象：";
            // 
            // txtObjectId
            // 
            this.txtObjectId.Location = new System.Drawing.Point(317, 131);
            this.txtObjectId.Name = "txtObjectId";
            this.txtObjectId.Size = new System.Drawing.Size(150, 30);
            this.txtObjectId.TabIndex = 30;
            this.txtObjectId.Text = "Stone";
            // 
            // FrmQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(755, 373);
            this.Controls.Add(this.txtObjectId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDriveLine);
            this.Controls.Add(this.btnWaybill);
            this.Controls.Add(this.btnDriver);
            this.Controls.Add(this.btnHistory);
            this.Controls.Add(this.btnMonitorEnd);
            this.Controls.Add(this.btnMonitorStart);
            this.Controls.Add(this.gbInfo);
            this.Controls.Add(this.btnQuery);
            this.Name = "FrmQuery";
            this.Text = "FrmExchange";
            this.Load += new System.EventHandler(this.FrmExchange_Load);
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox gbInfo;
        private Button btnQuery;
        private Label label4;
        private TextBox txtQuestion;
        private Label label3;
        private ComboBox cbObjectType;
        private Button btnMonitorStart;
        private Button btnMonitorEnd;
        private Button btnHistory;
        private Button btnDriver;
        private Button btnWaybill;
        private Button btnDriveLine;
        private Label label1;
        private TextBox txtObjectId;
    }
}