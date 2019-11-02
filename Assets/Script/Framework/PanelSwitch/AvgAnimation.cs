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

        //public override IEnumerator OpenSequence(UIAnimationCallback callback)
        //{
        //    return base.OpenSequence(() =>
        //    {
        //        transform.Find("Background_Panel").gameObject.SetActive(true);
        //        transform.Find("CharaGraph_Panel").gameObject.SetActive(true);
        //        if (DataManager.GetInstance().tempData.isDiaboxRecover)
        //        {
        //            transform.Find("Message_Panel").GetComponent<DialogBoxUIManager>().ShowWindow();
        //            DataManager.GetInstance().tempData.isDiaboxRecover = false;
        //        }
        //        callback();
        //    });
        //}

        public override IEnumerator CloseSequence(UIAnimationCallback callback)
        {
            return base.CloseSequence(() =>
            {
                //清除背景与前景
                transform.Find("Background_Panel/BackGround_Sprite").gameObject.GetComponent<Image>().sprite = null;
                for (int i = 0; i < transform.Find("CharaGraph_Panel").childCount; i++)
                {
                    Destroy(transform.Find("CharaGraph_Panel").GetChild(i));
                }
                //关闭其他panel
                transform.Find("Selection_Panel").gameObject.SetActive(false);

                callback();
            });
        }

    }
}
