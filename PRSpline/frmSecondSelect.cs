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
    public partial class frmSecondSelect : Form
    {
        private frmMain main;
        public frmSecondSelect(frmMain frmMain)
        {
            main = frmMain;
            InitializeComponent();
        }

        private void frmSecondSelect_Load(object sender, EventArgs e)
        {
            UpdataView();
        }

        private void UpdataView()
        {
            labFile1_Name.Text = main.strFileName1;
            labFile1_StartTime.Text = main.mParser_1.Schema.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss fff");
            labFile2_Name.Text = "";
            labFile2_StartTime.Text = "";
            labFile3_Name.Text = "";
            labFile3_StartTime.Text = "";
            this.groupBox3.Enabled = false;
            if (main.mParser_2 != null)
            {
                labFile2_Name.Text = main.strFileName2;
                labFile2_StartTime.Text = main.mParser_2.Schema.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss fff");
            }
            if (main.mParser_3 != null)
            {
                this.groupBox3.Enabled = true;
                labFile3_Name.Text = main.strFileName3;
                labFile3_StartTime.Text = main.mParser_3.Schema.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss fff");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            main.OpenSeondFile(frmMain.SelectFile.File_2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            main.OpenSeondFile(frmMain.SelectFile.File_3);
        }
    }
}
