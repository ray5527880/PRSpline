using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace PRSpline
{
    public partial class Vector : UserControl
    {
        private string[] DeviceName;
        private string[] strRms;
        private List<Label> labelList;
        private Color[] baseColor=new Color[]
        {
            Color.LawnGreen,
            Color.Red
        };
        public Vector(string[] _DeviceName)
        {
            InitializeComponent();
            DeviceName = _DeviceName;           
        }

        private void Vector_Load(object sender, EventArgs e)
        {
            labelList = new List<Label>();
            for (int ii = 0; ii < DeviceName.Length; ii++)
            {
                int width = (int)(ii / 3) * 25 + 5;
                int hith = ii % 3 * 85 + 5;
                CheckBox _CKB = new CheckBox();
                _CKB.Text = DeviceName[ii];
                _CKB.Name = "ckb_" + DeviceName[ii];
                _CKB.Location = new Point(hith, width);
                _CKB.Visible = true;
                _CKB.AutoSize = true;
                _CKB.Click += CKB_Click;
                panel1.Controls.Add(_CKB);
                this.chart1.Series.Add(new Series()
                {
                    Legend = DeviceName[ii],
                    LegendText = DeviceName[ii],
                    Name = DeviceName[ii],
                    BorderWidth = 2,
                    ChartType = SeriesChartType.Line,                    
                });
                Label _label = new Label();
                panel2.Controls.Add(_label);
                _label.Text = DeviceName[ii] + "";
                _label.Location = new Point(hith, width);
                _label.AutoSize = true;
                
                _label.ForeColor = chart1.Series[ii].Color;                
                labelList.Add(_label);
            }
          
            chart1.Series[0].Points.AddXY(0, 0);
            chart1.Series[0].Points.AddXY(-5, -5);
            chart1.Series[1].Points.AddXY(0, 0);
            chart1.Series[1].Points.AddXY(5, 5);
            for (int i = 0; i < 2; i++)
            {
                chart1.Series[i].Color = baseColor[i];
                labelList[i].ForeColor = baseColor[i];
            }
            this.chart1.ChartAreas[0].AxisX.Maximum = 10;
            this.chart1.ChartAreas[0].AxisX.Minimum = -10;
            this.chart1.ChartAreas[0].AxisY.Maximum = 10;
            this.chart1.ChartAreas[0].AxisY.Minimum = -10;
            this.chart1.ChartAreas[0].AxisX.Interval = 4;
            this.chart1.ChartAreas[0].AxisY.Interval = 4;
            this.chart1.Annotations.Add(new TextAnnotation()
            {
                AnchorX = 50,
                Name = "MoveTextY",
                ForeColor = Color.Red,
                LineWidth = 0,
                AxisY = this.chart1.ChartAreas[0].AxisY,
                Y = 5,
                Text = "Time ms",
                Font = new Font(Font.Name, 10, FontStyle.Bold),               
            });
        }
        private void CKB_Click(object sender, EventArgs e)
        {
            chart1.Series[((CheckBox)sender).Text].Enabled = !chart1.Series[((CheckBox)sender).Text].Enabled;
        }
        public void UpdataChart()
        {

        }
    }
}
