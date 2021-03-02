using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BF_FW;

namespace PRSpline
{
    public partial class frmDownload : Form
    {
        public frmDownload()
        {
            InitializeComponent();
        }

        private void frmDownload_Load(object sender, EventArgs e)
        {
            this.cbxName.Items.Clear();
            this.cbxName.Items.Add("All");
            foreach (var item in EditXml.mFTPData)
            {
                this.cbxName.Items.Add(item.strName);
            }
            this.cbxName.SelectedIndex = 0;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            Downloading frm = new Downloading(this.cbxName.SelectedIndex);
            frm.ShowDialog();
        }
    }
}
