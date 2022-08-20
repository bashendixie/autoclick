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
    public partial class Form4 : Form
    {
        private Form1 form;

        public Form4(Form1 form)
        {
            this.form = form;
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            this.radioButton1.Checked = this.form.settingModel.circleFlag;
            this.radioButton2.Checked = this.form.settingModel.fixedFlag;
            this.textBox1.Text = "" + this.form.settingModel.clickNums;
            this.textBox2.Text = "" + this.form.settingModel.intervals;
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            int clickNums, intervals;
            bool a = int.TryParse(this.textBox1.Text, out clickNums);
            bool b = int.TryParse(this.textBox2.Text, out intervals);

            if(a && b)
            {
                if(intervals<200)
                {
                    intervals = 200;
                }

                this.form.settingModel.circleFlag = this.radioButton1.Checked;
                this.form.settingModel.fixedFlag = this.radioButton2.Checked;
                this.form.settingModel.clickNums = clickNums;
                this.form.settingModel.intervals = intervals;
            }
            else
            {
                MessageBox.Show("数值错误");
                return;
            }

            XmlSerializeHelper.Save<SettingModel>(this.form.settingModel, Application.StartupPath + "\\Setting.xml");
            MessageBox.Show("保存成功");
        }

        /// <summary>
        /// 循环点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                this.textBox1.Enabled = false;
        }

        /// <summary>
        /// 固定次数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked)
                this.textBox1.Enabled = true;
        }
    }
}
