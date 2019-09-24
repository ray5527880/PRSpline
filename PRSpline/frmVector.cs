using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace PRSpline
{
    public partial class frmVector : Form
    {
        private int XMax, XMin, YMax, YMin;
        private string[] DeviceName;
        private int VectorLimit = 4;
        private List<Vector> mVector;
        private List<Button> mButton;
        public frmVector()
        {
            InitializeComponent();
        }
        public void GetDeviceName(string [] _Name)
        {
            DeviceName = _Name;
        }
        public void GetDeviceData()
        {

        }
        private void frmVector_Load(object sender, EventArgs e)
        {
            btnADD.Image = Image.FromFile("./res/ADD.png");
            btnLOSE.Image = Image.FromFile("./res/LOSE.png");
            btnLOSE.Enabled = false;
            mVector = new List<Vector>();
            mButton = new List<Button>();
            int[] x = new int[10];
            this.TopMost = true;
            this.BackColor = Color.LightSlateGray;
            this.cbxRefenence.Items.Add("---");
            this.cbxRefenence.SelectedIndex = 0;
            foreach (string _str in DeviceName)
            {
                this.cbxRefenence.Items.Add(_str);
            }
            
            Vector _vector = new Vector(DeviceName);
            this.Controls.Add(_vector);
            _vector.Show();
            mVector.Add(_vector);
            UpdataForm();
        }
        private void UpdataChart()
        {

        }
        private void UpdataForm()
        {
            for (int i = 0; i < mVector.Count; i++)
            {
                mVector[i].Location = new Point(i * 370 + 10, 140);
            }
            for (int i = 0; i < mButton.Count; i++)
            {
                mButton[i].Location = new Point((i+1) * 370 + 330, 100);
            }
            this.panel1.Location = new Point(160 + mButton.Count * 370, this.panel1.Location.Y);
            this.Width = 400 + (mVector.Count - 1) * 370;
        }
       
        private void btnADD_Click(object sender, EventArgs e)
        {
            Button _btn = new Button();
            _btn.Size = btnLOSE.Size;
            _btn.Text = btnLOSE.Text;
            _btn.Image = btnLOSE.Image;
            _btn.FlatStyle = btnLOSE.FlatStyle;
            _btn.FlatAppearance.BorderSize = 0;
            this.Controls.Add(_btn);
            _btn.Click += btn_Click;
            mButton.Add(_btn);

            Vector _vector = new Vector(DeviceName);
            this.Controls.Add(_vector);
            _vector.Show();
            mVector.Add(_vector);
            UpdataForm();
        }
        public void DELVector()
        {
            mVector.RemoveAt(mVector.Count - 1);
        }

        private void btn_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < mButton.Count; i++)
            {
                if (sender == mButton[i])
                {
                    mVector[i + 1].Dispose();
                    mVector.RemoveAt(i+1);
                    mButton[i].Dispose();
                    mButton.RemoveAt(i);
                    break;
                }
            }
            UpdataForm();
        }

        private void frmVector_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMain.btnVectorClick();
        }
    }
}
