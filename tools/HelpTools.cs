using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class HelpTools
    {
        /// <summary>
        /// 单击
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void SingleClick(int x, int y)
        {
            //鼠标移动
            Win32Api.SetCursorPos(x, y);

            //鼠标点击
            Win32Api.mouse_event(Win32Api.mouseeventf_leftdown, x, y, 0, 0);
            Win32Api.mouse_event(Win32Api.mouseeventf_leftup, x, y, 0, 0);
        }

        /// <summary>
        /// 双击
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void DoubleClick(int x, int y)
        {
            //鼠标移动
            Win32Api.SetCursorPos(x, y);

            //鼠标点击
            Win32Api.mouse_event(Win32Api.mouseeventf_leftdown, x, y, 0, 0);
            Win32Api.mouse_event(Win32Api.mouseeventf_leftup, x, y, 0, 0);

            //鼠标点击
            Win32Api.mouse_event(Win32Api.mouseeventf_leftdown, x, y, 0, 0);
            Win32Api.mouse_event(Win32Api.mouseeventf_leftup, x, y, 0, 0);
        }
    }
}
