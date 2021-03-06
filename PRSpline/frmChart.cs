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
        private struct CData
        {
            public decimal x;
            public decimal y;
        }

        private List<double[]> mDatData;

        private FFTData mFFTData;
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
        private Boolean bMoveView;
        private Boolean bZoonIn = false;
        private Boolean bMoveCheck = false;

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
        public frmChart(Parser parser, List<double[]> DatData, FFTData _FFTData)
        {
            InitializeComponent();

            mDatData = DatData;
            mParser = parser;
            mFFTData = _FFTData;
        }


        private void frmChart_Load(object sender, EventArgs e)
        {
            this.chart1.MouseDoubleClick += chart1_MouseDoubleClick;
            this.chart1.MouseDown += chart1_MouseDown;
            this.chart1.MouseUp += chart1_MouseUp;
            this.chart1.Legends.Clear();
            AddLegends();

            BlockGroup_Line = new List<Annotation>();
            BlockGroup_Block = new List<Annotation>();

            this.chart1.ChartAreas[0].Position = new ElementPosition(0, 5, 100, 73);
            this.chart1.ChartAreas[1].Position = new ElementPosition(0, 85, 100, 13);
            this.chart1.ChartAreas[2].Position = new ElementPosition(0, -10, 100, -10);

            this.chart1.ChartAreas[0].AxisX.Title = "Time(ms)";

            this.chart1.ChartAreas[0].BackColor = Color.White;
            this.chart1.ChartAreas[1].BackColor = Color.White;

            this.chart1.ChartAreas[0].AlignWithChartArea = "B";
            this.chart1.ChartAreas[1].AlignWithChartArea = "B";

            this.chart1.Series.Clear();

            AddChartPoint();

            this.chart1.Series.Add(new Series()
            {
                Legend = "B",
                LegendText = "Base Point",
                BorderWidth = 2,
                ChartType = SeriesChartType.Line
            });
            this.chart1.Series[chart1.Series.Count - 1].ChartArea = "B";
            chart1.Series[chart1.Series.Count - 1].Points.AddXY(1, 1);

            Set_ChartStyle();
        }
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
            this.chart1.ChartAreas.Add(new ChartArea("A"));
            this.chart1.ChartAreas.Add(new ChartArea("D"));
            this.chart1.ChartAreas.Add(new ChartArea("B"));
        }


        private void AddChartPoint()
        {
            for (int i = 0; i < mParser.Schema.TotalChannels; i++)
            {
                if (i < mParser.Schema.TotalAnalogChannels)
                {
                    if (mParser.Schema.AnalogChannels[i].Units == "V" || mParser.Schema.AnalogChannels[i].Units == "A")
                    {
                        this.chart1.Series.Add(new Series()
                        {
                            Legend = "A",
                            LegendText = mParser.Schema.AnalogChannels[i].Name + "(" + mParser.Schema.AnalogChannels[i].Units + ")",
                            BorderWidth = 2,
                            ChartType = SeriesChartType.Line
                        });
                    }
                    else
                    {
                        this.chart1.Series.Add(new Series()
                        {
                            Legend = "A",
                            LegendText = mParser.Schema.AnalogChannels[i].Name,
                            BorderWidth = 2,
                            ChartType = SeriesChartType.Line
                        });
                    }
                    this.chart1.Series[i].ChartArea = "A";
                }
                else
                {
                    this.chart1.Series.Add(new Series()
                    {
                        Legend = "D",
                        LegendText = mParser.Schema.DigitalChannels[i - mParser.Schema.TotalAnalogChannels].Name,
                        BorderWidth = 3,
                        ChartType = SeriesChartType.Line
                    });
                    this.chart1.Series[i].ChartArea = "D";
                }
            }


            var data = new List<CData>();
            try
            {
                for (int ii = 0; ii < mParser.Schema.TotalAnalogChannels; ii++)
                {
                    for (int i = 1; i < mParser.Schema.SampleRates[0].EndSample; i++)
                    {
                        chart1.Series[ii].Points.AddXY(mDatData[i][1], mDatData[i][ii + 2]);
                        if (ii < mParser.Schema.TotalAnalogChannels)
                        {
                            if (Y_MinValue >= mDatData[i][ii + 2])
                                Y_MinValue = mDatData[i][ii + 2];
                            if (Y_MaxValue <= mDatData[i][ii + 2])
                                Y_MaxValue = mDatData[i][ii + 2];
                        }
                    }
                }
                for (int ii = 0; ii < mParser.Schema.TotalDigitalChannels; ii++)
                {
                    for (int i = 1; i < mParser.Schema.SampleRates[0].EndSample; i++)
                    {
                        chart1.Series[ii + mParser.Schema.TotalAnalogChannels].Points.AddXY(mDatData[i][1], mDatData[i][ii + mParser.Schema.TotalAnalogChannels + 2]);
                    }
                }
            }
            catch (Exception ex1)
            {

            }
            try
            {
                int index = 0;
                for (int i = 0; i < mParser.Schema.TotalChannels; i++)
                {
                    if (mParser.Schema.AnalogChannels[i].Units != "")
                    {
                        if (mParser.Schema.AnalogChannels[i].Units == "V" || mParser.Schema.AnalogChannels[i].Units == "A")
                        {
                            this.chart1.Series.Add(new Series()
                            {
                                Legend = "A",
                                LegendText = mParser.Schema.AnalogChannels[i].Name + "_FFT(" + mParser.Schema.AnalogChannels[i].Units + ")",
                                BorderWidth = 2,
                                ChartType = SeriesChartType.Line
                            });
                        }
                        else
                        {
                            this.chart1.Series.Add(new Series()
                            {
                                Legend = "A",
                                LegendText = mParser.Schema.AnalogChannels[i].Name + "_FFT",
                                BorderWidth = 2,
                                ChartType = SeriesChartType.Line
                            });
                        }
                        this.chart1.Series[mParser.Schema.TotalChannels + i].ChartArea = "A";
                        for (int ii = 0; ii < mParser.Schema.SampleRates[0].EndSample; ii++)
                        {
                            chart1.Series[mParser.Schema.TotalChannels + index].Points.AddXY(mDatData[ii][1], mFFTData.arrFFTData[ii].Value[index]);
                        }
                        index++;
                    }
                }
            }
            catch (Exception ex1)
            {

            }
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
            this.chart1.ChartAreas[0].AxisY.Maximum = RE_Y_MaxValue;
            this.chart1.ChartAreas[0].AxisY.Minimum = RE_Y_MinValue;

            this.chart1.ChartAreas[0].AxisY.Interval = (this.chart1.ChartAreas[0].AxisY.Maximum - this.chart1.ChartAreas[0].AxisY.Minimum) / 6;

            this.chart1.ChartAreas[0].AxisX.Maximum = Bar_range;
            this.chart1.ChartAreas[0].AxisX.Minimum = 0;
            this.chart1.ChartAreas[1].AxisX.Maximum = Bar_range;
            this.chart1.ChartAreas[1].AxisX.Minimum = 0;

            AddAnnotations();

            this.hScrollBar1.Maximum = Convert.ToInt32(mDatData[mDatData.Count - 1][1]);
            this.hScrollBar1.Minimum = 0;
            this.hScrollBar1.LargeChange = Bar_range;
            this.hScrollBar1.Value = 0;
            this.chart1.MouseWheel += new MouseEventHandler(chart1_MouseWheel);
            this.chart1.ChartAreas[0].CursorX.IsUserEnabled = false;

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
            this.chart1.ChartAreas[0].AxisX.Interval = Bar_range / 6;
            this.chart1.ChartAreas[0].AxisX.LogarithmBase = 1000;

            double startTime_S = Convert.ToDouble(0);
            double triggerTime_S = TimeSpan.FromTicks(mParser.Schema.TriggerTime.Value - mParser.Schema.StartTime.Value).TotalMilliseconds;

            this.chart1.Annotations.Add(new VerticalLineAnnotation()
            {
                LineColor = Color.Red,
                X = (triggerTime_S - startTime_S),
                IsInfinitive = true,
                AxisX = this.chart1.ChartAreas[0].AxisX,
                LineWidth = 2
            });

            this.chart1.Annotations.Add(new TextAnnotation()
            {
                ForeColor = Color.Red,
                AxisX = this.chart1.ChartAreas[0].AxisX,
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
                AxisX = this.chart1.ChartAreas[0].AxisX,
                LineWidth = 2
            });

            this.chart1.Annotations.Add(new TextAnnotation()
            {
                Name = "MoveTextX",
                ForeColor = Color.White,
                AxisX = this.chart1.ChartAreas[0].AxisX,
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
                AxisY = this.chart1.ChartAreas[0].AxisY,
                LineWidth = 2,
                Y = 5
            });
            this.chart1.Annotations.Add(new TextAnnotation()
            {
                Name = "MoveTextY",
                ForeColor = Color.White,
                AnchorX = 2,
                LineWidth = 0,
                AxisY = this.chart1.ChartAreas[0].AxisY,
                Y = 5,
                Text = "Time ms",
                Font = new Font(Font.Name, 10, FontStyle.Bold),
            });
            this.chart1.Annotations["MoveLineX"].Visible = false;
            this.chart1.Annotations["MoveTextX"].Visible = false;
            this.chart1.Annotations["MoveLineY"].Visible = false;
            this.chart1.Annotations["MoveTextY"].Visible = false;
            chart1.AnnotationPositionChanged += chart1_AnnotationPositionChanged;
        }
        /*------------------------ChartFuntion------------------------*/
        #region ChartFuntion
        public void Chart1_Enable(int index, Panel panel)
        {
            this.chart1.Series[index].Enabled = !this.chart1.Series[index].Enabled;
            Chart_AnnotationsLineEnable(this.chart1);
            AllButtonEnable();

            bpnlAEnable = false;
            foreach (var item in panel.Controls)
            {
                if (item is Button)
                {
                    if (((Button)item).BackColor == Color.LightSlateGray)
                        bpnlAEnable = true;
                }
            }
        }

        public void Chart2_Enable(int index, Panel panel)
        {
            this.chart1.Series[index + mParser.Schema.TotalAnalogChannels].Enabled = !this.chart1.Series[index + mParser.Schema.TotalAnalogChannels].Enabled;

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
                    this.chart1.Series[chart1.Series.Count - 1].Enabled = true;
                    return;
                }
            }
            bButtonEnable = false;
            this.chart1.Annotations["MoveLineX"].Visible = false;
            this.chart1.Annotations["MoveTextX"].Visible = false;
            this.chart1.Annotations["MoveLineY"].Visible = false;
            this.chart1.Annotations["MoveTextY"].Visible = false;
            this.chart1.Series[chart1.Series.Count - 1].Enabled = false;
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
                _Chart.Annotations[0].Visible = false;
                _Chart.Annotations[1].Visible = false;
            }
            else
            {
                _Chart.Annotations[0].Visible = true;
                _Chart.Annotations[1].Visible = true;
            }
        }
        private void Chart_Enable()
        {
            bool bEnabl = false;
            foreach (var index in chart1.Series)
            {
                if (index.Enabled && index.Legend == "D")
                    bEnabl = true;
            }
            if (!bEnabl)
            {
                this.chart1.ChartAreas[1].Visible = false;
                this.chart1.ChartAreas[0].Position = new ElementPosition(0, 10, 100, 88);
            }
            else
            {
                this.chart1.ChartAreas[1].Visible = true;
                this.chart1.ChartAreas[0].Position = new ElementPosition(0, 10, 100, 68);
            }
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

                this._endY = this.chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);

                this.chart1.ChartAreas[0].AxisY.Minimum += this._endY > this._startY ? -Y_MaxValue / 10 : Y_MaxValue / 10;
                this.chart1.ChartAreas[0].AxisY.Maximum += this._endY > this._startY ? -Y_MaxValue / 10 : Y_MaxValue / 10;
            }
            else
            {
                if (e.Y <= 0 || e.Y >= this.chart1.Height) return;
                if (e.X <= 0 || e.X >= this.chart1.Width) return;
            }

            Task.Run(() =>
            {
                MoveLine_X = this.chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
                MoveLine_Y = this.chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);
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

                        if (!bpnlAEnable)
                        {
                            this.chart1.Annotations["MoveLineY"].Visible = false;
                            this.chart1.Annotations["MoveTextY"].Visible = false;
                        }
                        else
                        {
                            this.chart1.Annotations["MoveLineY"].Visible = true;
                            this.chart1.Annotations["MoveTextY"].Visible = true;
                        }

                        this.chart1.Annotations["MoveLineX"].EndPlacement();
                        this.chart1.Annotations["MoveLineY"].EndPlacement();
                    }
                }));
            if (bZoonIn)
            {
                this.chart1.Annotations["ZoonInLineX"].Width = this.chart1.ChartAreas[0].AxisX.ValueToPosition(MoveLine_X) - dZoonInStratPixs_X;
                this.chart1.Annotations["ZoonInLineY"].Height = this.chart1.ChartAreas[0].AxisY.ValueToPosition(MoveLine_Y) - dZoonInStratPixs_Y;
                bMoveCheck = true;
            }
        }
        //private void Mouse_MoveIn(object sender, EventArgs e)
        //{
        //    bMoveView = false;
        //    this.Cursor = System.Windows.Forms.Cursors.Cross;
        //}
        //private void Mouse_MoveOut(object sender, EventArgs e)
        //{
        //    this.Cursor = System.Windows.Forms.Cursors.Default;
        //    this.chart1.Annotations["MoveLineX"].Visible = false;
        //    this.chart1.Annotations["MoveTextX"].Visible = false;

        //    this.chart1.Annotations["MoveLineY"].Visible = false;
        //    this.chart1.Annotations["MoveTextY"].Visible = false;
        //}
        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            this._startY = this.chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);

            if (e.Button == MouseButtons.Left)
            {
                if (!bMoveView && bButtonEnable)
                {
                    dZoonInStratPixs_X = this.chart1.ChartAreas[0].AxisX.ValueToPosition(this.chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X));
                    dZoonInStratPixs_Y = this.chart1.ChartAreas[0].AxisY.ValueToPosition(this.chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y));
                    dZoonInStratPoint_X = this.chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
                    dZoonInStratPoint_Y = this.chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);
                    chart1.Annotations.Add(new LineAnnotation()
                    {
                        LineColor = Color.Black,
                        AxisX = this.chart1.ChartAreas[0].AxisX,
                        AxisY = this.chart1.ChartAreas[0].AxisY,
                        AnchorX = this.chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X),
                        AnchorY = this.chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y),
                        Width = 0,
                        Height = 0,
                        LineWidth = 3,
                        LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash,
                        Name = "ZoonInLineX",
                        AllowMoving = true
                    });
                    chart1.Annotations.Add(new LineAnnotation()
                    {
                        LineColor = Color.Black,
                        AxisX = this.chart1.ChartAreas[0].AxisX,
                        AxisY = this.chart1.ChartAreas[0].AxisY,
                        AnchorX = this.chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X),
                        AnchorY = this.chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y),
                        Width = 0,
                        Height = 0,
                        LineWidth = 3,
                        LineDashStyle = ChartDashStyle.Dash,
                        Name = "ZoonInLineY",
                        AllowMoving = true
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
            if (bZoonIn)
            {
                dZoonInEndPoint_X = this.chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X) < bar_OriginalRange ? this.chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X) : bar_OriginalRange;
                dZoonInEndPoint_Y = this.chart1.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);

                if (bMoveCheck && !bLineMove)
                {
                    bMoveCheck = false;
                    Bar_range = Convert.ToInt32(Math.Round((Math.Abs(dZoonInStratPoint_X - dZoonInEndPoint_X)), 1));
                    this.hScrollBar1.LargeChange = Bar_range;
                    if (Math.Abs(dZoonInStratPoint_X - dZoonInEndPoint_X) >= 6)
                    {
                        if (dZoonInStratPoint_X < dZoonInEndPoint_X)
                        {
                            chart1.ChartAreas[0].AxisX.Maximum = dZoonInStratPoint_X;
                            chart1.ChartAreas[0].AxisX.Minimum = dZoonInEndPoint_X;

                            this.hScrollBar1.Value = Convert.ToInt32(dZoonInStratPoint_X);
                        }
                        else
                        {
                            chart1.ChartAreas[0].AxisX.Minimum = dZoonInStratPoint_X > chart1.ChartAreas[0].AxisX.Minimum ? dZoonInStratPoint_X : chart1.ChartAreas[0].AxisX.Minimum;
                            chart1.ChartAreas[0].AxisX.Maximum = dZoonInEndPoint_X > chart1.ChartAreas[0].AxisX.Maximum ? dZoonInEndPoint_X : chart1.ChartAreas[0].AxisX.Maximum;

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
                        this.chart1.ChartAreas[0].AxisX.Interval = Bar_range / 6;
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
            if (e.Delta > 0)
            {
                this.chart1.ChartAreas[0].AxisY.Maximum += this.chart1.ChartAreas[0].AxisY.Interval;
                this.chart1.ChartAreas[0].AxisY.Minimum += this.chart1.ChartAreas[0].AxisY.Interval;
            }
            else if (e.Delta < 0)
            {
                this.chart1.ChartAreas[0].AxisY.Maximum -= this.chart1.ChartAreas[0].AxisY.Interval;
                this.chart1.ChartAreas[0].AxisY.Minimum -= this.chart1.ChartAreas[0].AxisY.Interval;
            }
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
                        AxisX = this.chart1.ChartAreas[0].AxisX,
                        X = this.chart1.ChartAreas[0].AxisX.PixelPositionToValue(e.X),
                        IsInfinitive = true,
                        LineWidth = 2,
                        LineDashStyle = ChartDashStyle.Dash,
                        Name = "BlockLine" + blockLine,
                        //AllowMoving = true
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
            chart1.ChartAreas[0].AxisX.Minimum = hScrollBar1.Value;
            chart1.ChartAreas[0].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Minimum + Bar_range;
            chart1.ChartAreas[1].AxisX.Minimum = hScrollBar1.Value;
            chart1.ChartAreas[1].AxisX.Maximum = chart1.ChartAreas[1].AxisX.Minimum + Bar_range;
        }
        public void chart1_XAxisAdd()
        {
            Bar_range /= 2;
            if (Bar_range >= 6)
            {
                this.hScrollBar1.LargeChange = Bar_range;
                chart1.ChartAreas[0].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Minimum + Bar_range;
                this.chart1.ChartAreas[0].AxisX.Interval = Bar_range / 6;
                chart1.ChartAreas[1].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Minimum + Bar_range;
                this.chart1.ChartAreas[1].AxisX.Interval = Bar_range / 6;
            }
            else
                Bar_range *= 2;
        }
        public void char1_XAxisLess()
        {
            Bar_range *= 2;
            if ((chart1.ChartAreas[0].AxisX.Minimum + Bar_range) < bar_OriginalRange)
            {
                this.hScrollBar1.LargeChange = Bar_range;
                chart1.ChartAreas[0].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Minimum + Bar_range;
                this.chart1.ChartAreas[0].AxisX.Interval = Bar_range / 6;
                chart1.ChartAreas[1].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Minimum + Bar_range;
                this.chart1.ChartAreas[1].AxisX.Interval = Bar_range / 6;
            }
            else
            {
                Bar_range = bar_OriginalRange;
                this.hScrollBar1.LargeChange = Bar_range;
                chart1.ChartAreas[0].AxisX.Maximum = bar_OriginalRange;
                this.chart1.ChartAreas[0].AxisX.Interval = Bar_range / 6;
                chart1.ChartAreas[1].AxisX.Maximum = bar_OriginalRange;
                this.chart1.ChartAreas[1].AxisX.Interval = Bar_range / 6;
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
                    douMax = chart1.ChartAreas[0].AxisY.Maximum * 2;
                    douMin = chart1.ChartAreas[0].AxisY.Minimum * 2;
                    break;
                case 2:
                    douMax = chart1.ChartAreas[0].AxisY.Maximum / 2;
                    douMin = chart1.ChartAreas[0].AxisY.Minimum / 2;
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
                chart1.ChartAreas[0].AxisY.Maximum = douMax;
                chart1.ChartAreas[0].AxisY.Minimum = douMin;
                chart1.ChartAreas[2].AxisY.Maximum = douMax;
                chart1.ChartAreas[2].AxisY.Minimum = douMin;
                this.chart1.ChartAreas[0].AxisY.Interval = (douMax - douMin) / 6;
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
                chart1.ChartAreas[0].AxisY.Maximum = douMax;
                chart1.ChartAreas[0].AxisY.Minimum = douMin;

                chart1.ChartAreas[2].AxisY.Maximum = douMax;
                chart1.ChartAreas[2].AxisY.Minimum = douMin;
                this.chart1.ChartAreas[0].AxisY.Interval = (douMax - douMin) / 6;
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
                    AnchorY = 8,
                    LineWidth = 0,
                    Text = "Location：" + mParser.Schema.StationName + "  StartDate：" + mParser.Schema.StartTime.Value.ToString("yyyy/MM/dd") + "   StartTime：" + mParser.Schema.StartTime.Value.ToString("HH:mm:ss.fff") + "\n\n Device：" + mParser.Schema.DeviceID + "   TriggerDate：" + mParser.Schema.TriggerTime.Value.ToString("yyyy/MM/dd") + "   TriggerTime：" + mParser.Schema.TriggerTime.Value.ToString("HH:mm:ss.fff"),
                    Name = "Information",
                    ForeColor=Color.White
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
                    AxisX = this.chart1.ChartAreas[0].AxisX,
                    LineWidth = 0,
                    AnchorY = 11,
                    X = Convert.ToDouble(Math.Round(arr_XValue[i + 1] + arr_XValue[i]) / 2),
                    Text = "dt=" + (Math.Round(arr_XValue[i + 1] - arr_XValue[i])).ToString() + " ms"
                });
                BlockGroup_Block.Add(chart1.Annotations["BlockText" + i]);
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
            this.chart1.ChartAreas[0].AxisX.Maximum = Bar_range;
            this.chart1.ChartAreas[0].AxisX.Minimum = 0;
            this.chart1.ChartAreas[1].AxisX.Maximum = Bar_range;
            this.chart1.ChartAreas[1].AxisX.Minimum = 0;
            this.chart1.ChartAreas[0].AxisX.Interval = Bar_range / 6;
            this.chart1.ChartAreas[1].AxisX.Interval = Bar_range / 6;

            this.chart1.ChartAreas[0].AxisY.Maximum = Math.Round(RE_Y_MaxValue * 1.1f);
            this.chart1.ChartAreas[0].AxisY.Minimum = Math.Round(RE_Y_MinValue * 1.1f);
            this.chart1.ChartAreas[0].AxisY.Interval = (RE_Y_MaxValue - RE_Y_MinValue) / 6;
        }
        public void Form_SizeChanged(int w, int h)
        {
            this.Size = new Size(w, h);
        }


        private void chart1_AnnotationPositionChanged_1(object sender, EventArgs e)
        {
          
        }
    }
}
