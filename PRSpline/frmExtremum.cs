using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BF_FW.data;

namespace PRSpline
{
    public partial class frmExtremum : Form
    {
        private ExtremumData.Extremum[] extremums;
        public frmExtremum(ExtremumData.Extremum[] extremum)
        {
            extremums = extremum;
            InitializeComponent();
        }

        private void frmExtremum_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < extremums.Length; i++)
            {
                panel1.Controls.Add(new UCExtremumData(extremums[i]) { Location = new Point(10, 10 + 140 * i) });
            }
        }
    }
}
