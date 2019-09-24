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


namespace PRSpline
{
    public partial class frmMain : Form
    {
        private static Boolean bfrmVector=true;
        private frmChart frmChartline;
        private List<PRData> arrPRData;
        private Compress_WinRAR mCompressWinRAR;
        private LoadDataFile m_LoadDataFile;
        private CFGData mCFGData;
        private DATData mDATData;
        private FFTData mFFTData;

        private frmVector m_frmVector;

        private VScrollBar mVScrollBar;

        public static List<PSData> mPSData=new List<PSData>();
        public struct PSData
        {
            public decimal P;
            public decimal S;
            public decimal PerUnit;
        }
        private struct PRData
        {
            public string FullName;
            public string Date;
            public string Time;
            public string Name;
        }

        public frmMain()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.pnlAnagol.MouseWheel += new MouseEventHandler(pnlAnagol_MouserWheel);

            set_Form();
            set_ButtonImage();

            setEnable(false);

            arrPRData = new List<PRData>();
            mCompressWinRAR = new Compress_WinRAR();
            mCFGData = new CFGData();
            mDATData = new DATData();
            m_LoadDataFile = new LoadDataFile();

            cbxPS.Items.Add("P");
            cbxPS.Items.Add("S");
            cbxPS.Items.Add("Per Unit");

            Clear_Information();

            GetAllRARFileName();
        }

        private void set_Form()
        {
            //this.BackColor = Color.LightSlateGray;
            //this.groupBox2.Paint += groupBox_Paint;
            //this.groupBox3.Paint += groupBox_Paint;
            this.groupBox4.Paint += groupBox_Paint;
        }
        private void set_ButtonImage()
        {
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
            //btnDownloading.Image = Iimage.Images[0];
            
            //btnDownloading.Image.
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

        private void GetAllRARFileName() 
        {
            string folderName = "./downloadFile";
            foreach (string fname in System.IO.Directory.GetFileSystemEntries(folderName))
            {
                if (fname.IndexOf(".zip") > 0 || fname.IndexOf(".rar") > 0)
                {
                    PRData mPRData = new PRData();
                    mPRData.FullName = fname.Substring(folderName.Length + 1, fname.Length - folderName.Length - 1);
                    string[] arrString = (fname.Substring(folderName.Length + 1, fname.Length - folderName.Length - 5)).Split('_');
                    mPRData.Date = arrString[0];
                    mPRData.Time = arrString[1];
                    mPRData.Name = arrString[2];
                    arrPRData.Add(mPRData);
                }
            }
        }
        /// <summary>
        /// 設定comboBoxItem
        /// </summary>
        /// <param name="cbx"></param>
        /// <param name="type">
        /// 0=Year
        /// 1=Month
        /// 2=Day
        /// </param>
        /// <param name="selectString">
        /// </param>
        private void cbxDateSet(PRData str,ComboBox cbx,int type, string selectString)
        {         
            if (cbx.Items.Count == 0)
            {
                string[] s = str.Date.Split('-');
                cbx.Items.Add(s[type]);
            }
            else
            {
                bool isPresence = false;
                foreach (string x in cbx.Items)
                {
                    if (str.Date.IndexOf(x) != -1)
                    {
                        isPresence = true;
                    }
                }
                if (!isPresence)
                {
                    string[] s = str.Date.Split('-');
                    cbx.Items.Add(s[type]);
                }
            }                
        }
        private void ClearButton()
        {
            pnlAnagol.Controls.Clear();
            pnlDigital.Controls.Clear();
        }

        private void AddNewButton(string name,int index,int type)
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
            frmChartline.Chart1_Enable(((Button)sender).TabIndex,pnlAnagol);
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
            frmChartline.Chart2_Enable(((Button)sender).TabIndex,pnlDigital);
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            if(panel1.Controls.Count>0)
                frmChartline.chart1_XAxisAdd();           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (panel1.Controls.Count > 0)
                frmChartline.char1_XAxisLess();
        }

        //private void btnEnter_Click(object sender, EventArgs e)
        //{
        //    foreach (PRData str in arrPRData)
        //    {
        //        if (str.FullName.IndexOf(cbxTime.SelectedItem.ToString()) != -1)
        //        {
        //            mCompressWinRAR.UnCompressRar("./CompressFile/", "./downloadFile/", str.FullName);
        //            m_LoadDataFile.DisplayValues_CFG("./downloadFile/CompressFile/" + (str.FullName).Substring(0, str.FullName.Length - 3)+"cfg", ref mCFGData);
        //            Set_Information(mCFGData);
        //            SetPSData(mCFGData);
                        
        //            m_LoadDataFile.DisplayValues_DAT("./downloadFile/CompressFile/" + (str.FullName).Substring(0, str.FullName.Length - 3) + "dat", ref mDATData, mCFGData.TotalAmount);

