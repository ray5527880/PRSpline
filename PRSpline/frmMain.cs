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
        private Compress_WinRAR mCompressWinRAR;

        private Parser mParser;



        private CFGData mCFGData;
        private DATData mDATData;
        private FFTData mFFTData;

        private frmVector m_frmVector;

        int _Mode = 0;

        public static List<PSData> mPSData = new List<PSData>();
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

            mCompressWinRAR = new Compress_WinRAR();
            mCFGData = new CFGData();
            mDATData = new DATData();

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
                frmChartline.Chart1_Enable(((Button)sender).TabIndex + mCFGData.D_Amount, pnlAnagol);
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
            if (System.IO.Directory.Exists("./downloadFile/CompressFile"))
            {
                System.IO.Directory.Delete("./downloadFile/CompressFile", true);
            }
        }
        private void Clear_Information()
        {
            labLocatiion.Text = "";
            labDevice.Text = "";
            labStartTime.Text = "";
            labTriggerTime.Text = "";
            labStartDate.Text = "";
            labTriggerDate.Text = "";
        }
        private void Set_Information(CFGData _CFGData)
        {
            Clear_Information();
            labLocatiion.Text = _CFGData.Location;
            labDevice.Text = _CFGData.Device;
            labStartDate.Text = _CFGData.startDate;
            labTriggerDate.Text = _CFGData.triggerDate;
            labStartTime.Text = _CFGData.startTime.Substring(0, _CFGData.startTime.Length - 3);
            labTriggerTime.Text = _CFGData.triggerTime.Substring(0, _CFGData.triggerTime.Length - 3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmChartline.save_chart1(mCFGData);
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
            //if (bfrmVector)
            //{
            //    m_frmVector = new frmVector();
            //    string[] _str = new string[mCFGData.A_Amount];
            //    for (int ii = 0; ii < mCFGData.A_Amount; ii++)
            //    {
            //        _str[ii] = mCFGData.arrAnalogyData[ii].Name;
            //    }
            //    m_frmVector.GetDeviceName(_str);
            //    m_frmVector.Show();
            //    bfrmVector = false;
            //    btnVector.Enabled = false;
            //}
            var frm = new frmVoltageSag();
            frm.ShowDialog();
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

            BeginInvoke(new Action(() =>
            {
                panel1.Controls.Clear();
                frmChartline = new frmChart(mCFGData, mDATData, mFFTData, _Mode);
                panel1.Controls.Add(frmChartline);
                frmChartline.CheakButtonEnable(pnlAnagol);

                frmChartline.CheakButtonEnable(pnlDigital);
                sizeChanged();
            }));
        }
        private void SetPSData(CFGData _CFGData)
        {
            mPSData.Clear();
            for (int i = 0; i < _CFGData.A_Amount; i++)
            {
                PSData _PSData = new PSData();
                if (_CFGData.arrAnalogyData[i].PrimaryOrSecondary == "P")
                {
                    _PSData.P = _CFGData.arrAnalogyData[i].Magnification1;
                    _PSData.PerUnit = _CFGData.arrAnalogyData[i].Magnification1 * _CFGData.arrAnalogyData[i].Magnification1;
                    _PSData.S = _PSData.PerUnit / _CFGData.arrAnalogyData[i].Magnification2;
                }
                else if (_CFGData.arrAnalogyData[i].PrimaryOrSecondary == "S")
                {
                    _PSData.S = _CFGData.arrAnalogyData[i].Zoom;
                    _PSData.PerUnit = 1 / _CFGData.arrAnalogyData[i].Magnification2 * _CFGData.arrAnalogyData[i].Zoom;
                    _PSData.P = _CFGData.arrAnalogyData[i].Magnification1 / _CFGData.arrAnalogyData[i].Magnification2 * _CFGData.arrAnalogyData[i].Zoom;
                }
                mPSData.Add(_PSData);
            }
        }
        private void setEnable(bool enable)
        {
            btnRemove.Enabled = enable;
            btnReZoom.Enabled = enable;
            btnScreenshot.Enabled = enable;
            //btnVector.Enabled = enable;
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

            setEnable(false);

            string strFile = this.openFileDialog1.FileName;
            string strRarPath = string.Empty;
            string strFileName = string.Empty;
            string strXmlFile = this.GetType().Assembly.Location;
            string strfilePath = strXmlFile = strXmlFile.Replace("PRSpline.exe", "CompressFile\\");
            if (strFile.IndexOf(".cfg") > 0)
            {
                if (File.Exists(strFile.Replace(".cfg", ".dat")))
                {
                    Directory.CreateDirectory(strfilePath);
                    File.Copy(strFile, (strfilePath + openFileDialog1.SafeFileName), true);
                    File.Copy(strFile.Replace(".cfg", ".dat"), strfilePath + openFileDialog1.SafeFileName.Replace(".cfg", ".dat"), true);
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
                    mCFGData = LoadDataFile.Get_CFGData(item);
                    break;
                }
                Set_Information(mCFGData);
                SetPSData(mCFGData);

                foreach (var item in Directory.GetFiles(strfilePath, "*.dat"))
                {
                    mDATData = LoadDataFile.Get_DATData(item, mCFGData.TotalAmount);
                    break;
                }

                int count = 0;
                for (int i = 0; i < mCFGData.arrAnalogyData.Count(); i++)
                {
                    if (mCFGData.arrAnalogyData[i].Name.IndexOf("MEAS") < 0)
                        count++;
                }
                mFFTData = new FFTData();
                var data = new List<FFTData.fftData>();
                for (int i = 0; i < mCFGData.TotalPoint; i++)
                {
                    FFTData.fftData _data = new FFTData.fftData();
                    _data.Value = new double[count];
                    _data.rad = new double[count];
                    for (int ii = 0; ii < count; ii++)
                    {
                        Complex mComplex = new Complex();

                        mComplex = FFTW(i, ii);
                        _data.Value[ii] = Math.Sqrt(Math.Pow(mComplex.Real, 2) + Math.Pow(mComplex.Imaginary, 2)) / (64 / 2) / Math.Sqrt(2);
                        _data.rad[ii] = Math.Atan2(mComplex.Imaginary, mComplex.Real);
                    }
                    data.Add(_data);
                }
                mFFTData.arrFFTData = data.ToArray();
                ClearButton();
                for (int i = 0; i < mCFGData.A_Amount; i++)
                {
                    AddNewButton(mCFGData.arrAnalogyData[i].Name, i, 0);
                }
                for (int i = 0; i < mCFGData.D_Amount; i++)
                {
                    AddNewButton(mCFGData.arrDigitalData[i].Name, i, 1);
                }
                for (int i = 0; i < mCFGData.arrAnalogyData.Count(); i++)
                {
                    if (mCFGData.arrAnalogyData[i].Name.IndexOf("MEAS") < 0)
                    {
                        AddNewButton(mCFGData.arrAnalogyData[i].Name + "_FFT", i + mCFGData.arrAnalogyData.Count(), 0);
                    }
                }
               


                cbxPS.Items.Add("P");
                cbxPS.Items.Add("S");
                cbxPS.Items.Add("Per Unit");

                if (mCFGData.arrAnalogyData[0].PrimaryOrSecondary == "P")
                {
                    cbxPS.SelectedIndex = 0;
                }
                else if (mCFGData.arrAnalogyData[0].PrimaryOrSecondary == "S")
                {
                    cbxPS.SelectedIndex = 1;
                }
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
        private Complex FFTW(int index_Entry, int index_Value)
        {
            int SIZE = Convert.ToInt32(mCFGData.SamplingPoint / mCFGData.Hz);

            double[] data = new double[SIZE];

            Complex[] cdata = new Complex[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                if ((index_Entry < SIZE / 2 && (i < SIZE / 2 - index_Entry)) || (i + index_Entry - (SIZE / 2 - 1)) > mCFGData.TotalPoint)
                {
                    cdata[i] = new Complex(0, 0);
                }
                else
                {
                    cdata[i] = new Complex(Convert.ToDouble(mDATData.arrData[i + index_Entry - SIZE / 2].value[index_Value]), 0);
                }
            }
            fftw_complexarray input = new fftw_complexarray(SIZE);
            fftw_complexarray ReData = new fftw_complexarray(SIZE);

            input.SetData(cdata);

            fftw_plan pf = fftw_plan.dft_1d(SIZE, input, ReData, fftw_direction.Forward, fftw_flags.Estimate);

            pf.Execute();
            var data_Complex = ReData.GetData_Complex();

            return data_Complex[1];
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
                this.panel1.Size = new System.Drawing.Size(this.Size.Width - 170, this.Size.Height - 150);
                frmChartline.Form_SizeChanged(this.panel1.Size.Width + 29, this.panel1.Size.Height - 30);
              
            }
            this.panel4.Height = this.Height - 51 - 113;
            this.panel2.Height = this.panel4.Height / 5 * 3 - 10;
            this.panel3.Location = new Point(3,this.panel2.Height + 10);
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
            var x = new ExtremumData.Extremum[0];
            decimal[] ps = new decimal[mPSData.Count()];
            for (int i = 0; i < ps.Length; i++)
            {
                switch (_Mode)
                {
                    case 1:
                        ps[i] = mPSData[i].P;
                        break;
                    case 2:
                        ps[i] = mPSData[i].S;
                        break;
                    case 3:
                        ps[i] = mPSData[i].PerUnit;
                        break;
                }
            }
            var frm = new frmExtremum(ExtremumFunction.GetExtremunData(mDATData.arrData, mCFGData, ps));
            frm.ShowDialog();
        }
    }
}
