using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Assets.Script.Framework.Effect
{
    public class TweenPosition : MonoBehaviour
    {
        public Vector3 from;
        public Vector3 to;

        private Transform mTrans;
        private RectTransform mRect;

        public Vector3 position;
        public float duration;

        Tween tw;

        private void Awake()
        {
            mRect = GetComponent<RectTransform>();
            //tw = DOTween.To(() => mRect.anchoredPosition, x => mRect.anchoredPosition = x, from, to, duration);
            //tw = mRect.DOAnchorPos(from,to,a);
        }

        //public float amountPerDelta
        //{
        //    get
        //    {
        //        if (duration == 0f) return 1000f;

        //        if (mDuration != duration)
        //        {
        //            mDuration = duration;
        //            mAmountPerDelta = Mathf.Abs(1f / duration) * Mathf.Sign(mAmountPerDelta);
        //        }
        //        return mAmountPerDelta;
        //    }
        //}

        //private void Update()
        //{
        //    value = from * (1f - factor) + to * factor;

        //    // Ping-pong style reverses the direction
        //    if (mFactor > 1f)
        //    {
        //        mFactor = 1f - (mFactor - Mathf.Floor(mFactor));
        //        mAmountPerDelta = -mAmountPerDelta;
        //    }
        //    else if (mFactor < 0f)
        //    {
        //        mFactor = -mFactor;
        //        mFactor -= Mathf.Floor(mFactor);
        //        mAmountPerDelta = -mAmountPerDelta;
        //    }

        //}

    }
}
