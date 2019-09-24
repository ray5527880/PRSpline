using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PRSpline
{
    public partial class frmSetup : Form
    {
        public int m_nCount = 0;
        public int m_nIndex = 0;
        public int selectNo = -1;
        public frmSetup()
        {
            InitializeComponent();
        }

        private void frmSetup_Load(object sender, EventArgs e)
        {
            DGVSetup.ColumnCount = 5;
            DGVSetup.Columns[0].Name = "名稱";
            DGVSetup.Columns[1].Name = "IP位置";
            DGVSetup.Columns[2].Name = "帳號";
            DGVSetup.Columns[3].Name = "密碼";
            DGVSetup.Columns[4].Name = "No";

            DGVSetup.Columns[0].Width = 175;
            DGVSetup.Columns[1].Width = 175;
            DGVSetup.Columns[2].Width = 100;
            DGVSetup.Columns[3].Width = 100;
            DGVSetup.Columns[4].Visible = false;

            DGVSetup.ReadOnly = true;
            DGVSetup.Rows.Clear();
            UpdataView();
            txtPath.Text = EditXml.strDownloadPath;
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
                    item.strName,item.strIP,item.strUser,item.strPwd,count.ToString()
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
                EditXml.FTPData _FTPData = new EditXml.FTPData()
                {
                    strName = txtName.Text,
                    strIP = txtIP.Text,
                    strUser = txtUser.Text,
                    strPwd = txtPwd.Text
                };
                EditXml.mFTPData[selectNo] = _FTPData;
            }
            else
            {
                EditXml.FTPData _FTPData = new EditXml.FTPData()
                {
                    strName = txtName.Text,
                    strIP = txtIP.Text,
                    strUser = txtUser.Text,
                    strPwd = txtPwd.Text
                };
                EditXml.mFTPData.Add(_FTPData);
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
                        EditXml.mFTPData.RemoveAt(Convert.ToInt32(item.Cells[4].Value) - index);
                        index++;
                    }
                }
            }
            else
            {
                EditXml.mFTPData.RemoveAt(selectNo);
            }
            UpdataView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {       
            string _Message= EditXml.SaveXml(txtPath.Text);
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
                selectNo = Convert.ToInt32(DGVSetup.Rows[m_nIndex].Cells[4].Value);
            }
        }
    }
}
