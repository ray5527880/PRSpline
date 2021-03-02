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

using System.Text.RegularExpressions;

using System.Runtime.InteropServices;
using FFTWSharp;
using System.Numerics;
using BF_FW.data;
using BF_FW;
using GSF.COMTRADE;

namespace PRSpline
{
    public partial class frmMain : Form
    {
        private static Boolean bfrmVector = true;
        private frmChart frmChartline;
        private CompressWinRAR mCompressWinRAR;

        private Parser mParser;

        private FFTData mFFTData;

        private frmVector m_frmVector;

        int _Mode = 0;

        public static List<PSData> mPSData = new List<PSData>();

        public List<double[]> PData;
        public List<double[]> SData;
        public List<double[]> PUData;

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
            set_Form();
            set_ButtonImage();

            setEnable(false);

            mCompressWinRAR = new CompressWinRAR();

            cbxPS.Items.Add("P");
            cbxPS.Items.Add("S");
            cbxPS.Items.Add("Per Unit");

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
            btnVector.Image = Image.FromFile(strpath + "Vector.png");

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
            if (((Button)sender).BackColor == Color.LightSteelBlue)
            {
                ((Button)sender).BackColor = Color.LightSlateGray;
            }
            else if (((Button)sender).BackColor == Color.LightSlateGray)
            {
                ((Button)sender).BackColor = Color.LightSteelBlue;
            }
            if (((Button)sender).Text.IndexOf("FFT") > 0)
                frmChartline.Chart1_Enable(((Button)sender).TabIndex + mParser.Schema.DigitalChannels.Length, pnlAnagol);
            else
                frmChartline.Chart1_Enable(((Button)sender).TabIndex, pnlAnagol);
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
            frmChartline.Chart2_Enable(((Button)sender).TabIndex, pnlDigital);
        }

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
            labLocatiion.Text = mParser.Schema.StationName;
            labDevice.Text = mParser.Schema.DeviceID;
            labStartDate.Text = mParser.Schema.StartTime.Value.ToString("yyyy/MM/dd");
            labTriggerDate.Text = mParser.Schema.TriggerTime.Value.ToString("yyyy/MM/dd");
            labStartTime.Text = mParser.Schema.StartTime.Value.ToString("HH:mm:ss.fff");
            labTriggerTime.Text = mParser.Schema.TriggerTime.Value.ToString("HH:mm:ss.fff");
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

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
        public static void btnVectorClick()
        {
            bfrmVector = !bfrmVector;
        }

        private void cbxPS_SelectedIndexChanged(object sender, EventArgs e)
        {
            PSClick(cbxPS.SelectedItem.ToString());
        }
        private void PSClick(string _strPS)
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
                mFFTData = new FFTData();
                var mfft = new FFTCal(FFTIndex, mParser);
                switch (_Mode)
                {
                    case 1:
                        mFFTData = mfft.GetFFTData(PData);
                        break;
                    case 2:
                        mFFTData = mfft.GetFFTData(SData);
                        break;
                    case 3:
                        mFFTData = mfft.GetFFTData(PUData);
                        break;
                }

