using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BF_FW;

namespace PRSpline
{
    public partial class frmSelectGroup : Form
    {
        public frmSelectGroup()
        {
            InitializeComponent();
        }

        private void frmSelectGroup_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "No";
            dataGridView1.Columns[1].Name = "日期";
            dataGridView1.Columns[2].Name = "主檔名稱";
            dataGridView1.Columns[3].Name = "備註";

            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 90;
            dataGridView1.Columns[2].Width = 300;
            dataGridView1.Columns[3].Width = 200;
            dataGridView1.ReadOnly = true;
            dataGridView1.Rows.Clear();
            UpdataView();
            btnAdd.Enabled = false;
            btnUpdata.Enabled = false;
            btnDel.Enabled = false;
            btnLoad.Enabled = false;
        }
        private void UpdataView()
        {
            dataGridView1.Rows.Clear();
            foreach (var item in Group.GroupDatas)
            {
                string[] str = new string[] { item.No.ToString(), item.dates.ToString("yyyy-MM-dd"), item.MainFileName, item.Remarks };
                dataGridView1.Rows.Add(str);
            }
            dataGridView1.Rows[0].Cells[0].Selected = false;
            dataGridView1.Rows[Group.GroupDatas.Count()].Selected = true;
            dataGridView1.Rows[Group.GroupDatas.Count()].Cells[0].Value = "新增...";
            //m_nIndex = m_nCount;
            dataGridView1.Refresh();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() != "新增...")
            {
                int _no = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                var item = from selectData in Group.GroupDatas
                           where selectData.No == _no
                           select selectData;
                labNo.Text = (item.ToArray())[0].No.ToString();
                labDate.Text = (item.ToArray())[0].dates.ToString("yyyy-MM-dd");
                labMainName.Text= (item.ToArray())[0].MainFileName;
                textBox2.Text= (item.ToArray())[0].Remarks;
                //labDate.Text=()
            }
        }
    }
}
