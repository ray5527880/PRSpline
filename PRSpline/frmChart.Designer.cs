namespace PRSpline
{
    partial class frmChart
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.btnNonal = new System.Windows.Forms.Button();
            this.btnMoveLine = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chart1.BackColor = System.Drawing.Color.DimGray;
            this.chart1.BorderlineColor = System.Drawing.SystemColors.HighlightText;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(16, 5);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(1296, 561);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            this.chart1.AnnotationPositionChanged += new System.EventHandler(this.chart1_AnnotationPositionChanged_1);
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar1.Location = new System.Drawing.Point(16, 575);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(1299, 17);
            this.hScrollBar1.TabIndex = 1;
            this.hScrollBar1.ValueChanged += new System.EventHandler(this.hScrollBar_ValueChange);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Jpeg|*.jpeg";
            this.saveFileDialog.Title = "Save Chart";
            // 
            // btnNonal
            // 
            this.btnNonal.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnNonal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNonal.Location = new System.Drawing.Point(16, 5);
            this.btnNonal.Name = "btnNonal";
            this.btnNonal.Size = new System.Drawing.Size(30, 25);
            this.btnNonal.TabIndex = 2;
            this.btnNonal.Text = "N";
            this.btnNonal.UseVisualStyleBackColor = false;
            this.btnNonal.Click += new System.EventHandler(this.btnNonal_Click);
            // 
            // btnMoveLine
            // 
            this.btnMoveLine.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnMoveLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveLine.Location = new System.Drawing.Point(52, 5);
            this.btnMoveLine.Name = "btnMoveLine";
            this.btnMoveLine.Size = new System.Drawing.Size(30, 25);
            this.btnMoveLine.TabIndex = 3;
            this.btnMoveLine.Text = "M";
            this.btnMoveLine.UseVisualStyleBackColor = false;
            this.btnMoveLine.Click += new System.EventHandler(this.btnMoveLine_Click);
            // 
            // frmChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CausesValidation = false;
            this.Controls.Add(this.btnMoveLine);
            this.Controls.Add(this.btnNonal);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.chart1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "frmChart";
            this.Size = new System.Drawing.Size(1346, 599);
            this.Load += new System.EventHandler(this.frmChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button btnNonal;
        private System.Windows.Forms.Button btnMoveLine;
    }
}
