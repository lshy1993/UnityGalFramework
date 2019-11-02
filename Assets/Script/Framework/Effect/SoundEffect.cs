using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Framework.Effect
{
    /// <summary>
    /// 脚本文件转换为效果块
    /// </summary>
    public class SoundEffect
    {
        public static bool fast = false;

        public enum SoundType { BGM, SE, Voice };
        /// <summary>
        /// 对象识别符
        /// </summary>
        public SoundType target;

        public enum OperateType { Set, Remove, VolumeUp, VolumeDown, Pause, Unpause };
        /// <summary>
        /// 操作符
        /// </summary>
        public OperateType operate;

        /// <summary>
        /// 目标文件名
        /// </summary>
        public string clip;

        /// <summary>
        /// 动作持续时间
        /// </summary>
        public float time;

        /// <summary>
        /// 音量变动（相对）
        /// </summary>
        public float argus;

        /// <summary>
        /// 是否持续循环
        /// </summary>
        public bool loop;

        public SoundEffect()
        {
            target = SoundType.BGM;
            operate = OperateType.Set;
            clip = "";
            time = 0;
            loop = false;
        }

    }
}
