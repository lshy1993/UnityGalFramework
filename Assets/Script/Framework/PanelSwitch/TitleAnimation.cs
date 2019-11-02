using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.Framework.UI
{
    public class TitleAnimation : PanelAnimation
    {
        /// <summary>
        /// 中间主按钮
        /// </summary>
        private GameObject btnTable;

        /// <summary>
        /// 底部外链按钮
        /// </summary>
        //private GameObject subTabel;

        /// <summary>
        /// 侧边标签容器
        /// </summary>
        //private GameObject sideLabelCon;

        /// <summary>
        /// 粒子特效容器
        /// </summary>
        //private GameObject particleCon;

        private GameObject backSprite, videoSprite;
        private GameObject titleLabel, titleEngLabel, sideLabel;

        public override void Init()
        {
            //btnTable = this.transform.Find("Title_Container/MainButton_Table").gameObject;
            btnTable = this.transform.Find("Title_Container").gameObject;
            //subTabel = this.transform.Find("Title_Container/MainButton_Table").gameObject;
            //sideLabelCon = this.transform.Find("Title_Container/SideLabel_Container").gameObject;
            //particleCon = this.transform.Find("Title_Container/Particle_Container").gameObject;

            backSprite = this.transform.Find("Back_Sprite").gameObject;
            videoSprite = this.transform.Find("Video_Sprite").gameObject;

            //titleLabel = this.transform.Find("Title_Container/TitleText_Label").gameObject;
            //titleEngLabel = this.transform.Find("Title_Container/TitleTextEng_Label").gameObject;
            //sideLabel = this.transform.Find("Title_Container/SideTextEng_Label").gameObject;

            base.Init();
        }

        public override void BeforeClose()
        {
            //base.BeforeClose();
            BlockBtn(false);
        }

        /// <summary>
        /// 自定义退出界面动效
        /// </summary>
        public override IEnumerator CloseSequence(UIAnimationCallback callback)
        {
            //yield return new WaitForSeconds(1f);
            //BlockBtn(false);
            //panel.alpha = 1;
            //float x = 1;
            //while (x > 0)
            //{
            //    x = Mathf.MoveTowards(x, 0, 1 / closeTime * Time.deltaTime);
            //    panel.alpha = x;
            //    Debug.Log(x);
            //    yield return null;
            //}
            //callback();

            return base.CloseSequence(() => {
                //particleCon.SetActive(false);
                callback();
            });
        }

        /// <summary>
        /// 自定义进入界面动效
        /// </summary>
        public override IEnumerator OpenSequence(UIAnimationCallback callback)
        {            
            InitBtn();
            BlockBtn(false);
            InitLabel();
            
            return base.OpenSequence(() =>
            {
                StartCoroutine(ShowText());
                callback();
            });
        }

        //文字显示动画
        private IEnumerator ShowText()
        {
            //sideLabelCon.SetActive(true);
            float x = 0;
            while (x < 1)
            {
                x = Mathf.MoveTowards(x, 1, 1 / 0.2f * Time.deltaTime);
                //titleLabel.GetComponent<UIRect>().alpha = x;
                yield return null;
            }
            StartCoroutine(ShowParticle());
            x = 0;
            while (x < 1)
            {
                x = Mathf.MoveTowards(x, 1, 1 / 0.2f * Time.deltaTime);
                //titleEngLabel.GetComponent<UIRect>().alpha = x;
                //sideLabel.GetComponent<UIRect>().alpha = x;
                yield return null;
            }
            StartCoroutine(ShowBtn());
        }

        //按钮显示动画
        private IEnumerator ShowBtn()
        {
            btnTable.GetComponent<CanvasGroup>().alpha = 0;
            btnTable.SetActive(true);
            float x = 0;
            while (x < 1)
            {
                x = Mathf.MoveTowards(x, 1, 1 / 0.3f * Time.fixedDeltaTime);
                btnTable.GetComponent<CanvasGroup>().alpha = x;
                yield return null;
            }
            //TODO: 按钮飞入

            //for (int i = 0; i < btnTable.transform.childCount; i++)
            //{
            //    //btnTable.transform.GetChild(i).GetComponent<UIRect>().alpha = 0;
            //}
            //for (int i = 0; i < btnTable.transform.childCount; i++)
            //{
                
            //}

            BlockBtn(true);
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
            //for (int i = 0; i < btnTable.transform.childCount; i++)
            //{
            //    btnTable.transform.GetChild(i).GetComponent<UIButton>().enabled = blocked;
            //}
        }

        /// <summary>
        /// 初始化按钮状态，透明且关闭
        /// </summary>
        private void InitBtn()
        {
            //btnTable.SetActive(false);
            //for (int i = 0; i < btnTable.transform.childCount; i++)
            //{
            //    btnTable.transform.GetChild(i).GetComponent<UIRect>().alpha = 0;
            //}
        }

        /// <summary>
        /// 初始化文字标签，透明
        /// </summary>
        private void InitLabel()
        {
            //titleLabel.GetComponent<UIRect>().alpha = 0;
            //titleEngLabel.GetComponent<UIRect>().alpha = 0;
            //sideLabel.GetComponent<UIRect>().alpha = 0;
            //sideLabelCon.SetActive(false);
        }
    }
}
