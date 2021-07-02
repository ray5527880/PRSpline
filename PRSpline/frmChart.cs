﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections;
using System.IO;
using System.Numerics;

using System.Threading.Tasks;
using System.Threading;

using BF_FW.data;
using GSF.COMTRADE;

namespace PRSpline
{
    public partial class frmChart : UserControl
    {
        public enum ChartNo
        {
            Main = 1,
            Second_1 = 2,
            Second_2 = 3
        }

        private int[] FileChannelCount = new int[5];
        private List<double[]> mDatData;

        private Parser mParser;
        private double _startY;
        private double _endY;
        private double Y_MinValue = 0;
        private double Y_MaxValue = 0;
        private double RE_Y_MaxValue;
        private double RE_Y_MinValue;

        private double MoveLine_X, MoveLine_Y;

        public int Bar_range;

        private int bar_OriginalRange;
        private bool bMoveView = false;
        private bool bZoonIn = false;
        private bool bMoveCheck = false;

        private bool IsNonal = false;
        private bool IsMoveLine = false;
        private bool IsSecondFileOpen = false;

        private double dZoonInStratPixs_X, dZoonInStratPixs_Y;
        private double dZoonInStratPoint_X, dZoonInStratPoint_Y;
        private double dZoonInEndPoint_X, dZoonInEndPoint_Y;

        private int blockLimit = 3;
        private int blockLine = 0;

        private List<Annotation> BlockGroup_Line;
        private List<Annotation> BlockGroup_Block;

        private bool bButtonEnable = false;
        private bool bpnlAEnable = false;
        private bool bLineMove = false;

        public frmChart()
        {
            InitializeComponent();
        }
        public frmChart(Parser parser, List<double[]> DatData)
        {
            InitializeComponent();

            mDatData = DatData;
            mParser = parser;
            FileChannelCount[0] = DatData[0].Length - 2;
        }


        private void frmChart_Load(object sender, EventArgs e)
        {
            this.chart1.MouseDoubleClick += chart1_MouseDoubleClick;
            this.chart1.MouseDown += chart1_MouseDown;
            this.chart1.MouseUp += chart1_MouseUp;
            this.chart1.Legends.Clear();
            AddLegends();
            AddChartAreas();

            this.chart1.ChartAreas[0].Position = new ElementPosition(0, -10, 100, -10);
            this.chart1.ChartAreas[1].Position = new ElementPosition(0, 85, 100, 13);
            this.chart1.ChartAreas[2].Position = new ElementPosition(0, 5, 100, 77);

            BlockGroup_Line = new List<Annotation>();
            BlockGroup_Block = new List<Annotation>();

            

            this.chart1.ChartAreas[1].BackColor = Color.White;
            this.chart1.ChartAreas[2].BackColor = Color.White;

            this.chart1.ChartAreas[1].AlignWithChartArea = "B";
            this.chart1.ChartAreas[2].AlignWithChartArea = "B";

            this.chart1.Series.Clear();
            AddSeries();
            AddChartPoint();

            this.chart1.Series[0].ChartArea = "B";
            chart1.Series[0].Points.AddXY(1, 1);

            Set_ChartStyle();
            IsNonal = true;
            btnNonal.Enabled = false;
            var _tooltip_1 = new ToolTip();
            _tooltip_1.InitialDelay = 200;
            _tooltip_1.ReshowDelay = 200;
            _tooltip_1.ShowAlways = true;
            _tooltip_1.IsBalloon = true;
            _tooltip_1.SetToolTip(this.btnNonal, "通常模式");
            var _tooltip_2 = new ToolTip();
            _tooltip_2.InitialDelay = 200;
            _tooltip_2.ReshowDelay = 200;
            _tooltip_2.ShowAlways = true;
            _tooltip_2.IsBalloon = true;
            _tooltip_2.SetToolTip(this.btnMoveLine, "線段移動");
        }

        #region SetBaseView
        private void AddLegends()
        {
            this.chart1.Legends.Add(new Legend("A")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center,
                BackColor = System.Drawing.Color.Transparent,
                LegendStyle = LegendStyle.Row,
                TableStyle = LegendTableStyle.Wide,
                ForeColor = Color.White
            });
            this.chart1.Legends.Add(new Legend("D")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center,
                BackColor = System.Drawing.Color.Transparent,
                Position = new ElementPosition(10, 80, 80, 4)
            });

