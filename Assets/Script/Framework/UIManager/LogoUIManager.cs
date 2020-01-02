using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    public class LogoUIManager : MonoBehaviour
    {
        public SoundManager sm;
        public PanelSwitch ps;

        private GameObject loadingContainer, clickCon;

        /// <summary>
        /// 淡入时长
        /// </summary>
        public float fadeInTime = 0.5f;
        /// <summary>
        /// 等待时长
        /// </summary>
        public float waitOutTime = 1.5f;
        /// <summary>
        /// 淡出时长
        /// </summary>
        public float fadeOutTime = 0.5f;
        /// <summary>
        /// 等待时长
        /// </summary>
        public float waitInTime = 0.5f;

        /// <summary>
        /// 需要显示的logo
        /// </summary>
        public List<GameObject> logoPanels;
        
        /// <summary>
        /// 是否读取结束
        /// </summary>
        private bool loadfinish;

        /// <summary>
        /// 当前步数
        /// 1进入，2为停留，3淡出
        /// </summary>
        private int currentStep;

        void Awake()
        {
            loadingContainer = transform.Find("Loading_Container").gameObject;
            loadfinish = false;
        }

        void OnEnable()
        {
            // 开始按照logo顺序播放
            StartCoroutine(OpenAnimate(0));
        }

        /// <summary>
        /// 跳过显示
        /// </summary>
        public void Skip()
        {
            // 已经在淡出无法跳过
            if (currentStep % 2 == 0 || (currentStep / 2) >= logoPanels.Count()) return;
            StopAllCoroutines();
            StartCoroutine(SkipAnimate(currentStep));
        }

        private IEnumerator SkipAnimate(int step)
        {
            Debug.Log("step:" + step);
            currentStep = step + 1;
            int pn = step / 2;
            yield return StartCoroutine(FadeOutLogo(logoPanels[pn], true));
            StartCoroutine(OpenAnimate(currentStep+1));
        }

        private IEnumerator OpenAnimate(int step)
        {
            currentStep = step;
            if(step == 0)
            {
                //运行外部加载
                DataLoad();
                yield return StartCoroutine(SetLoadingText());
                StartCoroutine(OpenAnimate(1));
            }
            else if (step > logoPanels.Count * 2)
            {
                StopAllCoroutines();
                //切换至标题画面
                ps.SwitchTo_VerifyIterative("Title_Panel");
            }
            else if(step % 2 == 1)
            {
                int pn = step / 2;
                //淡入Logo
                yield return StartCoroutine(FadeInLogo(logoPanels[pn]));
                //停留
                yield return new WaitForSeconds(waitOutTime);
                StartCoroutine(OpenAnimate(step+1));
            }
            else
            {
                int pn = (step / 2) - 1;
                //淡出
                yield return StartCoroutine(FadeOutLogo(logoPanels[pn]));
                if (step == logoPanels.Count * 2)
                {
                    StopAllCoroutines();
                    //切换至标题画面
                    ps.SwitchTo_VerifyIterative("Title_Panel");
                }
                else
                {
                    //间隔
                    yield return new WaitForSeconds(waitInTime);
                    StartCoroutine(OpenAnimate(step + 1));
                }
                
            }

        }

        private IEnumerator FadeInLogo(GameObject target)
        {
            target.SetActive(true);
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / fadeInTime * Time.deltaTime);
                target.GetComponent<CanvasGroup>().alpha = t;
                yield return null;
            }
        }

        private IEnumerator FadeOutLogo(GameObject target, bool fast = false)
        {
            float t = 1;
            float tall = fast ? 0.2f : fadeOutTime;
            while (t > 0)
            {
                t = Mathf.MoveTowards(t, 0, 1 / tall * Time.deltaTime);
                target.GetComponent<CanvasGroup>().alpha = t;
                yield return null;
            }
            target.SetActive(false);
        }

        /// <summary>
        /// 外部读取大数据使用
        /// </summary>
        private void DataLoad()
        {
            loadfinish = true;
        }

        /// <summary>
        /// 显示读取中文字/动画
        /// </summary>
        private IEnumerator SetLoadingText()
        {
            loadingContainer.SetActive(true);
            Text lb = loadingContainer.transform.Find("Loading_Label").GetComponent<Text>();
            lb.text = "读取中";
            while (!loadfinish)
            {
                if (lb.text == "读取中...") lb.text = "读取中";
                lb.text += ".";
                yield return new WaitForSeconds(0.1f);
            }
            //等待0.5s
            yield return new WaitForSeconds(0.5f);
            loadingContainer.SetActive(false);
        }
    }
}