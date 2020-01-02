using System.Collections;
using System.Collections.Generic;

using Assets.Script.Framework.Data;

using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using Assets.Script.Framework.Effect;

namespace Assets.Script.Framework.UI
{
    public class MenuUIManager : MonoBehaviour
    {
        public GameObject btnTable;
        public GameObject saveBtn, loadBtn;
        private Sequence tw;

        private void OnEnable()
        {
            // 视情况禁止玩家存读档
            bool slBlock = DataManager.GetInstance().IsSaveLoadBlocked();
            saveBtn.GetComponent<BasicButton>().enabled = !slBlock;
            loadBtn.GetComponent<BasicButton>().enabled = !slBlock;
            BlockBtn(false);
            InitBtn();
            Open();

        }

        public void Open()
        {
            // 飞入按钮
            AnimationCurve ac = AniCurveManager.GetInstance().Curve84;
            Debug.Log("menu open");
            if (tw != null)
            {
                tw.Kill();
            }
            tw = DOTween.Sequence();
            tw.Pause();
            tw.Append(gameObject.GetComponent<CanvasGroup>()
                .DOFade(1, 0.5f)
                .OnStart(() => { gameObject.SetActive(true); })
                .OnComplete(() => { BlockBtn(true); })
            );

            for (int i = 0; i < btnTable.transform.childCount; i++)
            {
                Transform tr = btnTable.transform.GetChild(i);
                tw.Join(tr.GetComponent<RectTransform>()
                    .DOAnchorPosX(1710, 0.5f)
                    .SetEase(ac)
                    .SetDelay(0.05f * i)
                );
            }
            tw.Play();
        }

        public void Close()
        {
            // 飞出按钮
            AnimationCurve ac = AniCurveManager.GetInstance().Curve84;
            if (tw != null)
            {
                tw.Kill();
            }
            tw = DOTween.Sequence();
            tw.Pause();
            tw.Append(
            btnTable.GetComponent<CanvasGroup>()
                .DOFade(0, 0.5f)
                .OnStart(() => { btnTable.SetActive(true); })
            );
            for (int i = 0; i < btnTable.transform.childCount; i++)
            {
                Transform tr = btnTable.transform.GetChild(i);
                tw.Join(
                tr.GetComponent<RectTransform>()
                    .DOAnchorPosX(1710 + 280, 0.5f)
                    .SetEase(ac)
                    .SetDelay(0.05f * i));
            }
            tw.Play();
        }

        /// <summary>
        /// 禁启用按钮
        /// </summary>
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
    }
}