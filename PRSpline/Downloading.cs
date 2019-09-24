using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading.Tasks;

using System.IO;
namespace PRSpline
{
    public partial class Downloading : Form
    {
        private int index;
        public Downloading(int _index )
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
                        string[] FTPfiles = mFTP.GetFTPFileName(item.strIP, item.strUser, item.strPwd);
                        for (int i = 0; i < FTPfiles.Length; i++)
                        {
                            if (FTPfiles[i].IndexOf(item.strName) > -1)
                            {
                                mFTP.FTP_Download(FTPfiles[i], item.strIP, item.strUser, item.strPwd);
                            }
                        }
                        this.colorProgressBar1.PerformStep();
                    }
                }
                else
                {
                    string[] FTPfiles = mFTP.GetFTPFileName(EditXml.mFTPData[index - 1].strIP, EditXml.mFTPData[index - 1].strUser, EditXml.mFTPData[index - 1].strPwd);
                    for (int i = 0; i < FTPfiles.Length; i++)
                    {
                        if (FTPfiles[i].IndexOf(EditXml.mFTPData[index - 1].strName) > -1)
                        {
                            mFTP.FTP_Download(FTPfiles[i], EditXml.mFTPData[index - 1].strIP, EditXml.mFTPData[index - 1].strUser, EditXml.mFTPData[index - 1].strPwd);
                        }
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
