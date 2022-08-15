using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Timers;
using System.Threading.Tasks;
using BF_FW;

namespace AutoDownloading
{
    public partial class frmAutoDownloading : Form
    {
        Thread m_Task;

        //public tVSData tVSData;
        public static string ConnectionAlarms;
        public ColorProgressBar m_PrgBar;

        public frmAutoDownloading()
        {
            InitializeComponent();
            m_PrgBar = new ColorProgressBar();
            this.m_PrgBar.BarColor = System.Drawing.Color.FromArgb(((System.Byte)(20)),
               ((System.Byte)(191)), ((System.Byte)(255)));
            this.m_PrgBar.BorderColor = System.Drawing.Color.White;
            this.m_PrgBar.FillStyle = ColorProgressBar.FillStyles.Solid;
            this.m_PrgBar.Location = new System.Drawing.Point(20, 55);

            this.m_PrgBar.Name = "PrgBar";
            this.m_PrgBar.Size = new System.Drawing.Size(400, 25);

            this.m_PrgBar.TabIndex = 0;
            this.m_PrgBar.Value = 0;

            this.Controls.Add(this.m_PrgBar);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            EditXml editXml = new EditXml();

            editXml.GetXmlData();
            this.m_PrgBar.Maximum = 1000;

            this.label4.Text = string.Empty;
            this.label5.Text = string.Empty;

            this.m_PrgBar.Step = EditXml.mFTPData.Count != 0 ? 1000 / EditXml.mFTPData.Count : 10;
            System.Timers.Timer ThreadStart = new System.Timers.Timer(EditXml.m_nTimes * 60 * 1000);

            ThreadStart.Elapsed += new ElapsedEventHandler(TimerThread);
            ThreadStart.AutoReset = true;
            ThreadStart.Enabled = true;
            this.WindowState = FormWindowState.Minimized;
            this.niCollect.Visible = true;
        }

