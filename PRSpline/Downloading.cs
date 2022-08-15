using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Threading.Tasks;

using BF_FW;

using System.IO;
namespace PRSpline
{
    public partial class Downloading : Form
    {
        private int index;
        public Downloading(int _index)
        {
            InitializeComponent();
            index = _index;
        }
        private void Downloading_Load(object sender, EventArgs e)
        {
            var mFTP = new FTPDownload();

            this.colorProgressBar1.Step = 100 / EditXml.mFTPData.Count;

            Task.Factory.StartNew(() =>
            {
                if (index == 0)
                {
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

                            Logger.MakeLogger(FunctionName,string.Format("{0} 連線失敗", item.strName));
                            continue;
                        }
                        string[] FTPfiles = mFTP.GetFTPFileName(item.strIP, item.strUser, item.strPwd);
                        for (int i = 0; i < FTPfiles.Length; i++)
                        {
                            string filePaht = EditXml.strDownloadPath + item.strName + @"\" + FTPfiles[i];
                            mFTP.FTP_Download(filePaht, FTPfiles[i], item.strIP, item.strUser, item.strPwd);
                            //if (File.Exists(filePaht))
                                //mFTP.FTP_Delete(FTPfiles[i], item.strIP, item.strUser, item.strPwd);
                        }
                        this.colorProgressBar1.PerformStep();
                    }
                }
                else
                {
                    int count = 0;
                    bool IsValidConnection = false;
                    while (count < 3 && !IsValidConnection)
                    {
                        IsValidConnection = mFTP.CheckConnection(EditXml.mFTPData[index - 1].strIP, EditXml.mFTPData[index - 1].strUser, EditXml.mFTPData[index - 1].strPwd);
                        count++;
                    }
                    if (!IsValidConnection)
                    {
                        string FunctionName = MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name;

                        Logger.MakeLogger(FunctionName, string.Format("{0} 連線失敗", EditXml.mFTPData[index - 1].strName));
                    }
                    string[] FTPfiles = mFTP.GetFTPFileName(EditXml.mFTPData[index - 1].strIP, EditXml.mFTPData[index - 1].strUser, EditXml.mFTPData[index - 1].strPwd);
                    for (int i = 0; i < FTPfiles.Length; i++)
                    {
                        if (!Directory.Exists(EditXml.strDownloadPath + EditXml.mFTPData[index - 1].strName + @"\"))
                            Directory.CreateDirectory(EditXml.strDownloadPath + EditXml.mFTPData[index - 1].strName + @"\");

                        string filePaht = EditXml.strDownloadPath + EditXml.mFTPData[index - 1].strName + @"\" + FTPfiles[i];
                        mFTP.FTP_Download(filePaht, FTPfiles[i], EditXml.mFTPData[index - 1].strIP, EditXml.mFTPData[index - 1].strUser, EditXml.mFTPData[index - 1].strPwd);
                        //if (File.Exists(filePaht))
                           // mFTP.FTP_Delete(FTPfiles[i], EditXml.mFTPData[index - 1].strIP, EditXml.mFTPData[index - 1].strUser, EditXml.mFTPData[index - 1].strPwd);
                    }
                    this.colorProgressBar1.PerformStep();
                }
            }).ContinueWith(antecedent =>
            {
                this.colorProgressBar1.Value = this.colorProgressBar1.Maximum;
                if (DialogResult.OK == MessageBox.Show("完成", "", MessageBoxButtons.OK))
                {
                    this.BeginInvoke(new Action(() => { this.Close(); }));
                }
            });
        }
    }
}
