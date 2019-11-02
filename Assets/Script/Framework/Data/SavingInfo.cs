using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Framework.Data
{
    /// <summary>
    /// 存档信息类
    /// </summary>
    public class SavingInfo
    {
        /// <summary>
        /// 存档时间
        /// </summary>
        public string saveTime;

        /// <summary>
        /// 游戏模式
        /// </summary>
        public string gameMode;

        /// <summary>
        /// 存档标签
        /// </summary>
        public string saveText;

        /// <summary>
        /// 缩略图名
        /// </summary>
        public string picPath;

        /// <summary>
        /// 最后的文字
        /// </summary>
        public string currentText;

        public SavingInfo(string mode,string time,string content,string pic)
        {
            this.gameMode = mode;
            this.saveTime = time;
            this.saveText = content;
            this.picPath = pic;
        }

        public SavingInfo() { }
    }
}
