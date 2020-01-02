using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Script.Framework.Effect;

using UnityEngine;

using DG.Tweening;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    public class TitleAnimation : PanelAnimation
    {
        /// <summary>
        /// 文字标题
        /// </summary>
        private GameObject titleCon;

        /// <summary>
        /// 主按钮Panel
        /// </summary>
        private GameObject btnCon;

        /// <summary>
        /// 动态背景层
        /// </summary>
        private GameObject videoCon;

        //public TitleVideoEffect tve;

        /// <summary>
        /// 底部外链按钮
        /// </summary>
        private GameObject otherCon;

        /// <summary>
        /// 侧边标签容器
        /// </summary>
        //private GameObject sideLabelCon;

        /// <summary>
        /// 粒子特效容器
        /// </summary>
        //private GameObject particleCon;

        private Sequence twq;

        //private GameObject backSprite, videoSprite, videoHover;
        private GameObject titleLabel, subtitleLabel, sideLabel;

        public override void Init()
        {
            titleCon = transform.Find("Title_Container").gameObject;
            
            btnCon = transform.Find("MainButton_Table").gameObject;
            videoCon = transform.Find("Video_Sprite").gameObject;
            //particleCon = this.transform.Find("Title_Container/Particle_Container").gameObject;
            
            //sideLabelCon = this.transform.Find("SideLabel_Container").gameObject;
            otherCon = transform.Find("Other_Container").gameObject;
            
            // 子部件
            titleLabel = titleCon.transform.Find("Title_Label").gameObject;
            subtitleLabel = titleCon.transform.Find("SubTitle_Label").gameObject;
            //sideLabel = sideLabelCon.transform.Find("SideTex_Label").gameObject;

            base.Init();
        }

        public override void BeforeClose()
        {
            BlockBtn(false);
        }

        /// <summary>
        /// 自定义退出界面动效
        /// </summary>
        public override IEnumerator CloseSequence(UIAnimationCallback callback)
        {
            // 飞出按钮
            AnimationCurve ac = AniCurveManager.GetInstance().Curve84;
            if(twq != null)
            {
                twq.Kill();
            }
            twq = DOTween.Sequence();
            //videoSprite.GetComponent<RectTransform>()
            //    .DOAnchorPosX(-45f, 0.66f);
            
            twq.Append(
                btnCon.GetComponent<CanvasGroup>()
                .DOFade(0, 0.5f)
                .OnStart(() => { btnCon.SetActive(true); })
            );

            for (int i = 0; i < btnCon.transform.childCount; i++)
            {
                Transform tr = btnCon.transform.GetChild(i);
                twq.Join(
                    tr.GetComponent<RectTransform>()
                    .DOAnchorPosX(280, 0.5f)
                    .SetEase(ac)
                    .SetDelay(0.05f * i)
                );
            }
            twq.Play();
            
            yield return new WaitForSeconds(0.2f);

            yield return base.CloseSequence(() =>
            {
                //particleCon.SetActive(false);
                videoCon.SetActive(false);
                callback();
            });
        }
 
        /// <summary>
        /// 自定义进入界面动效
        /// </summary>
        public override IEnumerator OpenSequence(UIAnimationCallback callback)
        {
            //Debug.Log("Animation Before Open");
            BlockBtn(true);
            InitLabel();
            GetComponent<TitleUIManager>().Init();

            //yield return StartCoroutine(tve.Init());

            yield return base.OpenSequence(() =>
            {
                StartCoroutine(EnterAnimation());
                callback();
            });
        }

        private IEnumerator EnterAnimation()
        {
            StartCoroutine(ShowTitleText());
            StartCoroutine(ShowBtn());
            yield return StartCoroutine(ShowOther());
        }

        //文字显示动画
        private IEnumerator ShowTitleText()
        {
            AnimationCurve ac = AniCurveManager.GetInstance().Curve84;
            Sequence sq = DOTween.Sequence();
            sq.Pause();
            //sq.Append(titleCon.GetComponent<CanvasGroup>()
            //        .DOFade(1, 0.5f)
            //        .OnStart(() =>
            //        {
            //            titleCon.GetComponent<CanvasGroup>().alpha = 0;
            //            titleCon.SetActive(true);
            //        }));
            sq.Append(titleLabel.GetComponent<Text>()
                .DOFade(1, 0.5f)
                .SetEase(ac));
            sq.Append(subtitleLabel.GetComponent<Text>()
                .DOFade(1, 0.5f)
                .SetEase(ac));
            sq.Play();
            yield return sq.WaitForCompletion();
        }

        //按钮显示动画
        private IEnumerator ShowBtn()
        {
            AnimationCurve ac = AniCurveManager.GetInstance().Curve84;
            Sequence sq = DOTween.Sequence();
            sq.Pause();
            sq.Append(btnCon.GetComponent<CanvasGroup>()
                    .DOFade(1, 0.5f)
                    .OnStart(() => {
                        Debug.Log("anistart");
                        btnCon.GetComponent<CanvasGroup>().alpha = 0;
                        btnCon.SetActive(true);
                    }));
            for (int i = 0; i < btnCon.transform.childCount; i++)
            {
                Transform tr = btnCon.transform.GetChild(i);
                sq.Join(
                tr.GetComponent<RectTransform>()
                    .DOAnchorPosX(0, 0.5f)
                    .SetEase(ac)
                    .SetDelay(0.05f * i));
            }
            sq.onComplete = (() => {
                BlockBtn(true);
            });
            sq.Play();
            yield return sq.WaitForCompletion();
        }

        private IEnumerator ShowOther()
        {
            AnimationCurve ac = AniCurveManager.GetInstance().Curve84;
            Sequence sq = DOTween.Sequence();
            sq.Pause();
            sq.Append(otherCon.GetComponent<CanvasGroup>()
                    .DOFade(1, 0.5f)
                    .OnStart(() =>
                    {
                        otherCon.GetComponent<CanvasGroup>().alpha = 0;
                        otherCon.SetActive(true);
                    }));
            sq.Play();
            yield return sq.WaitForCompletion();
        }

        //粒子效果层动画
        private IEnumerator ShowParticle()
        {
            //particleCon.SetActive(true);
            float x = 0;
            while (x < 1)
            {
                x = Mathf.MoveTowards(x, 1, 1 / 1.0f * Time.deltaTime);
                //particleCon.GetComponent<UIRect>().alpha = x;
                yield return null;
            }
        }

        private void BlockBtn(bool blocked)
        {
            //Debug.Log("block title button" + blocked);
            for (int i = 0; i < btnCon.transform.childCount; i++)
            {
                btnCon.transform.GetChild(i).GetComponent<BasicButton>().enabled = blocked;
            }
        }

        /// <summary>
        /// 初始化文字标签，透明
        /// </summary>
        private void InitLabel()
        {
            titleLabel.GetComponent<Text>().color = new Color(1,1,1,0);
            subtitleLabel.GetComponent<Text>().color = new Color(1, 1, 1, 0);
        }
    }
}
