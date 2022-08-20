using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.controls
{
    public partial class UserControl1 : UserControl
    {
        private Form1 form;
        public Mat mat;

        public UserControl1(Form1 form)
        {
            this.form = form;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "png文件|*.png|jpg文件|*.jpg|bmp文件|*.bmp";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = openFileDialog1.FileName;
                this.mat = new Mat(openFileDialog1.FileName);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.form.SetSettingType(SettingModes.ClickByPictures_PositionOne);
        }

        public void SetXYPosition(int x, int y)
        {
            this.textBox5.Text = "" + x;
            this.textBox6.Text = "" + y;
        }

        public bool ClickPositionFlag(out int x1, out int y1)
        {
            int x, y;
            bool a = int.TryParse(this.textBox5.Text, out x);
            bool b = int.TryParse(this.textBox6.Text, out y);
            x1 = x;
            y1 = y;
            return a && b;
        }
    }
}
