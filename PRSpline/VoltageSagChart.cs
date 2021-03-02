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
using System.Threading;
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
            chart1.Series.Clear();

            SetChartAreasStyle(0);
            SetChartAreasStyle(1);

            AddSeriel();
            Task.Run(() =>
            {
                Thread.Sleep(50);
                Invoke(new Action(() =>
                {
                    chart1.ChartAreas[0].InnerPlotPosition = chart1.ChartAreas[1].InnerPlotPosition;
                    chart1.ChartAreas[0].Position.Auto = false;
                    chart1.ChartAreas[0].Position.X = chart1.ChartAreas[1].Position.X;
                    chart1.ChartAreas[0].Position.Y = chart1.ChartAreas[1].Position.Y;
                    chart1.ChartAreas[0].Position.Height = chart1.ChartAreas[1].Position.Height;
                    chart1.ChartAreas[0].Position.Width = chart1.ChartAreas[1].Position.Width;
                }));
            });

        }


        private void SetChartAreasStyle(int index)
        {
            chart1.ChartAreas[index].AxisX.Minimum = 0.001;
            chart1.ChartAreas[index].AxisX.Maximum = 10;
            chart1.ChartAreas[index].AxisX.Interval = 1;
            chart1.ChartAreas[index].AxisX.MinorGrid.Enabled = true;
            chart1.ChartAreas[index].AxisX.MinorGrid.Interval = 1;
            chart1.ChartAreas[index].AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[index].AxisX.IsLogarithmic = true;
            chart1.ChartAreas[index].AxisX.LogarithmBase = 10;
            chart1.ChartAreas[index].AxisY.Minimum = 0;
            chart1.ChartAreas[index].AxisY.Maximum = 1;
            chart1.ChartAreas[index].AxisY.Interval = 0.1;

        }
        private void AddSeriel()
        {
            this.chart1.Series.Add(new Series()
            {
                ChartArea = "ChartArea2",
                //Legend = "ChartArea1",
                IsVisibleInLegend = false,
                BorderWidth = 2,

                ChartType = SeriesChartType.Area,
                Color = Color.LightCyan,
            });
            chart1.Series[0].Points.AddXY(0.05, 0.9);
            chart1.Series[0].Points.AddXY(1, 0.9);
            this.chart1.Series.Add(new Series()
            {
                ChartArea = "ChartArea2",
                //Legend = "ChartArea1",
                IsVisibleInLegend = false,
                BorderWidth = 2,

                ChartType = SeriesChartType.Area,
                Color = Color.Yellow,
            });
            chart1.Series[1].Points.AddXY(0.05, 0.5);
            chart1.Series[1].Points.AddXY(1, 0.5);
            this.chart1.Series.Add(new Series()
            {
                ChartArea = "ChartArea2",
                //Legend = "ChartArea1",
                IsVisibleInLegend = false,
                BorderWidth = 2,

                ChartType = SeriesChartType.Area,
                Color = Color.Yellow,
            });
            chart1.Series[2].Points.AddXY(0.2, 0.7);
            chart1.Series[2].Points.AddXY(1, 0.7);
            this.chart1.Series.Add(new Series()
            {
                ChartArea = "ChartArea2",
                //Legend = "ChartArea1",
                IsVisibleInLegend = false,
                BorderWidth = 2,

                ChartType = SeriesChartType.Area,
                Color = Color.Yellow,
            });
            chart1.Series[3].Points.AddXY(0.5, 0.8);
            chart1.Series[3].Points.AddXY(1, 0.8);


            this.chart1.Series.Add(new Series()
            {
                ChartArea = "ChartArea1",
                //Legend = "ChartArea1",
                IsVisibleInLegend = false,
                BorderWidth = 5,

                ChartType = SeriesChartType.Line,
                Color = Color.Red,
            });
            chart1.Series[chart1.Series.Count - 1].Points.AddXY(0.05, 0);
            chart1.Series[chart1.Series.Count - 1].Points.AddXY(0.05, 0.5);
            chart1.Series[chart1.Series.Count - 1].Points.AddXY(0.2, 0.5);
            chart1.Series[chart1.Series.Count - 1].Points.AddXY(0.2, 0.7);
            chart1.Series[chart1.Series.Count - 1].Points.AddXY(0.5, 0.7);
            chart1.Series[chart1.Series.Count - 1].Points.AddXY(0.5, 0.8);
            chart1.Series[chart1.Series.Count - 1].Points.AddXY(10, 0.8);

            int count = chart1.Series.Count;
            foreach (var item in _voltageSagDatas)
            {
                this.chart1.Series.Add(new Series()
                {
                    ChartArea = "ChartArea1",
                    ChartType = SeriesChartType.Point,
                    MarkerSize = 10,
                    Color = Color.Red,
                    LegendText = item.treggerDateTime.ToString()
                });
                chart1.Series[count].Points.AddXY(item.duration / 1000, GetMinPoint(item));
                count++;
            }
            this.chart1.Annotations.Add(new TextAnnotation()
            {
                Text = "A",
                AxisX = this.chart1.ChartAreas[1].AxisX,
                AxisY = this.chart1.ChartAreas[1].AxisY,
                X = 0.02,
                Y = 0.3,
                Font = new Font(FontFamily.GenericMonospace, 20),
                ForeColor = Color.Red
            });
            this.chart1.Annotations.Add(new TextAnnotation()
            {
                Text = "B",
                AxisX = this.chart1.ChartAreas[1].AxisX,
                AxisY = this.chart1.ChartAreas[1].AxisY,
                X = 0.1,
                Y = 0.7,
                Font = new Font(FontFamily.GenericMonospace, 20),
                ForeColor = Color.Red
            });
            this.chart1.Annotations.Add(new TextAnnotation()
            {
                Text = "C",
                AxisX = this.chart1.ChartAreas[1].AxisX,
                AxisY = this.chart1.ChartAreas[1].AxisY,
                X = 0.1,
                Y = 0.3,
                Font = new Font(FontFamily.GenericMonospace, 20),
                ForeColor = Color.Red
            });
            this.chart1.Annotations.Add(new TextAnnotation()
            {
                Text = "D",
                AxisX = this.chart1.ChartAreas[1].AxisX,
                AxisY = this.chart1.ChartAreas[1].AxisY,
                X = 2,
                Y = 0.3,
                Font = new Font(FontFamily.GenericMonospace, 20),
                ForeColor = Color.Red
            });
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

        private void chart1_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}
