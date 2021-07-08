using BF_FW;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRSpline
{
    public partial class frmSelectView : Form
    {
        public frmSelectView()
        {
            InitializeComponent();
        }

        private void btnSingle_Click(object sender, EventArgs e)
        {
            var frm = new frmMain();
            frm.ShowDialog();
        }

        private void btnMultitude_Click(object sender, EventArgs e)
        {
            var frm = new frmMain();
            frm.ShowDialog();
        }

        private void btnVS_Click(object sender, EventArgs e)
        {
            var frm = new frmVoltageSag();
            frm.ShowDialog();
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            EditXml mEditXml = new EditXml();
            mEditXml.GetXmlData();
            var frm = new frmSetup();
            frm.ShowDialog();
        }
    }
}
