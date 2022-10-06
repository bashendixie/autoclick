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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();

            this.richTextBox2.Text += "用户必须设置点击位置，之后程序会按基础设置的间隔进行鼠标左键单击";

            this.richTextBox1.Text += "程序在后台会按设定的时间间隔自动进行屏幕截取，然后自动匹配用户上传的图像的位置(请从屏幕截图，匹配效果能好一些)。\n";
            this.richTextBox1.Text += "如果用户设置了点击位置，则自动将鼠标移动过去进行鼠标左键单击。\n";
            this.richTextBox1.Text += "如果用户没有设置点击位置，则自动点击匹配到的图片的中心位置。\n";
            this.richTextBox1.Text += "目前只支持寻找一个匹配目标。\n";
        }

    }
}
