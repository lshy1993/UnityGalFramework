using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.Effect
{
    /// <summary>
    /// 用于控制 gameObject 的【特效
    /// 例如 位置移动 透明度 等变化
    /// 提供最【基本】的变化方式
    /// 复杂的【特效】由 AnimationBuilder 组合
    /// </summary>
    public class EffectBuilder
    {
        static ImageManager imageManager;
        static SoundManager soundManager;
        //static CharacterManager characterManager;
        public static Image backgroundSprite;
        public static GameObject dialog, click, charaPanel;
        private static Text dialoglabel, namelabel;

        public static void Init(ImageManager im, SoundManager sm) //, CharacterManager cm
        {
            imageManager = im;
            soundManager = sm;
            //characterManager = cm;
            //获取由ImageManager找到的组件
            //backgroundSprite = im.bgSprite;
            //click = im.clickContainer;
            //charaPanel = im.fgPanel.gameObject;
            //dialog = im.dialogContainer;
            //dialoglabel = im.dialogLabel;
            //namelabel = im.nameLabel;
        }


        public static ImageEffect Parallel(List<ImageEffect> effects)
        {
            ImageEffect effect = new ImageEffect();
            List<float> times = new List<float>();

            foreach (ImageEffect e in effects)
            {
                times.Add(e.time);
            }

            effect.time = times.Max();
            effect.origin = null;
            effect.final = null;
            effect.loop = false;
            effect.init = () =>
            {
                foreach (ImageEffect e in effects)
                {
                    e.init();
                }
            };

            effect.finish = () =>
            {
                foreach (ImageEffect e in effects)
                {
                    e.finish();
                }
            };

            effect.update = (a, time, nowtime) =>
            {
                foreach (ImageEffect e in effects)
                {
                    if (nowtime < e.time)
                        e.update(e.aimUI, e.time, nowtime);
                }
            };

            return effect;
        }

        private ImageEffect imageEffect;

        public EffectBuilder UI(Image sprite)
        {
            imageEffect = new ImageEffect();
            imageEffect.aimUI = sprite;
            return this;
        }

        public EffectBuilder Alpha(float alpha)
        {
            if (imageEffect.origin == null)
            {
                imageEffect.origin = new SpriteStatus();
            }

            imageEffect.origin.alpha = alpha;
            return this;
        }

        public EffectBuilder Position(Vector3 position)
        {
            if (imageEffect.origin == null)
            {
                imageEffect.origin = new SpriteStatus();
            }

            imageEffect.origin.position = position;
            return this;
        }

        public EffectBuilder TotalTime(float time)
        {
            imageEffect.time = time;
            return this;
        }

        public EffectBuilder Init(Action init)
        {
            imageEffect.init = init;
            return this;
        }

        public EffectBuilder Finish(Action finish)
        {
            imageEffect.finish = finish;
            return this;
        }

        public EffectBuilder AnimateUpdate(SingleUpdate update)
        {
            imageEffect.update = new SingleUpdate(update);
            return this;
        }

        public ImageEffect Get() { return imageEffect; }


        public static ImageEffect FadeIn(Image sprite, float time)
        {
            EffectBuilder builder = new EffectBuilder();
            ImageEffect e = builder.UI(sprite)
                .TotalTime(time)
                .Alpha(0)
                .AnimateUpdate((aim, totaltime, nowtime) =>
                {
                    if (nowtime < totaltime)
                    {
                        float a = Mathf.MoveTowards(aim.color.a, 255, 255 / totaltime * Time.fixedDeltaTime);
                        aim.color = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(a));
                    }
                }).Get();

            return e;
        }

        public static ImageEffect FadeOut(Image sprite, float time)
        {
            EffectBuilder builder = new EffectBuilder();
            ImageEffect e = builder.UI(sprite)
                .TotalTime(time)
                .Alpha(1)
                .AnimateUpdate((aim, totaltime, nowtime) =>
                {
                    if (nowtime < totaltime)
                    {
                        float a = Mathf.MoveTowards(aim.color.a, 0, 255 / totaltime * Time.fixedDeltaTime);
                        aim.color = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(a));
                    }
                }).Get();

            return e;
        }

        public static ImageEffect Move(Image sprite, Vector3 origin, Vector3 final, float time)
        {
            EffectBuilder builder = new EffectBuilder();
            ImageEffect e = builder.UI(sprite)
                .TotalTime(time)
                .Init(() => { sprite.transform.position = origin; })
                .Finish(() => { sprite.transform.position = final; })
                .AnimateUpdate((aim, totaltime, nowtime) =>
                {
                    if (nowtime < totaltime)
                    {
                        float a = Mathf.MoveTowards(aim.color.a, 255, 255 / totaltime * Time.fixedDeltaTime);
                        aim.color = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(a));
                    }
                }).Get();
            return e;
        }

        public static ImageEffect ChangeSprite(Image ui, Sprite sprite)
        {
            EffectBuilder builder = new EffectBuilder();
            ImageEffect e = builder.UI(ui)
                .TotalTime(0)
                .Init(() => { ui.sprite = sprite; })
                .AnimateUpdate((aim, totaltime, nowtime) => { })
                .Get();
            return e;
        }

        public static ImageEffect RemoveSprite(Image sprite)
        {
            return ChangeSprite(sprite, null);
            //EffectBuilder builder = new EffectBuilder();
            //ImageEffect e = builder.UI(sprite)
            //    .TotalTime(0)
            //    .Init(() => { sprite.sprite2D = null; })
            //    .AnimateUpdate((aim, totaltime, nowtime) => { })
            //    .Get();
            //return e;
        }

        #region 新增内容2 查询已有图片
        public static List<int> GetDepthNum()
        {
            List<int> nums = new List<int>();
            foreach(Transform child in charaPanel.transform)
            {
                int x = Convert.ToInt32(child.name.Remove(0, 6));
                nums.Add(x);
            }
            return nums;
        }
        #endregion

        #region 新增内容 根据Depth深度变动Sprite
        /* 采用了krkr的图像处理方式
         * Depth为图像的唯一编号
         * 表示了深度，数字越大越靠前
        */
        public static ImageEffect SetSpriteByDepth(int depth, Sprite sprite)
        {
            Image ui = null;
            if (charaPanel.transform.Find("sprite" + depth) != null)
            {
                ui = charaPanel.transform.Find("sprite" + depth).GetComponent<Image>();
            }
            else
            {
                GameObject go = Resources.Load("Prefab/Character") as GameObject;
                //go = NGUITools.AddChild(charaPanel, go);
                go.transform.parent = charaPanel.transform;
                go.transform.name = "sprite" + depth;
                ui = go.GetComponent<Image>();
            }
            EffectBuilder builder = new EffectBuilder();
            ImageEffect e = builder.UI(ui)
                .TotalTime(0)
                .Init(() =>
                {
                    //ui.alpha = 0;
                    ui.color = new Color32(0, 0, 0, 0);
                    //ui.depth = depth;
                    ui.sprite = sprite;
                    //ui.MakePixelPerfect();
                })
                .AnimateUpdate((aim, totaltime, nowtime) => { }).Get();
            return e;
        }

        public static ImageEffect DeleteSpriteByDepth(int depth)
        {
            if (charaPanel.transform.Find("sprite" + depth) == null)
            {
                return null;
            }
            else
            {
                EffectBuilder builder = new EffectBuilder();
                Image ui = charaPanel.transform.Find("sprite" + depth).GetComponent<Image>();
                ImageEffect e = builder.UI(ui)
                    .TotalTime(0)
                    .Init(()=> {
                        //if (charaPanel.transform.Find("sprite" + depth) != null)
                        //if(ui!=null)
                        // GameObject.Destroy(ui.transform.gameObject);
                        ui.sprite = null;
                    })
                    .AnimateUpdate((aim, totaltime, nowtime) => { })
                    .Finish(() => {  })
                    .Get();
                return e;
            }
        }

        public static ImageEffect ChangeByDepth(int depth, Sprite sprite)
        {
            if (charaPanel.transform.Find("sprite" + depth) == null)
            {
                return null;
            }
            else
            {
                EffectBuilder builder = new EffectBuilder();
                Image ui = charaPanel.transform.Find("sprite" + depth).GetComponent<Image>();
                ImageEffect e = builder.UI(ui)
                    .TotalTime(0)
                    .Init(() =>
                    {
                        ui.sprite = sprite;
                        //ui.MakePixelPerfect();
                    })
                    .AnimateUpdate((aim, totaltime, nowtime) => { })
                    .Finish(() => { })
                    .Get();
                return e;
            }
        }

        public static ImageEffect FadeInByDepth(int depth, float time)
        {
            Image ui;
            if (charaPanel.transform.Find("sprite" + depth) != null)
            {
                ui= charaPanel.transform.Find("sprite" + depth).GetComponent<Image>();
            }
            else
            {
                return null;
            }
            EffectBuilder builder = new EffectBuilder();
            ImageEffect e = builder.UI(ui)
                .TotalTime(time)
                .Init(() => { ui.color = new Color32(0, 0, 0, 0); })
                .AnimateUpdate((aim, totaltime, nowtime) =>
                {
                    if (nowtime < totaltime)
                    {
                        float a = Mathf.MoveTowards(aim.color.a, 255, 255 / totaltime * Time.fixedDeltaTime);
                        aim.color = new Color32(0, 0, 0, (byte)a); 
                    }
                })
                .Finish(() => { ui.color = new Color32(0, 0, 0, 255); }).Get();

            return e;
        }

        public static ImageEffect FadeOutByDepth(int depth, float time)
        {
            Image ui;
            if (charaPanel.transform.Find("sprite" + depth) != null)
            {
                ui = charaPanel.transform.Find("sprite" + depth).GetComponent<Image>();
            }
            else
            {
                return null;
            }
            EffectBuilder builder = new EffectBuilder();
            ImageEffect e = builder.UI(ui)
                .TotalTime(time)
                .Init(()=> { ui.color = new Color32(0, 0, 0, 255); })
                .AnimateUpdate((aim, totaltime, nowtime) =>
                {
                    if (nowtime < totaltime)
                    {
                        float a = Mathf.MoveTowards(aim.color.a, 0, 255 / totaltime * Time.fixedDeltaTime);
                        aim.color = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(a));
                    }
                })
                .Finish(()=> { ui.color = new Color32(0, 0, 0, 0); })
                .Get();

            return e;
        }

        public static ImageEffect SetPostionByDepth(int depth, Vector3 position)
        {
            Image ui = null;
            if (charaPanel.transform.Find("sprite" + depth) != null)
            {
                ui = charaPanel.transform.Find("sprite" + depth).GetComponent<Image>();
            }
            else
            {
                return null;
            }
            EffectBuilder builder = new EffectBuilder();
            ImageEffect e = builder.UI(ui)
                .TotalTime(0)
                .Init(() =>
                {
                    ui.transform.localPosition = position;
                })
                .AnimateUpdate((aim, totaltime, nowtime) => { }).Get();
            return e;
        }

        public static ImageEffect SetDefaultPostionByDepth(int depth, string pstr)
        {
            Image ui = null;
            if (charaPanel.transform.Find("sprite" + depth) != null)
            {
                ui = charaPanel.transform.Find("sprite" + depth).GetComponent<Image>();
            }
            else
            {
                return null;
            }
            float x;
            switch (pstr)
            {
                case "left":
                    x = -320;
                    break;
                case "middle":
                    x = 0;
                    break;
                case "right":
                    x = 320;
                    break;
                default:
                    x = 0;
                    break;
            }
            EffectBuilder builder = new EffectBuilder();
            ImageEffect e = builder.UI(ui)
                .TotalTime(0)
                .Init(() =>
                {
                    ui.transform.localPosition = new Vector3(x, -360 + ui.rectTransform.rect.height / 2);
                })
                .AnimateUpdate((aim, totaltime, nowtime) => { }).Get();
            return e;
        }

        public static ImageEffect MoveByDepth(int depth, Vector3 final, float time)
        {
            Image ui = null;
            if (charaPanel.transform.Find("sprite" + depth) != null)
            {
                ui = charaPanel.transform.Find("sprite" + depth).GetComponent<Image>();
            }
            else
            {
                return null;
            }
            EffectBuilder builder = new EffectBuilder();
            Vector3 origin = ui.transform.localPosition;
            ImageEffect e = builder.UI(ui)
                .TotalTime(time)
                .Init(() => { })
                .AnimateUpdate((aim, totaltime, nowtime) =>
                {
                    if (nowtime < totaltime)
                    {
                        float x = Mathf.MoveTowards(aim.transform.localPosition.x, final.x, Math.Abs(final.x - aim.transform.localPosition.x) / (totaltime - nowtime) * Time.fixedDeltaTime);
                        float y = Mathf.MoveTowards(aim.transform.localPosition.y, final.y, Math.Abs(final.y - aim.transform.localPosition.y) / (totaltime - nowtime) * Time.fixedDeltaTime);
                        aim.transform.localPosition = new Vector3(x, y);
                    }
                })
                .Finish(() => { })
                .Get();
            return e;
        }
        #endregion

        public static ImageEffect SetDialog(bool open)
        {
            EffectBuilder builder = new EffectBuilder();
            ImageEffect e = builder.UI(null)
                .TotalTime(0)
                .Init(() =>
                {
                    dialog.SetActive(open);
                    dialog.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                    //namelabel.text = "";
                    //dialoglabel.text = "";
                    //dialog.transform.Find("NextIcon_Sprite").gameObject.SetActive(false);
                })
                .AnimateUpdate((aim, totaltime, nowtime) =>{ })
                .Finish(() => { })
                .Get();
            return e;
        }

        public static ImageEffect FadeInDialog(float time)
        {
            EffectBuilder builder = new EffectBuilder();
            ImageEffect e = builder.UI(null)
                .TotalTime(time)
                .Init(() => { })
                .AnimateUpdate((aim, totaltime, nowtime) =>
                {
                    if (nowtime < totaltime)
                    {
                        float t = Mathf.MoveTowards(dialog.GetComponent<Image>().color.a, 255, 255 / totaltime * Time.fixedDeltaTime);
                        dialog.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(t));
                    }
                })
                .Finish(() =>
                {
                    dialog.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
                })
                .Get();
            return e;
        }

        public static ImageEffect FadeOutDialog(float time)
        {
            EffectBuilder builder = new EffectBuilder();
            ImageEffect e = builder.UI(null)
                .TotalTime(time)
                .Init(() =>
                {
                    dialog.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
                })
                .AnimateUpdate((aim, totaltime, nowtime) =>
                {
                    if (nowtime < totaltime)
                    {
                        float t = Mathf.MoveTowards(dialog.GetComponent<Image>().color.a, 0, 255 / totaltime * Time.fixedDeltaTime);
                        dialog.GetComponent<Image>().color = new Color32(0, 0, 0, (byte)Mathf.RoundToInt(t));
                    }
                })
                .Finish(() =>
                {
                    dialog.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
                    //dialoglabel.text = "";
                    //namelabel.text = "";
                })
                .Get();
            return e;
        }

        public static ImageEffect BlockClick(bool block)
        {
            EffectBuilder builder = new EffectBuilder();
            ImageEffect e = builder.UI(null)
                .TotalTime(0)
                .Init(() => { click.SetActive(block); })
                .Finish(() => { })
                .AnimateUpdate((aim, totaltime, nowtime) => { })
                .Get();
            return e;
        }
    }
}
