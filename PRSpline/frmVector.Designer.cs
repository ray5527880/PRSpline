namespace PRSpline
{
    partial class frmVector
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxRefenence = new System.Windows.Forms.ComboBox();
            this.btnADD = new System.Windows.Forms.Button();
            this.btnLOSE = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbxRefenence);
            this.panel1.Controls.Add(this.btnADD);
            this.panel1.Location = new System.Drawing.Point(160, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(210, 80);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Refenence：";
            // 
            // cbxRefenence
            // 
            this.cbxRefenence.FormattingEnabled = true;
            this.cbxRefenence.Location = new System.Drawing.Point(137, 8);
            this.cbxRefenence.Name = "cbxRefenence";
            this.cbxRefenence.Size = new System.Drawing.Size(70, 21);
            this.cbxRefenence.TabIndex = 0;
            // 
            // btnADD
            // 
            this.btnADD.BackColor = System.Drawing.Color.Transparent;
            this.btnADD.FlatAppearance.BorderSize = 0;
            this.btnADD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnADD.Location = new System.Drawing.Point(171, 35);
            this.btnADD.Name = "btnADD";
            this.btnADD.Size = new System.Drawing.Size(36, 36);
            this.btnADD.TabIndex = 1;
            this.btnADD.UseVisualStyleBackColor = false;
            this.btnADD.Click += new System.EventHandler(this.btnADD_Click);
            // 
            // btnLOSE
            // 
            this.btnLOSE.BackColor = System.Drawing.Color.Transparent;
            this.btnLOSE.FlatAppearance.BorderSize = 0;
            this.btnLOSE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLOSE.Location = new System.Drawing.Point(331, 98);
            this.btnLOSE.Name = "btnLOSE";
            this.btnLOSE.Size = new System.Drawing.Size(36, 36);
            this.btnLOSE.TabIndex = 2;
            this.btnLOSE.UseVisualStyleBackColor = false;
            // 
            // frmVector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 611);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnLOSE);
            this.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.Name = "frmVector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmVector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmVector_FormClosing);
            this.Load += new System.EventHandler(this.frmVector_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnADD;
        private System.Windows.Forms.ComboBox cbxRefenence;
        private System.Windows.Forms.Button btnLOSE;
    }
}