using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Script.Framework.Effect;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.Node
{
    /// <summary>
    /// 块 处理工厂：提供若干特效块 供剧情脚本文本调用
    /// 采用了类似krkr的语法，调用时会显示参数提示
    /// 这里已经预设了默认参数
    /// 详细的内容请参考【AnimationBuilder】【SoundBuilder】
    /// </summary>
    public class PieceFactory
    {
        private GameObject root;

        private Text nameLabel, dialogLabel;
        private Image avatarSprite;
        private DataManager manager;

        private GameObject msgpanel;
        //private GameObject timepanel, diaboxpanel, fulldiapanel, evipanel, hpui, inputpanel, sidepanel;

        private int id = 0;

        public PieceFactory(GameObject root, DataManager manager)
        {
            this.manager = manager;
            this.root = root;

            msgpanel = root.transform.Find("Avg_Panel/Message_Panel").gameObject;
            
            //timepanel = root.transform.Find("Avg_Panel/TimeSwitch_Panel").gameObject;
            //fulldiapanel = root.transform.Find("Avg_Panel/FullScreenBox_Panel").gameObject;
            //evipanel = root.transform.Find("Avg_Panel/EvidenceGet_Panel").gameObject;
            //hpui = root.transform.Find("Avg_Panel/HPMP_Panel").gameObject;
            //inputpanel = root.transform.Find("Avg_Panel/NameInput_Panel").gameObject;
            //sidepanel = root.transform.Find("Avg_Panel/SideLabel_Panel").gameObject;
        }

        #region TextPiece部分
        /// <summary>
        /// 生成一个简单的文字段
        /// </summary>
        /// <param name="name">名字,设为空则不改变</param>
        /// <param name="dialog">内容，空则不改变</param>
        public TextPiece t(string name = "", string dialog = "", string avatar = "", string voice = "", int model = -1)
        {
            //return new TextPiece(id++, nameLabel, dialogLabel, avatarSprite, name, dialog, avatar);
            return new TextPiece(id++, msgpanel, name, dialog, avatar, voice, model);
        }

        /// <summary>
        /// 生成一个带有简单逻辑的文字段
        /// </summary>
        /// <param name="name">名字，空则不改变</param>
        /// <param name="dialog">内容，空则不改变</param>
        /// <param name="simpleLogic">简单逻辑跳转,是一个返回int值的lambda表达式</param>
        public TextPiece t(string name, string dialog, string avatar, string voice, Func<int> simpleLogic)
        {
            //return new TextPiece(id++, nameLabel, dialogLabel, avatarSprite, name, dialog, avatar, simpleLogic);
            return new TextPiece(id++, msgpanel, name, dialog, avatar, voice, simpleLogic);
        }

        /// <summary>
        /// 生成一个带有复杂跳转的文字段
        /// </summary>
        /// <param name="name">名字，空则不改变</param>
        /// <param name="dialog">内容，空则不改变</param>
        /// <param name="complexLogic">复杂逻辑跳转，是一个形如(Hashtabel gVars, Hashtable lVars)=> {return ... }的lambda表达式</param>
        public TextPiece t(string name, string dialog, string avatar, string voice, Func<DataManager, int> complexLogic)
        {
            //return new TextPiece(id++, nameLabel, dialogLabel, avatarSprite, manager, complexLogic, name, dialog, avatar);
            return new TextPiece(id++, msgpanel, manager, complexLogic, name, dialog, avatar, voice);
        }

        public TextPiece t(string name, string dialog, string avatar, string voice, Action action)
        {
            //return new TextPiece(id++, nameLabel, dialogLabel, avatarSprite, manager, action, name, dialog, avatar);
            return new TextPiece(id++, msgpanel, manager, action, name, dialog, avatar, voice);
        }

        public TextPiece t(string name, string dialog, string avatar, string voice, Action<DataManager> action)
        {
            // return new TextPiece(id++, nameLabel, dialogLabel, avatarSprite, manager, action, name, dialog, avatar);
            return new TextPiece(id++, msgpanel, manager, action, name, dialog, avatar, voice);
        }

        public ExecPiece s(ExecPiece.Execute setVar)
        {
            return new ExecPiece(id++, manager, setVar);
        }

        /// <summary>
        /// 生成一个简单的全屏文字块
        /// </summary>
        /// <returns></returns>
        public TextPiece sc(string dialog)
        {
            return new TextPiece(id++, msgpanel, "", dialog, "", "");
        }
        #endregion


        public EffectPiece SetLive2dSprite(int depth, string modelName, int x = 0,int y = 0, float alpha = 1f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SetLive2dSpriteByDepth(depth, modelName));
            effects.Enqueue(NewEffectBuilder.SetAlphaByDepth(depth, alpha));
            effects.Enqueue(NewEffectBuilder.SetPostionByDepth(depth, new Vector3(x, y)));
            return new EffectPiece(id++, effects);
        }
        
        public EffectPiece FadeinLive2dSprite(int depth, string modelName, int x = 0, int y = 0, float fadein = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SetLive2dSpriteByDepth(depth, modelName));
            effects.Enqueue(NewEffectBuilder.SetAlphaByDepth(depth, 0));
            effects.Enqueue(NewEffectBuilder.SetPostionByDepth(depth, new Vector3(x, y)));
            effects.Enqueue(NewEffectBuilder.FadeInByDepth(depth, fadein));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece SetLive2dExpression(int depth, int expression)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.ChangeLive2dExpression(depth, expression));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece SetLive2dMotion(int depth, int mostion)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            //effects.Enqueue(NewEffectBuilder.ChangeLive2dExpression(depth, mostion));
            return new EffectPiece(id++, effects);
        }

        public MoviePiece PlayMovie(string filename)
        {
            return new MoviePiece(id++, filename);
        }


        #region EffectPiece部分
        /// <summary>
        /// 圆形旋转渐变特效（顺时针）
        /// </summary>
        /// <param name="spriteName">新图</param>
        /// <param name="inverse">是否逆向（默认否）</param>
        /// <param name="time">持续时长s（默认0.5s）</param>
        public EffectPiece RotateFade(string spriteName, bool inverse = false, float time = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.RotateFade(spriteName, inverse, time));
            effects.Enqueue(NewEffectBuilder.SetBackSprite(spriteName));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 单侧渐变特效
        /// </summary>
        /// <param name="spriteName">新图</param>
        /// <param name="direction">方向（默认：左）</param>
        /// <param name="time">持续时长s（默认0.5s）</param>
        public EffectPiece SideFade(int depth, string spriteName, string direction ="left", float time = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.PreTransByDepth(depth));
            effects.Enqueue(NewEffectBuilder.SetBackSprite(spriteName));
            effects.Enqueue(NewEffectBuilder.SideFade(spriteName, direction, time));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece SideFade(string spriteName, string direction = "left", float time = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SideFade(spriteName, direction, time));
            effects.Enqueue(NewEffectBuilder.SetBackSprite(spriteName));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 圆形展开特效（由内而外）
        /// </summary>
        /// <param name="spriteName">新图</param>
        /// <param name="inverse">是否逆向（默认否）</param>
        /// <param name="time">持续时长s（默认0.5s）</param>
        public EffectPiece Circle(string spriteName, bool inverse = false, float time = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.Circle(spriteName, inverse, time));
            effects.Enqueue(NewEffectBuilder.SetBackSprite(spriteName));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 卷动特效
        /// </summary>
        /// <param name="spriteName">新图</param>
        /// <param name="direction">方向（默认：左）</param>
        /// <param name="isBoth">是否两图同时卷动</param>
        /// <param name="time">持续时长s（默认0.5s）</param>
        public EffectPiece Scroll(string spriteName, string direction = "left", bool isBoth = false, float time = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            if (isBoth) effects.Enqueue(NewEffectBuilder.ScrollBoth(spriteName, direction, time));
            else effects.Enqueue(NewEffectBuilder.Scroll(spriteName, direction, time));
            effects.Enqueue(NewEffectBuilder.SetBackSprite(spriteName));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 通用渐变过渡
        /// </summary>
        /// <param name="spriteName">新图</param>
        /// <param name="ruleName">渐变图</param>
        /// <param name="time">持续时间s（默认0.5）</param>
        public EffectPiece Universial(string spriteName, string ruleName, float time = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.Universial(spriteName, ruleName, time));
            effects.Enqueue(NewEffectBuilder.SetBackSprite(spriteName));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 百叶窗过渡
        /// </summary>
        /// <param name="spriteName">新图</param>
        /// <param name="direction">方向（默认：左）</param>
        /// <param name="time">持续时长s（默认0.5s）</param>
        public EffectPiece Shutter(string spriteName, string direction = "left", float time = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.Shutter(spriteName, direction, time));
            effects.Enqueue(NewEffectBuilder.SetBackSprite(spriteName));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 蒙版
        /// </summary>
        /// <param name="maskName">蒙版图</param>
        public EffectPiece Mask(string maskName)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.Masking(maskName));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 模糊特效
        /// </summary>
        public EffectPiece Blur()
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.Blur());
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 马赛克特效
        /// </summary>
        public EffectPiece Mosaic()
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.Mosaic());
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 灰度化特效
        /// </summary>
        public EffectPiece Gray()
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.Gray());
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 老照片效果
        /// </summary>
        public EffectPiece OldPhoto()
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.OldPhoto());
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 等待
        /// </summary>
        /// <param name="time">时长s</param>
        public EffectPiece Wait(float time)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.Wait(time));
            return new EffectPiece(id++, effects);
        }

        #endregion


        #region Trans类
        /// <summary>
        /// 预渐变背景
        /// </summary>
        /// <returns></returns>
        public EffectPiece PreTransBackground()
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.PreTransBackSprite());
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 渐变
        /// </summary>
        /// <param name="transtime">转换时间</param>
        public EffectPiece TransBackground(float transtime = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            //effects.Enqueue(NewEffectBuilder.PreTransBackSprite(spriteName));
            effects.Enqueue(NewEffectBuilder.TransBackSprite(transtime));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 渐变到新的背景
        /// </summary>
        /// <param name="spriteName">目标图片名</param>
        /// <param name="transtime">转换时间</param>
        public EffectPiece TransBackgroundToImage(string spriteName, float transtime = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.PreTransBackSprite());
            effects.Enqueue(NewEffectBuilder.SetBackSprite(spriteName));
            effects.Enqueue(NewEffectBuilder.TransBackSprite(transtime));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece TransBackgroundToMovie(string spriteName, float transtime = 0.5f, bool isLoop = true)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.PreTransBackSprite());
            effects.Enqueue(NewEffectBuilder.SetBackSprite(spriteName, true, isLoop));
            effects.Enqueue(NewEffectBuilder.TransBackSprite(transtime));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 变更背景（淡出旧背景+淡入新背景）
        /// </summary>
        /// <param name="spriteName">需要更改的背景图片名</param>
        /// <param name="fadeout">原图淡出的时间，默认0.3s</param>
        /// <param name="fadein">新图淡入的时间，默认0.3s</param>
        public EffectPiece ChangeBackground(string spriteName, float fadeout = 0.5f, float fadein = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.FadeOutBackSprite(fadeout));
            effects.Enqueue(NewEffectBuilder.SetBackSprite(spriteName));
            effects.Enqueue(NewEffectBuilder.SetAlphaBackSprite(0));
            effects.Enqueue(NewEffectBuilder.FadeInBackSprite(fadein));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 预渐变立绘（预设坐标）
        /// </summary>
        /// <param name="depth">目标所在的层级</param>
        /// <param name="spriteName">新显示的图片名</param>
        /// <param name="position">新图片的位置</param>
        public EffectPiece PreTransCharacterSprite(int depth)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.PreTransByDepth(depth));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 预渐变立绘（xy坐标）
        /// </summary>
        /// <param name="depth">目标所在的层级</param>
        /// <param name="spriteName">新显示的图片名</param>
        /// <param name="x">x轴坐标</param>
        /// <param name="y">y轴坐标</param>
        public EffectPiece PreTransCharacterSprite(int depth, string spriteName, float x, float y)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.PreTransByDepth(depth, spriteName, new Vector3(x, y)));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 直接渐变立绘（预设坐标）
        /// </summary>
        /// <param name="depth">目标所在的层级</param>
        /// <param name="spriteName">新显示的图片名</param>
        /// <param name="position">新图片的位置，默认middle</param>
        /// <param name="transtime">渐变时间，默认0.5s</param>
        public EffectPiece TransCharacterSprite(int depth, float transtime = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.TransByDepth(depth, transtime));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 直接渐变立绘（xy坐标）
        /// </summary>
        /// <param name="depth">目标所在的层级</param>
        /// <param name="spriteName">新显示的图片名</param>
        /// <param name="x">x轴坐标</param>
        /// <param name="y">y轴坐标</param>
        /// <param name="transtime">渐变时间，默认0.5s</param>
        public EffectPiece TransCharacterSprite(int depth, string spriteName, float x, float y, float transtime = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.PreTransByDepth(depth, spriteName, new Vector3(x, y)));
            effects.Enqueue(NewEffectBuilder.TransByDepth(depth, transtime));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 淡入淡出立绘
        /// </summary>
        /// <param name="depth">目标所在的层级</param>
        /// <param name="spriteName">新显示的图片名</param>
        /// <param name="fadeout">原图淡出的时间，默认0.5s</param>
        /// <param name="fadein">淡入的时间，默认0.5s</param>
        public EffectPiece ChangeCharacterSprite(int depth, string spriteName, float fadeout = 0.5f, float fadein = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.FadeOutByDepth(depth, fadeout));
            effects.Enqueue(NewEffectBuilder.SetSpriteByDepth(depth, spriteName));
            effects.Enqueue(NewEffectBuilder.FadeInByDepth(depth, fadein));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 淡出所有立绘【需要与RemoveAllChara连用】
        /// </summary>
        /// <param name="fadeout">淡出时间s（默认0.3s）</param>
        public EffectPiece FadeoutAllChara(float fadeout = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.FadeOutAllChara(fadeout));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 淡出所有图片【需要与RemoveAllPic连用】
        /// </summary>
        /// <param name="fadeout">淡出时间s（默认0.3s）</param>
        public EffectPiece FadeoutAllPic(float fadeout = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.FadeOutAllPic(fadeout));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 淡出所有（包括对话框）【需要与RemoveAll连用】
        /// </summary>
        /// <param name="fadeout">淡出时间s（默认0.3s）</param>
        public EffectPiece FadeoutAll(float fadeout = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.FadeOutAll(fadeout));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece TransAll(float transtime = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.TransAll(transtime));
            return new EffectPiece(id++, effects);
        }


        /// <summary>
        /// 设置背景（不带有转换特效）
        /// </summary>
        /// <param name="spriteName">需要更改的背景图片名</param>
        public EffectPiece SetBackground(string spriteName)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SetBackSprite(spriteName));
            effects.Enqueue(NewEffectBuilder.SetAlphaBackSprite(1));
            return new EffectPiece(id++, effects);

        }

        public EffectPiece SetMovieBackground(string filename, bool isLoop = true)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SetBackSprite(filename, true, isLoop));
            effects.Enqueue(NewEffectBuilder.SetAlphaBackSprite(0));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 设置立绘（预设位置：左 中 右）
        /// </summary>
        /// <param name="depth">目标所在的层级</param>
        /// <param name="spriteName">需要更改的背景图片名</param>
        /// <param name="position">left | middle | right 左中右</param>
        public EffectPiece SetCharacterSprite(int depth, string spriteName, string position = "middle")
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SetSpriteByDepth(depth, spriteName));
            effects.Enqueue(NewEffectBuilder.SetAlphaByDepth(depth, 1));
            effects.Enqueue(NewEffectBuilder.SetDefaultPostionByDepth(depth, position));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 设置立绘（带坐标）
        /// </summary>
        /// <param name="depth">目标所在的层级</param>
        /// <param name="spriteName">需要更改的背景图片名</param>
        /// <param name="x">x轴坐标</param>
        /// <param name="y">y轴坐标</param>
        public EffectPiece SetCharacterSprite(int depth, string spriteName, float x, float y, float alpha)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SetSpriteByDepth(depth, spriteName));
            effects.Enqueue(NewEffectBuilder.SetAlphaByDepth(depth, alpha));
            effects.Enqueue(NewEffectBuilder.SetPostionByDepth(depth, new Vector3(x, y)));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece SetVideoSprite(int depth, string filename)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SetSpriteByDepth(depth, filename, true));
            effects.Enqueue(NewEffectBuilder.SetAlphaByDepth(depth, 0));
            effects.Enqueue(NewEffectBuilder.SetPostionByDepth(depth, new Vector3(0, 0)));
            return new EffectPiece(id++, effects);
        }


        #endregion


        #region Action类
        /// <summary>
        /// 设置背景（从0淡入）
        /// </summary>
        /// <param name="fadein">新图淡入的时间，默认0.5s</param>
        public EffectPiece FadeinBackground(float fadein = 0.5f, bool sync = false)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            //effects.Enqueue(NewEffectBuilder.SetBackSprite(spriteName));
            //effects.Enqueue(NewEffectBuilder.SetAlphaBackSprite(0));
            effects.Enqueue(NewEffectBuilder.FadeInBackSprite(fadein));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 设置动态背景
        /// </summary>
        /// <param name="spriteName">图片名</param>
        /// <param name="fadein">新图淡入的时间，默认0.5s</param>
        public EffectPiece FadeinBackground(string spriteName, float fadein = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SetBackSprite(spriteName));
            effects.Enqueue(NewEffectBuilder.SetAlphaBackSprite(0));
            effects.Enqueue(NewEffectBuilder.FadeInBackSprite(fadein));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 设置动态背景
        /// </summary>
        /// <param name="spriteName">影片名</param>
        /// <param name="fadein">新图淡入的时间，默认0.3s</param>
        public EffectPiece FadeinMovieBackground(string spriteName, float fadein = 0.5f, bool isLoop = true)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SetBackSprite(spriteName, true, isLoop));
            effects.Enqueue(NewEffectBuilder.SetAlphaBackSprite(0));
            effects.Enqueue(NewEffectBuilder.FadeInBackSprite(fadein));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 移除背景（淡出）
        /// </summary>
        /// <param name="fadeout">原图淡出的时间，默认0.3s</param>
        public EffectPiece FadeoutBackground(float fadeout = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.FadeOutBackSprite(fadeout));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 淡入立绘（预设坐标：左 中 右）
        /// </summary>
        /// <param name="depth">目标所在的层级</param>
        /// <param name="spriteName">需要更改的背景图片名</param>
        /// <param name="position">left | middle | right</param>
        /// <param name="fadein">淡入的时间，默认0.5s</param>
        public EffectPiece FadeinCharacterSprite(int depth, string spriteName, string position = "middle", float fadein = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SetSpriteByDepth(depth, spriteName));
            effects.Enqueue(NewEffectBuilder.SetAlphaByDepth(depth, 0));
            effects.Enqueue(NewEffectBuilder.SetDefaultPostionByDepth(depth, position));
            effects.Enqueue(NewEffectBuilder.FadeInByDepth(depth, fadein));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 淡入立绘（xy坐标）
        /// </summary>
        /// <param name="depth">目标所在的层级</param>
        /// <param name="spriteName">需要更改的背景图片名</param>
        /// <param name="x">x轴坐标</param>
        /// <param name="y">y轴坐标</param>
        /// <param name="fadein">淡入的时间，默认0.5s</param>
        public EffectPiece FadeinCharacterSprite(int depth, float fadein = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            //effects.Enqueue(NewEffectBuilder.SetSpriteByDepth(depth, spriteName));
            //effects.Enqueue(NewEffectBuilder.SetAlphaByDepth(depth, 0));
            //effects.Enqueue(NewEffectBuilder.SetPostionByDepth(depth, new Vector3(x, y)));
            effects.Enqueue(NewEffectBuilder.FadeInByDepth(depth, fadein));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece FadeinVideoSprite(int depth, string filename, float fadein = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SetSpriteByDepth(depth, filename, true));
            effects.Enqueue(NewEffectBuilder.SetAlphaByDepth(depth, 0));
            effects.Enqueue(NewEffectBuilder.FadeInByDepth(depth, fadein));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece FadeinVideoSprite(int depth, float fadein)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.FadeInByDepth(depth, fadein));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 淡出立绘
        /// </summary>
        /// <param name="depth">目标所在的层级</param>
        /// <param name="fadeout">淡出的时间，默认0.5s</param>
        public EffectPiece FadeoutCharacterSprite(int depth, float fadeout = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.FadeOutByDepth(depth, fadeout));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 移动立绘（从当前位置移动）
        /// </summary>
        /// <param name="depth">目标所在层级</param>
        /// <param name="position">left | middle | right</param>
        /// <param name="time">移动时间，默认0.5s</param>
        public EffectPiece MoveCharacterSprite(int depth, string position, float time = 0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.MoveDefaultByDepth(depth, position, time));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 移动立绘（从当前位置移动）
        /// </summary>
        /// <param name="depth">目标所在的层级</param>
        /// <param name="x">目标位置的x轴坐标</param>
        /// <param name="y">目标位置的y轴坐标</param>
        /// <param name="time">移动的时间，默认0.5s</param>
        public EffectPiece MoveCharacterSprite(int depth, float x, float y, float time = 0.5f, bool sync = false)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.MoveByDepth(depth, new Vector3(x, y), time, sync));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece MoveCharacterSpriteTo(int depth, float x, float y, float time = 0.5f, bool sync = false)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.MoveByDepth(depth, new Vector3(x, y), time, sync));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 移动立绘（从设置的坐标开始移动）
        /// </summary>
        /// <param name="depth">目标所在的层级</param>
        /// <param name="x_o">移动起点的x轴坐标</param>
        /// <param name="y_o">移动起点的y轴坐标</param>
        /// <param name="x">目标位置的x轴坐标</param>
        /// <param name="y">目标位置的y轴坐标</param>
        /// <param name="time">移动的时间，默认0.5s</param>
        public EffectPiece MoveCharacterSprite(int depth, float x_o, float y_o, float x, float y, float time = 0.5f, bool sync = false)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SetPostionByDepth(depth, new Vector3(x_o, y_o)));
            effects.Enqueue(NewEffectBuilder.MoveByDepth(depth, new Vector3(x, y), time, sync));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece MoveBackground(float x, float y, float time = 0.5f, bool sync = false)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.MoveByDepth(-1, new Vector3(x, y), time, sync));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece MoveBackground(float x_o, float y_o, float x, float y, float time = 0.5f, bool sync = false)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SetPostionByDepth(-1, new Vector3(x_o, y_o)));
            effects.Enqueue(NewEffectBuilder.MoveByDepth(-1, new Vector3(x, y), time, sync));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece RotateCharacterSprite(int depth, float angle, float time = 0.5f, bool sync = false)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.RotateToByDepth(depth, new Vector3(0, 0, angle), time));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece RotateCharacterSprite(int depth, float from, float to, float time = 0.5f, bool sync = false)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SetRotateByDepth(depth, new Vector3(0, 0, from)));
            effects.Enqueue(NewEffectBuilder.RotateToByDepth(depth, new Vector3(0, 0, to), time));
            return new EffectPiece(id++, effects);
        }


        public EffectPiece RotateBackground(float angle, float time = 0f, bool sync = false)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.RotateToByDepth(-1, new Vector3(0, 0, angle), time));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece RotateBackground(float from, float to, float time = 0f, bool sync = false)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SetRotateByDepth(-1, new Vector3(0, 0, from)));
            effects.Enqueue(NewEffectBuilder.RotateToByDepth(-1, new Vector3(0, 0, to), time));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece ScaleSprite(int depth, float x, float y, bool sync = false, float time = 0f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.ScaleToByDepth(depth, new Vector3(x, y, 1), time));
            return new EffectPiece(id++, effects);
        }

        public EffectPiece ScaleSprite(int depth, float scale, bool sync = false, float time = 0f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.ScaleToByDepth(depth, new Vector3(scale, scale, 1), time));
            return new EffectPiece(id++, effects);
        }

        //public EffectPiece ScaleBackground(float x, float y, float time = 0.5f, bool sync = false)
        //{
        //    Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
        //    effects.Enqueue(NewEffectBuilder.ScaleToByDepth(-1, new Vector3(x, y, 1), time));
        //    return new EffectPiece(id++, effects);
        //}

        public EffectPiece ScaleBackground(float scale, float time = 0.5f, bool sync = false)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.ScaleToByDepth(-1, new Vector3(scale, scale, 1), time));
            return new EffectPiece(id++, effects);
        }


        /// <summary>
        /// 立绘摇动
        /// </summary>
        /// <param name="time"></param>
        public EffectPiece SpriteShake()
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            //effects.Enqueue(NewEffectBuilder.SpriteShakeByDepth(freq, v, freq, time));
            return new EffectPiece(id++, effects);
        }

        /// <summary>
        /// 立绘震动
        /// </summary>
        /// <param name="depth">震动对象</param>
        /// <param name="v">震动量</param>
        /// <param name="speed">每秒震动次数</param>
        /// <param name="time">持续时间</param>
        public EffectPiece SpriteVibration(int depth, float v=10f, int freq = 100 , float time=0.5f)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.SpriteShakeByDepth(depth, v, freq, time));
            return new EffectPiece(id++, effects);
        }

            /// <summary>
            /// 窗口抖动（含UI）
            /// </summary>
            /// <param name="time">持续时间s，0则为永久</param>
            /// <param name="v">震动量</param>
            /// <param name="freq">每秒震动次数</param>
        public EffectPiece WindowVibration(float time = 0.5f, float v = 0.01f, int freq = 100)
        {
            Queue<NewImageEffect> effects = new Queue<NewImageEffect>();
            effects.Enqueue(NewEffectBuilder.WindowShake(v, freq, time));
            return new EffectPiece(id++, effects);
        }
        #endregion

        #region 其他功能Piece
        /// <summary>
        /// 打开姓名输入框
        /// </summary>
        //public InputPiece SetName()
        //{
        //    return new InputPiece(id++, inputpanel);
        //}

        /// <summary>
        /// 章节名显示
        /// </summary>
        /// <param name="chapter">显示内容</param>
        //public ChapterNamePiece ShowChapter(string chapter="")
        //{
        //    return new ChapterNamePiece(id++, sidepanel ,chapter);
        //}

        /// <summary>
        /// 时间切换特效
        /// </summary>
        /// <param name="time">显示时间</param>
        /// <param name="place">显示文字</param>
        //public TimeSwitchPiece TimeSwitch(string time, string place)
        //{
        //    return new TimeSwitchPiece(id++, timepanel, time, place);
        //}

        /// <summary>
        /// 获得证据
        /// </summary>
        /// <param name="eviName">证据唯一ID</param>
        //public EviGetPiece GetEvidence(string eviName)
        //{
        //    return new EviGetPiece(id++, evipanel, eviName);
        //}

        /// <summary>
        ///  扣血
        /// </summary>
        /// <param name="x">血量，注意负数</param>
        //public HPPiece HPChange(int x)
        //{
        //    return new HPPiece(id++, hpui, x);
        //}

        /// <summary>
        /// 隐去对话框
        /// </summary>
        /// <param name="time">淡出的时间，默认0.5s</param>
        public DiaboxPiece CloseDialog(float time = 0.5f)
        {
            return new DiaboxPiece(id++, msgpanel, false, time);
        }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="time">淡入的时间，默认0.5s</param>
        public DiaboxPiece OpenDialog(float time = 0.5f)
        {
            return new DiaboxPiece(id++, msgpanel, true, time);
        }
        #endregion

        #region SoundPiece部分
        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="name">音乐名</param>
        /// <param name="fadein">淡入时间，默认0.5s</param>
        /// <param name="loop">是否循环，默认为否</param>
        /// <returns></returns>
        public SoundPiece PlayBGM(string name, float fadein = 0.5f, bool loop = true)
        {
            Queue<SoundEffect> effectsq = new Queue<SoundEffect>();
            effectsq.Enqueue(SoundBuilder.SetBGM(name, fadein, loop));
            //effectsq.Enqueue(SoundBuilder.FadeInBGM(fadein));
            return new SoundPiece(id++, effectsq);
        }

        /// <summary>
        /// 停止背景音乐
        /// </summary>
        /// <param name="fadeout">淡出时间，默认0.5s</param>
        /// <returns></returns>
        public SoundPiece StopBGM(float fadeout = 0.5f)
        {
            Queue<SoundEffect> effectsq = new Queue<SoundEffect>();
            if (fadeout != 0f) effectsq.Enqueue(SoundBuilder.VolumeDownBGM(fadeout, 1));
            effectsq.Enqueue(SoundBuilder.RemoveBGM());
            return new SoundPiece(id++, effectsq);
        }

        /// <summary>
        /// 暂停背景音乐
        /// </summary>
        /// <param name="fadeout">淡出时间，默认0s</param>
        /// <returns></returns>
        public SoundPiece PauseBGM(float fadeout = 0f)
        {
            Queue<SoundEffect> effectsq = new Queue<SoundEffect>();
            if (fadeout != 0f) effectsq.Enqueue(SoundBuilder.VolumeDownBGM(fadeout, 1));
            effectsq.Enqueue(SoundBuilder.PauseBGM());
            return new SoundPiece(id++, effectsq);
            //return null;
        }

        /// <summary>
        /// 继续背景音乐
        /// </summary>
        /// <param name="fadein">淡入时间，默认0</param>
        /// <returns></returns>
        public SoundPiece UnpauseBGM(float fadein = 0f)
        {
            Queue<SoundEffect> effectsq = new Queue<SoundEffect>();
            effectsq.Enqueue(SoundBuilder.UnpauseBGM());
            if (fadein != 0f) effectsq.Enqueue(SoundBuilder.VolumeUpBGM(fadein, 1));
            return new SoundPiece(id++, effectsq);
        }

        public SoundPiece ChangeVolumeBGM(float time,float p)
        {
            Queue<SoundEffect> effectsq = new Queue<SoundEffect>();
            effectsq.Enqueue(SoundBuilder.VolumeUpBGM(time, p));
            return new SoundPiece(id++, effectsq);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="name">音效名</param>
        /// <param name="fadein">淡入时间，默认0s</param>
        /// <param name="loop">是否循环，默认为否</param>
        /// <returns></returns>
        public SoundPiece PlaySE(string name, float fadein = 0f, bool loop = false)
        {
            Queue<SoundEffect> effectsq = new Queue<SoundEffect>();
            effectsq.Enqueue(SoundBuilder.SetSE(name, fadein, loop));
            //effectsq.Enqueue(SoundBuilder.FadeInSE(fadein));
            return new SoundPiece(id++, effectsq);
        }

        /// <summary>
        /// 停止当前正在播放的音效
        /// </summary>
        /// <param name="fadeout">淡出时间，默认0s</param>
        /// <returns></returns>
        public SoundPiece StopSE(float fadeout = 0f)
        {
            Queue<SoundEffect> effectsq = new Queue<SoundEffect>();
            if (fadeout != 0f) effectsq.Enqueue(SoundBuilder.VolumeDownSE(fadeout, 1));
            effectsq.Enqueue(SoundBuilder.RemoveSE());
            return new SoundPiece(id++, effectsq);
        }
        #endregion
        
    }
}
