using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    public class AvgAnimation : PanelAnimation
    {
        //TODO：读档的预处理？
        public GameObject backCon;
        public GameObject charaCon;
        public GameObject selectCon;

        public override IEnumerator OpenSequence(UIAnimationCallback callback)
        {
            Debug.Log("AVG Animation Init");
            selectCon.SetActive(false);
            backCon.SetActive(true);
            charaCon.SetActive(true);

            return base.OpenSequence(() =>
            {
                callback();
            });
        }

        public override IEnumerator CloseSequence(UIAnimationCallback callback)
        {
            return base.CloseSequence(() =>
            {
                //清除背景与前景
                foreach (Transform child in transform.Find("Background_Panel"))
                {
                    DestroyImmediate(child.gameObject);
                }
                for (int i = 0; i < transform.Find("CharaGraph_Panel").childCount; i++)
                {
                    Destroy(transform.Find("CharaGraph_Panel").GetChild(i).gameObject);
                }
                //关闭其他panel
                selectCon.SetActive(false);
                callback();
            });
        }

    }
}