        public void TimerThread(object source, ElapsedEventArgs e)
        {
            m_Task = new Thread(ThreadTask);
            m_Task.Start(this);
        }
        private static void ThreadTask(Object objfrm)
        {
            EditXml editXml = new EditXml();

            editXml.GetXmlData();
            ConnectionAlarms = "server=" + EditXml.DBPath + "; database=" + EditXml.DBName + ";uid=" + EditXml.DBUser + ";pwd=" + EditXml.DBPwd;

            var tVSData = new tVSData(ConnectionAlarms);

            var frm = (frmAutoDownloading)objfrm;
            FTPDownload mFTP = new FTPDownload();

            DateTime dtNow = DateTime.Now;
            DelegateCentre.UpdateLabel(dtNow.ToString("HH:mm:ss"), frm.label4);
            DelegateCentre.UpdateLabel("", frm.label5);
            DelegateCentre.InitPrgBar(frm.m_PrgBar);

            foreach (var item in EditXml.mFTPData)
            {
                //var m_tVSIEDName = new tVSIEDName(ConnectionAlarms);
               // var VSIED_Data = m_tVSIEDName.GetData(item.strName);

                if (!Directory.Exists(EditXml.strDownloadPath + item.strName + @"\"))
                    Directory.CreateDirectory(EditXml.strDownloadPath + item.strName + @"\");
                int count = 0;
                bool IsValidConnection = false;
                while (count < 3 && !IsValidConnection)
                {
                    IsValidConnection = mFTP.CheckConnection(item.strIP, item.strUser, item.strPwd);
                    count++;
                }
                if (!IsValidConnection)
                {
                    string FunctionName = MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name;

                    Logger.MakeLogger(FunctionName, string.Format("{0} 連線失敗", item.strName));
                    continue;
                }
                string[] FTPfiles = mFTP.GetFTPFileName(item.strIP, item.strUser, item.strPwd);
                for (int i = 0; i < FTPfiles.Length; i++)
                {
                    try {
                        string filePaht = EditXml.strDownloadPath + item.strName + @"\" + FTPfiles[i];
                        mFTP.FTP_Download(filePaht, FTPfiles[i], item.strIP, item.strUser, item.strPwd);
                        if (File.Exists(filePaht))
                            //mFTP.FTP_Delete(FTPfiles[i], item.strIP, item.strUser, item.strPwd);
                        if (EditXml.VoltageSag == 1)
                        {
                            VoltageSagCal VolSagVal = new VoltageSagCal(filePaht, item.BaseValue);

                            if (VolSagVal.VoltageSagDatas.duration != 0)
                            {
                                string _filePaht = string.Format(@"./VSData/{0}.xml", item.strPathName);
                                VolSagVal.VoltageSagDatas.strPSValue = string.Empty;
                                var VSXml = new VoltageSagXml(_filePaht);
                                VSXml.AddData(VolSagVal.VoltageSagDatas);
                                string str = VolSagVal.VoltageSagDatas.treggerDateTime.ToString("yyyy-MM-dd_HH-mm-ss") +"發生電驛："+ item.strPathName + " 持續時間：" + VolSagVal.VoltageSagDatas.duration + "ms 持續週期：" + VolSagVal.VoltageSagDatas.cycle
                    + "cycle 電壓：R=" + VolSagVal.VoltageSagDatas.PValue + "pu S=" + VolSagVal.VoltageSagDatas.QValue + "pu T=" + VolSagVal.VoltageSagDatas.SValue + "pu";
                                tVSData.AddData(str);
                                
                                //if (EditXml.IsUserSQL == 1)
                                //{
                                //    tVSData.VSData vSDatas = new tVSData.VSData();
                                //    vSDatas.Year = VolSagVal.VoltageSagDatas.treggerDateTime.Year;
                                //    vSDatas.MD = VolSagVal.VoltageSagDatas.treggerDateTime.Month * 100 + VolSagVal.VoltageSagDatas.treggerDateTime.Day;
                                //    vSDatas.HM = VolSagVal.VoltageSagDatas.treggerDateTime.Hour * 100 + VolSagVal.VoltageSagDatas.treggerDateTime.Minute;
                                //    vSDatas.SS = VolSagVal.VoltageSagDatas.treggerDateTime.Second;
                                //    vSDatas.DUR = VolSagVal.VoltageSagDatas.duration;
                                //    vSDatas.V1 = VolSagVal.VoltageSagDatas.PValue * 100;
                                //    vSDatas.V2 = VolSagVal.VoltageSagDatas.QValue * 100;
                                //    vSDatas.V3 = VolSagVal.VoltageSagDatas.SValue * 100;
                                //    if (vSDatas.V1 < vSDatas.V2)
                                //    {
                                //        if (vSDatas.V1 < vSDatas.V3)
                                //            vSDatas.Down = 100 - vSDatas.V1;
                                //        else
                                //            vSDatas.Down = 100 - vSDatas.V3;
                                //    }
                                //    else
                                //    {
                                //        if (vSDatas.V2 < vSDatas.V3)
                                //            vSDatas.Down = 100 - vSDatas.V2;
                                //        else
                                //            vSDatas.Down = 100 - vSDatas.V3;
                                //    }
                                //    vSDatas.Cycle = VolSagVal.VoltageSagDatas.cycle;
                                //    vSDatas.Type = GetType(100 - vSDatas.Down, vSDatas.DUR);

                                //    //   if (vSDatas.V1 < 90 || vSDatas.V2 < 90 || vSDatas.V3 < 90)
                                //    tVSData.AddData(vSDatas, VSIED_Data.ID);
                                //}
                            }
                        }
                    }catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    }
                DelegateCentre.UpdatePrgBar(frm.m_PrgBar);
            }
            DelegateCentre.UpdateLabel((DateTime.Now - dtNow).ToString(@"mm\:ss"), frm.label5);
        }
        public static int GetType(decimal _Value, decimal _Time)
        {
            int reValue = 0;
            if (_Time < 50)
            {
                reValue = 1;
            }
            else if (_Time > 1000 * 60)
            {
                reValue = 4;
            }
            else
            {
                if (_Time <= 200 && _Value >= 50)
                    reValue = 2;
                else if (_Time <= 500 && _Value >= 70)
                    reValue = 2;
                else if (_Time >= 200 && _Value > 80)
                    reValue = 2;

            }
            return reValue;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否關閉 ?", "重要", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.niCollect.Visible = true;
            this.Visible = false;
            this.ShowInTaskbar = false;
        }

        private void frmAutoDownloading_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_Task != null)
                m_Task.Abort();
            this.niCollect.Visible = false;
            this.niCollect.Dispose();
        }

        private void niCollect_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void frmAutoDownloading_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.niCollect.Visible = true;
                this.Visible = false;
                this.ShowInTaskbar = false;
            }
            else
            {
                this.niCollect.Visible = false;
            }
        }
    }
    delegate void InitPrgBarCallback(ColorProgressBar Pgr);
    delegate void UpdatePrgBarCallback(ColorProgressBar Pgr);
    delegate void UpdateLabelCallback(string strWorkTime, Label lbl);
    delegate void SetPrgBarMaxback(ColorProgressBar Pgr);

    class DelegateCentre
    {
        public static void InitPrgBar(ColorProgressBar Pgr)
        {
            if (Pgr.InvokeRequired)
            {
                InitPrgBarCallback myUpdate = new InitPrgBarCallback(InitPrgBar);
                Pgr.Invoke(myUpdate, Pgr);
            }
            else
            {
                Pgr.Value = 0;
            }

        }

        public static void UpdatePrgBar(ColorProgressBar Pgr)
        {
            if (Pgr.InvokeRequired)
            {
                UpdatePrgBarCallback myUpdate = new UpdatePrgBarCallback(UpdatePrgBar);
                Pgr.Invoke(myUpdate, Pgr);
            }
            else
            {
                if (Pgr.Value < Pgr.Maximum * 0.98)
                    Pgr.PerformStep();
            }
        }
        public static void SetPrgBarMax(ColorProgressBar Pgr)
        {
            if (Pgr.InvokeRequired)
            {
                SetPrgBarMaxback myUpdate = new SetPrgBarMaxback(SetPrgBarMax);
                Pgr.Invoke(myUpdate, Pgr);
            }
            else
            {
                Pgr.Value = Pgr.Maximum;
            }
        }

        public static void UpdateLabel(string strWorkTime, Label lbl)
        {
            if (lbl.InvokeRequired)
            {
                UpdateLabelCallback myUpdate = new UpdateLabelCallback(UpdateLabel);
                lbl.Invoke(myUpdate, strWorkTime, lbl);
            }
            else
            {
                lbl.Text = strWorkTime;
            }
        }
    }
}
