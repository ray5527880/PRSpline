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
            labFile4_Name.Text = "";
            labFile4_StartTime.Text = "";
            labFile5_Name.Text = "";
            labFile5_StartTime.Text = "";
            this.groupBox3.Enabled = false;
            this.groupBox4.Enabled = false;
            this.groupBox5.Enabled = false;
            if (main.strFileName2 != string.Empty)
            {
                labFile2_Name.Text = main.strFileName2;
                labFile2_StartTime.Text = main.mParser_2.Schema.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss fff");
                this.groupBox3.Enabled = true;
                this.groupBox2.Enabled = false;
            }
            if (main.strFileName3 != string.Empty)
            {
                this.groupBox4.Enabled = true;
                this.groupBox3.Enabled = false;
                labFile3_Name.Text = main.strFileName3;
                labFile3_StartTime.Text = main.mParser_3.Schema.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss fff");
            }
            if (main.strFileName4 != string.Empty)
            {
                this.groupBox5.Enabled = true;
                this.groupBox4.Enabled = false;
                labFile4_Name.Text = main.strFileName4;
                labFile4_StartTime.Text = main.mParser_4.Schema.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss fff");
            }
            if (main.strFileName5 != string.Empty)
            {
                this.groupBox5.Enabled = false;
                labFile5_Name.Text = main.strFileName5;
                labFile5_StartTime.Text = main.mParser_5.Schema.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss fff");
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (await main.OpenSeondFile(frmMain.SelectFile.File_2))
            {
                labFile2_Name.Text = main.strFileName2;
                labFile2_StartTime.Text = main.mParser_2.Schema.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss fff");
                this.groupBox3.Enabled = true;
                this.groupBox2.Enabled = false;
            }
            else
                button1.Enabled = true;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            if (await main.OpenSeondFile(frmMain.SelectFile.File_3))
            {
                labFile3_Name.Text = main.strFileName2;
                labFile3_StartTime.Text = main.mParser_2.Schema.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss fff");
                this.groupBox4.Enabled = true;
                this.groupBox3.Enabled = false;
            }
            else
                button2.Enabled = true;
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            if (await main.OpenSeondFile(frmMain.SelectFile.File_4))
            {
                labFile3_Name.Text = main.strFileName2;
                labFile3_StartTime.Text = main.mParser_2.Schema.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss fff");
                this.groupBox5.Enabled= true;
                this.groupBox4.Enabled= false;
            }
            else
                button3.Enabled = true;
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (await main.OpenSeondFile(frmMain.SelectFile.File_5))
            {
                labFile3_Name.Text = main.strFileName2;
                labFile3_StartTime.Text = main.mParser_2.Schema.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss fff");
                this.groupBox5.Enabled = false;

            }
        }
    }
}
