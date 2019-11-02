using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Framework.Data
{
    /// <summary>
    /// 综合游戏数据类
    /// </summary>
    public class GameData
    {
        /// <summary>
        /// 当前游戏模式
        /// </summary>
        public string MODE;

        /// <summary>
        /// 当前脚本名
        /// </summary>
        public string currentScript;

        /// <summary>
        /// 当前文字位置
        /// </summary>
        public int currentTextPos;

        /// <summary>
        /// 背景图片名
        /// </summary>
        public string bgSprite;

        /// <summary>
        /// 立绘信息
        /// </summary>
        public Dictionary<int, SpriteState> fgSprites;

        /// <summary>
        /// 选项开启状态
        /// </summary>
        public List<string> selectionSwitch;
        
        /// <summary>
        /// 当前所处于的选项ID
        /// </summary>
        public string selectID;

        /// <summary>
        /// 当前播放的BGM曲名
        /// </summary>
        public string BGM;

        /// <summary>
        /// 当前播放的SE曲名
        /// </summary>
        public string SE;

        /// <summary>
        /// 当前播放的语音文件名
        /// </summary>
        public string Voice;

    }
}
