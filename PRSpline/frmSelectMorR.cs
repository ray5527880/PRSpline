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

namespace PRSpline
{
    public partial class frmSelectMorR : Form
    {
        OpenFileDialog _openFileDialog;
        public frmSelectMorR(ref OpenFileDialog openFileDialogs)
        {
            _openFileDialog = openFileDialogs;
            InitializeComponent();
        }

        private void frmSelectMorR_Load(object sender, EventArgs e)
        {
            string strpath = "./res/";
            // Iimage.Images.Add(Image.FromFile(strpath + "download.png"));

            button1.Image = Image.FromFile(strpath + "Meter.jpg");
            button2.Image = Image.FromFile(strpath + "Relay_1.jpg");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this._openFileDialog.InitialDirectory = EditXml.MeterFilePaht;
            if (this._openFileDialog.ShowDialog() == DialogResult.OK)
            {
                frmMain.IsOpenFlies = true;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this._openFileDialog.InitialDirectory = EditXml.RelayFilePaht;
            if (this._openFileDialog.ShowDialog() == DialogResult.OK)
            {
                frmMain.IsOpenFlies = true;
                this.Close();
            }
        }

    }
}
