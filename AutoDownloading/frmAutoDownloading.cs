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
using BF_FW;

namespace AutoDownloading
{
    public partial class frmAutoDownloading : Form
    {
        Thread m_Task;

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

            this.m_PrgBar.Step = 1000 / EditXml.mFTPData.Count;
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
            var frm =(frmAutoDownloading) objfrm;
            FTPDownload mFTP = new FTPDownload();

            DateTime dtNow = DateTime.Now;
            DelegateCentre.UpdateLabel(dtNow.ToString("HH:mm:ss"), frm.label4);
            DelegateCentre.UpdateLabel("", frm.label5);
            DelegateCentre.InitPrgBar(frm.m_PrgBar);

            foreach (var item in EditXml.mFTPData)
            {
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
                    string filePaht = EditXml.strDownloadPath + item.strName + @"\" + FTPfiles[i];
                    mFTP.FTP_Download(filePaht, FTPfiles[i], item.strIP, item.strUser, item.strPwd);
                    if (File.Exists(filePaht))
                        mFTP.FTP_Delete(FTPfiles[i], item.strIP, item.strUser, item.strPwd);
                }

                DelegateCentre.UpdatePrgBar(frm.m_PrgBar);
            }
            DelegateCentre.UpdateLabel((DateTime.Now- dtNow).ToString("HH:mm:ss"), frm.label5);
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
    delegate void UpdatePrgBarCallback( ColorProgressBar Pgr);
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

        public static void UpdatePrgBar( ColorProgressBar Pgr)
        {
            if (Pgr.InvokeRequired)
            {
                UpdatePrgBarCallback myUpdate = new UpdatePrgBarCallback(UpdatePrgBar);
                Pgr.Invoke(myUpdate,  Pgr);
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
