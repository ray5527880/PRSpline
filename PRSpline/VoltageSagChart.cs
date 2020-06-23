using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BF_FW.data;

namespace PRSpline
{
    public partial class VoltageSagChart : UserControl
    {
        private VoltageSagData.voltageSagData[] _voltageSagDatas;
        public VoltageSagChart(VoltageSagData.voltageSagData[] voltageSagDatas)
        {
            _voltageSagDatas = voltageSagDatas;
            InitializeComponent();
        }

        private void VoltageSagChart_Load(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].BackColor = Color.White;

            chart1.Series.Clear();

            AddSeriel();

            chart1.ChartAreas[0].AxisX.Minimum = 0.001;
            chart1.ChartAreas[0].AxisX.Maximum = 10;
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisX.MinorGrid.Interval = 1;
            chart1.ChartAreas[0].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisX.IsLogarithmic = true;
            chart1.ChartAreas[0].AxisX.LogarithmBase = 10;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 1;
            chart1.ChartAreas[0].AxisY.Interval = 0.1;
        }
        private void AddSeriel()
        {
            this.chart1.Series.Add(new Series()
            {
                //Legend = "ChartArea1",           
                IsVisibleInLegend = false,
                BorderWidth = 2,

                ChartType = SeriesChartType.Line,
                Color = Color.Blue,

            });
            chart1.Series[0].Points.AddXY(0.02f, 0);
            chart1.Series[0].Points.AddXY(0.02f, 0.5f);
            chart1.Series[0].Points.AddXY(0.2f, 0.5f);
            chart1.Series[0].Points.AddXY(0.2f, 0.7f);
            chart1.Series[0].Points.AddXY(0.5f, 0.7f);
            chart1.Series[0].Points.AddXY(0.5f, 0.8f);
            chart1.Series[0].Points.AddXY(10f, 0.8f);

            int count = 1;
            foreach (var item in _voltageSagDatas)
            {
                this.chart1.Series.Add(new Series()
                {
                    ChartType = SeriesChartType.Point,
                    MarkerSize = 10,
                    Color = Color.Red,
                    LegendText = item.treggerDateTime.ToString("")
                });
                chart1.Series[count].Points.AddXY(item.duration, GetMinPoint(item));
                count++;
            }
        }
        private decimal GetMinPoint(VoltageSagData.voltageSagData data)
        {
            decimal value = 0;
            value = data.PValue;
            if (value > data.QValue)
                value = data.QValue;
            if (value > data.SValue)
                value = data.SValue;            
            return value;
        }
    }
}
