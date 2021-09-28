namespace PRSpline
{
    partial class frmVoltageSag
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVoltageSag));
            this.panChart = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxRalay = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnVS = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cbxItem = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPS = new System.Windows.Forms.TextBox();
            this.btnUpdata = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panChart
            // 
            this.panChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panChart.Location = new System.Drawing.Point(12, 172);
            this.panChart.Name = "panChart";
            this.panChart.Size = new System.Drawing.Size(1042, 454);
            this.panChart.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "選擇電驛：";
            // 
            // cbxRalay
            // 
            this.cbxRalay.FormattingEnabled = true;
            this.cbxRalay.Location = new System.Drawing.Point(115, 11);
            this.cbxRalay.Name = "cbxRalay";
            this.cbxRalay.Size = new System.Drawing.Size(143, 28);
            this.cbxRalay.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.btnConfirm);
            this.panel2.Controls.Add(this.dateTimePicker2);
            this.panel2.Controls.Add(this.dateTimePicker1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.cbxRalay);
            this.panel2.Location = new System.Drawing.Point(12, 7);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1042, 54);
            this.panel2.TabIndex = 3;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(829, 10);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(90, 30);
            this.btnConfirm.TabIndex = 6;
            this.btnConfirm.Text = "查詢";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(668, 11);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(140, 29);
            this.dateTimePicker2.TabIndex = 5;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(385, 11);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(140, 29);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(531, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "結束時間(不含)：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(264, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "開始時間(起)：";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.btnUpdata);
            this.panel3.Controls.Add(this.txtPS);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.btnVS);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.cbxItem);
            this.panel3.Location = new System.Drawing.Point(12, 67);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1042, 99);
            this.panel3.TabIndex = 4;
            // 
            // btnVS
            // 
            this.btnVS.BackColor = System.Drawing.Color.Transparent;
            this.btnVS.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnVS.Image = ((System.Drawing.Image)(resources.GetObject("btnVS.Image")));
            this.btnVS.Location = new System.Drawing.Point(925, 5);
            this.btnVS.Name = "btnVS";
            this.btnVS.Size = new System.Drawing.Size(44, 44);
            this.btnVS.TabIndex = 44;
            this.btnVS.UseVisualStyleBackColor = false;
            this.btnVS.Click += new System.EventHandler(this.btnVS_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(829, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 30);
            this.button1.TabIndex = 4;
            this.button1.Text = "選擇";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbxItem
            // 
            this.cbxItem.Enabled = false;
            this.cbxItem.FormattingEnabled = true;
            this.cbxItem.Location = new System.Drawing.Point(24, 14);
            this.cbxItem.Name = "cbxItem";
            this.cbxItem.Size = new System.Drawing.Size(784, 28);
            this.cbxItem.TabIndex = 3;
            this.cbxItem.SelectedIndexChanged += new System.EventHandler(this.cbxItem_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 20);
            this.label4.TabIndex = 45;
            this.label4.Text = "備註：";
            // 
            // txtPS
            // 
            this.txtPS.Location = new System.Drawing.Point(83, 53);
            this.txtPS.Name = "txtPS";
            this.txtPS.Size = new System.Drawing.Size(725, 29);
            this.txtPS.TabIndex = 46;
            // 
            // btnUpdata
            // 
            this.btnUpdata.Enabled = false;
            this.btnUpdata.Location = new System.Drawing.Point(829, 52);
            this.btnUpdata.Name = "btnUpdata";
            this.btnUpdata.Size = new System.Drawing.Size(90, 30);
            this.btnUpdata.TabIndex = 47;
            this.btnUpdata.Text = "更新";
            this.btnUpdata.UseVisualStyleBackColor = false;
            this.btnUpdata.Click += new System.EventHandler(this.btnUpdata_Click);
            // 
            // frmVoltageSag
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1066, 638);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panChart);
            this.Font = new System.Drawing.Font("微軟正黑體", 12F);
            this.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmVoltageSag";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmVoltageSag";
            this.Load += new System.EventHandler(this.frmVoltageSag_Load);
            this.SizeChanged += new System.EventHandler(this.frmVoltageSag_SizeChanged);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panChart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxRalay;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cbxItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnVS;
        private System.Windows.Forms.Button btnUpdata;
        private System.Windows.Forms.TextBox txtPS;
        private System.Windows.Forms.Label label4;
    }
}