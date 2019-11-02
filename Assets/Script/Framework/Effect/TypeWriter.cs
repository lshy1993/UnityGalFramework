using System;
using System.Text;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

//using TMPro;

namespace Assets.Script.Framework.Effect
{

    //[RequireComponent(typeof(Text))]
    public class TypeWriter : MonoBehaviour
    {
        static public TypeWriter current;

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

        // 全文本
        private string mFullText = "";
        private int mCurrentOffset = 0;
        private float mNextChar = 0f;
        private bool mReset = true;
        private bool mFinished = false;

        /// <summary>
        /// 判断是否结束打字
        /// </summary>
        public bool isFinish { get { return mFinished; } }

        /// <summary>
        /// 重新开始打字效果
        /// </summary>
        public void ResetToBeginning()
        {
            mFinished = false;
            mReset = true;
            ////mNextChar = 0f;
            //mCurrentOffset = 0;
            //Update();
            mLabel = GetComponent<Text>();
            mFullText = mLabel.text;
            mLabel.text = string.Empty;
            float duration = mFullText.Length / charsPerSecond;
            DOTween.Kill(GetComponent<Text>());
            //Debug.Log(mFullText + duration);
            GetComponent<Text>()
                .DOText(mFullText, duration)
                .OnComplete(()=> { Finish(); })
                .Play();
        }

        /// <summary>
        /// 结束打字 显示全部文本
        /// </summary>
        public void Finish()
        {
            Debug.Log("finished");
            DOTween.Kill(GetComponent<Text>());
            mLabel.text = mFullText;
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

        void Update()
        {
            return;
            if (mFinished) return;
            if (mReset)
            {
                mCurrentOffset = 0;
                mReset = false;
                mLabel = GetComponent<Text>();
                mFullText = mLabel.text;
                //mFullText = mLabel.processedText;
            }
            if (string.IsNullOrEmpty(mFullText)) return;

            while (mCurrentOffset < mFullText.Length && mNextChar <= Time.time)
            {
                int lastOffset = mCurrentOffset;
                charsPerSecond = Mathf.Max(1, charsPerSecond);

                // Automatically skip all symbols
                //if (mLabel.supportEncoding)
                //    while (NGUIText.ParseSymbol(mFullText, ref mCurrentOffset)) { }

                ++mCurrentOffset;

                // Reached the end? We're done.
                if (mCurrentOffset > mFullText.Length) break;

                float delay = 1f / charsPerSecond;

                if (mNextChar == 0f)
                {
                    mNextChar = Time.time + delay;
                }
                else
                {
                    mNextChar += delay;
                }
                mLabel.text = mFullText.Substring(0, mCurrentOffset);
            }

            // Alpha-based fading
            if (mFullText.Length > 0 && mCurrentOffset >= mFullText.Length)
            {
                mLabel.text = mFullText;
                current = this;
                onFinished.Invoke();
                //EventDelegate.Execute(onFinished);
                current = null;
                mFinished = true;
            }

        }

    }
}