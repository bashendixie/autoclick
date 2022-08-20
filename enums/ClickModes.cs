using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal enum ClickModes
    {
        /// <summary>
        /// 根据图片点击
        /// </summary>
        ClickByPictures,
        /// <summary>
        /// 根据文字点击
        /// </summary>
        ClickByText,
        /// <summary>
        /// 根据颜色点击
        /// </summary>
        ClickByColor,
        /// <summary>
        /// 自定义的点击
        /// </summary>
        ClickByCustomize
    }
}