            this.chart1.Legends.Add(new Legend("B")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center,
                BackColor = System.Drawing.Color.Transparent,
                Position = new ElementPosition(0, -10, 80, -10)
            });
            this.chart1.Legends.Add(new Legend("A_2")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center,
                BackColor = System.Drawing.Color.Transparent,
                LegendStyle = LegendStyle.Row,
                TableStyle = LegendTableStyle.Wide,
                ForeColor = Color.White,
                 Position = new ElementPosition(10, 80, 80, 4)
            });
            this.chart1.Legends.Add(new Legend("A_3")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center,
                BackColor = System.Drawing.Color.Transparent,
                LegendStyle = LegendStyle.Row,
                TableStyle = LegendTableStyle.Wide,
                ForeColor = Color.White,
                Position = new ElementPosition(10, 80, 80, 4)
            });
            this.chart1.Legends.Add(new Legend("A_4")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center,
                BackColor = System.Drawing.Color.Transparent,
                LegendStyle = LegendStyle.Row,
                TableStyle = LegendTableStyle.Wide,
                ForeColor = Color.White,
                Position = new ElementPosition(10, 80, 80, 4)
            }); this.chart1.Legends.Add(new Legend("A_5")
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center,
                BackColor = System.Drawing.Color.Transparent,
                LegendStyle = LegendStyle.Row,
                TableStyle = LegendTableStyle.Wide,
                ForeColor = Color.White,
                Position = new ElementPosition(10, 80, 80, 4)
            });
        }
        private void AddChartAreas()
        {
            this.chart1.ChartAreas.Add(new ChartArea("B"));
            this.chart1.ChartAreas.Add(new ChartArea("D"));
            this.chart1.ChartAreas.Add(new ChartArea("A"));
            this.chart1.ChartAreas.Add(new ChartArea("A_2"));
            this.chart1.ChartAreas.Add(new ChartArea("A_3"));
            this.chart1.ChartAreas.Add(new ChartArea("A_4"));
            this.chart1.ChartAreas.Add(new ChartArea("A_5"));
            this.chart1.ChartAreas[3].Visible = false;
            this.chart1.ChartAreas[4].Visible = false;
            this.chart1.ChartAreas[5].Visible = false;
            this.chart1.ChartAreas[6].Visible = false;


        }

        private void AddSeries()
        {
            this.chart1.Series.Add(new Series()
            {
                Legend = "B",
                LegendText = "Base Point",
                BorderWidth = 2,
                ChartType = SeriesChartType.FastLine
            });
            foreach (var item in mParser.Schema.DigitalChannels)
            {
                this.chart1.Series.Add(new Series()
                {
                    Legend = "D",
                    LegendText = item.Name,
                    BorderWidth = 3,
                    ChartType = SeriesChartType.FastLine,
                    ChartArea = "D"
                });
            }
            foreach (var item in mParser.Schema.AnalogChannels)
            {
                string LegendText = item.Units == "V" || item.Units == "A" ? item.Name + "(" + item.Units + ")" : item.Name;
                this.chart1.Series.Add(new Series()
                {
                    Legend = "A",
                    LegendText = LegendText,
                    BorderWidth = 2,
                    ChartType = SeriesChartType.FastLine,
                    ChartArea = "A"
                });
            }

            foreach (var item in mParser.Schema.AnalogChannels)
            {
                if (item.Units == "V" || item.Units == "A")
                {
                    this.chart1.Series.Add(new Series()
                    {
                        Legend = "A",
                        LegendText = item.Name + "_FFT(" + item.Units + ")",
                        BorderWidth = 2,
                        ChartType = SeriesChartType.FastLine,
                        ChartArea = "A"
                    });
                }
            }
        }
        private void AddChartPoint()
        {
            try
            {
                foreach (var item in mDatData)
                {
                    for (int i = 0; i < mParser.Schema.TotalDigitalChannels; i++)
                    {
                        chart1.Series[i + 1].Points.AddXY(item[1], item[i + mParser.Schema.TotalAnalogChannels + 2]);
                    }
                    for (int i = 0; i < mParser.Schema.TotalAnalogChannels; i++)
                    {
                        chart1.Series[i + mParser.Schema.TotalDigitalChannels + 1].Points.AddXY(item[1], item[i + 2]);

                        if (Y_MinValue > item[i + 2])
                            Y_MinValue = item[i + 2];
                        if (Y_MaxValue < item[i + 2])
                            Y_MaxValue = item[i + 2];
                    }
                    var _FFTLenght = item.Length - 2 - mParser.Schema.TotalChannels;
                    for (int i = 0; i < _FFTLenght; i++)
                    {
                        chart1.Series[i + mParser.Schema.TotalChannels + 1].Points.AddXY(item[1], item[i + mParser.Schema.TotalChannels + 2]);
                    }
                }

            }
            catch (Exception ex1)
            {

            }
        }
        #endregion

        #region SetSecondBaseView

       
        private void AddSecondSeries(int SecondNo, Parser _mParser)
        {
            string LegendName = "A_" + SecondNo;
            IsSecondFileOpen = true;
            foreach (var item in _mParser.Schema.AnalogChannels)
            {
                string LegendText = item.Units == "V" || item.Units == "A" ? item.Name + "(" + item.Units + ")" : item.Name;

                this.chart1.Series.Add(new Series()
                {
                    Legend = LegendName,
                    LegendText = LegendText,
                    BorderWidth = 2,
                    ChartType = SeriesChartType.FastLine,
                    ChartArea = LegendName
                });
            }
            foreach (var FFTItem in _mParser.Schema.AnalogChannels)
            {
                if (FFTItem.Units == "V" || FFTItem.Units == "A")
                {
                    this.chart1.Series.Add(new Series()
                    {
                        Legend = LegendName,
                        LegendText = FFTItem.Name + "_FFT(" + FFTItem.Units + ")",
                        BorderWidth = 2,
                        ChartType = SeriesChartType.FastLine,
                        ChartArea = LegendName
                    });
                }
            }
        }
        private void AddSecondChartPoint(List<double[]> datas, Parser _mParser, int SecondNo)
        {
            string LegendName = "A_" + SecondNo;
            int IndexBase = FileChannelCount[0];
            switch (SecondNo)
            {
                case 2:
                    IndexBase = FileChannelCount[0] +1;
                    FileChannelCount[1] = datas[0].Length - 2;
                    break;
                case 3:
                    IndexBase = FileChannelCount[0] + FileChannelCount[1] + 1;
                    FileChannelCount[2] = datas[0].Length - 2;
                    break;
                case 4:
                    IndexBase = FileChannelCount[0] + FileChannelCount[1] + FileChannelCount[2]+ 1;
                    FileChannelCount[3] = datas[0].Length - 2;
                    break;
                case 5:
                    IndexBase = FileChannelCount[0] + FileChannelCount[1] + FileChannelCount[2] + FileChannelCount[3] + 1;
                    FileChannelCount[4] = datas[0].Length - 2;
                    break;
            }

            try
            {
                foreach (var item in datas)
                {
                    if (item[1] > 60000) break;
                    for (int i = 0; i < item.Length - 2; i++)
                    {
                        chart1.Series[IndexBase + i].Points.AddXY(item[1], item[i + 2]);
                    }
                }
                for (int i = 0; i < datas[0].Length - 2; i++)
                {
                    chart1.Series[IndexBase + i].Enabled = false;
                }
            }
            catch (Exception ex1)
            {

            }
        }
        #endregion

        public void AddSecondFile(List<double[]> datas, int SecondNo, Parser _mParser)
        {
            FileChannelCount[SecondNo - 1] = datas[0].Length - 2;
            this.chart1.ChartAreas[3].Position = new ElementPosition(0, 50, 100, 30);
            this.chart1.ChartAreas[4].Position = new ElementPosition(0, 85, 100, 13);
            this.chart1.ChartAreas[5].Position = new ElementPosition(0, 85, 100, 13);
            this.chart1.ChartAreas[6].Position = new ElementPosition(0, 85, 100, 13);

            for(int i = 0; i < 4; i++)
            {
                this.chart1.ChartAreas[i+3].BackColor = Color.White;
                this.chart1.ChartAreas[i + 3].AlignWithChartArea = "B";
                this.chart1.ChartAreas[i + 3].Visible = false;
            }

            this.chart1.ChartAreas[1].Visible = true;
            IsSecondFileOpen = true;
            //AddSecondLegends(SecondNo);
            AddSecondSeries(SecondNo, _mParser);
            AddSecondChartPoint(datas, _mParser, SecondNo);
            SetSecondStyle(SecondNo);
        }



        private void SetSecondStyle(int SecondNo)
        {
            bar_OriginalRange = 60000;
            //Bar_range = 60000;

            this.hScrollBar1.Maximum = 60000;
            this.hScrollBar1.Minimum = 0;
            this.hScrollBar1.LargeChange = Bar_range;
            
            this.chart1.ChartAreas[SecondNo + 1].AxisY.Maximum = this.chart1.ChartAreas[2].AxisY.Maximum;
            this.chart1.ChartAreas[SecondNo + 1].AxisY.Minimum = this.chart1.ChartAreas[2].AxisY.Minimum;

            this.chart1.ChartAreas[SecondNo + 1].AxisX.Interval = this.chart1.ChartAreas[2].AxisX.Interval;
            this.chart1.ChartAreas[SecondNo + 1].AxisY.Interval = this.chart1.ChartAreas[2].AxisY.Interval;

            this.chart1.ChartAreas[SecondNo + 1].AxisX.Maximum = this.chart1.ChartAreas[2].AxisX.Maximum;
            this.chart1.ChartAreas[SecondNo + 1].AxisX.Minimum = this.chart1.ChartAreas[2].AxisX.Minimum;

            Chart_Enable();
            Chart_AnnotationsLineEnable(this.chart1);
        }


        private void Set_ChartStyle()
        {
            decimal flot = Convert.ToDecimal(Math.Ceiling(mDatData[mDatData.Count() - 1][1]));
            int fLength = 0;
            while (flot > 10)
            {
                flot /= 10;
                flot = Math.Ceiling(flot);
                fLength++;
            }
            for (int i = 0; i < fLength; i++)
            {
                flot *= 10;
            }
            bar_OriginalRange = Convert.ToInt32(flot);
            Bar_range = Convert.ToInt32(flot);

            foreach (var index in chart1.Series)
            {
                index.Enabled = false;
                index.ChartType = SeriesChartType.FastLine;
            }

            if (Math.Abs(Y_MaxValue) >= Math.Abs(Y_MinValue))
            {
                RE_Y_MaxValue = Y_MaxValue > 0 ? Math.Ceiling((Y_MaxValue * 1.1f) / 10) * 10 : (-1) * Math.Ceiling((Y_MaxValue * 1.1f) / 10) * 10;
                RE_Y_MinValue = (-1) * Y_MaxValue < 0 ? (-1) * Math.Ceiling((Y_MaxValue * 1.1f) / 10) * 10 : Math.Ceiling((Y_MaxValue * 1.1f) / 10) * 10;
            }
            else
            {
                RE_Y_MaxValue = Y_MinValue > 0 ? Math.Ceiling((Y_MinValue * 1.1f) / 10) * 10 : (-1) * Math.Ceiling((Y_MinValue * 1.1f) / 10) * 10;
                RE_Y_MinValue = (-1) * Y_MinValue < 0 ? (-1) * Math.Ceiling((Y_MinValue * 1.1f) / 10) * 10 : Math.Ceiling((Y_MinValue * 1.1f) / 10) * 10;
            }

            while ((RE_Y_MaxValue - RE_Y_MinValue) % 6 != 0)
            {
                RE_Y_MaxValue += 10;
                RE_Y_MinValue -= 10;
            }
            this.chart1.ChartAreas[2].AxisY.Maximum = RE_Y_MaxValue;
            this.chart1.ChartAreas[2].AxisY.Minimum = RE_Y_MinValue;

            this.chart1.ChartAreas[2].AxisY.Interval = (this.chart1.ChartAreas[2].AxisY.Maximum - this.chart1.ChartAreas[2].AxisY.Minimum) / 6;

            this.chart1.ChartAreas[2].AxisX.Maximum = Bar_range;
            this.chart1.ChartAreas[2].AxisX.Minimum = 0;
            this.chart1.ChartAreas[1].AxisX.Maximum = Bar_range;
            this.chart1.ChartAreas[1].AxisX.Minimum = 0;

            AddAnnotations();

            this.hScrollBar1.Maximum = Convert.ToInt32(mDatData[mDatData.Count - 1][1]);
            this.hScrollBar1.Minimum = 0;
            this.hScrollBar1.LargeChange = Bar_range;
            this.hScrollBar1.Value = 0;
            this.chart1.MouseWheel += new MouseEventHandler(chart1_MouseWheel);
            this.chart1.ChartAreas[2].CursorX.IsUserEnabled = false;

            this.chart1.ChartAreas[1].AxisY.Maximum = 2;
            this.chart1.ChartAreas[1].AxisY.Minimum = 0;
            this.chart1.ChartAreas[1].AxisY.Interval = 1;

            Chart_Enable();
            Chart_AnnotationsLineEnable(this.chart1);
            this.chart1.Palette = ChartColorPalette.Fire;
        }

        public void CheakButtonEnable(Panel _Panel)
        {
            foreach (var item in _Panel.Controls)
            {
                if (item is Button)
                {
                    if (_Panel.Name == "pnlDigital")
                        ((Button)item).BackColor = Color.LightSteelBlue;
                    if (((Button)item).BackColor == Color.LightSlateGray)
                    {
                        if (((Button)item).Text.IndexOf("FFT") > 0)
                            this.chart1.Series[((Button)item).TabIndex + mParser.Schema.TotalAnalogChannels].Enabled = true;
                        else
                            this.chart1.Series[((Button)item).TabIndex].Enabled = true;
                    }
                }
            }
        }

        private void AddAnnotations()
        {
            this.chart1.ChartAreas[2].AxisX.Interval = Bar_range / 6;
            this.chart1.ChartAreas[2].AxisX.LogarithmBase = 1000;

            double startTime_S = Convert.ToDouble(0);
            double triggerTime_S = TimeSpan.FromTicks(mParser.Schema.TriggerTime.Value - mParser.Schema.StartTime.Value).TotalMilliseconds;

            this.chart1.Annotations.Add(new VerticalLineAnnotation()
            {
                LineColor = Color.Red,
                X = (triggerTime_S - startTime_S),
                IsInfinitive = true,
                AxisX = this.chart1.ChartAreas[2].AxisX,
                LineWidth = 2
            });

            this.chart1.Annotations.Add(new TextAnnotation()
            {
                ForeColor = Color.Red,
                AxisX = this.chart1.ChartAreas[2].AxisX,
                AnchorY = 100,
                LineWidth = 0,
                X = (triggerTime_S - startTime_S) * 1.01f,
                Text = "Trigger " + ((triggerTime_S - startTime_S)).ToString() + "ms"
            });


            this.chart1.Annotations.Add(new VerticalLineAnnotation()
            {
                Name = "MoveLineX",
                LineColor = Color.Black,
                IsInfinitive = true,
                AxisX = this.chart1.ChartAreas[2].AxisX,
                LineWidth = 2
            });

            this.chart1.Annotations.Add(new TextAnnotation()
            {
                Name = "MoveTextX",
                ForeColor = Color.White,
                AxisX = this.chart1.ChartAreas[2].AxisX,
                LineWidth = 0,
                AnchorY = 11,

                Text = "Time " + ((triggerTime_S - startTime_S)).ToString() + "ms",
                Font = new Font(Font.Name, 10, FontStyle.Bold)
            });

            this.chart1.Annotations.Add(new HorizontalLineAnnotation()
            {
                Name = "MoveLineY",
                LineColor = Color.Black,
                IsInfinitive = true,
                AxisY = this.chart1.ChartAreas[2].AxisY,
                LineWidth = 2,
                Y = 5
            });
            this.chart1.Annotations.Add(new TextAnnotation()
            {
                Name = "MoveTextY",
                ForeColor = Color.White,
                AnchorX = 2,
                LineWidth = 0,
                AxisY = this.chart1.ChartAreas[2].AxisY,
                Y = 5,
                Text = "Time ms",
                Font = new Font(Font.Name, 10, FontStyle.Bold),
            });
            this.chart1.Annotations["MoveLineX"].Visible = true;
            this.chart1.Annotations["MoveTextX"].Visible = true;
            this.chart1.Annotations["MoveLineY"].Visible = true;
            this.chart1.Annotations["MoveTextY"].Visible = true;
            chart1.AnnotationPositionChanged += chart1_AnnotationPositionChanged;
        }
        /*------------------------ChartFuntion------------------------*/
        #region ChartFuntion
        public void Chart1_Enable(int index/*, Panel panel*/, string ButtonName)
        {
            this.chart1.Series[index + 1].Enabled = !this.chart1.Series[index + 1].Enabled;
            Chart_AnnotationsLineEnable(this.chart1);
            AllButtonEnable();

            bpnlAEnable = false;
            Chart_Enable();
            int _index = -1;
            foreach(var item in chart1.Series)
            {
                _index++;
                if (_index < mParser.Schema.TotalDigitalChannels + 1) continue;
                if (item.Enabled)
                {
                    bpnlAEnable = true;
                    break;
                }
            }
            MoveLineEnable();
        }

        public void Chart2_Enable(int index/*, Panel panel*/, string ButtonName)
        {
            this.chart1.Series[index + 1].Enabled = !this.chart1.Series[index + 1].Enabled;

            Chart_Enable();
            AllButtonEnable();
        }

        private void AllButtonEnable()
        {
            foreach (var item in this.chart1.Series)
            {
                if (item.Enabled)
                {
                    bButtonEnable = true;
                    this.chart1.Annotations["MoveLineX"].Visible = true;
                    this.chart1.Annotations["MoveTextX"].Visible = true;
                    this.chart1.Series[0].Enabled = true;
                    return;
                }
            }
            bButtonEnable = false;
            this.chart1.Annotations["MoveLineX"].Visible = false;
            this.chart1.Annotations["MoveTextX"].Visible = false;
            this.chart1.Annotations["MoveLineY"].Visible = false;
            this.chart1.Annotations["MoveTextY"].Visible = false;
            this.chart1.Series[0].Enabled = false;
        }

        private void Chart_AnnotationsLineEnable(Chart _Chart)
        {
            bool bSeriseEnable = true;
            int count = 0;
            foreach (var serise in _Chart.Series)
            {
                if (serise.Enabled)
                {
                    bSeriseEnable = false;
                    break;
                }
                count++;
            }
            if (bSeriseEnable)
            {
                _Chart.Annotations[1].Visible = false;
                _Chart.Annotations[2].Visible = false;
            }
            else
            {
                _Chart.Annotations[1].Visible = true;
                _Chart.Annotations[2].Visible = true;
            }
        }
        private void Chart_Enable()
        {
            bool bEnabl = false;
            bool SecondEnable_1 = false;
            bool SecondEnable_2 = false;
            bool SecondEnable_3 = false;
            bool SecondEnable_4 = false;
            int index = 0;
            foreach (var item in chart1.Series)
            {
                if (item.Enabled && item.Legend == "D")
                    bEnabl = true;
                if (item.Enabled && index > FileChannelCount[0])
                {
                    if (index > FileChannelCount[0] + FileChannelCount[1] + FileChannelCount[2] + FileChannelCount[3])
                        SecondEnable_4 = true;
                    else if (index > FileChannelCount[0] + FileChannelCount[1] + FileChannelCount[2])
                        SecondEnable_3 = true;
                    else  if (index > FileChannelCount[0] + FileChannelCount[1])
                        SecondEnable_2 = true;
                    else SecondEnable_1 = true;
                }

                index++;
            }
            if (!bEnabl)
            {
                this.chart1.ChartAreas[1].Visible = false;
                this.chart1.ChartAreas[2].Position = new ElementPosition(0, 10, 100, 88);
                
            }
            else
            {
                this.chart1.ChartAreas[1].Visible = true;
                this.chart1.ChartAreas[2].Position = new ElementPosition(0, 10, 100, 68);
            }
            this.chart1.ChartAreas[2].AxisX.Title = "Time(ms)";
            if (SecondEnable_1)
            {
                this.chart1.ChartAreas[2].Visible = true;
                this.chart1.ChartAreas[3].Visible = true;                

                this.chart1.ChartAreas[2].Position = new ElementPosition(0, 10, 100, 40);
                this.chart1.ChartAreas[3].Position = new ElementPosition(0, 55, 100, 45);
                this.chart1.Legends[3].Position = new ElementPosition(10, 50, 80, 4);

                //this.chart1.ChartAreas[2].AxisX.Enabled = AxisEnabled.False;
                //this.chart1.ChartAreas[3].AxisX.Enabled = AxisEnabled.Auto;

                this.chart1.ChartAreas[2].AxisX.Title = "";
                this.chart1.ChartAreas[3].AxisX.Title = "Time(ms)";
            }
            else
            {
                this.chart1.ChartAreas[3].Visible = false;                
            }
            if (SecondEnable_2)
            {                
                this.chart1.ChartAreas[4].Visible = true;

                this.chart1.ChartAreas[2].Position = new ElementPosition(0, 10, 100, 25);
                this.chart1.ChartAreas[3].Position = new ElementPosition(0, 40, 100, 25);
                this.chart1.ChartAreas[4].Position = new ElementPosition(0, 70, 100, 30);
                this.chart1.Legends[3].Position = new ElementPosition(10, 35, 80, 4);
                this.chart1.Legends[4].Position = new ElementPosition(10, 65, 80, 4);
                
                //this.chart1.ChartAreas[2].AxisX.Enabled = AxisEnabled.False;
                //this.chart1.ChartAreas[3].AxisX.Enabled = AxisEnabled.False;
                //this.chart1.ChartAreas[4].AxisX.Enabled = AxisEnabled.Auto;

                this.chart1.ChartAreas[2].AxisX.Title = "";
                this.chart1.ChartAreas[3].AxisX.Title = "";
                this.chart1.ChartAreas[4].AxisX.Title = "Time(ms)";
            }
            else
            {
                this.chart1.ChartAreas[4].Visible = false;
            }

            if (SecondEnable_3)
            {
                this.chart1.ChartAreas[5].Visible = true;

                this.chart1.ChartAreas[2].Position = new ElementPosition(0, 10, 100, 17);
                this.chart1.ChartAreas[3].Position = new ElementPosition(0, 32, 100, 17);
                this.chart1.ChartAreas[4].Position = new ElementPosition(0, 54, 100, 17);
                this.chart1.ChartAreas[5].Position = new ElementPosition(0, 76, 100, 22);
                this.chart1.Legends[3].Position = new ElementPosition(10, 25, 80, 4);
                this.chart1.Legends[4].Position = new ElementPosition(10, 45, 80, 4);
                this.chart1.Legends[5].Position = new ElementPosition(10, 65, 80, 4);



                this.chart1.ChartAreas[2].AxisX.Title = "";
                this.chart1.ChartAreas[3].AxisX.Title = "";
                this.chart1.ChartAreas[4].AxisX.Title = "";
                this.chart1.ChartAreas[5].AxisX.Title = "Time(ms)";
            }
            else
            {
                this.chart1.ChartAreas[5].Visible = false;
            }
            if (SecondEnable_4)
            {
                this.chart1.ChartAreas[6].Visible = true;

                this.chart1.ChartAreas[2].Position = new ElementPosition(0, 10, 100, 13);
                this.chart1.ChartAreas[3].Position = new ElementPosition(0, 28, 100, 13);
                this.chart1.ChartAreas[4].Position = new ElementPosition(0, 46, 100, 13);
                this.chart1.ChartAreas[5].Position = new ElementPosition(0, 64, 100, 13);
                this.chart1.ChartAreas[6].Position = new ElementPosition(0, 82, 100, 17);
                this.chart1.Legends[3].Position = new ElementPosition(10, 22, 80, 4);
                this.chart1.Legends[4].Position = new ElementPosition(10, 39, 80, 4);
                this.chart1.Legends[5].Position = new ElementPosition(10, 56, 80, 4);
                this.chart1.Legends[6].Position = new ElementPosition(10, 73, 80, 4);
                this.chart1.ChartAreas[2].AxisX.Title = "";
                this.chart1.ChartAreas[3].AxisX.Title = "";
                this.chart1.ChartAreas[4].AxisX.Title = "";
                this.chart1.ChartAreas[5].AxisX.Title = "";
                this.chart1.ChartAreas[6].AxisX.Title = "Time(ms)";
            }
            else
            {
                this.chart1.ChartAreas[6].Visible = false;
            }


            MoveLineEnable();
        }
        #endregion 
        /*------------------------ChartEvent------------------------*/
        #region 

        #endregion
        /*------------------------MouseEvent------------------------*/
        #region MouseEvent

        

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (bMoveView)
            {
                if (!(e.Button == MouseButtons.Left)) return;

                if (e.Y <= 0 || e.Y >= this.chart1.Height) return;

                this._endY = this.chart1.ChartAreas[2].AxisY.PixelPositionToValue(e.Y);

                this.chart1.ChartAreas[2].AxisY.Minimum += this._endY > this._startY ? -Y_MaxValue / 10 : Y_MaxValue / 10;
                this.chart1.ChartAreas[2].AxisY.Maximum += this._endY > this._startY ? -Y_MaxValue / 10 : Y_MaxValue / 10;
            }
            else
            {
                if (e.Y <= 0 || e.Y >= this.chart1.Height) return;
                if (e.X <= 0 || e.X >= this.chart1.Width) return;
            }

            Task.Run(() =>
            {
                MoveLine_X = this.chart1.ChartAreas[2].AxisX.PixelPositionToValue(e.X);
                MoveLine_Y = this.chart1.ChartAreas[2].AxisY.PixelPositionToValue(e.Y);
            }).Wait();

            BeginInvoke(new Action(() =>
                {
                    if (bButtonEnable && (MoveLine_X > 0))
                    {
                        this.chart1.Annotations["MoveLineX"].BeginPlacement();
                        this.chart1.Annotations["MoveLineY"].BeginPlacement();

                        this.chart1.Annotations["MoveLineX"].Visible = true;
                        this.chart1.Annotations["MoveLineX"].X = MoveLine_X;
                      
                            this.chart1.Annotations["MoveTextX"].Visible = true;
                      
                         this.chart1.Annotations["MoveTextX"].X = MoveLine_X;
                        ((TextAnnotation)(this.chart1.Annotations["MoveTextX"])).Text = "Time=" + Math.Round(MoveLine_X, 1).ToString() + "ms";


                        this.chart1.Annotations["MoveLineY"].Y = MoveLine_Y;

                        this.chart1.Annotations["MoveTextY"].Y = this.chart1.Annotations["MoveLineY"].Y * 1.01f;
                        ((TextAnnotation)(this.chart1.Annotations["MoveTextY"])).Text = "I=" + Math.Round(MoveLine_Y, 2).ToString();
                        
                        MoveLineEnable();

                        this.chart1.Annotations["MoveLineX"].EndPlacement();
                        this.chart1.Annotations["MoveLineY"].EndPlacement();
                    }
                }));
            if (bZoonIn)
            {
                this.chart1.Annotations["ZoonInLineX"].Width = this.chart1.ChartAreas[2].AxisX.ValueToPosition(MoveLine_X) - dZoonInStratPixs_X;
                this.chart1.Annotations["ZoonInLineY"].Height = this.chart1.ChartAreas[2].AxisY.ValueToPosition(MoveLine_Y) - dZoonInStratPixs_Y;
                bMoveCheck = true;
            }
        }
       
        private void MoveLineEnable()
        {

            if (!bpnlAEnable)
            {
                this.chart1.Annotations["MoveLineX"].Visible = false;
                this.chart1.Annotations["MoveTextX"].Visible = false;
                this.chart1.Annotations["MoveLineY"].Visible = false;
                this.chart1.Annotations["MoveTextY"].Visible = false;
            }
            else
            {
                this.chart1.Annotations["MoveLineX"].Visible = true;
                this.chart1.Annotations["MoveTextX"].Visible = true;
                if (!IsSecondFileOpen)
                {
                    this.chart1.Annotations["MoveLineY"].Visible = true;
                    this.chart1.Annotations["MoveTextY"].Visible = true;
                }
            }
        }
        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            this._startY = this.chart1.ChartAreas[2].AxisY.PixelPositionToValue(e.Y);

            if (e.Button == MouseButtons.Left&& IsNonal&&!IsSecondFileOpen)
            {

                if (!bMoveView && bButtonEnable)
                {
                    dZoonInStratPixs_X = this.chart1.ChartAreas[2].AxisX.ValueToPosition(this.chart1.ChartAreas[2].AxisX.PixelPositionToValue(e.X));
                    dZoonInStratPixs_Y = this.chart1.ChartAreas[2].AxisY.ValueToPosition(this.chart1.ChartAreas[2].AxisY.PixelPositionToValue(e.Y));
                    dZoonInStratPoint_X = this.chart1.ChartAreas[2].AxisX.PixelPositionToValue(e.X);
                    dZoonInStratPoint_Y = this.chart1.ChartAreas[2].AxisY.PixelPositionToValue(e.Y);
                    chart1.Annotations.Add(new LineAnnotation()
                    {
                        LineColor = Color.Black,
                        AxisX = this.chart1.ChartAreas[2].AxisX,
                        AxisY = this.chart1.ChartAreas[2].AxisY,
                        AnchorX = this.chart1.ChartAreas[2].AxisX.PixelPositionToValue(e.X),
                        AnchorY = this.chart1.ChartAreas[2].AxisY.PixelPositionToValue(e.Y),
                        Width = 0,
                        Height = 0,
                        LineWidth = 3,
                        LineDashStyle = ChartDashStyle.Dash,
                        Name = "ZoonInLineX",
                    });
                    chart1.Annotations.Add(new LineAnnotation()
                    {
                        LineColor = Color.Black,
                        AxisX = this.chart1.ChartAreas[2].AxisX,
                        AxisY = this.chart1.ChartAreas[2].AxisY,
                        AnchorX = this.chart1.ChartAreas[2].AxisX.PixelPositionToValue(e.X),
                        AnchorY = this.chart1.ChartAreas[2].AxisY.PixelPositionToValue(e.Y),
                        Width = 0,
                        Height = 0,
                        LineWidth = 3,
                        LineDashStyle = ChartDashStyle.Dash,
                        Name = "ZoonInLineY",
                        //AllowMoving = true
                    });
                    bZoonIn = !bZoonIn;
                }
                if (this.Cursor != Cursors.Default)
                {
                    bLineMove = true;
                }
            }
        }
        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            if (bZoonIn&&IsNonal)
            {
                dZoonInEndPoint_X = this.chart1.ChartAreas[2].AxisX.PixelPositionToValue(e.X) < bar_OriginalRange ? this.chart1.ChartAreas[2].AxisX.PixelPositionToValue(e.X) : bar_OriginalRange;
                dZoonInEndPoint_Y = this.chart1.ChartAreas[2].AxisY.PixelPositionToValue(e.Y);

                if (bMoveCheck && !bLineMove)
                {
                    bMoveCheck = false;
                    Bar_range = Convert.ToInt32(Math.Round((Math.Abs(dZoonInStratPoint_X - dZoonInEndPoint_X)), 1));
                    this.hScrollBar1.LargeChange = Bar_range;
                    if (Math.Abs(dZoonInStratPoint_X - dZoonInEndPoint_X) >= 6)
                    {
                        if (dZoonInStratPoint_X < dZoonInEndPoint_X)
                        {
                            chart1.ChartAreas[2].AxisX.Maximum = dZoonInStratPoint_X;
                            chart1.ChartAreas[2].AxisX.Minimum = dZoonInEndPoint_X;

                            this.hScrollBar1.Value = Convert.ToInt32(dZoonInStratPoint_X);
                        }
                        else
                        {
                            chart1.ChartAreas[2].AxisX.Minimum = dZoonInStratPoint_X > chart1.ChartAreas[2].AxisX.Minimum ? dZoonInStratPoint_X : chart1.ChartAreas[2].AxisX.Minimum;
                            chart1.ChartAreas[2].AxisX.Maximum = dZoonInEndPoint_X > chart1.ChartAreas[2].AxisX.Maximum ? dZoonInEndPoint_X : chart1.ChartAreas[2].AxisX.Maximum;

                            this.hScrollBar1.Value = Convert.ToInt32(dZoonInEndPoint_X) > 0 ? Convert.ToInt32(dZoonInEndPoint_X) : 0;
                        }
                    }

                    if (dZoonInStratPoint_Y < dZoonInEndPoint_Y)
                    {
                        Smooth_Y(dZoonInEndPoint_Y, dZoonInStratPoint_Y);
                    }
                    else
                    {
                        Smooth_Y(dZoonInStratPoint_Y, dZoonInEndPoint_Y);
                    }

                    if (Math.Abs(dZoonInStratPoint_X - dZoonInEndPoint_X) > 5)
                    {
                        Bar_range = Convert.ToInt32(Math.Round((Math.Abs(dZoonInStratPoint_X - dZoonInEndPoint_X)), 1));
                        this.hScrollBar1.LargeChange = Bar_range;
                        this.chart1.ChartAreas[2].AxisX.Interval = Bar_range / 6;
                    }
                }
                if (bLineMove)
                    bLineMove = false;
                bZoonIn = !bZoonIn;
                for (int i = 0; i < this.chart1.Annotations.Count; i++)
                {
                    if (chart1.Annotations[i].Name == "ZoonInLineX")
                    {
                        chart1.Annotations.RemoveAt(i);
                        break;
                    }
                }
                for (int i = 0; i < this.chart1.Annotations.Count; i++)
                {
                    if (chart1.Annotations[i].Name == "ZoonInLineY")
                    {
                        chart1.Annotations.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        private void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!IsSecondFileOpen)
            {
                if (e.Delta > 0)
                {
                    this.chart1.ChartAreas[2].AxisY.Maximum += this.chart1.ChartAreas[2].AxisY.Interval;
                    this.chart1.ChartAreas[2].AxisY.Minimum += this.chart1.ChartAreas[2].AxisY.Interval;
                }
                else if (e.Delta < 0)
                {
                    this.chart1.ChartAreas[2].AxisY.Maximum -= this.chart1.ChartAreas[2].AxisY.Interval;
                    this.chart1.ChartAreas[2].AxisY.Minimum -= this.chart1.ChartAreas[2].AxisY.Interval;
                }
            }
        }
        private void chart_AnnotationsMoveIn(object sender,MouseEventArgs e)
        {


        }

        private void chart1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            if (!bMoveView && e.Button == MouseButtons.Left && bButtonEnable)
            {
                if (blockLimit >= blockLine - 1)
                {
                    chart1.Annotations.Add(new VerticalLineAnnotation()
                    {
                        LineColor = Color.Yellow,
                        AxisX = this.chart1.ChartAreas[2].AxisX,
                        X = this.chart1.ChartAreas[2].AxisX.PixelPositionToValue(e.X),
                        IsInfinitive = true,
                        LineWidth = 2,
                        LineDashStyle = ChartDashStyle.Dash,
                        Name = "BlockLine" + blockLine,                       
                        AllowMoving=IsMoveLine,

                        
                    });                    
                    BlockGroup_Line.Add(chart1.Annotations["BlockLine" + blockLine]);
                    
                    blockLine++;
                }
                if (blockLine > 1)
                {
                    updataBlockText();
                }
            }
        }
        #endregion


        private void hScrollBar_ValueChange(object sender, EventArgs e)
        {
            chart1.ChartAreas[1].AxisX.Minimum = hScrollBar1.Value;
            chart1.ChartAreas[1].AxisX.Maximum = chart1.ChartAreas[1].AxisX.Minimum + Bar_range;

            chart1.ChartAreas[2].AxisX.Minimum = hScrollBar1.Value;
            chart1.ChartAreas[2].AxisX.Maximum = chart1.ChartAreas[2].AxisX.Minimum + Bar_range;
            for(int i=3;i< chart1.ChartAreas.Count(); i++)
            {
                if (chart1.ChartAreas[i].Visible)
                {
                    chart1.ChartAreas[i].AxisX.Minimum = hScrollBar1.Value;
                    chart1.ChartAreas[i].AxisX.Maximum = chart1.ChartAreas[i].AxisX.Minimum + Bar_range;
                }
            }
          
        }
        public void chart1_XAxisAdd()
        {
            Bar_range /= 2;
            if (Bar_range >= 6)
            {
                this.hScrollBar1.LargeChange = Bar_range;

                for (int i = 1; i < chart1.ChartAreas.Count(); i++)
                {
                    chart1.ChartAreas[i].AxisX.Maximum = chart1.ChartAreas[2].AxisX.Minimum + Bar_range;
                    this.chart1.ChartAreas[i].AxisX.Interval = Bar_range / 6;
                }

            }
            else
                Bar_range *= 2;
        }
        public void char1_XAxisLess()
        {
            Bar_range *= 2;
            if ((chart1.ChartAreas[2].AxisX.Minimum + Bar_range) < bar_OriginalRange)
            {
                this.hScrollBar1.LargeChange = Bar_range;

                for (int i = 1; i < chart1.ChartAreas.Count(); i++)
                {
                    chart1.ChartAreas[i].AxisX.Maximum = chart1.ChartAreas[2].AxisX.Minimum + Bar_range;
                    this.chart1.ChartAreas[i].AxisX.Interval = Bar_range / 6;
                }
                
            }
            else
            {
                Bar_range = bar_OriginalRange;
                this.hScrollBar1.LargeChange = Bar_range;
                for (int i=1;i < chart1.ChartAreas.Count(); i++)
                {
                    chart1.ChartAreas[i].AxisX.Maximum = bar_OriginalRange;
                    this.chart1.ChartAreas[i].AxisX.Interval = Bar_range / 6;
                }
            }
        }
        public void chart_YAxisAdd()
        {
            Smooth_Y(1);
        }
        public void chart_YAxisLess()
        {
            Smooth_Y(2);
        }
        private void Smooth_Y(int Type)
        {
            double douMax = 0;
            double douMin = 0;
            switch (Type)
            {
                case 1:
                    douMax = chart1.ChartAreas[2].AxisY.Maximum * 2;
                    douMin = chart1.ChartAreas[2].AxisY.Minimum * 2;
                    break;
                case 2:
                    douMax = chart1.ChartAreas[2].AxisY.Maximum / 2;
                    douMin = chart1.ChartAreas[2].AxisY.Minimum / 2;
                    break;
            }
            if (douMax != 0 && douMin != 0)
            {
                int count = 0;
                if (Math.Abs(douMax - douMin) > 500)
                {
                    douMax = Math.Round(douMax / 100) * 100;
                    douMin = Math.Round(douMin / 100) * 100;
                    while ((douMax - douMin) % 6 > 0)
                    {
                        if (count > 5)
                            return;
                        douMax += 50;
                        douMin -= 50;
                        count++;
                    }
                }
                else if (Math.Abs(douMax - douMin) > 100)
                {
                    douMax = Math.Round(douMax / 10) * 10;
                    douMin = Math.Round(douMin / 10) * 10;
                    while ((douMax - douMin) % 6 > 0)
                    {
                        if (count > 5)
                            return;
                        douMax += 10;
                        douMin -= 10;
                        count++;
                    }
                }
                else if (Math.Abs(douMax - douMin) > 50)
                {
                    douMax = Math.Round(douMax / 10) * 10;
                    douMin = Math.Round(douMin / 10) * 10;
                    while ((douMax - douMin) % 6 > 0)
                    {
                        if (count > 5)
                            return;
                        douMax += 5;
                        douMin -= 5;
                        count++;
                    }
                }
                else if (Math.Abs(douMax - douMin) > 10)
                {
                    douMax = Math.Round(douMax);
                    douMin = Math.Round(douMin);
                    if ((douMax + douMin) % 2 == 1)
                        douMax++;
                    while ((douMax - douMin) % 6 > 0)
                    {
                        if (count > 5)
                            return;
                        douMax += 1;
                        douMin -= 1;
                        count++;
                    }
                }
                else if (Math.Abs(douMax - douMin) >= 6)
                {
                    douMax = Math.Round(douMax * 10) / 10;
                    douMin = Math.Round(douMin * 10) / 10;
                    if ((douMax + douMin) % 2 == 1)
                        douMax++;
                    while ((douMax - douMin) % 6 > 0)
                    {
                        if (count > 5)
                            return;
                        douMax += 0.5f;
                        douMin -= 0.5f;
                        count++;
                    }
                }
                else if (Math.Abs(douMax - douMin) >= 0.2f)
                {
                    if ((douMax + douMin) * 100 % 2 == 1)
                        douMax = Math.Round((douMax + 0.1), 1);
                    while ((douMax - douMin) * 100 % 6 > 0)
                    {
                        if (count > 5)
                            return;
                        douMax = Math.Round((douMax + 0.1), 1);
                        douMin = Math.Round((douMin - 0.1), 1);
                        count++;
                    }
                }
                if (Math.Abs(douMax - douMin) < 0.2)
                    return;

                for(int i=2;i< chart1.ChartAreas.Count(); i++)
                {
                    chart1.ChartAreas[i].AxisY.Maximum = douMax;
                    chart1.ChartAreas[i].AxisY.Minimum = douMin;
                    chart1.ChartAreas[i].AxisY.Interval = (douMax - douMin) / 6;
                }
               
            }
        }
        private void Smooth_Y(double douMax, double douMin)
        {
            int count = 0;
            if (douMax != 0 && douMin != 0)
            {
                if (Math.Abs(douMax - douMin) > 500)
                {
                    douMax = Math.Round(douMax / 100) * 100;
                    douMin = Math.Round(douMin / 100) * 100;
                    while ((douMax - douMin) % 6 > 0)
                    {
                        if (count > 5)
                            return;
                        douMax += 50;
                        douMin -= 50;
                        count++;
                    }
                }
                else if (Math.Abs(douMax - douMin) > 100)
                {
                    douMax = Math.Round(douMax / 10) * 10;
                    douMin = Math.Round(douMin / 10) * 10;
                    while ((douMax - douMin) % 6 > 0)
                    {
                        if (count > 5)
                            return;
                        douMax += 10;
                        douMin -= 10;
                        count++;
                    }
                }
                else if (Math.Abs(douMax - douMin) > 50)
                {
                    douMax = Math.Round(douMax / 10) * 10;
                    douMin = Math.Round(douMin / 10) * 10;
                    while ((douMax - douMin) % 6 > 0)
                    {
                        if (count > 5)
                            return;
                        douMax += 5;
                        douMin -= 5;
                        count++;
                    }
                }
                else if (Math.Abs(douMax - douMin) > 10)
                {
                    douMax = Math.Round(douMax);
                    douMin = Math.Round(douMin);
                    if ((douMax + douMin) % 2 == 1)
                        douMax++;
                    while ((douMax - douMin) % 6 > 0)
                    {
                        if (count > 5)
                            return;
                        douMax += 1;
                        douMin -= 1;
                        count++;
                    }
                }
                else if (Math.Abs(douMax - douMin) >= 6)
                {
                    douMax = Math.Round(douMax * 10) / 10;
                    douMin = Math.Round(douMin * 10) / 10;
                    if ((douMax + douMin) % 2 == 1)
                        douMax++;
                    while ((douMax - douMin) % 6 > 0)
                    {
                        if (count > 5)
                            return;
                        douMax += 0.5f;
                        douMin -= 0.5f;
                        count++;
                    }
                }
                else if (Math.Abs(douMax - douMin) >= 0.2f)
                {
                    if ((douMax + douMin) * 100 % 2 == 1)
                        douMax = Math.Round((douMax + 0.1), 1);
                    while ((douMax - douMin) * 100 % 6 > 0)
                    {
                        if (count > 5)
                            return;
                        douMax = Math.Round((douMax + 0.1), 1);
                        douMin = Math.Round((douMin - 0.1), 1);
                        count++;
                    }
                }
                if (Math.Abs(douMax - douMin) < 0.2)
                    return;
                chart1.ChartAreas[2].AxisY.Maximum = douMax;
                chart1.ChartAreas[2].AxisY.Minimum = douMin;
                chart1.ChartAreas[3].AxisY.Maximum = douMax;
                chart1.ChartAreas[3].AxisY.Minimum = douMin; 
                chart1.ChartAreas[4].AxisY.Maximum = douMax;
                chart1.ChartAreas[4].AxisY.Minimum = douMin;

                chart1.ChartAreas[2].AxisY.Maximum = douMax;
                chart1.ChartAreas[2].AxisY.Minimum = douMin;
                this.chart1.ChartAreas[2].AxisY.Interval = (douMax - douMin) / 6;
                this.chart1.ChartAreas[3].AxisY.Interval = (douMax - douMin) / 6;
                this.chart1.ChartAreas[4].AxisY.Interval = (douMax - douMin) / 6;
            }
        }

        public void save_chart1()
        {
            var chartImgFormats = Enum.GetNames(typeof(ChartImageFormat));
            string strFilter = string.Empty;
            //foreach (string format in chartImgFormats)
            //{
            //    strFilter += format + "(*." + format + ")|*." + format + "|";
            //}
            //strFilter += "All files (*.*)|*.*";
            //saveFileDialog.Filter = strFilter;
            saveFileDialog.ShowDialog();
            if (saveFileDialog.CheckPathExists && !string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                this.chart1.Annotations.Add(new TextAnnotation()
                {
                    AnchorX = 80,
                    AnchorY = 6,
                    LineWidth = 0,
                    Text = "Location：" + mParser.Schema.StationName + "  StartDate：" + mParser.Schema.StartTime.Value.ToString("yyyy/MM/dd") + "   StartTime：" + mParser.Schema.StartTime.Value.ToString("HH:mm:ss.fff") + "\n\n Device：" + mParser.Schema.DeviceID + "   TriggerDate：" + mParser.Schema.TriggerTime.Value.ToString("yyyy/MM/dd") + "   TriggerTime：" + mParser.Schema.TriggerTime.Value.ToString("HH:mm:ss.fff"),
                    Name = "Information",
                    ForeColor = Color.White,
                    
                });
                //var x = (ChartImageFormat)Enum.Parse(typeof(ChartImageFormat), saveFileDialog.DefaultExt);
                chart1.SaveImage(saveFileDialog.FileName, ChartImageFormat.Jpeg);

                this.chart1.Annotations.Remove(this.chart1.Annotations["Information"]);
            }
        }

        private void chart1_AnnotationPositionChanged(object sender, EventArgs e)
        {
            if (blockLine > 1)
            {
                updataBlockText();
            }
        }
        private void updataBlockText()
        {
            double[] arr_XValue = new double[blockLine];
            if (BlockGroup_Block.Count > 0)
            {
                for (int i = 0; i < BlockGroup_Block.Count; i++)
                {
                    chart1.Annotations.Remove(chart1.Annotations["BlockText" + i]);
                }
                BlockGroup_Block.Clear();
            }
            for (int ii = 0; ii < blockLine; ii++)
            {
                arr_XValue[ii] = BlockGroup_Line[ii].X;
            }
            Array.Sort(arr_XValue);
            for (int i = 0; i < blockLine - 1; i++)
            {
                this.chart1.Annotations.Add(new TextAnnotation()
                {
                    Name = "BlockText" + i,
                    ForeColor = Color.Yellow,
                    AxisX = this.chart1.ChartAreas[2].AxisX,
                    LineWidth = 0,
                    AnchorY = 9,
                    X = Convert.ToDouble(Math.Round(arr_XValue[i + 1] + arr_XValue[i]) / 2),
                    Text = "dt=" + (Math.Round(arr_XValue[i + 1] - arr_XValue[i])).ToString() + " ms"
                });
                BlockGroup_Block.Add(chart1.Annotations["BlockText" + i]);
            }
        }

        private void btnNonal_Click(object sender, EventArgs e)
        {
            btnNonal.Enabled = false;
            btnMoveLine.Enabled = true;
            IsNonal = true;
            IsMoveLine = false;
            
            BlockGroup_Block.Clear();
            
            for (int i = 0; i < BlockGroup_Line.Count; i++)
            {
                chart1.Annotations["BlockLine" + i].AllowMoving = IsMoveLine;
            }


        }

        private void btnMoveLine_Click(object sender, EventArgs e)
        {
            btnNonal.Enabled = true;
            btnMoveLine.Enabled = false;
            IsNonal = false;
            IsMoveLine = true;
            for (int i = 0; i < BlockGroup_Line.Count; i++)
            {
                chart1.Annotations["BlockLine" + i].AllowMoving = IsMoveLine;
            }
        }

        public void ClearBlock()
        {
            for (int i = 0; i < BlockGroup_Block.Count; i++)
            {
                chart1.Annotations.Remove(chart1.Annotations["BlockText" + i]);
            }
            BlockGroup_Block.Clear();
            for (int i = 0; i < BlockGroup_Line.Count; i++)
            {
                chart1.Annotations.Remove(chart1.Annotations["BlockLine" + i]);
            }
            BlockGroup_Line.Clear();
            blockLine = 0;
        }
        public void ReZoom()
        {
            Bar_range = bar_OriginalRange;
            this.hScrollBar1.LargeChange = Bar_range;

            for(int i=1;i< this.chart1.ChartAreas.Count(); i++)
            {
                this.chart1.ChartAreas[i].AxisX.Maximum = Bar_range;
                this.chart1.ChartAreas[i].AxisX.Minimum = 0;
                this.chart1.ChartAreas[i].AxisX.Interval = Bar_range / 6;
                if (i > 1)
                {
                    this.chart1.ChartAreas[i].AxisY.Maximum = Math.Round(RE_Y_MaxValue * 1.1f);
                    this.chart1.ChartAreas[i].AxisY.Minimum = Math.Round(RE_Y_MinValue * 1.1f);
                    this.chart1.ChartAreas[i].AxisY.Interval = (RE_Y_MaxValue - RE_Y_MinValue) / 6;
                }
            }


        }
        public void Form_SizeChanged(int w, int h)
        {
            this.Size = new Size(w, h);
        }
        public bool IsSeries(int  Chartindex)
        {     
            return this.chart1.Series[Chartindex].Enabled;
        }

        private void chart1_AnnotationPositionChanged_1(object sender, EventArgs e)
        {

        }
    }
}
