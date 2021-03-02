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

namespace PRSpline
{
    public partial class VSChart_2 : UserControl
    {
        public double[] Value_A;
        public double[] Value_B;
        public double[] Value_C;
        public double[] Times;
        public double TA, TB, TriggerTime;
        private double minValue = 0;
        private double maxValue = 0;
        private double minTime = 0;
        private double maxTime = 0;


        public VSChart_2()
        {
            InitializeComponent();
        }
        public void SetData(double triggertime, double tA, double tB)
        {
            TriggerTime = triggertime;
            TA = tA;
            TB = tB;
        }
        public void SetData(double tA, double tB)
        {
            TA = tA;
            TB = tB;
        }

        private void VSChart_2_Load(object sender, EventArgs e)
        {
            var STime = TA - (Times[1] - Times[0]) * 64 * 5;
            var ETime = TB + (Times[1] - Times[0]) * 64 * 5;

            if (Value_A == null || Value_B == null || Value_C == null || Times == null) return;
            if (Value_A.Length != Times.Length) return;

            this.chart1.ChartAreas[0].AxisX.Title = "Time(ms)";
            this.chart1.ChartAreas[0].BackColor = Color.White;

            this.chart1.Series.Clear();


            InsertChart("Va", Value_A, Times, STime, ETime);
            InsertChart("Vb", Value_B, Times, STime, ETime);
            InsertChart("Vc", Value_C, Times, STime, ETime);

            if (TriggerTime != 0)
            {
                this.chart1.Annotations.Add(new VerticalLineAnnotation()
                {
                    LineColor = Color.Red,
                    X = TriggerTime,
                    IsInfinitive = true,
                    AxisX = this.chart1.ChartAreas[0].AxisX,
                    LineWidth = 2
                });
            }
            else if (TA != 0 && TB != 0)
            {
                this.chart1.Annotations.Add(new VerticalLineAnnotation()
                {
                    LineColor = Color.LightSkyBlue,
                    X = TA,
                    IsInfinitive = true,
                    AxisX = this.chart1.ChartAreas[0].AxisX,
                    LineWidth = 2
                });
                this.chart1.Annotations.Add(new VerticalLineAnnotation()
                {
                    LineColor = Color.LightGreen,
                    X = TB,
                    IsInfinitive = true,
                    AxisX = this.chart1.ChartAreas[0].AxisX,
                    LineWidth = 2
                });
            }

            if (Math.Abs(minValue) > maxValue)
            {
                var x = minValue * 2;
                chart1.ChartAreas[0].AxisY.Maximum = Math.Abs(Math.Ceiling(x));
                chart1.ChartAreas[0].AxisY.Minimum = Math.Ceiling(x);
            }
            else
            {
                var x = maxValue * 2;
                chart1.ChartAreas[0].AxisY.Maximum = Math.Ceiling(x);
                chart1.ChartAreas[0].AxisY.Minimum = -1 * Math.Ceiling(x);
            }

            if (chart1.ChartAreas[0].AxisY.Maximum < 15)
            {
                chart1.ChartAreas[0].AxisY.Maximum = 1.2;
                chart1.ChartAreas[0].AxisY.Minimum = 0;
            }

            this.chart1.ChartAreas[0].AxisX.Maximum = ETime;
            this.chart1.ChartAreas[0].AxisX.Minimum = STime;
            this.chart1.ChartAreas[0].AxisY.Interval = (this.chart1.ChartAreas[0].AxisY.Maximum - this.chart1.ChartAreas[0].AxisY.Minimum) / 4;

        }
        public void SetValue(int index, double[] value)
        {
            switch (index)
            {
                case 0:
                    Value_A = value;
                    break;
                case 1:
                    Value_B = value;
                    break;
                case 2:
                    Value_C = value;
                    break;
            }
        }

        private void InsertChart(string chartName, double[] Value, double[] Time, double STime, double ETime)
        {
            this.chart1.Series.Add(new Series()
            {
                LegendText = chartName,
                BorderWidth = 2,
                ChartType = SeriesChartType.Line
            });

            for (int i = 64; i < Value.Length - 64; i++)
            {
                if (Times[i] >= STime && Times[i] <= ETime)
                {
                    chart1.Series[chart1.Series.Count - 1].Points.AddXY(Times[i], Value[i]);
                    if (Value[i] > maxValue)
                    {
                        maxTime = Time[i];
                        maxValue = Value[i];
                    }
                    else if (Value[i] < minValue)
                    {
                        minTime = Time[i];
                        minValue = Value[i];
                    }
                }
            }
        }
    }
}
