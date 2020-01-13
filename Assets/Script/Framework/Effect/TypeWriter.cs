using System;
using System.Text;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using DG.Tweening;
using System.Collections;
//using TMPro;

namespace Assets.Script.Framework.Effect
{
    [RequireComponent(typeof(Text))]
    public class TypeWriter : MonoBehaviour
    {
        //static public TypeWriter current;

        // 文字显示速度
        public float charsPerSecond = 20f;

        [Serializable]
        public class TypeFinishedEvent : UnityEvent
        {
            public TypeFinishedEvent() { }
        }

        // 打字结束后委托
        [SerializeField]
        public TypeFinishedEvent onFinished = new TypeFinishedEvent();

        private Text mLabel;

        /// <summary>
        /// 已显示的文本
        /// </summary>
        private string mLastText = "";

        /// <summary>
        /// 全文本
        /// </summary>
        private string mFullText = "";

        class TextColorPair
        {
            public string text;
            public Color color;
        }

        List<TextColorPair> dynamicText = new List<TextColorPair>();

        /// <summary>
        /// 判断是否结束打字
        /// </summary>
        public bool isFinish { get; private set; } = false;

        private Tween tw;

        private void Awake()
        {
            mLabel = GetComponent<Text>();
        }

        //void Update()
        //{
        //    if (dynamicText.Count == 0 && mLabel.text == mLastText)
        //    {
        //        Finish();
        //        return;
        //    }
        //    string txt = mLastText;
        //    foreach (var it in dynamicText)
        //    {
        //        txt += "<color=#" + ColorUtility.ToHtmlStringRGBA(it.color) + ">" + it.text + "</color>";
        //    }
        //    mLabel.text = txt;
        //}

        public void AddText(string txt)
        {
            Color startColor = mLabel.color;
            startColor.a = 0;
            var dst = new TextColorPair { text = txt, color = startColor };
            dynamicText.Add(dst);
            if (tw != null) tw.Kill();
            tw = DOTween.ToAlpha(() => dst.color, (c) => dst.color = c, mLabel.color.a, 0.5f).OnComplete(() =>
            {
                dynamicText.Remove(dst);
                mLastText += dst.text;
            });
        }

        /// <summary>
        /// 重新开始打字效果
        /// </summary>
        public void ResetToBeginning(string fstr)
        {
            isFinish = false;
            //StartCoroutine(SetTextAnimation(fstr));
            //mLastText = mLabel.text;
            //mFullText = fstr;
            //return;

            //mFullText = mLabel.text;
            mFullText = fstr;
            mLabel.text = string.Empty;
            float duration = mFullText.Length / charsPerSecond;
            //Debug.Log(mFullText + duration);

            DOTween.Kill(mLabel);
            mLabel
                .DOText(mFullText, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => { Finish(); })
                .Play();
        }

        public void ResetTo(string wstr)
        {
            isFinish = false;

            //StartCoroutine(SetTextAnimation(wstr));
            //mLastText = mLabel.text;
            //mFullText = mLabel.text + wstr;
            //return;
            
            mFullText = mLabel.text + wstr;
            float duration = wstr.Length / charsPerSecond;
            DOTween.Kill(mLabel);
            mLabel
                .DOText(mFullText, duration)
                .SetEase(Ease.Linear)
                .OnComplete(() => { Finish(); })
                .Play();
        }

        private IEnumerator SetTextAnimation(string mFullText)
        {
            for (int i = 0; i < mFullText.Length; i++)
            {
                AddText(mFullText[i].ToString());
                float duration = 1 / charsPerSecond;
                yield return new WaitForSeconds(duration);
            }
        }

        /// <summary>
        /// 结束打字 显示全部文本
        /// </summary>
        public void Finish()
        {
            //Debug.Log("finished");
            DOTween.Kill(GetComponent<Text>());
            mLabel.text = mFullText;
            mLastText = "";
            onFinished.Invoke();
            return;

            //if (!mFinished)
            //{
            //    mFinished = true;
            //    mCurrentOffset = mFullText.Length;
            //    mLabel.text = mFullText;
            //    current = this;
            //    onFinished.Invoke();
            //    current = null;
            //}
        }

        //void OnEnable() { mReset = true; mFinished = false; }

        //void OnDisable() { Finish(); }

    }
}