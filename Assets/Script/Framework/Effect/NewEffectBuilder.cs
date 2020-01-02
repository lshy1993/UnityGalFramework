using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Framework.Effect
{
    public class NewEffectBuilder
    {
        private NewImageEffect imageEffect;

        private NewEffectBuilder UI(NewImageEffect.ImageType target)
        {
            imageEffect = new NewImageEffect();
            imageEffect.state = new SpriteState();
            imageEffect.target = target;
            return this;
        }

        private NewEffectBuilder UI(int depth)
        {
            imageEffect = new NewImageEffect();
            imageEffect.state = new SpriteState();
            imageEffect.target = NewImageEffect.ImageType.Fore;
            imageEffect.depth = depth;
            return this;
        }

        private NewEffectBuilder Operate(NewImageEffect.OperateMode operate)
        {
            imageEffect.operate = operate;
            return this;
        }

        private NewEffectBuilder Movie(bool ison)
        {
            imageEffect.movie = ison;
            return this;
        }

        private NewEffectBuilder Loop(bool isloop)
        {
            imageEffect.loop = isloop;
            return this;
        }

        private NewEffectBuilder Trans(NewImageEffect.TransMode operate)
        {
            imageEffect.transmode = operate;
            return this;
        }
        private NewEffectBuilder Effect(NewImageEffect.EffectMode operate)
        {
            imageEffect.effectmode = operate;
            return this;
        }
        private NewEffectBuilder Action(NewImageEffect.ActionMode operate)
        {
            imageEffect.actionmode = operate;
            return this;
        }
        private NewEffectBuilder Live2d(NewImageEffect.Live2dMode operate)
        {
            imageEffect.l2dmode = operate;
            return this;
        }

        private NewEffectBuilder TotalTime(float time)
        {
            imageEffect.time = time;
            return this;
        }

        private NewEffectBuilder Sync(bool sync)
        {
            imageEffect.sync = sync;
            return this;
        }

        private NewEffectBuilder Source(string name)
        {
            imageEffect.state.spriteName = name;
            return this;
        }

        private NewEffectBuilder Mask(string image)
        {
            imageEffect.maskImage = image;
            return this;
        }

        private NewEffectBuilder Direction(string direction)
        {
            imageEffect.direction = NewImageEffect.Direction.Left;
            if (direction.Equals("left",StringComparison.OrdinalIgnoreCase))
                imageEffect.direction = NewImageEffect.Direction.Left;
            if (direction.Equals("right", StringComparison.OrdinalIgnoreCase))
                imageEffect.direction = NewImageEffect.Direction.Right;
            if (direction.Equals("top", StringComparison.OrdinalIgnoreCase))
                imageEffect.direction = NewImageEffect.Direction.Top;
            if (direction.Equals("bottom", StringComparison.OrdinalIgnoreCase))
                imageEffect.direction = NewImageEffect.Direction.Bottom;
            return this;
        }

        private NewEffectBuilder Direction(bool inverse)
        {
            imageEffect.inverse = inverse;
            return this;
        }

        private NewEffectBuilder FinalAlpha(float alpha)
        {
            imageEffect.state.spriteAlpha = alpha;
            return this;
        }

        private NewEffectBuilder FinalPosition(Vector3 pos)
        {
            imageEffect.state.SetPosition(pos);
            return this;
        }

        private NewEffectBuilder FinalPosition(string str)
        {
            imageEffect.defaultpos = str;
            return this;
        }

        private NewEffectBuilder Scale(Vector3 scale)
        {
            imageEffect.scale = scale;
            return this;
        }

        private NewEffectBuilder Rotate(Vector3 angle)
        {
            imageEffect.angle = angle;
            return this;
        }

        private NewEffectBuilder Frequency(int freq)
        {
            imageEffect.freq = freq;
            return this;
        }

        private NewEffectBuilder Variation(float v)
        {
            imageEffect.v = v;
            return this;
        }

        private NewImageEffect Get() { return imageEffect; }


        public static NewImageEffect Wait(float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.All)
                .TotalTime(time)
                .Operate(NewImageEffect.OperateMode.Wait)
                .Get();
            return e;
        }

        public static NewImageEffect TransAll(float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.All)
                .TotalTime(time)
                .Operate(NewImageEffect.OperateMode.TransAll)
                .Get();
            return e;
        }


        #region 图层相关操作
        public static NewImageEffect SetBackSprite(string sprite, bool isMovie = false, bool isLoop = false)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.Back)
                .Source(sprite)
                .Operate(NewImageEffect.OperateMode.Set)
                .Movie(isMovie)
                .Loop(isLoop)
                .Get();
            return e;
        }

        public static NewImageEffect SetAlphaBackSprite(float alpha)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.Back)
                .FinalAlpha(alpha)
                .Operate(NewImageEffect.OperateMode.SetAlpha)
                .Get();
            return e;
        }

        //public static NewImageEffect PreTransBackSprite(string sprite)
        //{
        //    NewEffectBuilder builder = new NewEffectBuilder();
        //    NewImageEffect e = builder.UI(-1)
        //        .Source(sprite)
        //        .Operate(NewImageEffect.OperateMode.PreTrans)
        //        .Get();
        //    return e;
        //}

        public static NewImageEffect PreTransBackSprite()
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(-1)
                .Operate(NewImageEffect.OperateMode.PreTrans)
                .Get();
            return e;
        }

        public static NewImageEffect SetPostionByDepth(int depth, Vector3 position)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .Operate(NewImageEffect.OperateMode.SetPos)
                .FinalPosition(position)
                .Get();
            return e;
        }

        public static NewImageEffect SetDefaultPostionByDepth(int depth, string pstr)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .Operate(NewImageEffect.OperateMode.SetPos)
                .FinalPosition(pstr)
                .Get();
            return e;
        }

        public static NewImageEffect SetRotateByDepth(int depth, Vector3 angle)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .Operate(NewImageEffect.OperateMode.SetRotate)
                .Rotate(angle)
                .Get();
            return e;
        }

        public static NewImageEffect SetAlphaByDepth(int depth, float alpha)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .Operate(NewImageEffect.OperateMode.SetAlpha)
                .FinalAlpha(alpha)
                .Get();
            return e;
        }
        public static NewImageEffect SetSpriteByDepth(int depth, string sprite, bool isMovie = false)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .Source(sprite)
                .Operate(NewImageEffect.OperateMode.Set)
                .Movie(isMovie)
                .Get();
            return e;
        }

        public static NewImageEffect DeleteSpriteByDepth(int depth)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .Operate(NewImageEffect.OperateMode.Delete)
                .Get();
            return e;
        }

        public static NewImageEffect ChangeByDepth(int depth, string sprite)
        {
            return SetSpriteByDepth(depth, sprite);
        }

        public static NewImageEffect TransBackSprite(float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.Back)
                .TotalTime(time)
                .Operate(NewImageEffect.OperateMode.Trans)
                .Get();
            return e;
        }
        #endregion

        public static NewImageEffect SetLive2dSpriteByDepth(int depth, string modelName)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .Source(modelName)
                .Operate(NewImageEffect.OperateMode.Live2d)
                .Live2d(NewImageEffect.Live2dMode.SetLive2D)
                .Get();
            return e;
        }

        public static NewImageEffect ChangeLive2dExpression(int depth, int expre)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .Frequency(expre)
                .Operate(NewImageEffect.OperateMode.Live2d)
                .Live2d(NewImageEffect.Live2dMode.ChangeExpression)
                .Get();
            return e;
        }


        #region Trans类
        public static NewImageEffect PreTransByDepth(int depth)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .Operate(NewImageEffect.OperateMode.PreTrans)
                .Get();
            return e;
        }

        public static NewImageEffect PreTransByDepth(int depth, string sprite, Vector3 postision)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .Source(sprite)
                .Operate(NewImageEffect.OperateMode.PreTrans)
                .FinalPosition(postision)
                .Get();
            return e;
        }

        public static NewImageEffect PreTransByDepth(int depth, string sprite, string postision)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .Source(sprite)
                .Operate(NewImageEffect.OperateMode.PreTrans)
                .FinalPosition(postision)
                .Get();
            return e;
        }

        public static NewImageEffect TransByDepth(int depth, float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .TotalTime(time)
                .Operate(NewImageEffect.OperateMode.Trans)
                .Get();
            return e;
        }

        public static NewImageEffect RotateFade(string sprite, bool inverse, float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.Back)
                .Source(sprite)
                .TotalTime(time)
                .Direction(inverse)
                .Operate(NewImageEffect.OperateMode.Trans)
                .Trans(NewImageEffect.TransMode.RotateFade)
                .Get();
            return e;
        }

        public static NewImageEffect SideFade(string sprite, string direction, float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.Back)
                .Source(sprite)
                .TotalTime(time)
                .Direction(direction)
                .Operate(NewImageEffect.OperateMode.Trans)
                .Trans(NewImageEffect.TransMode.SideFade)
                .Get();
            return e;
        }

        public static NewImageEffect Circle(string sprite, bool inverse, float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.Back)
                .Source(sprite)
                .TotalTime(time)
                .Direction(inverse)
                .Operate(NewImageEffect.OperateMode.Trans)
                .Trans(NewImageEffect.TransMode.Circle)
                .Get();
            return e;
        }

        public static NewImageEffect Scroll(string sprite, string direction, float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.Back)
                .Source(sprite)
                .TotalTime(time)
                .Direction(direction)
                .Operate(NewImageEffect.OperateMode.Trans)
                .Trans(NewImageEffect.TransMode.Scroll)
                .Get();
            return e;
        }

        public static NewImageEffect ScrollBoth(string sprite, string direction, float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.Back)
                .Source(sprite)
                .TotalTime(time)
                .Direction(direction)
                .Operate(NewImageEffect.OperateMode.Trans)
                .Trans(NewImageEffect.TransMode.ScrollBoth)
                .Get();
            return e;
        }

        public static NewImageEffect Universial(string sprite, string rule, float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.Back)
                .Source(sprite)
                .TotalTime(time)
                .Operate(NewImageEffect.OperateMode.Trans)
                .Trans(NewImageEffect.TransMode.Universial)
                .Mask(rule)
                .Get();
            return e;
        }

        public static NewImageEffect Shutter(string sprite, string direction, float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.Back)
                .Source(sprite)
                .TotalTime(time)
                .Operate(NewImageEffect.OperateMode.Trans)
                .Trans(NewImageEffect.TransMode.Shutter)
                .Direction(direction)
                .Get();
            return e;
        }

        #endregion

        #region Effect类
        public static NewImageEffect Masking(string mask)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.Back)
                .Operate(NewImageEffect.OperateMode.Effect)
                .Effect(NewImageEffect.EffectMode.Mask)
                .Mask(mask)
                .Get();
            return e;
        }

        public static NewImageEffect Blur()
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.Back)
                .Operate(NewImageEffect.OperateMode.Effect)
                .Effect(NewImageEffect.EffectMode.Blur)
                .Get();
            return e;
        }

        public static NewImageEffect Mosaic()
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.Back)
                .Operate(NewImageEffect.OperateMode.Effect)
                .Effect(NewImageEffect.EffectMode.Mosaic)
                .Get();
            return e;
        }

        public static NewImageEffect Gray()
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.Back)
                .Operate(NewImageEffect.OperateMode.Effect)
                .Effect(NewImageEffect.EffectMode.Gray)
                .Get();
            return e;
        }

        public static NewImageEffect OldPhoto()
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.Back)
                .Operate(NewImageEffect.OperateMode.Effect)
                .Effect(NewImageEffect.EffectMode.OldPhoto)
                .Get();
            return e;
        }
        #endregion


        #region Action类
        public static NewImageEffect FadeInBackSprite(float time)
        {
            return FadeToByDepth(-1, 1, time);
        }

        public static NewImageEffect FadeOutBackSprite(float time)
        {
            return FadeToByDepth(-1, 0, time);
        }

        public static NewImageEffect FadeInByDepth(int depth, float time)
        {
            return FadeToByDepth(depth, 1, time);
        }

        public static NewImageEffect FadeOutByDepth(int depth, float time)
        {
            return FadeToByDepth(depth, 0, time);
        }


        public static NewImageEffect FadeToByDepth(int depth, float alpha, float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .TotalTime(time)
                .Operate(NewImageEffect.OperateMode.Action)
                .Action(NewImageEffect.ActionMode.Fade)
                .FinalAlpha(alpha)
                .Get();
            return e;
        }

        public static NewImageEffect ScaleToByDepth(int depth, Vector3 scale, float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .TotalTime(time)
                .Operate(NewImageEffect.OperateMode.Action)
                .Action(NewImageEffect.ActionMode.Scale)
                .Scale(scale)
                .Get();
            return e;
        }

        public static NewImageEffect RotateToByDepth(int depth, Vector3 angle, float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .TotalTime(time)
                .Operate(NewImageEffect.OperateMode.Action)
                .Action(NewImageEffect.ActionMode.Rotate)
                .Rotate(angle)
                .Get();
            return e;
        }

        public static NewImageEffect MoveByDepth(int depth, Vector3 final, float time, bool sync)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .TotalTime(time)
                .Sync(sync)
                .Operate(NewImageEffect.OperateMode.Action)
                .Action(NewImageEffect.ActionMode.Move)
                .FinalPosition(final)
                .Get();
            return e;
        }

        public static NewImageEffect MoveDefaultByDepth(int depth, string pstr, float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .TotalTime(time)
                .Operate(NewImageEffect.OperateMode.Action)
                .Action(NewImageEffect.ActionMode.Move)
                .FinalPosition(pstr)
                .Get();
            return e;
        }
        public static NewImageEffect SpriteShakeByDepth(int depth, float v, int freq, float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(depth)
                .TotalTime(time)
                .Operate(NewImageEffect.OperateMode.Action)
                .Action(NewImageEffect.ActionMode.Shake)
                .Variation(v)
                .Frequency(freq)
                .Get();
            return e;
        }

        public static NewImageEffect WindowShake(float v, int freq, float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.All)
                .TotalTime(time)
                .Operate(NewImageEffect.OperateMode.Action)
                .Action(NewImageEffect.ActionMode.WinShake)
                .Variation(v)
                .Frequency(freq)
                .Get();
            return e;
        }
        # endregion

        public static NewImageEffect FadeOutAll(float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.All)
                .TotalTime(time)
                .Operate(NewImageEffect.OperateMode.Action)
                .Action(NewImageEffect.ActionMode.Fade)
                .FinalAlpha(0)
                .Get();
            return e;
        }

        public static NewImageEffect FadeOutAllChara(float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.AllChara)
                .TotalTime(time)
                .Operate(NewImageEffect.OperateMode.Action)
                .Action(NewImageEffect.ActionMode.Fade)
                .FinalAlpha(0)
                .Get();
            return e;
        }

        public static NewImageEffect FadeOutAllPic(float time)
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.AllPic)
                .TotalTime(time)
                .Operate(NewImageEffect.OperateMode.Action)
                .Action(NewImageEffect.ActionMode.Fade)
                .FinalAlpha(0)
                .Get();
            return e;
        }

        public static NewImageEffect RemoveAll()
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.All)
                .Operate(NewImageEffect.OperateMode.Delete)
                .Get();
            return e;
        }

        public static NewImageEffect RemoveAllChara()
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.AllChara)
                .Operate(NewImageEffect.OperateMode.Delete)
                .Get();
            return e;
        }

        public static NewImageEffect RemoveAllPic()
        {
            NewEffectBuilder builder = new NewEffectBuilder();
            NewImageEffect e = builder.UI(NewImageEffect.ImageType.AllPic)
                .Operate(NewImageEffect.OperateMode.Delete)
                .Get();
            return e;
        }

    }
}
