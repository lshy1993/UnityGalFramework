using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
        /// 是否动态背景
        /// </summary>
        public bool movie = false;

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
            Set,
            SetPos,
            SetAlpha,
            SetRotate,
            Delete,
            Wait,
            PreTrans,
            Trans,
            TransAll,
            Action,
            Effect,
            Live2d,
            Spine
        }
        public OperateMode operate = 0;

        public enum TransMode
        {
            Normal, CrossFade, Universial,
            Scroll, ScrollBoth, Shutter, Twirl, Vortex,
            Circle, RotateFade, SideFade
        }
        /// <summary>
        /// 渐变类型
        /// </summary>
        public TransMode transmode = 0;

        public enum EffectMode
        {
            Blur, Mosaic, Gray, OldPhoto, Mask, 
        }
        /// <summary>
        /// 特效类型
        /// </summary>
        public EffectMode effectmode = 0;

        
        public enum ActionMode
        {
            Fade, FadeFrom, FadeTo, Move, Scale, Rotate, Shake, WinShake
        }
        /// <summary>
        /// 动作类型
        /// </summary>
        public ActionMode actionmode = 0;

        public enum Live2dMode
        {
            SetLive2D,
            ChangeMotion,
            ChangeExpression
        }
        public Live2dMode l2dmode = 0;

        public enum SpineMode
        {

        }

        //public enum OperateType
        //{
        //    SetSprite, SetPos, SetAlpha,
        //    Delete,
        //    Wait,
        //    PreTrans, Trans, TransAll,
        //    Fade, Move, Shutter, Twirl, Vortex,
        //    Blur, Mosaic, Gray, OldPhoto,
        //    Scroll, ScrollBoth, Circle, RotateFade,
        //    SideFade,
        //    Mask,
        //    Universial,
        //    Shake,WinShake,
        //    SetLive2D,ChangeMotion,ChangeExpression
        //};

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
        /// 是否异步操作
        /// </summary>
        public bool sync = false;

        /// <summary>
        /// 缩放
        /// </summary>
        public Vector3 scale = Vector3.one;

        /// <summary>
        /// 旋转
        /// </summary>
        public Vector3 angle = Vector3.zero;

        /// <summary>
        /// 震动量
        /// </summary>
        public float v;

        /// <summary>
        /// 震动频率
        /// </summary>
        public int freq;

        /// <summary>
        /// 遮罩层
        /// </summary>
        public string maskImage;

        public NewImageEffect()
        {
            //time = 0;
            //depth = -1;

            //target = ImageType.Back;
            //operate = OperateMode.SetSprite;
        }

        public override string ToString()
        {
            string tt = string.Empty;
            tt += state.ToString();
            tt += movie ? "【Movie】\n" : "";
            tt += string.Format(
                "depth:{0},time:{1},target:{2},mode:{3}\n",
                depth,time,target,operate
                );
            tt += string.Format(
                "direction:{0},invert:{1},v:{2},freq:{3}\n",
                direction, inverse, v, freq
                );
            return tt;
        }

        public string ToDebugString()
        {
            string tt = string.Empty;
            tt += string.Format(
                "operate:【{0}】",
                operate
                );
            if(operate == OperateMode.Trans)
            {
                tt += "-【" + transmode + "】\n";
            }
            else if(operate == OperateMode.Action)
            {
                tt += "-【" + actionmode + "】\n";
            }
            else if(operate == OperateMode.Effect)
            {
                tt += "-【" + effectmode + "】\n";
            }
            else
            {
                tt += "\n";
            }
            tt += string.Format(
                "depth:{0},time:{1},target:{2}\n",
                depth, time, target
            );
            tt += string.Format(
                "direction:{0},invert:{1},v:{2},freq:{3}\n",
                direction, inverse, v, freq
                );
            return tt;
        }

    }
}
