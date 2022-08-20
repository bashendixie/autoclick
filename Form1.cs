using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WindowsFormsApp1.controls;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private UserControl1 control1;
        private UserControl2 control2;
        private UserControl3 control3;
        private UserControl4 control4;
        private ClickModes clickModes = ClickModes.ClickByPictures;
        private SettingModes settingModes = SettingModes.NotSettingState;
        public SettingModel settingModel;


        MouseHook mh;
        System.Timers.Timer timer;
        /// <summary>
        /// 用于鼠标点击模式1 点击后再次点击的标记
        /// </summary>
        bool clickFlag = true;
        bool startFlag = false;
        int alts, altd, f1, f5, f6;
        List<string> list = new List<string>();
        /// <summary>
        /// 用于鼠标点击模式2 位置1点击后再次点击的标记
        /// </summary>
        bool clickFlag1 = true;
        /// <summary>
        /// 用于鼠标点击模式2 位置2点击后再次点击的标记
        /// </summary>
        bool clickFlag2 = true;


        #region 系统方法开始

        public Form1()
        {
            InitializeComponent();
            InitializeUserControls();
            this.panel1.Controls.Add(control1);
            this.panel1.Height = 106;
            this.Height = 259;
            settingModel = XmlSerializeHelper.DESerializer<SettingModel>(FileOperationHelper.ReadStringFromFile(Application.StartupPath + "\\Setting.xml", FileMode.Open));
            
            InitializeTimer();

            alts = Win32Api.GlobalAddAtom("Alt-S");
            altd = Win32Api.GlobalAddAtom("Alt-D");
            f1 = Win32Api.GlobalAddAtom("F1");
            f5 = Win32Api.GlobalAddAtom("F5");
            f6 = Win32Api.GlobalAddAtom("F6");
            Win32Api.RegisterHotKey(this.Handle, alts, Win32Api.KeyModifiers.Alt, (int)Keys.S);
            Win32Api.RegisterHotKey(this.Handle, altd, Win32Api.KeyModifiers.Alt, (int)Keys.D);
            Win32Api.RegisterHotKey(this.Handle, f1, Win32Api.KeyModifiers.None, (int)Keys.F1);
            Win32Api.RegisterHotKey(this.Handle, f5, Win32Api.KeyModifiers.None, (int)Keys.F5);
            Win32Api.RegisterHotKey(this.Handle, f6, Win32Api.KeyModifiers.None, (int)Keys.F6);


            /*Mat mat1 = new Mat(@"C:\\Users\\zyh\\Desktop\\111.jpg");
            Mat mat2 = new Mat(@"C:\\Users\\zyh\\Desktop\\222.jpg");
            double m = ImagesTools.Compare_SSIM(mat1, mat2, 3);

            Mat mat4 = new Mat(@"C:\\Users\\zyh\\Desktop\\222.jpg");*/
        }

        private void InitializeUserControls()
        {
            control1 = new UserControl1(this);
            control2 = new UserControl2(this);
            control3 = new UserControl3(this);
            control4 = new UserControl4(this);
        }

        /// <summary>
        /// 监视Windows消息
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case Win32Api.WM_HOTKEY:
                    ProcessHotkey(m);//按下热键时调用ProcessHotkey()函数
                    break;
            }
            base.WndProc(ref m); //将系统消息传递自父类的WndProc
        }

        /// <summary>
        /// 按下设定的键时调用该函数
        /// </summary>
        /// <param name="m"></param>
        private void ProcessHotkey(Message m)
        {
            IntPtr id = m.WParam;//IntPtr用于表示指针或句柄的平台特定类型
            int sid = id.ToInt32();
            if (sid == alts)
            {
                //this.button2_Click(null, null);
            }
            else if (sid == altd)
            {
                //this.button5_Click(null, null);
            }
            else if (sid == f1)
            {
                this.settingModes = SettingModes.NotSettingState;
                GetMonitorArea(1);
                GetMonitorArea(2);
            }
            else if (sid == f5)
            {
                this.button2_Click(null, null);
            }
            else if (sid == f6)
            {
                this.button5_Click(null, null);
            }
        }

        # endregion 系统方法结束



        #region 模式切换开始
        /// <summary>
        /// 根据图像进行点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(control1);
            this.panel1.Height = 106;
            this.Height = 259;

            /*if (this.radioButton1.Checked)
            {
                this.checkBox2.Checked = true;
                this.checkBox2.Enabled = true;
            }*/
        }

        /// <summary>
        /// 根据文字进行点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.clickModes = ClickModes.ClickByText;
            this.panel1.Controls.Clear();
            /*if (this.radioButton2.Checked)
            {
                this.checkBox2.Checked = true;
                this.checkBox2.Enabled = false;
            }*/
        }

        /// <summary>
        /// 根据颜色进行点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.clickModes = ClickModes.ClickByColor;
            this.panel1.Controls.Clear();
        }

        /// <summary>
        /// 自定义进行点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            this.clickModes = ClickModes.ClickByCustomize;
            this.panel1.Controls.Clear();
            this.panel1.Controls.Add(control4);
            this.Height = 672;
        }
        # endregion 模式切换结束




        /// <summary>
        /// 初始化定时器
        /// </summary>
        private void InitializeTimer()
        {
            timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = settingModel.intervals;    //执行间隔时间,单位为毫秒
            timer.Elapsed += new System.Timers.ElapsedEventHandler(myTimedTask);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mh = new MouseHook();
            mh.SetHook();
            mh.MouseMoveEvent += mh_MouseMoveEvent;
            mh.MouseClickEvent += mh_MouseClickEvent;
        }

        private void Form1_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            mh.UnHook();
            if (timer != null) timer.Stop();
            if (this.notifyIcon1 != null) this.notifyIcon1.Dispose();
        }

        private void mh_MouseClickEvent(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //this.position = 0;
                //GetMonitorArea(1);
                //GetMonitorArea(2);
            }
        }

        private void mh_MouseMoveEvent(object sender, MouseEventArgs e)
        {
            int x = e.Location.X;
            int y = e.Location.Y;
            
            switch(settingModes)
            {
                case SettingModes.ClickByPictures_PositionOne:
                    this.control1.SetXYPosition(x, y);
                    break;
            }
        }

        /// <summary>
        /// 开始监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            switch (clickModes)
            {
                case ClickModes.ClickByPictures:
                    if(this.control1.mat == null)
                    {
                        MessageBox.Show("请选择待匹配图片。");
                        return;
                    }
                    break;

            }

            this.BackColor = Color.AliceBlue;
            this.startFlag = true;
            this.timer.Start();


            /*//进行初始判断
            int flag = GetAvaildFlag();
            if(flag == 1)
            {
                MessageBox.Show("左上坐标、右下坐标、鼠标点击位置不能为空。");
                return;
            }
            if(flag == 2)
            {
                MessageBox.Show("左上坐标、右下坐标、鼠标点击位置不能为负数。");
                return;
            }
            if (flag == 3)
            {
                MessageBox.Show("左上坐标的x或y不能小于右下坐标对应x或y。");
                return;
            }
            if (flag == 4)
            {
                MessageBox.Show("左上坐标、右下坐标、鼠标点击位置必须为数值类型。");
                return;
            }

            if(this.checkBox2.Checked)
            {
                if (flag == 10)
                {
                    MessageBox.Show("下面区域的左上坐标、右下坐标不能为空。");
                    return;
                }
                if (flag == 20)
                {
                    MessageBox.Show("下面区域的左上坐标、右下坐标不能为负数。");
                    return;
                }
                if (flag == 30)
                {
                    MessageBox.Show("下面区域的左上坐标的x或y不能小于右下坐标对应x或y。");
                    return;
                }
            }

            
            this.position = 0;
            GetMonitorArea(1);
            GetMonitorArea(2);
            */
        }

        /// <summary>
        /// 结束监控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(240,240,240);
            this.startFlag = false;
            timer.Stop();
        }

        /// <summary>
        /// 进行点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void myTimedTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (startFlag)
            {
                switch (this.clickModes)
                {
                    case ClickModes.ClickByPictures:
                        int x2, y2;
                        if(this.GetShouldClickFlagByPictures(out x2, out y2))
                        {
                            int x1, y1;
                            if(this.control1.ClickPositionFlag(out x1, out y1))
                            {
                                HelpTools.SingleClick(x1, y1);
                            }
                            else
                            {
                                HelpTools.SingleClick(x2, y2);
                            }
                        }
                        break;
                    case ClickModes.ClickByText:

                        break;
                    case ClickModes.ClickByColor:

                        break;
                    case ClickModes.ClickByCustomize:

                        break;

                    default:
                        break;
                }
            }
               


            /*
            {
                if(this.radioButton1.Checked)
                {
                    //进行判断,是否需要点击
                    if (GetShouldClickFlag() && this.clickFlag)
                    {
                        //鼠标移动
                        Win32Api.SetCursorPos(int.Parse(this.textBox5.Text), int.Parse(this.textBox6.Text));

                        //鼠标点击
                        Win32Api.mouse_event(Win32Api.mouseeventf_leftdown, int.Parse(this.textBox5.Text), int.Parse(this.textBox6.Text), 0, 0);
                        Win32Api.mouse_event(Win32Api.mouseeventf_leftup, int.Parse(this.textBox5.Text), int.Parse(this.textBox6.Text), 0, 0);


                        int x, y;
                        if (int.TryParse(this.textBox11.Text, out x) && int.TryParse(this.textBox12.Text, out y))
                        {
                            //鼠标移动
                            Win32Api.SetCursorPos(x, y);

                            //鼠标点击
                            Win32Api.mouse_event(Win32Api.mouseeventf_leftdown, x, y, 0, 0);
                            Win32Api.mouse_event(Win32Api.mouseeventf_leftup, x, y, 0, 0);
                        }


                        this.clickFlag = false;
                        this.list.Add(DateTime.Now.ToLocalTime().ToString());
                    }
                }    
                
                //鼠标点击模式2
                if (this.radioButton2.Checked)
                {
                    //获得文字消失后需要点击的图中黄色文字的数量
                    int count1 = 0;
                    //获得文字消失后不需要点击的图中黄色文字的数量
                    int count2 = 0;

                    //获得文字消失后需要点击的图
                    Bitmap b1 = GetMonitorArea(1);
                    //获得对应的mat
                    Mat m1 = OpenCvSharp.Extensions.BitmapConverter.ToMat(b1);


                    //获得文字消失后不需要点击的图
                    Bitmap b2 = GetMonitorArea(2);
                    //获得对应的mat
                    Mat m2 = OpenCvSharp.Extensions.BitmapConverter.ToMat(b2);

                    //遍历图像像素
                    for (int h = 0; h < m1.Height; h++)
                    {
                        for (int w = 0; w < m1.Width; w++)
                        {
                            Vec3b vec3B = m1.At<Vec3b>(h, w);
                            if ((vec3B.Item0 > 50 && vec3B.Item0 < 150) && (vec3B.Item1 > 180 && vec3B.Item1 < 250) && (vec3B.Item2 > 180 && vec3B.Item2 < 250))
                            {
                                count1++;
                            }
                        }
                    }

                    //遍历图像像素
                    for (int h = 0; h < m2.Height; h++)
                    {
                        for (int w = 0; w < m2.Width; w++)
                        {
                            Vec3b vec3B = m2.At<Vec3b>(h, w);
                            if ((vec3B.Item0 > 50 && vec3B.Item0 < 150) && (vec3B.Item1 > 180 && vec3B.Item1 < 250) && (vec3B.Item2 > 180 && vec3B.Item2 < 250))
                            {
                                count2++;
                            }
                        }
                    }

                    //如果都消失就不点击
                    if(count1==0 && count2==0)
                    {

                    }
                    else
                    {
                        if(count1==0 && this.clickFlag1)
                        {
                            //鼠标移动
                            Win32Api.SetCursorPos(int.Parse(this.textBox5.Text), int.Parse(this.textBox6.Text));

                            //鼠标点击
                            Win32Api.mouse_event(Win32Api.mouseeventf_leftdown, int.Parse(this.textBox5.Text), int.Parse(this.textBox6.Text), 0, 0);
                            Win32Api.mouse_event(Win32Api.mouseeventf_leftup, int.Parse(this.textBox5.Text), int.Parse(this.textBox6.Text), 0, 0);
                            this.clickFlag1 = false;
                        }
                        if (count1 > 0 && !this.clickFlag1) this.clickFlag1 = true;


                        if (count2 == 0 && this.clickFlag2)
                        {
                            int x, y;
                            if (int.TryParse(this.textBox11.Text, out x) && int.TryParse(this.textBox12.Text, out y))
                            {
                                //鼠标移动
                                Win32Api.SetCursorPos(x, y);

                                //鼠标点击
                                Win32Api.mouse_event(Win32Api.mouseeventf_leftdown, x, y, 0, 0);
                                Win32Api.mouse_event(Win32Api.mouseeventf_leftup, x, y, 0, 0);
                                this.clickFlag2 = false;
                            }
                        }
                        if (count2 > 0 && !this.clickFlag2) this.clickFlag2 = true;
                    }
                }
            }*/
        }

        /// <summary>
        /// 判断图片模式是否需要点击
        /// </summary>
        /// <returns></returns>
        public bool GetShouldClickFlagByPictures(out int x1, out int y1)
        {
            //Cv2.ImWrite(@"C:\Users\zyh\Desktop\123456.jpg", mat);

            //全屏截图
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;
            Rectangle smallRect = new Rectangle(0, 0, width, height);
            Rectangle tScreenRect = new Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Bitmap tSrcBmp = new Bitmap(width, height); // 用于屏幕原始图片保存
            Graphics gp = Graphics.FromImage(tSrcBmp);
            gp.CopyFromScreen(0, 0, 0, 0, smallRect.Size);
            gp.DrawImage(tSrcBmp, 0, 0, smallRect, GraphicsUnit.Pixel);

            //全屏截图转mat
            Mat mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(tSrcBmp);
            Cv2.CvtColor(mat, mat, ColorConversionCodes.BGRA2BGR);
            
            //从全屏截图查找待匹配图像
            Mat out1 = new Mat();
            Cv2.MatchTemplate(mat, this.control1.mat, out1, TemplateMatchModes.CCoeffNormed);
            
            //寻找匹配到的最大最小值
            double minVal, maxVal;
            OpenCvSharp.Point minLoc, maxLoc;
            Cv2.MinMaxLoc(out1, out minVal, out maxVal, out minLoc, out maxLoc, null);
            //Cv2.Rectangle(mat, maxLoc, new OpenCvSharp.Point(maxLoc.X + this.control1.mat.Width, maxLoc.Y + this.control1.mat.Height), 255, 2);

            //获取匹配到的部分图像
            Mat child = new Mat(mat, new OpenCvSharp.Rect(maxLoc.X, maxLoc.Y, this.control1.mat.Width, this.control1.mat.Height));

            x1 = maxLoc.X + this.control1.mat.Width / 2;
            y1 = maxLoc.Y + this.control1.mat.Height / 2;

            //比较匹配到的和原图的相似度
            double ssim = ImagesTools.Compare_SSIM(child.Clone(), this.control1.mat.Clone());
            Console.WriteLine("相似性:" + ssim);
            if (ssim > 0.9)
            {
                return true;
            }

            System.GC.Collect();
            return false;
        }

        /// <summary>
        /// 判断文字模式是否需要点击
        /// </summary>
        /// <returns></returns>
        public bool GetShouldClickFlagByText()
        {
            return false;
        }

        /// <summary>
        /// 判断颜色模式是否需要点击
        /// </summary>
        /// <returns></returns>
        public bool GetShouldClickFlagByColor()
        {
            return false;
        }

        /// <summary>
        /// 鼠标点击模式1判断是否需要点击
        /// </summary>
        /// <returns></returns>
        public bool GetShouldClickFlag()
        {
            /*//获得文字消失后需要点击的图
            Bitmap b1 = GetMonitorArea(1);

            //获得对应的mat
            Mat m1 = OpenCvSharp.Extensions.BitmapConverter.ToMat(b1);

            //获得文字消失后需要点击的图中黄色文字的数量 
            int count1 = 0;
            //获得文字消失后不需要点击的图中黄色文字的数量
            int count2 = 0;

            //R：180 - 250
            //G：180 - 250
            //B：50 - 150

            //遍历图像像素
            for (int h=0; h< m1.Height; h++)
            {
                for (int w = 0; w < m1.Width; w++)
                {
                    Vec3b vec3B = m1.At<Vec3b>(h, w);
                    if((vec3B.Item0>50 && vec3B.Item0<150) && (vec3B.Item1 > 180 && vec3B.Item1 < 250) && (vec3B.Item2 > 180 && vec3B.Item2 < 250))
                    {
                        count1++;
                    }
                }
            }

            if(this.checkBox2.Checked)
            {
                //获得文字消失后不需要点击的图
                Bitmap b2 = GetMonitorArea(2);
                //获得对应的mat
                Mat m2 = OpenCvSharp.Extensions.BitmapConverter.ToMat(b2);

                //遍历图像像素
                for (int h = 0; h < m2.Height; h++)
                {
                    for (int w = 0; w < m2.Width; w++)
                    {
                        Vec3b vec3B = m2.At<Vec3b>(h, w);
                        if ((vec3B.Item0 > 50 && vec3B.Item0 < 150) && (vec3B.Item1 > 180 && vec3B.Item1 < 250) && (vec3B.Item2 > 180 && vec3B.Item2 < 250))
                        {
                            count2++;
                        }
                    }
                }
            }

            System.Console.WriteLine("1:" + count1);
            System.Console.WriteLine("2:" + count2);

            //如果上下都不为空，则标记为下次需要点击
            if (count1>0 && (this.checkBox2.Checked ? (count2>0) : true))
            {
                this.clickFlag = true;
            }

            //如果上面为空，下面不为空，则进行点击
            if (count1 == 0 && (this.checkBox2.Checked ? (count2 > 0) : true)) return true;
*/
            return false;
        }


        /// <summary>
        /// 获取监控区域的截图
        /// </summary>
        /// <returns></returns>
        public Bitmap GetMonitorArea(int type)
        {
            return null;
            /*if (this.position > 0) return null;

            Bitmap tSrcBmp = null;
            if (type == 1)
            {
                if(string.IsNullOrEmpty(this.textBox1.Text) || string.IsNullOrEmpty(this.textBox2.Text)
                    || string.IsNullOrEmpty(this.textBox3.Text) || string.IsNullOrEmpty(this.textBox4.Text))
                {
                    return null;
                }

                //截图
                int width = int.Parse(this.textBox3.Text) - int.Parse(this.textBox1.Text);
                int height = int.Parse(this.textBox4.Text) - int.Parse(this.textBox2.Text);
                int x1 = int.Parse(this.textBox1.Text);
                int x2 = int.Parse(this.textBox3.Text);
                int y1 = int.Parse(this.textBox2.Text);
                int y2 = int.Parse(this.textBox4.Text);

                if (width < 0 || height < 0) return null;

                Rectangle smallRect = new Rectangle(0, 0, width, height);
                Rectangle tScreenRect = new Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                tSrcBmp = new Bitmap(width, height); // 用于屏幕原始图片保存
                Graphics gp = Graphics.FromImage(tSrcBmp);
                //gp.CopyFromScreen(0, 0, 0, 0, tScreenRect.Size);
                //gp.DrawImage(tSrcBmp, 0, 0, tScreenRect, GraphicsUnit.Pixel);
                gp.CopyFromScreen(x1, y1, 0, 0, smallRect.Size);
                gp.DrawImage(tSrcBmp, 0, 0, smallRect, GraphicsUnit.Pixel);

                this.pictureBox1.Image = (Image)tSrcBmp.Clone();
                
            }
            else if(type ==2 )
            {
                if (string.IsNullOrEmpty(this.textBox7.Text) || string.IsNullOrEmpty(this.textBox8.Text)
                    || string.IsNullOrEmpty(this.textBox9.Text) || string.IsNullOrEmpty(this.textBox10.Text))
                {
                    return null;
                }

                //截图
                int width = int.Parse(this.textBox9.Text) - int.Parse(this.textBox7.Text);
                int height = int.Parse(this.textBox10.Text) - int.Parse(this.textBox8.Text);
                int x1 = int.Parse(this.textBox7.Text);
                int x2 = int.Parse(this.textBox9.Text);
                int y1 = int.Parse(this.textBox8.Text);
                int y2 = int.Parse(this.textBox10.Text);

                if (width < 0 || height < 0) return null;

                Rectangle smallRect = new Rectangle(0, 0, width, height);
                Rectangle tScreenRect = new Rectangle(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                tSrcBmp = new Bitmap(width, height); // 用于屏幕原始图片保存
                Graphics gp = Graphics.FromImage(tSrcBmp);
                //gp.CopyFromScreen(0, 0, 0, 0, tScreenRect.Size);
                //gp.DrawImage(tSrcBmp, 0, 0, tScreenRect, GraphicsUnit.Pixel);
                gp.CopyFromScreen(x1, y1, 0, 0, smallRect.Size);
                gp.DrawImage(tSrcBmp, 0, 0, smallRect, GraphicsUnit.Pixel);

                this.pictureBox2.Image = (Image)tSrcBmp.Clone();
            }
            System.GC.Collect();

            return (Bitmap)tSrcBmp.Clone();*/
        }

        /// <summary>
        /// 判断左上和右下和鼠标点击设置是否正确
        /// </summary>
        /// <returns></returns>
        public int GetAvaildFlag()
        {
            /*if (string.IsNullOrEmpty(this.textBox1.Text) || string.IsNullOrEmpty(this.textBox2.Text)
                 || string.IsNullOrEmpty(this.textBox3.Text) || string.IsNullOrEmpty(this.textBox4.Text)
                  || string.IsNullOrEmpty(this.textBox5.Text) || string.IsNullOrEmpty(this.textBox6.Text))
            {
                return 1;
            }

            if(this.checkBox2.Checked)
            {
                if (string.IsNullOrEmpty(this.textBox7.Text) || string.IsNullOrEmpty(this.textBox8.Text)
                  || string.IsNullOrEmpty(this.textBox9.Text) || string.IsNullOrEmpty(this.textBox10.Text))
                {
                    return 10;
                }
            }

            try
            {
                int x1 = int.Parse(this.textBox1.Text);
                int y1 = int.Parse(this.textBox2.Text);

                int x2 = int.Parse(this.textBox3.Text);
                int y2 = int.Parse(this.textBox4.Text);

                int x3 = int.Parse(this.textBox5.Text);
                int y3 = int.Parse(this.textBox6.Text);

                if (x1<0 || y1<0 || x2<0 || y2<0 || x3<0 || y3 <0)
                {
                    return 2;
                }

                if ((x2 - x1) <0  || (y2 - y1) < 0)
                {
                    return 3;
                }

                if (this.checkBox2.Checked)
                {
                    int x4 = int.Parse(this.textBox7.Text);
                    int y4 = int.Parse(this.textBox8.Text);

                    int x5 = int.Parse(this.textBox9.Text);
                    int y5 = int.Parse(this.textBox10.Text);

                    if (x4 < 0 || y4 < 0 || x5 < 0 || y5 < 0)
                    {
                        return 20;
                    }

                    if ((x5 - x4) < 0 || (y5 - y4) < 0)
                    {
                        return 30;
                    }
                }
            }
            catch(Exception e)
            {
                return 4;
            }*/

            return 100;
        }

        
        /// <summary>
        /// 设置设置文字消失后点击鼠标的监控区域左上坐标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            //this.position = 1;
        }

        /// <summary>
        /// 设置设置文字消失后点击鼠标的监控区域右下坐标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            //this.position = 2;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //this.position = 6;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.SetValues(this.list);
            form.ShowDialog();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // 判断只有最小化时，隐藏窗体
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            // 正常显示窗体
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            form.ShowDialog();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            /*if(this.checkBox2.Checked)
            {
                this.textBox7.Enabled = true;
                this.textBox8.Enabled = true;
                this.textBox9.Enabled = true;
                this.textBox10.Enabled = true;
                this.button1.Enabled = true;
                this.button3.Enabled = true;
            }
            else
            {
                this.textBox7.Enabled = false;
                this.textBox8.Enabled = false;
                this.textBox9.Enabled = false;
                this.textBox10.Enabled = false;
                this.button1.Enabled = false;
                this.button3.Enabled = false;
            }*/
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form4 form = new Form4(this);
            form.ShowDialog();
        }

        public void SetSettingType(SettingModes settingModes)
        {
            this.settingModes = settingModes;
        }

        /// <summary>
        /// 设置文字消失后不点击鼠标的监控区域左上坐标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //this.position = 3;
        }

        /// <summary>
        /// 设置文字消失后不点击鼠标的监控区域右下坐标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //this.position = 4;
        }

        /// <summary>
        /// 设置鼠标点击位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            //this.position = 5;
        }
    }
}
