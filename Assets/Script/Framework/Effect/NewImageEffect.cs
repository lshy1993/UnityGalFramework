using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Framework.Effect
{
    public class NewImageEffect
    {
        public static bool fast = false;
        public bool end = false;

        /// <summary>
        /// 状态参数
        /// </summary>
        public SpriteState state;

        /// <summary>
        /// 特效持续时间
        /// </summary>
        public float time = 0;

        /// <summary>
        /// 特效应用层级
        /// </summary>
        public int depth = -1;

        /// <summary>
        /// 是否无限循环
        /// </summary>
        public bool loop = false;

        /// <summary>
        /// 默认位置字符
        /// </summary>
        public string defaultpos;

        public enum ImageType { Back, Fore, AllChara, AllPic, All };
        /// <summary>
        /// 目标应用对象
        /// </summary>
        public ImageType target = 0;

        public enum OperateMode
        {
            SetSprite, SetPos, SetAlpha,
            Delete,
            Wait,
            PreTrans, Trans, TransAll,
            Fade, Move, Shutter, Twirl, Vortex,
            Blur, Mosaic, Gray, OldPhoto,
            Scroll, ScrollBoth, Circle, RotateFade,
            SideFade,
            Mask,
            Shake,WinShake,
            SetLive2D,ChangeMotion,ChangePos
        };
        /// <summary>
        /// 特效执行模式
        /// </summary>
        public OperateMode operate = 0;

        public enum Direction
        {
            Left, Right, Top, Bottom
        }
        /// <summary>
        /// 特效方向
        /// </summary>
        public Direction direction = 0;

        /// <summary>
        /// 正反向
        /// </summary>
        public bool inverse;

        /// <summary>
        /// 震动量
        /// </summary>
        public float v;

        /// <summary>
        /// 震动频率
        /// </summary>
        public int freq;

        public string maskImage;

        public NewImageEffect()
        {
            //time = 0;
            //depth = -1;

            //target = ImageType.Back;
            //operate = OperateMode.SetSprite;
        }


    }
}
