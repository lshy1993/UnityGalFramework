using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Framework.Data
{
    /// <summary>
    /// 游戏系统设置数据
    /// </summary>
    public class ConfigData
    {
        /// <summary>
        /// 当前系统设置模式
        /// </summary>
        public Constants.Setting_Mode settingMode;

        /// <summary>
        /// 是否全屏
        /// </summary>
        public bool fullScreen;

        /// <summary>
        /// 是否开启转换特效
        /// </summary>
        public bool fadingSwitch;

        /// <summary>
        /// 是否开启动画特效
        /// </summary>
        public bool animateSwitch;

        /// <summary>
        /// 是否开启头像
        /// </summary>
        public bool avatarSwitch;

        /// <summary>
        /// 是否窗体置顶
        /// </summary>
        public bool topMost;

        /// <summary>
        /// 快捷菜单隐藏
        /// </summary>
        public bool autoHide;

        /// <summary>
        /// 跳过模式
        /// </summary>
        public bool skipAll;

        /// <summary>
        /// BGM曲名显示时长
        /// </summary>
        public int BGMTime;

        /// <summary>
        /// 章节名显示时长
        /// </summary>
        public int chapterTime;

        /// <summary>
        /// 打字机速度
        /// </summary>
        public float textSpeed;

        /// <summary>
        /// 自动模式等待时长
        /// </summary>
        public float waitTime;

        /// <summary>
        /// 对话框透明度
        /// </summary>
        public int diaboxAlpha;

        /// <summary>
        /// 主音量
        /// </summary>
        public float masterVolume;

        /// <summary>
        /// BGＭ音量
        /// </summary>
        public float userBGMVolume;

        /// <summary>
        /// SE音量
        /// </summary>
        public float userSEVolume;

        /// <summary>
        /// 系统效果音量
        /// </summary>
        public float userSysSEVolume;

        /// <summary>
        /// 角色语音音量
        /// </summary>
        public float userVoiceVolume;

        /// <summary>
        /// 角色语音默认显示项
        /// </summary>
        public int defaultCharaNum;

        /// <summary>
        /// 角色语音音量表
        /// </summary>
        public float[] charaVoiceVolume;

        /// <summary>
        /// 角色语音开启表
        /// </summary>
        public bool[] charaVoice;

        public ConfigData()
        {
            settingMode = Constants.Setting_Mode.Graphic;
            fullScreen = true;
            fadingSwitch = true;
            animateSwitch = true;
            avatarSwitch = true;
            topMost = false;
            autoHide = false;
            skipAll = false;
            BGMTime = 3;
            chapterTime = 3;
            textSpeed = 60f;
            waitTime = 1.5f;
            diaboxAlpha = 75;
            defaultCharaNum = 0;
            masterVolume = 1f;
            userBGMVolume = 0.5f;
            userSEVolume = 0.75f;
            userSysSEVolume = 0.35f;
            userVoiceVolume = 1f;
            charaVoiceVolume = new float[] { 1, 1, 1, 1, 1, 1 };
            charaVoice = new bool[] { true, true, true, true, true, true };

        }

    }
}
