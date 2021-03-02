using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using BF_FW;
using BF_FW.data;

namespace PRSpline
{
    public partial class frmVoltageSag : Form
    {
        private VoltageSagChart frm;
        public VoltageSagData.voltageSagData SelectData;
        public VoltageSagData.voltageSagData[] voltageSagDatas;
        public frmVoltageSag()
        {
            InitializeComponent();
        }
        int[] baseVoltage;
        private void frmVoltageSag_Load(object sender, EventArgs e)
        {

            EditXml mEditXml = new EditXml();
            mEditXml.GetXmlData();
            cbxRalay.Items.Clear();
            baseVoltage = new int[EditXml.mFTPData.Count];
            int index = 0;
            foreach (var item in EditXml.mFTPData)
            {
                cbxRalay.Items.Add(item.strName);
                baseVoltage[index] = item.BaseValue;
                index++;
            }
            cbxRalay.SelectedIndex = 0;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            panChart.Controls.Clear();
            string filePaht = string.Format(@"./downloadFile/{0}/{0}.xml", cbxRalay.SelectedItem);
            var VSData = new VoltageSagXml(filePaht);

            var data = VSData.GetXmlData();

            var datas = new List<VoltageSagData.voltageSagData>();
            foreach (var item in data)
            {
                if (item.treggerDateTime >= dateTimePicker1.Value.AddDays(-1) && item.treggerDateTime < dateTimePicker2.Value)
                {
                    datas.Add(item);
                }
            }
            voltageSagDatas = datas.ToArray();
            UpdataCbxItem(datas.ToArray());
            frm = new VoltageSagChart(datas.ToArray());
            frm.Width = panChart.Width;
            frm.Height = panChart.Height;
            panChart.Controls.Add(frm);
        }

        private void frmVoltageSag_SizeChanged(object sender, EventArgs e)
        {
            this.panel2.Location = new Point((this.Width - panel2.Width) / 2 + 12, this.panel2.Location.Y);
            this.panel3.Location = new Point((this.Width - panel3.Width) / 2 + 12, this.panel3.Location.Y);
            if (panChart.Controls.Count != 0)
            {
                panChart.Controls.Clear();
                string filePaht = string.Format(@"./downloadFile/{0}/{0}.xml", cbxRalay.SelectedItem);
                var VSData = new VoltageSagXml(filePaht);

                var data = VSData.GetXmlData();

                var datas = new List<VoltageSagData.voltageSagData>();
                foreach (var item in data)
                {
                    if (item.treggerDateTime >= dateTimePicker1.Value.AddDays(-1) && item.treggerDateTime < dateTimePicker2.Value)
                    {
                        datas.Add(item);
                    }
                }
                voltageSagDatas = datas.ToArray();
                UpdataCbxItem(datas.ToArray());
                frm = new VoltageSagChart(datas.ToArray());
                frm.Width = panChart.Width;
                frm.Height = panChart.Height;
                panChart.Controls.Add(frm);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string FolderPaht = string.Format(@"./downloadFile/{0}/", cbxRalay.SelectedItem);
            DirectoryInfo di = new DirectoryInfo(FolderPaht);
            var FileName = (dynamic)this.cbxItem.SelectedItem;
            string filePath = string.Empty;

            foreach (var item in di.GetFiles())
            {
                if (item.ToString().IndexOf((string)FileName.Value) > -1)
                {
                    filePath = FolderPaht + item.ToString();
                    break;
                }
            }
            if (filePath != string.Empty)
            {
                var frm = new VoltageSagChart1(filePath, baseVoltage[cbxRalay.SelectedIndex]);
                frm.mVoltageSagData = voltageSagDatas[cbxItem.SelectedIndex];
                //frm.VSStartTime
                frm.Show();
            }
            else
            {
                MessageBox.Show("查無此檔案");
            }

        }
        private void UpdataCbxItem(VoltageSagData.voltageSagData[] datas)
        {
            this.cbxItem.DataSource = null;
            this.cbxItem.Items.Clear();
            ArrayList alData = new ArrayList();
            foreach (var item in datas)
            {
                cbxItem.Items.Add("");
                string str = item.treggerDateTime.ToString("yyyy-MM-dd_HH-mm-ss") + " 持續時間：" + item.duration + "ms 持續週期：" + item.cycle
                    + "cycle 電壓：R=" + item.PValue + "pu; S=" + item.QValue + "pu; T=" + item.SValue + "pu";
                string strTime = item.treggerDateTime.ToString("yyyy-MM-dd_HH-mm-ss");
                alData.Add(new DictionaryEntry(str, strTime));
            }
            this.cbxItem.DisplayMember = "Key";
            this.cbxItem.ValueMember = "Value";
            this.cbxItem.DataSource = alData;
            if (alData.Count > 0)
            {
                this.button1.Enabled = true;
                this.cbxItem.Enabled = true;
                this.cbxItem.SelectedIndex = 0;
            }
            else
            {
                this.button1.Enabled = false;
                this.cbxItem.Enabled = false;
            }
        }

        private void cbxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectData = voltageSagDatas[this.cbxItem.SelectedIndex];
        }

        private void btnVS_Click(object sender, EventArgs e)
        {
            var frm = new frmDescription();
            frm.ShowDialog();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
