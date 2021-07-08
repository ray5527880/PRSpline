using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BF_FW;
using Microsoft.VisualBasic.FileIO;

namespace PRSpline
{
    public partial class frmSetup : Form
    {
        public int m_nCount = 0;
        public int m_nIndex = 0;
        public int selectNo = -1;

        public string ConnectionAlarms;

        public frmSetup()
        {
            InitializeComponent();
        }

        private void frmSetup_Load(object sender, EventArgs e)
        {
            DGVSetup.ColumnCount = 6;
            DGVSetup.Columns[0].Name = "名稱";
            DGVSetup.Columns[1].Name = "IP位置";
            DGVSetup.Columns[2].Name = "帳號";
            DGVSetup.Columns[3].Name = "密碼";
            DGVSetup.Columns[4].Name = "基準電壓";
            DGVSetup.Columns[5].Name = "No";

            DGVSetup.Columns[0].Width = 175;
            DGVSetup.Columns[1].Width = 175;
            DGVSetup.Columns[2].Width = 100;
            DGVSetup.Columns[3].Width = 100;
            DGVSetup.Columns[4].Width = 100;
            DGVSetup.Columns[5].Visible = false;

            DGVSetup.ReadOnly = true;
            DGVSetup.Rows.Clear();

            ConnectionAlarms = "server=" + EditXml.DBPath + "; database=" + EditXml.DBName + ";uid=" + EditXml.DBUser + ";pwd=" + EditXml.DBPwd;

            UpdataView();
        }
        private void UpdataView()
        {            
            DGVSetup.Rows.Clear();
            int count = 0;
            m_nCount = EditXml.mFTPData.Count;
            foreach (var item in EditXml.mFTPData)
            {
                string[] arr = new string[]
                {
                    item.strName,item.strIP,item.strUser,item.strPwd,item.BaseValue.ToString(),count.ToString()
                };
                DGVSetup.Rows.Add(arr);
                count++;
            }
            DGVSetup.Rows[0].Cells[0].Selected = false;
            DGVSetup.Rows[m_nCount].Selected = true;
            DGVSetup.Rows[m_nCount].Cells[0].Value = "新增...";
            m_nIndex = m_nCount;
            DGVSetup.Refresh();
        }