                BeginInvoke(new Action(() =>
                {
                    panel1.Controls.Clear();
                    switch (_Mode)
                    {
                        case 1:
                            frmChartline = new frmChart(mParser, PData, mFFTData);
                            break;
                        case 2:
                            frmChartline = new frmChart(mParser, SData, mFFTData);
                            break;
                        case 3:
                            frmChartline = new frmChart(mParser, PUData, mFFTData);
                            break;
                    }
                    panel1.Controls.Add(frmChartline);
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
                    //frmChartline.CheakButtonEnable(pnlAnagol);

                    //frmChartline.CheakButtonEnable(pnlDigital);
                    sizeChanged();
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void btnFileOpen_Click(object sender, EventArgs e)
        {
            if (Directory.Exists("./CompressFile"))
            {
                Directory.Delete("./CompressFile", true);
            }

            if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                (sender as Button).Enabled = true;
                return;
            }
            string strtext = this.openFileDialog1.FileName;
            string strtext2 = string.Empty;

            while (strtext.IndexOf(@"\") > -1)
            {
                strtext2 += strtext.Substring(0, strtext.IndexOf(@"\") + 1);
                strtext = strtext.Substring(strtext.IndexOf(@"\") + 1);
            }
            strtext2 += @"../";


            setEnable(false);

            string strFile = this.openFileDialog1.FileName;
            string strRarPath = string.Empty;
            string strFileName = string.Empty;
            string strXmlFile = this.GetType().Assembly.Location;
            string strfilePath = strXmlFile = strXmlFile.Replace("PRSpline.exe", "CompressFile\\");
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
                    return;
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

            try
            {
                cbxPS.Items.Clear();

                foreach (var item in Directory.GetFiles(strfilePath, "*.cfg"))
                {
                    LoadDataFile.GetCFGData(item, ref mParser);
                    break;
                }
                foreach (var item in Directory.GetFiles(strfilePath, "*.CFG"))
                {
                    LoadDataFile.GetCFGData(item, ref mParser);
                    break;
                }
                PData = new List<double[]>();
                SData = new List<double[]>();
                PUData = new List<double[]>();
                LoadDataFile.GetDatData(mParser, ref PData, ref SData, ref PUData);

                Set_Information();

                List<int> _fftIndex = new List<int>();
                for (int i = 0; i < mParser.Schema.TotalAnalogChannels; i++)
                {
                    if (mParser.Schema.AnalogChannels[i].Units == "V" || mParser.Schema.AnalogChannels[i].Units == "A")
                        _fftIndex.Add(i);
                }
                FFTIndex = _fftIndex.ToArray();

                ClearButton();
                for (int i = 0; i < mParser.Schema.TotalAnalogChannels; i++)
                {
                    AddNewButton(mParser.Schema.AnalogChannels[i].Name, i, 0);
                }
                for (int i = 0; i < mParser.Schema.TotalDigitalChannels; i++)
                {
                    AddNewButton(mParser.Schema.DigitalChannels[i].Name, i, 1);
                }
                for (int i = 0; i < FFTIndex.Length; i++)
                {
                    AddNewButton(mParser.Schema.AnalogChannels[FFTIndex[i]].Name + "_FFT", i + mParser.Schema.TotalAnalogChannels, 0);
                }
                cbxPS.Items.Add("P");
                cbxPS.Items.Add("S");
                cbxPS.Items.Add("Per Unit");

                cbxPS.SelectedIndex = 1;
                sizeChanged();
            }
            catch (ApplicationException message)
            {
                MessageBox.Show(String.Format("{0}: {1}", strFile, message.Message));
            }
            finally
            {
                setEnable(true);
            }
        }

        private void btnDownloading_Click(object sender, EventArgs e)
        {
            EditXml mEditXml = new EditXml();
            mEditXml.GetXmlData();
            frmDownload frm = new frmDownload();
            frm.ShowDialog();
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            EditXml mEditXml = new EditXml();
            mEditXml.GetXmlData();
            var frm = new frmSetup();
            frm.ShowDialog();
        }

        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            sizeChanged();
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
            this.panel3.Location = new Point(3, this.panel2.Height + 10);
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

        private void btnExtremum_Click(object sender, EventArgs e)
        {
            var data = new ExtremumData.Extremum[0];
            switch (_Mode)
            {
                case 1:
                    data = GetExtremum(PData);
                    break;
                case 2:
                    data = GetExtremum(SData);
                    break;
                case 3:
                    data = GetExtremum(PUData);
                    break;
            }
            var frm = new frmExtremum(data);
            frm.ShowDialog();

        }
        private ExtremumData.Extremum[] GetExtremum(List<double[]> Data)
        {
            var reData = new ExtremumData.Extremum[mParser.Schema.TotalAnalogChannels + mFFTData.arrFFTData[0].Value.Length];
            for (int i = 0; i < mParser.Schema.TotalAnalogChannels; i++)
            {
                reData[i].strName = mParser.Schema.AnalogChannels[i].Name;
            }
            for (int i = 0; i < FFTIndex.Length; i++)
            {
                reData[i + mParser.Schema.TotalAnalogChannels].strName = mParser.Schema.AnalogChannels[FFTIndex[i]].Name + "_FFT";
            }

            var d = new ExtremumData.Extremum();

            for (int i = 0; i < mParser.Schema.SampleRates[0].EndSample; i++)
            {
                if (i > mParser.Schema.SampleRates[0].Rate / mParser.Schema.NominalFrequency / 2 && i < mParser.Schema.SampleRates[0].EndSample - mParser.Schema.SampleRates[0].Rate / mParser.Schema.NominalFrequency / 2)
                {
                    for (int j = 0; j < mParser.Schema.TotalAnalogChannels; j++)
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
                        if (reData[j + mParser.Schema.TotalAnalogChannels].MaxValue < mFFTData.arrFFTData[i].Value[j])
                        {
                            reData[j + mParser.Schema.TotalAnalogChannels].MaxValue = mFFTData.arrFFTData[i].Value[j];
                            reData[j + mParser.Schema.TotalAnalogChannels].MaxTime = Data[i][1];
                        }
                    }
                }
            }

            for (int j = 0; j < mFFTData.arrFFTData[0].Value.Length; j++)
            {
                reData[j + mParser.Schema.TotalAnalogChannels].MinValue = reData[j + mParser.Schema.TotalAnalogChannels].MaxValue;
                reData[j + mParser.Schema.TotalAnalogChannels].MinTime = reData[j + mParser.Schema.TotalAnalogChannels].MaxTime;
            }

            for (int i = 0; i < mParser.Schema.SampleRates[0].EndSample; i++)
            {
                if (i > mParser.Schema.SampleRates[0].Rate / mParser.Schema.NominalFrequency / 2 && i < mParser.Schema.SampleRates[0].EndSample - mParser.Schema.SampleRates[0].Rate / mParser.Schema.NominalFrequency / 2)
                {
                    for (int j = 0; j < mFFTData.arrFFTData[0].Value.Length; j++)
                    {
                        if (reData[j + mParser.Schema.TotalAnalogChannels].MinValue > mFFTData.arrFFTData[i].Value[j])
                        {
                            reData[j + mParser.Schema.TotalAnalogChannels].MinValue = mFFTData.arrFFTData[i].Value[j];
                            reData[j + mParser.Schema.TotalAnalogChannels].MinTime = Data[i][1];
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
    }
}
