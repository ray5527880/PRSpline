using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.COMTRADE;
using BF_FW.data;
using BF_FW;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PRSpline
{
    public partial class VoltageSagChart1 : Form
    {
        string[] arrName = new string[] { "Va", "Vb", "Vc", "Va(pu)", "Vb(pu)", "Vc(pu)" };
        private string _filePaht;
        Parser mParser = new Parser();
        FFTData mFFTData = new FFTData();
        public VoltageSagData.voltageSagData mVoltageSagData;
        public int BaseVoltage;

        public VoltageSagChart1(string filePath,int baseVoltage)
        {
            _filePaht = filePath;
            BaseVoltage = baseVoltage;
            InitializeComponent();
        }

        private void VoltageSagChart1_Load(object sender, EventArgs e)
        {
            _filePaht = Path.GetFullPath(_filePaht);
            string strFileName = _filePaht;
            string strFolderPath = string.Empty;
            string strUnFilePath = string.Empty;

            while (strFileName.IndexOf(@"\") > -1)
            {
                strFolderPath += strFileName.Substring(0, strFileName.IndexOf(@"\") + 1);
                strFileName = strFileName.Substring(strFileName.IndexOf(@"\") + 1);
            }
            strUnFilePath = Path.GetFullPath(strFolderPath + @"../.././CompressFile");

            if (Directory.Exists(strUnFilePath))
            {
                Directory.Delete(strUnFilePath, true);
            }

            CompressWinRAR compressWinRAR = new CompressWinRAR();

            compressWinRAR.UnCompressRar(strUnFilePath, strFolderPath, strFileName);

            foreach (var item in Directory.GetFiles(strUnFilePath, "*.cfg"))
            {
                LoadDataFile.GetCFGData(item,ref mParser);
                break;
            }
            foreach (var item in Directory.GetFiles(strUnFilePath, "*.CFG"))
            {
                LoadDataFile.GetCFGData(item,ref mParser);
                break;
            }

            var PData = new List<double[]>();
            var SData = new List<double[]>();
            var PUData = new List<double[]>();
            LoadDataFile.GetDatData(mParser, ref PData, ref SData, ref PUData);

            double douMinTime = 1 / mParser.Schema.NominalFrequency / 2;
            int[] VIndex = new int[3];
            for (int i = 0; i < mParser.Schema.TotalAnalogChannels; i++)
            {
                if (mParser.Schema.AnalogChannels[i].Units == "V")
                {
                    if (mParser.Schema.AnalogChannels[i].PhaseDesignation == "A")
                        VIndex[0] = i;
                    else if (mParser.Schema.AnalogChannels[i].PhaseDesignation == "B")
                        VIndex[1] = i;
                    else if (mParser.Schema.AnalogChannels[i].PhaseDesignation == "C")
                        VIndex[2] = i;
                }
            }
            var mfft = new FFTCal(VIndex, mParser);
            mFFTData = mfft.GetFFTData(PData);

            double startTime_S = Convert.ToDouble(0);
            double triggerTime_S = TimeSpan.FromTicks(mParser.Schema.TriggerTime.Value - mParser.Schema.StartTime.Value).TotalMilliseconds;

            VSChart[] VSCharts = new VSChart[6];
            //double[] value = new double[mParser.Schema.SampleRates[0].EndSample];
            //double[] time = new double[mParser.Schema.SampleRates[0].EndSample];
            int maxTime = Convert.ToInt32(mParser.Schema.SampleRates[0].EndSample / mParser.Schema.SampleRates[0].Rate);

            button3.Enabled = false;
            panel3.Location = new Point(3, 127);
            panel3.Size = new Size(1492, 770);
            panel3.Visible = false;
            panel4.Location = new Point(3, 127);
            panel4.Size = new Size(1492, 770);

            var chart_1 = new VSChart_2();
            var chart_2 = new VSChart_2();

            for (int i = 0; i < 6; i++)
            {
                double[] value = new double[mParser.Schema.SampleRates[0].EndSample];
                double[] time = new double[mParser.Schema.SampleRates[0].EndSample];
                if (i < 3)
                {
                    for (int ii = 0; ii < mParser.Schema.SampleRates[0].EndSample; ii++)
                    {
                        value[ii] = PData[ii][VIndex[i] + 2];
                        time[ii] = PData[ii][1];
                    }
                    this.panel3.Controls.Add(new VSChart(arrName[i], value, time, (triggerTime_S - startTime_S), Convert.ToDouble(mVoltageSagData.StartTime), Convert.ToDouble(mVoltageSagData.StartTime + mVoltageSagData.duration), maxTime)
                    { Location = new Point(3, 3 + 255 * i) });
                    switch (i)
                    {
                        case 0:
                            chart_1.SetData((triggerTime_S - startTime_S), Convert.ToDouble(mVoltageSagData.StartTime), Convert.ToDouble(mVoltageSagData.StartTime + mVoltageSagData.duration));
                            chart_1.SetValue(0, value);
                            chart_1.Times = time;
                            break;  
                        case 1:
                            chart_1.SetValue(1, value);
                            break; 
                        case 2:
                            chart_1.SetValue(2, value);
                            this.panel4.Controls.Add(chart_1);
                            chart_1.Location = new Point(3, 3);
                            break;
                    }
                }
                
                else
                {
                    for (int ii = 0; ii < mParser.Schema.SampleRates[0].EndSample; ii++)
                    {
                        value[ii] = mFFTData.arrFFTData[ii].Value[i - 3] / BaseVoltage;
                        time[ii] = PData[ii][1];
                    }
                    
                    this.panel3.Controls.Add(new VSChart(arrName[i], value, time, Convert.ToDouble(mVoltageSagData.StartTime), Convert.ToDouble(mVoltageSagData.StartTime + mVoltageSagData.duration), maxTime)
                    { Location = new Point(751, 3 + 255 * (i - 3)) });
                    switch (i)
                    {
                        case 3:
                            chart_2.SetData( Convert.ToDouble(mVoltageSagData.StartTime), Convert.ToDouble(mVoltageSagData.StartTime + mVoltageSagData.duration));
                            chart_2.SetValue(0, value);
                            chart_2.Times = time;
                            break;
                        case 4:
                            chart_2.SetValue(1, value);
                            break;
                        case 5:
                            chart_2.SetValue(2, value);
                            this.panel4.Controls.Add(chart_2);
                            chart_2.Location = new Point(3, 385);
                            break;
                    }
                }

            }
            Set_Information();
        }
        private void Clear_Information()
        {
            label1.Text = string.Empty;
            label2.Text = string.Empty;
            label3.Text = string.Empty;
            label4.Text = string.Empty;
            label5.Text = string.Empty;
            label7.Text = string.Empty;
            label8.Text = string.Empty;
            label11.Text = string.Empty;
            label15.Text = string.Empty;
        }
        private void Set_Information()
        {
            Clear_Information();
            label1.Text = mParser.Schema.StationName + "_" + mParser.Schema.DeviceID + "_" + mParser.Schema.StartTime.Value.ToString("yyyy/MM/dd") + mParser.Schema.StartTime.Value.ToString(" HH:mm:ss") + "電壓驟降";
            label2.Text = mParser.Schema.StationName;
            label3.Text = mParser.Schema.DeviceID;
            label7.Text = mParser.Schema.StartTime.Value.ToString("yyyy/MM/dd");
            label4.Text = mParser.Schema.TriggerTime.Value.ToString("yyyy/MM/dd");
            label8.Text = mParser.Schema.StartTime.Value.ToString("HH:mm:ss.ffffff");
            label5.Text = mParser.Schema.TriggerTime.Value.ToString("HH:mm:ss.ffffff");
            label11.Text = mVoltageSagData.StartTime.ToString("#0.00");
            label15.Text = (mVoltageSagData.StartTime + mVoltageSagData.duration).ToString("#0.00");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strFilter = string.Empty;

            strFilter += "(*.jpg)|*.jpg";
            saveFileDialog.Filter = strFilter;
            saveFileDialog.ShowDialog();
            if (saveFileDialog.CheckPathExists && !string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                Point screenPoint = panel1.PointToScreen(new Point());
                Rectangle rect = new Rectangle(screenPoint, panel1.Size);
                Image img = new Bitmap(rect.Width, rect.Height);
                Graphics g = Graphics.FromImage(img);

                Thread.Sleep(300);

                g.CopyFromScreen(rect.X - 1, rect.Y - 1, 0, 0, rect.Size);
                img.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = true;
            panel3.Visible = true;
            panel4.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button3.Enabled = false;
            panel3.Visible = false;
            panel4.Visible = true;
        }
    }
}