        private void btnApp_Click(object sender, EventArgs e)
        {
            if (m_nIndex < m_nCount && m_nIndex > -1)
            {
                string OldName = EditXml.mFTPData[selectNo].strName;
                EditXml.FTPData _FTPData = new EditXml.FTPData()
                {
                    strName = txtName.Text,
                    strIP = txtIP.Text,
                    strUser = txtUser.Text,
                    strPwd = txtPwd.Text,
                    BaseValue = Convert.ToInt32(txtBaseValue.Text)
                };
                EditXml.mFTPData[selectNo] = _FTPData;
                if (_FTPData.strName != OldName)
                {
                    var strPath = System.IO.Directory.GetCurrentDirectory() + @"\downloadFile\";
                    FileSystem.RenameFile(Path.GetFullPath(strPath + @"\" + OldName + @"\" + OldName + ".xml"), txtName.Text + ".xml");
                    FileSystem.RenameDirectory(Path.GetFullPath(strPath + @"\" + OldName), txtName.Text);
                }
             //   var m_tVSIEDName = new tVSIEDName(ConnectionAlarms);
               // var VSIEDData = m_tVSIEDName.GetData(OldName);
                
                string _Message = EditXml.SaveXml();

              //  bool IsSuccess = m_tVSIEDName.UpDateData(VSIEDData, txtName.Text);

                if (_Message == string.Empty)
                {
                    MessageBox.Show("修改成功");
                    //if (IsSuccess)
                    //    MessageBox.Show("修改成功");
                    //else
                    //    MessageBox.Show("修改失敗 資料庫錯誤");
                }
                else
                    MessageBox.Show("修改失敗 錯誤訊息：" + _Message);
            }
            else
            {
                var strPath = System.IO.Directory.GetCurrentDirectory() + @"\downloadFile\" + txtName.Text;

                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                var VSXml = new VoltageSagXml(strPath + @"\" + txtName.Text + ".xml");

                bool isCreate = VSXml.CreateFile();
                if (isCreate)
                {
                    EditXml.FTPData _FTPData = new EditXml.FTPData()
                    {
                        strName = txtName.Text,
                        strIP = txtIP.Text,
                        strUser = txtUser.Text,
                        strPwd = txtPwd.Text,
                        BaseValue = Convert.ToInt32(txtBaseValue.Text)
                    };
                    EditXml.mFTPData.Add(_FTPData);
                    string _Message = EditXml.SaveXml();

                  //  var m_tVSIEDName = new tVSIEDName(ConnectionAlarms);
                    //bool IsSuccess = m_tVSIEDName.AddData(txtName.Text);

                    if (_Message == string.Empty)
                    {
                        MessageBox.Show("新增成功");
                        //if (IsSuccess)
                        //    MessageBox.Show("新增成功");
                        //else
                        //    MessageBox.Show("新增失敗 資料庫錯誤");
                    }
                    else
                        MessageBox.Show("新增失敗 錯誤訊息：" + _Message);
                }
                else
                    MessageBox.Show("新增失敗");
            }
            UpdataView();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (DGVSetup.SelectedRows.Count > 1)
            {
                int index = 0;
                foreach (DataGridViewRow item in DGVSetup.SelectedRows)
                {
                    if (item.Cells[0].Value.ToString() != "新增...")
                    {
                        EditXml.mFTPData.RemoveAt(Convert.ToInt32(item.Cells[5].Value) - index);

                        string _Message = EditXml.SaveXml();

                       // var m_tVSIEDName = new tVSIEDName(ConnectionAlarms);
                        //bool IsSuccess = m_tVSIEDName.DeleteData(item.Cells[0].Value.ToString());

                        if (_Message == string.Empty)
                        {
                            //if (IsSuccess)
                            MessageBox.Show("刪除成功");
                            //else
                            //    MessageBox.Show("刪除失敗 資料庫錯誤");
                        }
                        else
                            MessageBox.Show("刪除失敗 錯誤訊息：" + _Message);

                        index++;
                    }
                }
            }
            else
            {
                string OldName = EditXml.mFTPData[selectNo].strName;
                EditXml.mFTPData.RemoveAt(selectNo);
                string _Message = EditXml.SaveXml();

                //var m_tVSIEDName = new tVSIEDName(ConnectionAlarms);
                //bool IsSuccess = m_tVSIEDName.DeleteData(OldName);

                if (_Message == string.Empty)
                {
                    //if (IsSuccess)
                    MessageBox.Show("刪除成功");
                    //else
                    //    MessageBox.Show("刪除失敗 資料庫錯誤");
                }
                else
                    MessageBox.Show("刪除失敗 錯誤訊息：" + _Message);
            }
            UpdataView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {       
            string _Message= EditXml.SaveXml();
            if (_Message == string.Empty)
                MessageBox.Show("儲存成功");
            else
                MessageBox.Show("儲存失敗 錯誤訊息：" + _Message);
        }

        private void DGVSetup_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            m_nIndex = e.RowIndex;

            if (DGVSetup.SelectedRows.Count > 1)
                btnApp.Enabled = false;
            else
                btnApp.Enabled = true;

            if (m_nIndex < m_nCount && m_nIndex > -1)
            {
                txtName.Text = DGVSetup.Rows[m_nIndex].Cells[0].Value.ToString();
                txtIP.Text = DGVSetup.Rows[m_nIndex].Cells[1].Value.ToString();
                txtUser.Text = DGVSetup.Rows[m_nIndex].Cells[2].Value.ToString();
                txtPwd.Text = DGVSetup.Rows[m_nIndex].Cells[3].Value.ToString();
                txtBaseValue.Text = DGVSetup.Rows[m_nIndex].Cells[4].Value.ToString();
                selectNo = Convert.ToInt32(DGVSetup.Rows[m_nIndex].Cells[5].Value);
            }
        }
    }
}
