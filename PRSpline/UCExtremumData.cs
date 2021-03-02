using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BF_FW.data;
namespace PRSpline
{
    public partial class UCExtremumData : UserControl
    {
        ExtremumData.Extremum _extremum;
        public UCExtremumData(ExtremumData.Extremum extremum)
        {
            _extremum = extremum;
            InitializeComponent();
        }

        private void UCExtremumData_Load(object sender, EventArgs e)
        {
            label1.Text = _extremum.strName;
            label6.Text = _extremum.MaxValue.ToString("#0.000");
            label7.Text = _extremum.MaxTime.ToString("#0.000") + " ms";
            label8.Text = _extremum.MinValue.ToString("#0.000");
            label9.Text = _extremum.MinTime.ToString("#0.000") + " ms";
        }
    }
}
