using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1 
{
    public enum SettingModes
    {
        NotSettingState,

        /// <summary>
        /// 间隔点击模式的点击位置设置
        /// </summary>
        ClickByStep_PositionOne,

        /// <summary>
        /// 根据图片的点击位置设置
        /// </summary>
        ClickByPictures_PositionOne,

        /// <summary>
        /// 根据文字的点击位置设置
        /// </summary>
        ClickByText_PositionOne,

        /// <summary>
        /// 根据颜色的点击位置设置
        /// </summary>
        ClickByColor_PositionOne,

        /// <summary>
        /// 根据自定义的点击设置
        /// </summary>
        ClickByCustomize_PositionOne,
        ClickByCustomize_PositionTwo,
        ClickByCustomize_PositionThree,
        ClickByCustomize_PositionFour,
        ClickByCustomize_PositionFive,
        ClickByCustomize_PositionSix,
    }
}
