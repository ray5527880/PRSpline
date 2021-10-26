using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

using System.Runtime.InteropServices;
using System.Numerics;
using BF_FW.data;
using BF_FW;
using GSF.COMTRADE;

namespace PRSpline
{
    public partial class frmMain : Form
    {

        public static bool IsOpenFlies=false;
        public enum SelectFile
        {
            File_1 = 1,
            File_2 = 2,
            File_3 = 3,
            File_4 = 4,
            File_5 = 5,
            File_6 = 6
        };
        public PRData PRData_1;

        public ChartData ChartData_1;
        public ChartData ChartData_2;
        public ChartData ChartData_3;
        public ChartData ChartData_4;
        public ChartData ChartData_5;
        
        private DateTime StartDateTime;

        //private List<string> ButtonName_1 = new List<string>();
        //private List<string> ButtonName_2 = new List<string>();
        //private List<string> ButtonName_3 = new List<string>();
        //private List<string> ButtonName_4 = new List<string>();
        //private List<string> ButtonName_5 = new List<string>();
        //private List<string> ButtonName_6 = new List<string>();
        private List<string> ButtonName_D = new List<string>();
        private static Boolean bfrmVector = true;
        private frmChart frmChartline;
        private CompressWinRAR mCompressWinRAR;

        //public string strFileName1 = "";
        //public string strFileName2 = "";
        //public string strFileName3 = "";
        //public string strFileName4 = "";
        //public string strFileName5 = "";
        //public string strFileName6 = "";

        //public Parser mParser_1;
        //public Parser mParser_2;
        //public Parser mParser_3;
        //public Parser mParser_4;
        //public Parser mParser_5;
        //public Parser mParser_6;

        private FFTData mFFTData;

        int _Mode = 0;

        private SelectFile selectFileValue;

        public static List<PSData> mPSData = new List<PSData>();

        //public List<double[]> PData_1;
        //public List<double[]> SData_1;
        //public List<double[]> PUData_1;

        //public List<double[]> PData_2;
        //public List<double[]> SData_2;
        //public List<double[]> PUData_2;

        //public List<double[]> PData_3;
        //public List<double[]> SData_3;
        //public List<double[]> PUData_3;

        //public List<double[]> PData_4;
        //public List<double[]> SData_4;
        //public List<double[]> PUData_4;

        //public List<double[]> PData_5;
        //public List<double[]> SData_5;
        //public List<double[]> PUData_5;

        //public List<double[]> PData_6;
        //public List<double[]> SData_6;
        //public List<double[]> PUData_6;

        public int[] FFTIndex;

        public struct PSData
        {
            public decimal P;
            public decimal S;
            public decimal PerUnit;
        }

        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            EditXml mEditXml = new EditXml();
            mEditXml.GetXmlData();
            set_Form();
            set_ButtonImage();

            setEnable(false);

            btnSecond.Enabled = false;
            cbxitem.Enabled = false;

            mCompressWinRAR = new CompressWinRAR();

            cbxPS.Items.Add("P");
            cbxPS.Items.Add("S");
            cbxPS.Items.Add("Per Unit");

            ChartData_1 = new ChartData();
            ChartData_2= new ChartData();
            ChartData_3 = new ChartData();
            ChartData_4 = new ChartData();
            ChartData_5 = new ChartData();
            //ChartData_1 = new ChartData();

            Clear_Information();
        }

        private void set_Form()
        {
            this.groupBox4.Paint += groupBox_Paint;
        }
        private void set_ButtonImage()
        {
            //this.MaximizeBox
            ImageList Iimage = new ImageList();
            string strpath = "./res/";
            Iimage.Images.Add(Image.FromFile(strpath + "download.png"));

            btnXZoomIn.Image = Image.FromFile(strpath + "XZoonIn.png");
            btnXZoomOut.Image = Image.FromFile(strpath + "XZoonOut.png"); ;
            btnYZoomIn.Image = Image.FromFile(strpath + "YZoonIn.png");
            btnYZoomOut.Image = Image.FromFile(strpath + "YZoonOut.png");
            btnReZoom.Image = Image.FromFile(strpath + "Zoon.png");
            btnScreenshot.Image = Image.FromFile(strpath + "Screenshot.png");
            btnRemove.Image = Image.FromFile(strpath + "Remove.png");
            btnSecond.Image = Image.FromFile(strpath + "Vector.png");

            btnDownloading.Image = Image.FromFile(strpath + "download1.png");
            btnFileOpen.Image = Image.FromFile(strpath + "openfile.png");
            btnSetup.Image = Image.FromFile(strpath + "setup_SP.png");
            btnVS.Image = Image.FromFile(strpath + "VS.png");
            btnExtremum.Image = Image.FromFile(strpath + "Ex.png");
        }
        /// <summary>
        /// set groupBox line and string color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void groupBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox gBox = (GroupBox)sender;

