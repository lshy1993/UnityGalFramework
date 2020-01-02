using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Framework.UI
{
    public class PanelAnimation : MonoBehaviour
    {
        public float maxAlpha = 1, minAlpha = 0;
        public float closeTime = 0.5f, openTime = 0.5f;
        protected CanvasGroup panel;

        public virtual void Init()
        {
            Debug.Log(transform.name + " Animation Init");
            panel = transform.GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// Panel关闭前的处理
        /// </summary>
        public virtual void BeforeClose() {}

        /// <summary>
        /// Panel关闭动画
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public virtual IEnumerator CloseSequence(UIAnimationCallback callback)
        {
            //Debug.Log(Time.time + " Close Panel:" + panel.name);
            panel.alpha = maxAlpha;
            float fadeSpeed = Math.Abs(maxAlpha - minAlpha) / closeTime;
            while (panel.alpha > minAlpha)
            {
                panel.alpha = Mathf.MoveTowards(panel.alpha, minAlpha, fadeSpeed * Time.deltaTime);
                //Debug.Log(Time.time + " " + panel.alpha);
                yield return null;
            }

            callback();

        }

        /// <summary>
        /// Panel开启动画
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <returns></returns>
        public virtual IEnumerator OpenSequence(UIAnimationCallback callback)
        {
            //Debug.Log(Time.time + " Open Panel:" + panel.name);
            panel.alpha = minAlpha;
            float fadeSpeed = Math.Abs(maxAlpha - minAlpha) / openTime;
            while (panel.alpha < maxAlpha)
            {
                panel.alpha = Mathf.MoveTowards(panel.alpha, maxAlpha, fadeSpeed * Time.deltaTime);
                //Debug.Log(Time.time + " " + panel.alpha);
                yield return null;
            }
            
            callback();
        }
    }
}
