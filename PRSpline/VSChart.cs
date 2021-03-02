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
    public partial class VSChart : UserControl
    {
        private double STime, ETime;
        public VSChart()
        {
            InitializeComponent();
        }
        public VSChart(string chartName, double[] value, double[] time, double triggertime, double tA, double tB, int x_maxTime)
        {
            Value = value;
            Time = time;
            TA = tA;
            TB = tB;
            triggerTime = triggertime;
            strName = chartName;
            X_maxTime = x_maxTime;
            InitializeComponent();
        }
        public VSChart(string chartName, double[] value, double[] time, double tA, double tB, int x_maxTime)
        {
            Value = value;
            Time = time;
            TA = tA;
            TB = tB;
            strName = chartName;
            X_maxTime = x_maxTime;
            InitializeComponent();
        }
        public double[] Value;
        public double[] Time;
        public double triggerTime = 0;
        public double TA = 0;
        public double TB = 0;
        public string strName;
        private double minValue = 0;
        private double maxValue = 0;
        private double minTime = 0;
        private double maxTime = 0;
        private int X_maxTime;


        private void VSChart_Load(object sender, EventArgs e)
        {            
            STime = TA - (Time[1] - Time[0]) * 64 * 5;
            ETime = TB + (Time[1] - Time[0]) * 64 * 5;

            if (Value == null || Time == null) return;
            if (Value.Length != Time.Length) return;

            this.chart1.ChartAreas[0].AxisX.Title = "Time(ms)";
            this.chart1.ChartAreas[0].BackColor = Color.White;

            this.chart1.Series.Clear();
            this.chart1.Series.Add(new Series()
            {
                LegendText = "",
                BorderWidth = 2,
                ChartType = SeriesChartType.Line
            });
            for (int i = 64; i < Value.Length - 64; i++)
            {
                if (Time[i] >= STime && Time[i] <= ETime)
                {
                    chart1.Series[0].Points.AddXY(Time[i], Value[i]);
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
            if (TA != 0 && TB != 0)
            {
                minTime = maxTime;
                minValue = maxValue;
            }
            for (int i = 64; i < Value.Length - 64; i++)
            {
                if (Value[i] < minValue)
                {
                    minTime = Time[i];
                    minValue = Value[i];
                }
            }
            if (triggerTime != 0)
            {
                this.chart1.Annotations.Add(new VerticalLineAnnotation()
                {
                    LineColor = Color.Red,
                    X = triggerTime,
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
            this.label5.Text = strName;
            this.label7.Text = maxValue.ToString("#0.000");
            this.label8.Text = maxTime.ToString("#0.000");
            this.label9.Text = minValue.ToString("#0.000");
            this.label10.Text = minTime.ToString("#0.000");

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

            if (chart1.ChartAreas[0].AxisY.Maximum <15)
            {
                chart1.ChartAreas[0].AxisY.Maximum = 1.2;
                chart1.ChartAreas[0].AxisY.Minimum = 0;
            }

            this.chart1.ChartAreas[0].AxisX.Maximum = ETime;
            this.chart1.ChartAreas[0].AxisX.Minimum = STime;
            this.chart1.Legends[0].Enabled = false;
            this.chart1.ChartAreas[0].AxisY.Interval = (this.chart1.ChartAreas[0].AxisY.Maximum - this.chart1.ChartAreas[0].AxisY.Minimum) / 4;
            
        }
    }
}
