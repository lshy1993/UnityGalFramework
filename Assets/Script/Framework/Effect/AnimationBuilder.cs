using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.Effect
{
    /// <summary>
    /// 用于【组合】基本的底层特效，返回一个Queue
    /// 在这设置每一个【组合】的所有需求参数，供PieceFactory调用
    /// PieceFactory 会提供默认参数
    /// 详细基层变化请参看【EffectBuilder】
    /// </summary>
    public class AnimationBuilder
    {
        public Queue<ImageEffect> animation;

        public AnimationBuilder BeginWith(ImageEffect e)
        {
            animation = new Queue<ImageEffect>();
            animation.Enqueue(EffectBuilder.BlockClick(false));
            animation.Enqueue(e);
            return this;
        }

        public AnimationBuilder Then(ImageEffect e)
        {
            animation.Enqueue(e);
            return this;
        }

        public Queue<ImageEffect> Get()
        {
            animation.Enqueue(EffectBuilder.BlockClick(true));
            return animation;
        }

        #region 新增加特效 立绘部分
        /// <summary>
        /// 设置并淡入立绘
        /// </summary>
        /// <param name="depth">图像层数</param>
        /// <param name="sprite">图像名</param>
        /// <param name="time">淡入时间</param>
        public static Queue<ImageEffect> SetCharacterSprite(int depth, Sprite sprite, float time)
        {
            AnimationBuilder builder = new AnimationBuilder();
            return builder.BeginWith(EffectBuilder.SetSpriteByDepth(depth, sprite))
                .Then(EffectBuilder.FadeInByDepth(depth,time))
                .Get();
        }

        /// <summary>
        /// 设置并淡入立绘（预设位置）
        /// </summary>
        /// <param name="depth">图像层数</param>
        /// <param name="sprite">图像名</param>
        /// <param name="pstr">位置名称</param>
        /// <param name="time">淡入时间</param>
        public static Queue<ImageEffect> SetCharacterSprite(int depth, Sprite sprite, string pstr, float time)
        {
            AnimationBuilder builder = new AnimationBuilder();
            return builder.BeginWith(EffectBuilder.SetSpriteByDepth(depth, sprite))
                .Then(EffectBuilder.SetDefaultPostionByDepth(depth, pstr))
                .Then(EffectBuilder.FadeInByDepth(depth, time))
                .Get();
        }

        /// <summary>
        /// 设置并淡入立绘（带坐标）
        /// </summary>
        /// <param name="depth">图像层数</param>
        /// <param name="sprite">图像名</param>
        /// <param name="position">位置</param>
        /// <param name="time">淡入时间</param>
        public static Queue<ImageEffect> SetCharacterSprite(int depth, Sprite sprite, Vector3 position, float time)
        {
            AnimationBuilder builder = new AnimationBuilder();
            return builder.BeginWith(EffectBuilder.SetSpriteByDepth(depth, sprite))
                .Then(EffectBuilder.SetPostionByDepth(depth, position))
                .Then(EffectBuilder.FadeInByDepth(depth, time))
                .Get();
        }

        /// <summary>
        /// 改变原有立绘（淡出淡入）
        /// </summary>
        /// <param name="depth">图像层数</param>
        /// <param name="sprite">图像名</param>
        /// <param name="fadeout">原图淡出时间</param>
        /// <param name="fadein">淡入时间</param>
        public static Queue<ImageEffect> ChangeCharacterSprite(int depth, Sprite sprite, float fadeout, float fadein)
        {
            AnimationBuilder builder = new AnimationBuilder();
            if(fadeout == 0)
            {
                builder.BeginWith(EffectBuilder.ChangeByDepth(depth, sprite));
            }
            else
            {
                builder.BeginWith(EffectBuilder.FadeOutByDepth(depth, fadeout))
                .Then(EffectBuilder.SetSpriteByDepth(depth, sprite));
            }
            if(fadein != 0)
            {
                return builder.Then(EffectBuilder.FadeInByDepth(depth, fadein)).Get();
            }
            return builder.Get();
        }

        /// <summary>
        /// 淡出并移除立绘
        /// </summary>
        /// <param name="depth">图像层数</param>
        /// <param name="time">淡出时间</param>
        public static Queue<ImageEffect> RemoveCharacterSprite(int depth, float time)
        {
            AnimationBuilder builder = new AnimationBuilder();
            return builder.BeginWith(EffectBuilder.FadeOutByDepth(depth, time))
                .Then(EffectBuilder.DeleteSpriteByDepth(depth))
                .Get();
        }

        /// <summary>
        /// 从当前位置向某一位置移动
        /// </summary>
        /// <param name="depth">图像层数</param>
        /// <param name="target">目标位置</param>
        /// <param name="time">移动时间</param>
        public static Queue<ImageEffect> MoveCharacterSprite(int depth, Vector3 target, float time)
        {
            AnimationBuilder builder = new AnimationBuilder();
            return builder.BeginWith(EffectBuilder.MoveByDepth(depth, target, time))
                .Get();
        }

        /// <summary>
        /// 从初始值开始的移动
        /// </summary>
        /// <param name="depth">图像层数</param>
        /// <param name="origin">起始位置</param>
        /// <param name="target">目标位置</param>
        /// <param name="time">移动时间</param>
        public static Queue<ImageEffect> MoveCharacterSpriteFrom(int depth, Vector3 origin, Vector3 target, float time)
        {
            AnimationBuilder builder = new AnimationBuilder();
            return builder.BeginWith(EffectBuilder.SetPostionByDepth(depth, origin))
                .Then(EffectBuilder.MoveByDepth(depth, target, time))
                .Get();
        }
        #endregion

        #region 新增加特效 背景部分
        /// <summary>
        /// 设置背景
        /// </summary>
        /// <param name="sprite">图像名</param>
        public static Queue<ImageEffect> SetBackground(Sprite sprite)
        {
            AnimationBuilder builder = new AnimationBuilder();
            Image ui = EffectBuilder.backgroundSprite;
            return builder.BeginWith(EffectBuilder.ChangeSprite(ui, sprite)).Get();
        }

        /// <summary>
        /// 设置并淡入背景
        /// </summary>
        /// <param name="sprite">图像名</param>
        /// <param name="time">淡入时间</param>
        public static Queue<ImageEffect> FadeInBackground(Sprite sprite, float time)
        {
            AnimationBuilder builder = new AnimationBuilder();
            Image ui = EffectBuilder.backgroundSprite;
            return builder.BeginWith(EffectBuilder.ChangeSprite(ui, sprite))
                .Then(EffectBuilder.FadeIn(ui, time))
                .Get();
        }

        /// <summary>
        /// 移除背景
        /// </summary>
        public static Queue<ImageEffect> RemoveBackground()
        {
            AnimationBuilder builder = new AnimationBuilder();
            Image ui = EffectBuilder.backgroundSprite;
            return builder.BeginWith(EffectBuilder.ChangeSprite(ui, null)).Get();
        }

        /// <summary>
        /// 淡出并移除背景
        /// </summary>
        /// <param name="time">淡出时间</param>
        public static Queue<ImageEffect> FadeOutBackground(float time)
        {
            AnimationBuilder builder = new AnimationBuilder();
            Image ui = EffectBuilder.backgroundSprite;
            return builder.BeginWith(EffectBuilder.FadeOut(ui, time)).Then(EffectBuilder.RemoveSprite(ui)).Get();
        }

        /// <summary>
        /// 更换背景（含淡入淡出）
        /// </summary>
        /// <param name="sprite">图像名</param>
        /// <param name="fadeout">原图淡出时间</param>
        /// <param name="fadein">淡入时间</param>
        public static Queue<ImageEffect> ChangeBackground(Sprite sprite, float fadeout, float fadein)
        {
            AnimationBuilder builder = new AnimationBuilder();
            Image ui = EffectBuilder.backgroundSprite;
            return builder.BeginWith(EffectBuilder.FadeOut(ui, fadeout))
                .Then(EffectBuilder.ChangeSprite(ui, sprite))
                .Then(EffectBuilder.FadeIn(ui, fadein))
                .Get();
        }
        #endregion

        #region 废弃
        /// <summary>
        /// 淡出所有立绘
        /// </summary>
        /// <param name="time">淡出时间</param>
        //public static Queue<ImageEffect> FadeOutAllChara(float time)
        //{
        //    AnimationBuilder builder = new AnimationBuilder();
        //    List<int> charanums = EffectBuilder.GetDepthNum();
        //    Queue<ImageEffect> animation = new Queue<ImageEffect>();
        //    animation.Enqueue(EffectBuilder.BlockClick(false));
        //    foreach (int x in charanums)
        //    {
        //        animation.Enqueue(EffectBuilder.FadeOutByDepth(x, time));
        //    }
        //    return animation;
        //}

        ///// <summary>
        ///// 移除所有立绘
        ///// </summary>
        //public static Queue<ImageEffect> RemoveAllChara()
        //{
        //    AnimationBuilder builder = new AnimationBuilder();
        //    List<int> charanums = EffectBuilder.GetDepthNum();
        //    Queue<ImageEffect> animation = new Queue<ImageEffect>();
        //    animation.Enqueue(EffectBuilder.BlockClick(false));
        //    foreach (int x in charanums)
        //    {
        //        animation.Enqueue(EffectBuilder.DeleteSpriteByDepth(x));
        //    }
        //    animation.Enqueue(EffectBuilder.BlockClick(true));
        //    return animation;
        //}

        ///// <summary>
        ///// 淡出所有图片（包括背景）
        ///// </summary>
        ///// <param name="time">淡出时间</param>
        //public static Queue<ImageEffect> FadeOutAllPic(float time)
        //{
        //    AnimationBuilder builder = new AnimationBuilder();
        //    List<int> charanums = EffectBuilder.GetDepthNum();
        //    Queue<ImageEffect> animation = new Queue<ImageEffect>();
        //    animation.Enqueue(EffectBuilder.BlockClick(false));
        //    foreach (int x in charanums)
        //    {
        //        animation.Enqueue(EffectBuilder.FadeOutByDepth(x, time));
        //    }
        //    animation.Enqueue(EffectBuilder.FadeOut(EffectBuilder.backgroundSprite, time));
        //    return animation;
        //}
        #endregion

        #region 新增同步特效 所有图片
        /// <summary>
        /// 淡出所有
        /// </summary>
        /// <param name="back">是否包含背景</param>
        /// <param name="dialog">是否包含对话框</param>
        /// <param name="time">淡出时间</param>
        public static Queue<ImageEffect>FadeOutAll(bool back, bool dialog, float time)
        {
            List<int> charanums = EffectBuilder.GetDepthNum();
            Queue<ImageEffect> animation = new Queue<ImageEffect>();
            animation.Enqueue(EffectBuilder.BlockClick(false));
            if (dialog) animation.Enqueue(EffectBuilder.FadeOutDialog(time));
            foreach (int x in charanums)
            {
                animation.Enqueue(EffectBuilder.FadeOutByDepth(x, time));
            }
            if (back) animation.Enqueue(EffectBuilder.FadeOut(EffectBuilder.backgroundSprite, time));
            return animation;
        }

        /// <summary>
        /// 删除所有
        /// </summary>
        /// <param name="back">是否包含背景</param>
        public static Queue<ImageEffect> RemoveAll(bool back)
        {
            List<int> charanums = EffectBuilder.GetDepthNum();
            Queue<ImageEffect> animation = new Queue<ImageEffect>();
            animation.Enqueue(EffectBuilder.BlockClick(false));
            foreach(int x in charanums)
            {
                //Debug.Log(x);
                animation.Enqueue(EffectBuilder.DeleteSpriteByDepth(x));
            }
            //if (back) animation.Enqueue(EffectBuilder.RemoveSprite(EffectBuilder.backgroundSprite));
            animation.Enqueue(EffectBuilder.BlockClick(true));
            return animation;
        }
        #endregion

        #region 新增特效 对话框
        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="fadein">淡入时间</param>
        public static Queue<ImageEffect> FadeInDialog(float fadein)
        {
            AnimationBuilder builder = new AnimationBuilder();
            return builder.BeginWith(EffectBuilder.SetDialog(true))
                .Then(EffectBuilder.FadeInDialog(fadein))
                .Get();
        }

        /// <summary>
        /// 消除对话框
        /// </summary>
        /// <param name="fadeout">淡出时间</param>
        public static Queue<ImageEffect> FadeOutDialog(float fadeout)
        {
            AnimationBuilder builder = new AnimationBuilder();
            return builder.BeginWith(EffectBuilder.FadeOutDialog(fadeout))
                .Then(EffectBuilder.SetDialog(false))
                .Get();
        }

        /// <summary>
        /// 暂时消去文字
        /// </summary>
        /// <returns></returns>
        public static Queue<ImageEffect> RemoveDialogText()
        {
            return null;
        }
        #endregion

        public static Queue<ImageEffect> ChangeSprite(Image uiSprite, Sprite sprite)
        {
            AnimationBuilder builder = new AnimationBuilder();
            return builder.BeginWith(EffectBuilder.ChangeSprite(uiSprite, sprite
                )).Get();
        }

        //public static Queue<ImageEffect> ChangeFront(string character, Sprite sprite)
        //{
        //    ImageManager im = GameObject.Find("GameManager").GetComponent<ImageManager>();
        //    return ChangeSprite(im.GetFront(character), sprite);
        //}

        public static Queue<ImageEffect> ChangeSpriteFade(Image ui, Sprite sprite, float fadeout, float fadein)
        {
            AnimationBuilder builder = new AnimationBuilder();
            return builder.BeginWith(EffectBuilder.FadeOut(ui, fadeout))
                .Then(EffectBuilder.ChangeSprite(ui, sprite))
                .Then(EffectBuilder.FadeIn(ui, fadein))
                .Get();
        }

        public static Queue<ImageEffect> ChangeBackgroundFade(Sprite sprite, float fadeout, float fadein)
        {
            return ChangeSpriteFade(EffectBuilder.backgroundSprite, sprite, fadeout, fadein);
        }

        //public static Queue<ImageEffect> ChangeFront(string character, Sprite sprite, float fadeout = 0.5f, float fadein = 0.5f)
        //{
        //    ImageManager im = GameObject.Find("GameManager").GetComponent<ImageManager>();
        //    return ChangeSpriteFade(im.GetFront(character), sprite, fadeout, fadein);
        //}

    }
}
