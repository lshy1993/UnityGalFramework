using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Script.Framework.Effect;

using UnityEngine;

using DG.Tweening;

namespace Assets.Script.Framework.UI
{
    public class SystemAnimation : PanelAnimation
    {
        /// <summary>
        /// 按钮列表
        /// </summary>
        private GameObject btnTable;


        /// <summary>
        /// 主面板
        /// </summary>
        private GameObject menuCon;


        public override void Init()
        {
            menuCon = transform.Find("Menu_Panel").gameObject;
            //btnCon = transform.Find("MainButton_Table").gameObject;
            btnTable = menuCon.transform.Find("Button_Panel").gameObject;
            base.Init();
        }

        public override void BeforeClose()
        {
            BlockBtn(false);
        }

        public override IEnumerator CloseSequence(UIAnimationCallback callback)
        {
            // 飞出按钮
            AnimationCurve ac = AniCurveManager.GetInstance().Curve84;

            btnTable.GetComponent<CanvasGroup>()
                .DOFade(0, 0.5f)
                .OnStart(() => { btnTable.SetActive(true); });
            for (int i = 0; i < btnTable.transform.childCount; i++)
            {
                Transform tr = btnTable.transform.GetChild(i);
                tr.GetComponent<RectTransform>()
                    .DOAnchorPosX(1710 + 280, 0.5f)
                    .SetEase(ac)
                    .SetDelay(0.05f * i);
            }

            yield return new WaitForSeconds(0.2f);

            yield return base.CloseSequence(() =>
            {
                //particleCon.SetActive(false);
                callback();
            });
        }

        /// <summary>
        /// 自定义进入界面动效
        /// </summary>
        public override IEnumerator OpenSequence(UIAnimationCallback callback)
        {
            Debug.Log("Animation Open Sequence");
            InitBtn();
            BlockBtn(false);
            //InitLabel();

            return base.OpenSequence(() =>
            {
                //StartCoroutine(ShowText());
                Debug.Log("Animation Opened");
                callback();
            });
        }

        private void BlockBtn(bool blocked)
        {
            for (int i = 0; i < btnTable.transform.childCount; i++)
            {
                btnTable.transform.GetChild(i).GetComponent<TitleButton>().enabled = blocked;
            }
        }

        /// <summary>
        /// 初始化按钮位置
        /// </summary>
        private void InitBtn()
        {
            for (int i = 0; i < btnTable.transform.childCount; i++)
            {
                RectTransform rt = btnTable.transform.GetChild(i)
                    .GetComponent<RectTransform>();
                Vector3 vet = rt.anchoredPosition;
                rt.anchoredPosition = new Vector3(1710 + 220, vet.y, vet.z);
            }
        }

        private IEnumerator Open()
        {
            yield return StartCoroutine(Fadein(0.2f));
        }

        private IEnumerator Fadein(float time)
        {
            CanvasGroup panel = transform.GetComponent<CanvasGroup>();
            float f = time == 0 ? 1 : 0;
            panel.alpha = f;
            while (f < 1f)
            {
                f = Mathf.MoveTowards(f, 1f, Time.deltaTime / time);
                panel.alpha = f;
                yield return null;
            }
        }

        private IEnumerator Fadeout(float time, GameObject target)
        {
            CanvasGroup panel = target.GetComponent<CanvasGroup>();
            float f = time == 0 ? 0 : 1;
            panel.alpha = f;
            while (f > 0)
            {
                f = Mathf.MoveTowards(f, 0, Time.deltaTime / time);
                panel.alpha = f;
                yield return null;
            }
            target.SetActive(false);
        }
    }
}
