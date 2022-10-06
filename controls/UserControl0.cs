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
    public partial class UserControl0 : UserControl
    {
        private Form1 form;
        public Mat mat;

        public UserControl0(Form1 form)
        {
            this.form = form;
            InitializeComponent();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.form.SetSettingType(SettingModes.ClickByStep_PositionOne);
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