            e.Graphics.Clear(gBox.BackColor);
            e.Graphics.DrawString(gBox.Text, gBox.Font, Brushes.White, 10, 1);
            var vSize = e.Graphics.MeasureString(gBox.Text, gBox.Font);
            e.Graphics.DrawLine(Pens.White, 1, vSize.Height / 2, 8, vSize.Height / 2);
            e.Graphics.DrawLine(Pens.White, vSize.Width + 8, vSize.Height / 2, gBox.Width - 2, vSize.Height / 2);
            e.Graphics.DrawLine(Pens.White, 1, vSize.Height / 2, 1, gBox.Height - 2);
            e.Graphics.DrawLine(Pens.White, 1, gBox.Height - 2, gBox.Width - 2, gBox.Height - 2);
            e.Graphics.DrawLine(Pens.White, gBox.Width - 2, vSize.Height / 2, gBox.Width - 2, gBox.Height - 2);
        }
        private void ClearButton()
        {
            pnlAnagol.Controls.Clear();
            pnlDigital.Controls.Clear();
        }

        private void AddNewButton(string name, int index, int type)
        {
            Button NewButton = new Button();
            NewButton.Name = name;
            NewButton.Text = name;
            switch (type)
            {
                case 0:
                    pnlAnagol.Controls.Add(NewButton);
                    NewButton.Click += new EventHandler(NewButton_A_Click);
                    break;
                case 1:
                    pnlDigital.Controls.Add(NewButton);
                    NewButton.Click += new EventHandler(NewButton_D_Click);
                    break;
            }

            NewButton.Location = new Point(0, 24 * index);
            NewButton.Size = new System.Drawing.Size(90, 25);
            NewButton.BackColor = Color.LightSteelBlue;
        }
        private void NewButton_A_Click(object sender, EventArgs e)
        {
            int baseValue = ButtonName_D.Count;
            if (selectFileValue == SelectFile.File_2) baseValue = ChartData_1.ButtonName.Count + ButtonName_D.Count;
            if (selectFileValue == SelectFile.File_3) baseValue = ChartData_1.ButtonName.Count + ButtonName_D.Count + ChartData_2.ButtonName.Count ;
            if (selectFileValue == SelectFile.File_4) baseValue = ChartData_1.ButtonName.Count + ButtonName_D.Count + ChartData_2.ButtonName.Count + ChartData_3.ButtonName.Count ;
            if (selectFileValue == SelectFile.File_5) baseValue = ChartData_1.ButtonName.Count + ButtonName_D.Count + ChartData_2.ButtonName.Count + ChartData_3.ButtonName.Count + ChartData_4.ButtonName.Count;

            if (((Button)sender).BackColor == Color.LightSteelBlue)
            {
                ((Button)sender).BackColor = Color.LightSlateGray;
            }
            else if (((Button)sender).BackColor == Color.LightSlateGray)
            {
                ((Button)sender).BackColor = Color.LightSteelBlue;
            }
            if (((Button)sender).Text.IndexOf("FFT") > 0 && selectFileValue == SelectFile.File_1)
            {
                frmChartline.Chart1_Enable(((Button)sender).TabIndex + ChartData_1.mParser.Schema.DigitalChannels.Length,/* pnlAnagol,*/ ((Button)sender).Text);
            }
            else
                frmChartline.Chart1_Enable(((Button)sender).TabIndex + baseValue,/* pnlAnagol,*/ ((Button)sender).Text);
        }
        private void NewButton_D_Click(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == Color.LightSteelBlue)
            {
                ((Button)sender).BackColor = Color.LightSlateGray;
            }
            else if (((Button)sender).BackColor == Color.LightSlateGray)
            {
                ((Button)sender).BackColor = Color.LightSteelBlue;
            }

            frmChartline.Chart2_Enable(((Button)sender).TabIndex, /*pnlDigital,*/ ((Button)sender).Text);
        }


        private void Clear_Information()
        {
            labLocatiion.Text = string.Empty;
            labDevice.Text = string.Empty;
            labStartTime.Text = string.Empty;
            labTriggerTime.Text = string.Empty;
            labStartDate.Text = string.Empty;
            labTriggerDate.Text = string.Empty;
        }
        private void Set_Information()
        {
            Clear_Information();
            labLocatiion.Text = ChartData_1.mParser.Schema.StationName;
            labDevice.Text = ChartData_1.mParser.Schema.DeviceID;
            labStartDate.Text = ChartData_1.mParser.Schema.StartTime.Value.ToString("yyyy/MM/dd");
            labTriggerDate.Text = ChartData_1.mParser.Schema.TriggerTime.Value.ToString("yyyy/MM/dd");
            labStartTime.Text = ChartData_1.mParser.Schema.StartTime.Value.ToString("HH:mm:ss.fff");
            labTriggerTime.Text = ChartData_1.mParser.Schema.TriggerTime.Value.ToString("HH:mm:ss.fff");
        }

        public static void btnVectorClick()
        {
            bfrmVector = !bfrmVector;
        }

        #region FormEvent
        private void button1_Click(object sender, EventArgs e)
        {
            if (panel1.Controls.Count > 0)
                frmChartline.chart1_XAxisAdd();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (panel1.Controls.Count > 0)
                frmChartline.char1_XAxisLess();
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Directory.Exists("./downloadFile/CompressFile"))
            {
                Directory.Delete("./downloadFile/CompressFile", true);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            frmChartline.save_chart1();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmChartline.chart_YAxisLess();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmChartline.chart_YAxisAdd();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmChartline.ReZoom();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            frmChartline.ClearBlock();
        }
        private async void cbxPS_SelectedIndexChanged(object sender, EventArgs e)
        {
            await PSClick(cbxPS.SelectedItem.ToString());
        }
        private async void btnFileOpen_Click(object sender, EventArgs e)
        {
            IsOpenFlies = false;
            var frm = new frmSelectMorR(ref this.openFileDialog1);
            frm.ShowDialog();
            if (!IsOpenFlies) return;
            //if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
            //{
            //    (sender as Button).Enabled = true;
            //    return;
            //}
            EnableView(false);
            ChartData_1 = new ChartData();
            ChartData_2 = new ChartData();
            ChartData_3 = new ChartData();
            ChartData_4 = new ChartData();
            ChartData_5 = new ChartData();

            ChartData_1.strFileName = openFileDialog1.SafeFileName;
            
            try
            {
                if (!await LoadFile(SelectFile.File_1)) return;
                //if (!LoadFile(SelectFile.File_1)) return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("{0}: {1}", this.openFileDialog1.FileName, ex.Message));
            }

            StartDateTime = ChartData_1.mParser.Schema.StartTime.Value;
            setEnable(false);
            cbxPS.Items.Clear();
            Set_Information();
            ClearButton();

            setEnable(true);
            sizeChanged();
            for (int i = 0; i < ChartData_1.ButtonName.Count + ButtonName_D.Count; i++)
            {
                if (i < ChartData_1.mParser.Schema.TotalAnalogChannels)
                    AddNewButton(ChartData_1.ButtonName[i], i, 0);
                else if (i < ChartData_1.mParser.Schema.TotalChannels)
                    AddNewButton(ButtonName_D[i - ChartData_1.mParser.Schema.TotalAnalogChannels], i - ChartData_1.mParser.Schema.TotalAnalogChannels, 1);
                else
                    AddNewButton(ChartData_1.ButtonName[i - ChartData_1.mParser.Schema.TotalDigitalChannels], i - ChartData_1.mParser.Schema.TotalDigitalChannels, 0);
            }

            cbxPS.Items.Add("P");
            cbxPS.Items.Add("S");
            cbxPS.Items.Add("Per Unit");

            cbxPS.SelectedIndex = 1;

            btnSecond.Enabled = true;
            cbxitem.Enabled = false;
            EnableView(true);
        }
        private void btnDownloading_Click(object sender, EventArgs e)
        {
            EditXml mEditXml = new EditXml();
            mEditXml.GetXmlData();
            frmDownload frm = new frmDownload();
            frm.ShowDialog();
        }
        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            sizeChanged();
        }
        private void btnExtremum_Click(object sender, EventArgs e)
        {
            var data = new ExtremumData.Extremum[0];
            switch (_Mode)
            {
                case 1:
                    data = GetExtremum(ChartData_1.PData);
                    break;
                case 2:
                    data = GetExtremum(ChartData_1.SData);
                    break;
                case 3:
                    data = GetExtremum(ChartData_1.PUData);
                    break;
            }
            var frm = new frmExtremum(data);
            frm.ShowDialog();

        }
        private void cbxitem_SelectedIndexChanged(object sender, EventArgs e)
        {
            int baseValue = ButtonName_D.Count;

            pnlAnagol.Controls.Clear();

            int _index = 0;

            switch (cbxitem.SelectedIndex)
            {
                case 0:
                    selectFileValue = SelectFile.File_1;
                    for (int i = 0; i < ChartData_1.ButtonName.Count; i++)
                    {
                        AddNewButton(ChartData_1.ButtonName[i], i, 0);
                    }
                    if (!(ChartData_2.ButtonName.Count() > 0))
                    {
                        for (int i = 0; i < ButtonName_D.Count; i++)
                        {
                            AddNewButton(ButtonName_D[i], i, 1);
                        }
                    }
                    if (frmChartline != null)
                    {
                        _index = ButtonName_D.Count() + 1;
                        foreach (var itme in pnlAnagol.Controls)
                        {                           
                            if (frmChartline.IsSeries(_index))
                                ((Button)itme).BackColor = Color.LightSlateGray;
                            _index++;
                        }
                    }

                    break;
                case 1:
                    for (int i = 0; i < ChartData_2.ButtonName.Count; i++)
                    {
                        AddNewButton(ChartData_2.ButtonName[i], i, 0);
                    }
                    selectFileValue = SelectFile.File_2;
                    baseValue = ChartData_1.ButtonName.Count + ButtonName_D.Count+1;
                    if (frmChartline != null)
                    {
                        _index = baseValue ;
                        foreach (var itme in pnlAnagol.Controls)
                        {
                            if (frmChartline.IsSeries(_index))
                                ((Button)itme).BackColor = Color.LightSlateGray;
                            _index++;
                        }
                    }
                    break;
                case 2:
                    for (int i = 0; i < ChartData_3.ButtonName.Count; i++)
                    {
                        AddNewButton(ChartData_3.ButtonName[i], i, 0);
                    }
                    selectFileValue = SelectFile.File_3;
                    baseValue = ChartData_1.ButtonName.Count + ButtonName_D.Count + ChartData_2.ButtonName.Count + 1;
                    if (frmChartline != null)
                    {
                        _index = baseValue;
                        foreach (var itme in pnlAnagol.Controls)
                        {
                            if (frmChartline.IsSeries(_index))
                                ((Button)itme).BackColor = Color.LightSlateGray;
                            _index++;
                        }
                    }
                    break;
                case 3:
                    for (int i = 0; i < ChartData_4.ButtonName.Count; i++)
                    {
                        AddNewButton(ChartData_4.ButtonName[i], i, 0);
                    }
                    selectFileValue = SelectFile.File_4;
                    baseValue = ChartData_1.ButtonName.Count + ButtonName_D.Count + ChartData_2.ButtonName.Count + ChartData_3.ButtonName.Count + 1;
                    if (frmChartline != null)
                    {
                        _index = baseValue;
                        foreach (var itme in pnlAnagol.Controls)
                        {
                            if (frmChartline.IsSeries(_index))
                                ((Button)itme).BackColor = Color.LightSlateGray;
                            _index++;
                        }
                    }
                    break;
                case 4:
                    for (int i = 0; i < ChartData_5.ButtonName.Count; i++)
                    {
                        AddNewButton(ChartData_5.ButtonName[i], i, 0);
                    }
                    selectFileValue = SelectFile.File_5;
                    baseValue = ChartData_1.ButtonName.Count + ButtonName_D.Count + ChartData_2.ButtonName.Count + ChartData_3.ButtonName.Count + ChartData_4.ButtonName.Count + 1;
                    if (frmChartline != null)
                    {
                        _index = baseValue;
                        foreach (var itme in pnlAnagol.Controls)
                        {
                            if (frmChartline.IsSeries(_index))
                                ((Button)itme).BackColor = Color.LightSlateGray;
                            _index++;
                        }
                    }
                    break;
            }
        }
        #endregion


        #region Private Function
        private async Task PSClick(string _strPS)
        {
            _Mode = 0;
            if (_strPS == "P")
            {
                _Mode = 1;
            }
            else if (_strPS == "S")
            {
                _Mode = 2;
            }
            else if (_strPS == "Per Unit")
            {
                _Mode = 3;
            }
            try
            {
                EnableView(false);
                BeginInvoke(new Action(() =>
                {
                    panel1.Controls.Clear();
                    //panel1.Controls.
                    switch (_Mode)
                    {
                        case 1:

                            frmChartline = new frmChart(ChartData_1.mParser, ChartData_1.PData);
                            break;
                        case 2:
                            frmChartline = new frmChart(ChartData_1.mParser, ChartData_1.SData);
                            break;
                        case 3:
                            frmChartline = new frmChart(ChartData_1.mParser, ChartData_1.PUData);
                            break;
                    }
                    panel1.Controls.Add(frmChartline);

                    AddSecondFile(ChartData_2, _Mode, 2);
                    AddSecondFile(ChartData_3, _Mode, 3);
                    AddSecondFile(ChartData_4, _Mode, 4);
                    AddSecondFile(ChartData_5, _Mode, 5);
                    //switch (_Mode)
                    //{
                    //    case 1:
                    //        if (ButtonName_2.Count() > 0)
                    //            frmChartline.AddSecondFile(PData_2, 2, mParser_2);
                    //        if (ButtonName_3.Count() > 0)
                    //            frmChartline.AddSecondFile(PData_3, 3, mParser_3);
                    //        if (ButtonName_4.Count() > 0)
                    //            frmChartline.AddSecondFile(PData_4, 4, mParser_4);
                    //        if (ButtonName_5.Count() > 0)
                    //            frmChartline.AddSecondFile(PData_5, 5, mParser_5);

                    //        break;
                    //    case 2:
                    //        if (ButtonName_2.Count() > 0)
                    //            frmChartline.AddSecondFile(SData_2, 2, mParser_2);
                    //        if (ButtonName_3.Count() > 0)
                    //            frmChartline.AddSecondFile(SData_3, 3, mParser_3);
                    //        if (ButtonName_4.Count() > 0)
                    //            frmChartline.AddSecondFile(SData_4, 4, mParser_4);
                    //        if (ButtonName_5.Count() > 0)
                    //            frmChartline.AddSecondFile(SData_5, 5, mParser_5);
                    //        break;

                    //    case 3:
                    //        if (ButtonName_2.Count() > 0)
                    //            frmChartline.AddSecondFile(PUData_2, 2, mParser_2);
                    //        if (ButtonName_3.Count() > 0)
                    //            frmChartline.AddSecondFile(PUData_3, 3, mParser_3);
                    //        if (ButtonName_4.Count() > 0)
                    //            frmChartline.AddSecondFile(PUData_4, 4, mParser_4);
                    //        if (ButtonName_5.Count() > 0)
                    //            frmChartline.AddSecondFile(PUData_5, 5, mParser_5);
                    //        break;
                    //}
                    foreach (var item in pnlAnagol.Controls)
                    {
                        if (item is Button)
                        {
                            ((Button)item).BackColor = Color.LightSteelBlue;
                        }
                    }
                    foreach (var item in pnlDigital.Controls)
                    {
                        if (item is Button)
                        {
                            ((Button)item).BackColor = Color.LightSteelBlue;
                        }
                    }

                    sizeChanged();
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                EnableView(true);
            }
        }

        private void AddSecondFile(ChartData _chartData,int _Mode,int _No)
        {
            switch (_Mode)
            {
                case 1:
                    if (_chartData.ButtonName.Count() > 0)
                        frmChartline.AddSecondFile(_chartData.PData, _No, _chartData.mParser);
                    break;
                case 2:
                    if (_chartData.ButtonName.Count() > 0)
                        frmChartline.AddSecondFile(_chartData.SData, _No, _chartData.mParser);
                    break;
                case 3:
                    if (_chartData.ButtonName.Count() > 0)
                        frmChartline.AddSecondFile(_chartData.PUData, _No, _chartData.mParser);
                    break;
            }            
        }

        private void setEnable(bool enable)
        {
            btnRemove.Enabled = enable;
            btnReZoom.Enabled = enable;
            btnScreenshot.Enabled = enable;
            btnXZoomIn.Enabled = enable;
            btnXZoomOut.Enabled = enable;
            btnYZoomIn.Enabled = enable;
            btnYZoomOut.Enabled = enable;
            btnExtremum.Enabled = enable;
            cbxPS.Enabled = enable;
        }

        //private async Task getAllData(List<double[]> datas, Parser parser, SelectFile selectFiles)
        //{
        //    ChartData_1.PData = await GetAllData(_PData, _mParser, selectFile);
        //}
        private async Task<List<double[]>>  /*List<double[]>*/ GetAllData(List<double[]> datas, Parser parser, SelectFile selectFiles)
        {
            List<int> _fftIndex = new List<int>();
            for (int i = 0; i < parser.Schema.TotalAnalogChannels; i++)
            {
                if (parser.Schema.AnalogChannels[i].Units == "V" || parser.Schema.AnalogChannels[i].Units == "A")
                    _fftIndex.Add(i);
            }
            //FFTIndex = _fftIndex.ToArray();

            var value = new List<double[]>();
            var _mFFTData = new FFTData();
            var mfft = new FFTCal(_fftIndex.ToArray(), parser);
            await Task.Run(() => 
            {
                try
                {
                    _mFFTData = mfft.GetFFTData(datas);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (selectFiles == SelectFile.File_1) mFFTData = _mFFTData;

                for (int i = 0; i < datas.Count; i++)
                {
                    var _double = new List<double>();
                    int index = 0;
                    foreach (var item in datas[i])
                    {
                        if (selectFiles != SelectFile.File_1)
                        {
                            if (index == parser.Schema.TotalAnalogChannels + 2)
                                break;
                        }
                        _double.Add(item);
                        index++;
                    }

                    foreach (var item in _mFFTData.arrFFTData[i].Value)
                    {
                        _double.Add(item);
                    }
                    value.Add(_double.ToArray());
                }
            });
            

            return value;
        }
        private void btnSetup_Click(object sender, EventArgs e)
        {
            EditXml mEditXml = new EditXml();
            mEditXml.GetXmlData();
            var frm = new frmSetup();
            frm.ShowDialog();
        }
        private void sizeChanged()
        {
            if (this.Width < 1481)
                this.Width = 1481;
            if (this.Height < 767)
                this.Height = 767;
            if (frmChartline != null)
            {
                this.panel1.Size = new System.Drawing.Size(this.Size.Width - 190, this.Size.Height - 150);
                frmChartline.Form_SizeChanged(this.panel1.Size.Width + 29, this.panel1.Size.Height - 30);
            }
            this.panel4.Height = this.Height - 51 - 113;
            this.panel2.Height = this.panel4.Height / 5 * 3 - 10;
            this.panel3.Location = new Point(3, this.panel2.Height + 40);
            this.panel3.Height = this.panel4.Height / 5 * 2 - 10;
            int TotleWidth = (this.groupBox4.Width - 20);

            label9.Location = new Point(10, label9.Location.Y);
            labLocatiion.Location = new Point(80, labLocatiion.Location.Y);
            label10.Location = new Point(10, label10.Location.Y);
            labDevice.Location = new Point(80, labDevice.Location.Y);

            label13.Location = new Point(10 + TotleWidth / 3, label13.Location.Y);
            labStartDate.Location = new Point(90 + TotleWidth / 3, labStartDate.Location.Y);
            label14.Location = new Point(10 + TotleWidth / 3, label14.Location.Y);
            labTriggerDate.Location = new Point(90 + TotleWidth / 3, labTriggerDate.Location.Y);

            label17.Location = new Point(10 + TotleWidth / 3 * 2, label17.Location.Y);
            labStartTime.Location = new Point(90 + TotleWidth / 3 * 2, labStartTime.Location.Y);
            label18.Location = new Point(10 + TotleWidth / 3 * 2, label18.Location.Y);
            labTriggerTime.Location = new Point(90 + TotleWidth / 3 * 2, labTriggerTime.Location.Y);
        }
        private ExtremumData.Extremum[] GetExtremum(List<double[]> Data)
        {
            var reData = new ExtremumData.Extremum[ChartData_1.mParser.Schema.TotalAnalogChannels + mFFTData.arrFFTData[0].Value.Length];
            for (int i = 0; i < ChartData_1.mParser.Schema.TotalAnalogChannels; i++)
            {
                reData[i].strName = ChartData_1.mParser.Schema.AnalogChannels[i].Name;
            }
            for (int i = 0; i < FFTIndex.Length; i++)
            {
                reData[i + ChartData_1.mParser.Schema.TotalAnalogChannels].strName = ChartData_1.mParser.Schema.AnalogChannels[FFTIndex[i]].Name + "_FFT";
            }

            var d = new ExtremumData.Extremum();

            for (int i = 0; i < ChartData_1.mParser.Schema.SampleRates[0].EndSample; i++)
            {
                if (i > ChartData_1.mParser.Schema.SampleRates[0].Rate / ChartData_1.mParser.Schema.NominalFrequency / 2 && i < ChartData_1.mParser.Schema.SampleRates[0].EndSample - ChartData_1.mParser.Schema.SampleRates[0].Rate / ChartData_1.mParser.Schema.NominalFrequency / 2)
                {
                    for (int j = 0; j < ChartData_1.mParser.Schema.TotalAnalogChannels; j++)
                    {
                        if (reData[j].MaxValue > Data[i][j + 2])
                        {
                            reData[j].MaxValue = Data[i][j + 2];
                            reData[j].MaxTime = Data[i][1];
                        }
                        else if (reData[j].MinValue < Data[i][j + 2])
                        {
                            reData[j].MinValue = Data[i][j + 2];
                            reData[j].MinTime = Data[i][1];
                        }
                    }
                    for (int j = 0; j < mFFTData.arrFFTData[i].Value.Length; j++)
                    {
                        if (reData[j + ChartData_1.mParser.Schema.TotalAnalogChannels].MaxValue < mFFTData.arrFFTData[i].Value[j])
                        {
                            reData[j + ChartData_1.mParser.Schema.TotalAnalogChannels].MaxValue = mFFTData.arrFFTData[i].Value[j];
                            reData[j + ChartData_1.mParser.Schema.TotalAnalogChannels].MaxTime = Data[i][1];
                        }
                    }
                }
            }

            for (int j = 0; j < mFFTData.arrFFTData[0].Value.Length; j++)
            {
                reData[j + ChartData_1.mParser.Schema.TotalAnalogChannels].MinValue = reData[j + ChartData_1.mParser.Schema.TotalAnalogChannels].MaxValue;
                reData[j + ChartData_1.mParser.Schema.TotalAnalogChannels].MinTime = reData[j + ChartData_1.mParser.Schema.TotalAnalogChannels].MaxTime;
            }

            for (int i = 0; i < ChartData_1.mParser.Schema.SampleRates[0].EndSample; i++)
            {
                if (i > ChartData_1.mParser.Schema.SampleRates[0].Rate / ChartData_1.mParser.Schema.NominalFrequency / 2 && i < ChartData_1.mParser.Schema.SampleRates[0].EndSample - ChartData_1.mParser.Schema.SampleRates[0].Rate / ChartData_1.mParser.Schema.NominalFrequency / 2)
                {
                    for (int j = 0; j < mFFTData.arrFFTData[0].Value.Length; j++)
                    {
                        if (reData[j + ChartData_1.mParser.Schema.TotalAnalogChannels].MinValue > mFFTData.arrFFTData[i].Value[j])
                        {
                            reData[j + ChartData_1.mParser.Schema.TotalAnalogChannels].MinValue = mFFTData.arrFFTData[i].Value[j];
                            reData[j + ChartData_1.mParser.Schema.TotalAnalogChannels].MinTime = Data[i][1];
                        }
                    }
                }
            }

            return reData.ToArray();
        }
        private void btnVS_Click(object sender, EventArgs e)
        {
            var frm = new frmVoltageSag();
            frm.ShowDialog();
        }
        private void btnSecond_Click(object sender, EventArgs e)
        {
            var _frmSecondSelect = new frmSecondSelect(this);
            _frmSecondSelect.ShowDialog();

        }
        public async Task< bool> OpenSeondFile(SelectFile _selectFile)
        {
            bool IsSuccess = false;
            if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            //switch (_selectFile)
            //{
            //    case SelectFile.File_2:
            //        //   PRData_1 = new PRData();
            //        break;
            //}
            //Task.Run(() =>
            //{
            EnableView(false);
            if (_selectFile == SelectFile.File_2)
            {
                if (await LoadFile(SelectFile.File_2)) IsSuccess = true;
            }
            else if (_selectFile == SelectFile.File_3)
            {
                if (await LoadFile(SelectFile.File_3)) IsSuccess = true;
            }
            else if (_selectFile == SelectFile.File_4)
            {
                if (await LoadFile(SelectFile.File_4)) IsSuccess = true;
            }
            else if (_selectFile == SelectFile.File_5)
            {
                if (await LoadFile(SelectFile.File_5)) IsSuccess = true;
            }
            else IsSuccess = false;
            if (IsSuccess)
            {
                cbxitem.Enabled = true;
                pnlDigital.Enabled = false;
            }
            EnableView(true);
            return IsSuccess;
            //});
        }


        private async Task<bool>/*bool*/ LoadFile(SelectFile selectFile)
        {
            bool IsSuccess = false;
            if (Directory.Exists("./CompressFile") && SelectFile.File_1 == selectFile)
            {
                Directory.Delete("./CompressFile", true);
            }

            string strtext = this.openFileDialog1.FileName;
            string strtext2 = string.Empty;

            while (strtext.IndexOf(@"\") > -1)
            {
                strtext2 += strtext.Substring(0, strtext.IndexOf(@"\") + 1);
                strtext = strtext.Substring(strtext.IndexOf(@"\") + 1);
            }
            strtext2 += @"../";

            string strFile = this.openFileDialog1.FileName;

            string strRarPath = string.Empty;
            string strFileName = string.Empty;

            string strXmlFile = this.GetType().Assembly.Location;
            string strfilePath = strXmlFile = strXmlFile.Replace("PRSpline.exe", "CompressFile\\");
            var opfileNames = this.openFileDialog1.SafeFileName.Split('.');
            strfilePath = strXmlFile = strfilePath + opfileNames[0] + "\\";
            if (strFile.IndexOf(".cfg") > 0 || strFile.IndexOf(".CFG") > 0)
            {
                if (File.Exists(strFile.Replace(".cfg", ".dat")) || File.Exists(strFile.Replace(".CFG", ".DAT")))
                {
                    Directory.CreateDirectory(strfilePath);
                    File.Copy(strFile, (strfilePath + openFileDialog1.SafeFileName), true);
                    File.Copy(strFile.Replace(".cfg", ".dat"), strfilePath + openFileDialog1.SafeFileName.Replace(".cfg", ".dat"), true);
                    File.Copy(strFile.Replace(".CFG", ".DAT"), strfilePath + openFileDialog1.SafeFileName.Replace(".CFG", ".DAT"), true);
                }
                else
                {
                    MessageBox.Show("無dat檔");
                    return IsSuccess;
                }
            }
            else
            {
                while (strFile.IndexOf(@"\") > -1)
                {
                    strRarPath += strFile.Substring(0, strFile.IndexOf(@"\") + 1);
                    strFile = strFile.Substring(strFile.IndexOf(@"\") + 1);
                }

                mCompressWinRAR.UnCompressRar(strfilePath, strRarPath, strFile);
            }
            var _mParser = new Parser();
            try
            {
                foreach (var item in Directory.GetFiles(strfilePath, "*.cfg"))
                {
                    LoadDataFile.GetCFGData(item, ref _mParser);
                    break;
                }
                foreach (var item in Directory.GetFiles(strfilePath, "*.CFG"))
                {
                    LoadDataFile.GetCFGData(item, ref _mParser);
                    break;
                }

                if (selectFile != SelectFile.File_1)
                {
                    if (!((DateTime)_mParser.Schema.StartTime.Value >= StartDateTime && (DateTime)_mParser.Schema.StartTime.Value < StartDateTime.AddSeconds(60)))
                    {
                      //  int StartTime_FFF = StartDateTime.Millisecond;

                        //int FileStartTime_FFF = ((DateTime)_mParser.Schema.StartTime.Value).Millisecond;
                        //MessageBox.Show(StartTime_FFF.ToString() + "," + FileStartTime_FFF.ToString());

                        MessageBox.Show("副檔時間錯誤");
                        return IsSuccess;
                    }
                }

                var _PData = new List<double[]>();
                var _SData = new List<double[]>();
                var _PUData = new List<double[]>();

                LoadDataFile.GetDatData(_mParser, ref _PData, ref _SData, ref _PUData);

                List<int> _fftIndex = new List<int>();
                for (int i = 0; i < _mParser.Schema.TotalAnalogChannels; i++)
                {
                    if (_mParser.Schema.AnalogChannels[i].Units == "V" || _mParser.Schema.AnalogChannels[i].Units == "A")
                        _fftIndex.Add(i);
                }
                switch (selectFile)
                {
                    case SelectFile.File_1:
                        ChartData_1.ButtonName.Clear();
                        ChartData_2.ButtonName.Clear();
                        ChartData_3.ButtonName.Clear();
                        ChartData_4.ButtonName.Clear();
                        ChartData_5.ButtonName.Clear();
                                               
                        FFTIndex = _fftIndex.ToArray();
                        break;
                    case SelectFile.File_2:
                        ChartData_2.ButtonName.Clear();
                        break;
                    case SelectFile.File_3:
                        ChartData_3.ButtonName.Clear();
                        break;
                    case SelectFile.File_4:
                        ChartData_4.ButtonName.Clear();
                        break;
                    case SelectFile.File_5:
                        ChartData_5.ButtonName.Clear();
                        break;                 
                }
                for (int i = 0; i < _mParser.Schema.TotalAnalogChannels; i++)
                {
                    switch (selectFile)
                    {
                        case SelectFile.File_1:
                            ChartData_1.ButtonName.Add(_mParser.Schema.AnalogChannels[i].Name);
                            break;
                        case SelectFile.File_2:
                            ChartData_2.ButtonName.Add(_mParser.Schema.AnalogChannels[i].Name);
                            break;
                        case SelectFile.File_3:
                            ChartData_3.ButtonName.Add(_mParser.Schema.AnalogChannels[i].Name);
                            break;
                        case SelectFile.File_4:
                            ChartData_4.ButtonName.Add(_mParser.Schema.AnalogChannels[i].Name);
                            break;
                        case SelectFile.File_5:
                            ChartData_5.ButtonName.Add(_mParser.Schema.AnalogChannels[i].Name);
                            break;                      
                    }
                }
                if (selectFile == SelectFile.File_1)
                {
                    ButtonName_D.Clear();
                    for (int i = 0; i < _mParser.Schema.TotalDigitalChannels; i++)
                    {
                        ButtonName_D.Add(_mParser.Schema.DigitalChannels[i].Name);
                    }

                }
                for (int i = 0; i < FFTIndex.Length; i++)
                {
                    switch (selectFile)
                    {
                        case SelectFile.File_1:
                            ChartData_1.ButtonName.Add(_mParser.Schema.AnalogChannels[FFTIndex[i]].Name + "_FFT");
                            break;
                        case SelectFile.File_2:
                            ChartData_2.ButtonName.Add(_mParser.Schema.AnalogChannels[FFTIndex[i]].Name + "_FFT");
                            break;
                        case SelectFile.File_3:
                            ChartData_3.ButtonName.Add(_mParser.Schema.AnalogChannels[FFTIndex[i]].Name + "_FFT");
                            break;
                        case SelectFile.File_4:
                            ChartData_4.ButtonName.Add(_mParser.Schema.AnalogChannels[FFTIndex[i]].Name + "_FFT");
                            break;
                        case SelectFile.File_5:
                            ChartData_5.ButtonName.Add(_mParser.Schema.AnalogChannels[FFTIndex[i]].Name + "_FFT");
                            break;                       
                    }
                }
                cbxitem.Items.Clear();
                switch (selectFile)
                {
                    case SelectFile.File_1:
                        try
                        {
                            //PData_1 = GetAllData(_PData, _mParser, selectFile);
                            //SData_1=GetAllData(_SData, _mParser, selectFile);
                            //PUData_1 = GetAllData(_PUData, _mParser, selectFile);
                            ChartData_1.PData = await GetAllData(_PData, _mParser, selectFile);
                            ChartData_1.SData = await GetAllData(_SData, _mParser, selectFile);
                            ChartData_1.PUData = await GetAllData(_PUData, _mParser, selectFile);
                            ChartData_1.mParser = _mParser;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        break;
                    case SelectFile.File_2:
                        ChartData_2.PData = await GetAllData(_PData, _mParser, selectFile);
                        ChartData_2.SData = await GetAllData(_SData, _mParser, selectFile);
                        ChartData_2.PUData = await GetAllData(_PUData, _mParser, selectFile);
                        ChartData_2.mParser = _mParser;

                        ChartData_2.strFileName = openFileDialog1.SafeFileName;

                        var AddTimeValueTotel = ((DateTime)(_mParser.Schema.StartTime.Value - ChartData_1.mParser.Schema.StartTime.Value));
                        var AddTimeValue = AddTimeValueTotel.Second * 1000 + AddTimeValueTotel.Millisecond;
                        foreach (var item in ChartData_2.PData)
                        {
                            item[1] += AddTimeValue;
                        }

                        foreach (var item in ChartData_2.SData)
                        {
                            item[1] += AddTimeValue;
                        }
                        foreach (var item in ChartData_2.PUData)
                        {
                            item[1] += AddTimeValue;
                        }
                        AddSecondFile(ChartData_2,_Mode,2);
                        //switch (_Mode)
                        //{
                            
                        //    case 1:
                        //        frmChartline.AddSecondFile(PData_2, 2, _mParser);
                        //        break;
                        //    case 2:
                        //        frmChartline.AddSecondFile(SData_2, 2, _mParser);
                        //        break;
                        //    case 3:
                        //        frmChartline.AddSecondFile(PUData_2, 2, _mParser);
                        //        break;
                        //}
                        break;
                    case SelectFile.File_3:
                        ChartData_3.PData = await GetAllData(_PData, _mParser, selectFile);
                        ChartData_3.SData = await GetAllData(_SData, _mParser, selectFile);
                        ChartData_3.PUData = await GetAllData(_PUData, _mParser, selectFile);
                        ChartData_3.mParser = _mParser;

                        ChartData_3.strFileName = openFileDialog1.SafeFileName;

                        var AddTimeValueTotel_2 = ((DateTime)(_mParser.Schema.StartTime.Value - ChartData_1.mParser.Schema.StartTime.Value));
                        var AddTimeValue_2 = AddTimeValueTotel_2.Second * 1000 + AddTimeValueTotel_2.Millisecond;
                        foreach (var item in ChartData_3.PData)
                        {
                            item[1] += AddTimeValue_2;
                        }

                        foreach (var item in ChartData_3.SData)
                        {
                            item[1] += AddTimeValue_2;
                        }
                        foreach (var item in ChartData_3.PUData)
                        {
                            item[1] += AddTimeValue_2;
                        }
                        //switch (_Mode)
                        //{
                        //    case 1:
                        //        frmChartline.AddSecondFile(PData_3, 3, mParser_3);
                        //        break;
                        //    case 2:
                        //        frmChartline.AddSecondFile(SData_3, 3, mParser_3);
                        //        break;
                        //    case 3:
                        //        frmChartline.AddSecondFile(PUData_3, 3, mParser_3);
                        //        break;
                        //}
                        AddSecondFile(ChartData_3, _Mode, 3);

                        break;
                    case SelectFile.File_4:
                        ChartData_4.PData = await GetAllData(_PData, _mParser, selectFile);
                        ChartData_4.SData = await GetAllData(_SData, _mParser, selectFile);
                        ChartData_4.PUData = await GetAllData(_PUData, _mParser, selectFile);
                        ChartData_4.mParser = _mParser;

                        ChartData_4.strFileName = openFileDialog1.SafeFileName;

                        var AddTimeValueTotel_3 = ((DateTime)(_mParser.Schema.StartTime.Value - ChartData_1.mParser.Schema.StartTime.Value));
                        var AddTimeValue_3 = AddTimeValueTotel_3.Second * 1000 + AddTimeValueTotel_3.Millisecond;
                        foreach (var item in ChartData_4.PData)
                        {
                            item[1] += AddTimeValue_3;
                        }

                        foreach (var item in ChartData_4.SData)
                        {
                            item[1] += AddTimeValue_3;
                        }
                        foreach (var item in ChartData_4.PUData)
                        {
                            item[1] += AddTimeValue_3;
                        }
                        AddSecondFile(ChartData_4, _Mode, 4);
                        //switch (_Mode)
                        //{
                        //    case 1:
                        //        frmChartline.AddSecondFile(PData_4, 4, mParser_4);
                        //        break;
                        //    case 2:
                        //        frmChartline.AddSecondFile(SData_4, 4, mParser_4);
                        //        break;
                        //    case 3:
                        //        frmChartline.AddSecondFile(PUData_4, 4, mParser_4);
                        //        break;
                        //}

                        break;
                    case SelectFile.File_5:
                        ChartData_5.PData = await GetAllData(_PData, _mParser, selectFile);
                        ChartData_5.SData = await GetAllData(_SData, _mParser, selectFile);
                        ChartData_5.PUData = await GetAllData(_PUData, _mParser, selectFile);
                        ChartData_5.mParser = _mParser;

                        ChartData_5.strFileName = openFileDialog1.SafeFileName;

                        var AddTimeValueTotel_4 = ((DateTime)(_mParser.Schema.StartTime.Value - ChartData_1.mParser.Schema.StartTime.Value));
                        var AddTimeValue_4 = AddTimeValueTotel_4.Second * 1000 + AddTimeValueTotel_4.Millisecond;
                        foreach (var item in ChartData_5.PData)
                        {
                            item[1] += AddTimeValue_4;
                        }

                        foreach (var item in ChartData_5.SData)
                        {
                            item[1] += AddTimeValue_4;
                        }
                        foreach (var item in ChartData_5.PUData)
                        {
                            item[1] += AddTimeValue_4;
                        }
                        AddSecondFile(ChartData_5, _Mode, 5);
                        //switch (_Mode)
                        //{
                        //    case 1:
                        //        frmChartline.AddSecondFile(PData_5, 5, mParser_5);
                        //        break;
                        //    case 2:
                        //        frmChartline.AddSecondFile(SData_5, 5, mParser_5);
                        //        break;
                        //    case 3:
                        //        frmChartline.AddSecondFile(PUData_5, 5, mParser_5);
                        //        break;
                        //}

                        break;
                }
                if (ChartData_1.ButtonName.Count > 0) cbxitem.Items.Add("主檔");
                if (ChartData_2.ButtonName.Count > 0) cbxitem.Items.Add("副檔1");
                if (ChartData_3.ButtonName.Count > 0) cbxitem.Items.Add("副檔2");
                if (ChartData_4.ButtonName.Count > 0) cbxitem.Items.Add("副檔3");
                if (ChartData_5.ButtonName.Count > 0) cbxitem.Items.Add("副檔4");
                //if (ButtonName_6.Count > 0) cbxitem.Items.Add("副檔5");
                cbxitem.SelectedIndex = 0;
            }
            catch (ApplicationException message)
            {
                MessageBox.Show(String.Format("{0}: {1}", strFile, message.Message));
                return IsSuccess;
            }
            finally
            {

            }
            IsSuccess = true;
            return IsSuccess;
        }

        private void AddSecondChart(SelectFile _selectFile)
        {
            //switch (_Mode)
            //{
            //    case 1:
            //        switch (_selectFile)
            //        {
            //            case SelectFile.File_2:
            //                frmChartline.AddSecondFile(PData_2, 2, mParser_2);
            //                break;
            //        }

            //        break;
            //    case 2:
            //        switch (_selectFile)
            //        {
            //            case SelectFile.File_2:
            //                frmChartline.AddSecondFile(SData_2, 2, mParser_2);
            //                break;
            //        }

            //        break;
            //    case 3:
            //        switch (_selectFile)
            //        {
            //            case SelectFile.File_2:
            //                frmChartline.AddSecondFile(PUData_2, 2, mParser_2);
            //                break;
            //        }

            //        break;
            //}
        }
        private void LoadSecondFile()
        {

        }

        private void AddCbxItemItem()
        {

        }

        #endregion

        private void EnableView(bool IsEnable)
        {
            groupBox2.Enabled = IsEnable;
            groupBox4.Enabled = IsEnable;
            panel1.Enabled = IsEnable;
            panel2.Enabled = IsEnable;
            panel3.Enabled = IsEnable;
            panel4.Enabled = IsEnable;
        }

    }
}
