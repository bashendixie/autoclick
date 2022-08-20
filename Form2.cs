using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }


        public void SetValues(List<string> list)
        {
            this.richTextBox1.Text = "";

            for(int i = 0; i < list.Count; i++)
            {
                this.richTextBox1.Text += list[i] + "\n";
            }
        }
    }
}
