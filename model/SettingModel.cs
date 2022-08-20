using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    [XmlRoot("ROOT")]
    public class SettingModel
    {
        /// <summary>
        /// 循环点击
        /// </summary>
        [XmlElement("circleFlag")]
        public bool circleFlag = true;

        /// <summary>
        /// 固定次数
        /// </summary>
        [XmlElement("fixedFlag")]
        public bool fixedFlag = false;

        /// <summary>
        /// 如果设置为固定次数，点击次数
        /// </summary>
        [XmlElement("clickNums")]
        public int clickNums = 10;

        /// <summary>
        /// 程序运行间隔时间
        /// </summary>
        [XmlElement("intervals")]
        public int intervals = 1000;

    }
}