        //            ClearButton();
        //            for (int i = 0; i < mCFGData.A_Amount; i++)
        //            {
        //                AddNewButton(mCFGData.arrAnalogyData[i].Name, i,0);
        //            }
        //            for (int i = 0; i < mCFGData.D_Amount; i++)
        //            {
        //                AddNewButton(mCFGData.arrDigitalData[i].Name, i, 1);
        //            }
        //            if (mCFGData.arrAnalogyData[0].PrimaryOrSecondary == "P")
        //            {
        //                cbxPS.SelectedIndex = 0;                      
        //            }
        //            else if (mCFGData.arrAnalogyData[0].PrimaryOrSecondary == "S")
        //            {
        //                cbxPS.SelectedIndex = 1;                       
        //            }
        //            else
        //            {
        //                MessageBox.Show("");
        //                break;
        //            }
                   
        //            break;
        //        }
        //    }
        //    setEnable(true);
        //}

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
            if (bfrmVector)
            {
                m_frmVector = new frmVector();
                string[] _str = new string[mCFGData.A_Amount];
                for (int ii = 0; ii < mCFGData.A_Amount; ii++)
                {
                    _str[ii] = mCFGData.arrAnalogyData[ii].Name;
                }
                m_frmVector.GetDeviceName(_str);
                m_frmVector.Show();
                bfrmVector = false;
                btnVector.Enabled = false;
            }
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
            int _Mode = 0;
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
                    _PSData.PerUnit = 1/ _CFGData.arrAnalogyData[i].Magnification2  * _CFGData.arrAnalogyData[i].Zoom;
                    _PSData.P = _CFGData.arrAnalogyData[i].Magnification1 / _CFGData.arrAnalogyData[i].Magnification2 *_CFGData.arrAnalogyData[i].Zoom;
                }
                mPSData.Add(_PSData);
            }
        }
        private void setEnable(bool enable)
        {
            btnRemove.Enabled = enable;
            btnReZoom.Enabled = enable;
            btnScreenshot.Enabled = enable;
            btnVector.Enabled = enable;
            btnXZoomIn.Enabled = enable;
            btnXZoomOut.Enabled = enable;
            btnYZoomIn.Enabled = enable;
            btnYZoomOut.Enabled = enable;
            cbxPS.Enabled =enable;
        }

        private void btnFileOpen_Click(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists("./CompressFile"))
            {
                System.IO.Directory.Delete("./CompressFile", true);
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

                foreach (var item in System.IO.Directory.GetFiles(strfilePath, "*.cfg"))
                {
                    m_LoadDataFile.DisplayValues_CFG(item, ref mCFGData);
                    break;
                }
                Set_Information(mCFGData);
                SetPSData(mCFGData);

                foreach (var item in System.IO.Directory.GetFiles(strfilePath, "*.dat"))
                {
                    m_LoadDataFile.DisplayValues_DAT(item, ref mDATData, mCFGData.TotalAmount);
                    break;
                }

                int count = 0;
                for (int i = 0; i < mCFGData.arrAnalogyData.Count(); i++)
                {
                    if (mCFGData.arrAnalogyData[i].Name.IndexOf("MEAS") < 0)
                        count++;
                }
                mFFTData = new FFTData();
                mFFTData.arrFFTData = new List<FFTData.Data>();
                for (int i = 0; i < mCFGData.TotalPoint; i++)
                {
                    FFTData.Data _Data = new FFTData.Data();
                    _Data.Value = new double[count];
                    _Data.rad = new double[count];
                    for (int ii = 0; ii < count; ii++)
                    {
                        Complex mComplex = new Complex();

                        mComplex = FFTW(i, ii);
                        _Data.Value[ii] = Math.Sqrt(Math.Pow(mComplex.Real, 2) + Math.Pow(mComplex.Imaginary, 2)) / (64 / 2) / Math.Sqrt(2);
                        _Data.rad[ii] = Math.Atan2(mComplex.Imaginary, mComplex.Real);
                    }
                    mFFTData.arrFFTData.Add(_Data);
                }

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
                if (pnlAnagol.Controls.Count > 15)
                    AddVScrollBar();
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
        private void AddVScrollBar()
        {
            mVScrollBar = new VScrollBar();
            pnlAnagol.Controls.Add(mVScrollBar);
            mVScrollBar.Location = new Point(93, 0);
            mVScrollBar.Height = 360;
            mVScrollBar.Minimum = 0;
            mVScrollBar.Maximum = pnlAnagol.Controls.Count - 16;
            mVScrollBar.LargeChange = 1;
            mVScrollBar.ValueChanged += new EventHandler(VScrollBar_ValueChanged);
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
        private void pnlAnagol_MouserWheel(object sender, MouseEventArgs e)
        {
            if (pnlAnagol.Controls.Count > 15)
            {
                if (e.Delta < 0)
                {
                    if (mVScrollBar.Maximum >= mVScrollBar.Value + 1)
                    {
                        mVScrollBar.Value++;                      
                    }
                }
                else if (e.Delta > 0)
                {
                    if (mVScrollBar.Minimum <= mVScrollBar.Value - 1)
                    {
                        mVScrollBar.Value--;                       
                    }
                }
            }
        }

        private void VScrollBar_ValueChanged(Object seder, EventArgs e)
        {
            if (pnlAnagol.Controls.Count < 16)
                return;
            int index = 0;
            foreach (var button in pnlAnagol.Controls)
            {
                if (button is Button)
                {
                    ((Button)button).Location = new Point(((Button)button).Location.X, 0 - 24 * (mVScrollBar.Value) + 24 * index);
                    index++;
                }
            }
        }
    }
}
