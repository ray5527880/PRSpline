using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BF_FW;
using BF_FW.data;

namespace PRSpline
{
    public partial class frmVoltageSag : Form
    {
        
        public frmVoltageSag()
        {
            InitializeComponent();
        }

        private void frmVoltageSag_Load(object sender, EventArgs e)
        {
            
            EditXml mEditXml = new EditXml();
            mEditXml.GetXmlData();
            cbxRalay.Items.Clear();
            foreach(var item in EditXml.mFTPData)
            {
                cbxRalay.Items.Add(item.strName);
            }
            cbxRalay.SelectedIndex = 0;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string filePaht = string.Format(@"./downloadFile/{0}/{0}.xml", cbxRalay.SelectedItem);
            var VSData = new VoltageSagXml(filePaht);
            var data = VSData.GetXmlData();
            var _frm = new VoltageSagChart(data);
            panChart.Controls.Add(_frm);
        }
    }
}
