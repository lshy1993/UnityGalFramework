using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    public class MessageAnimation : PanelAnimation
    {
        //TODO：读档的预处理？
        public GameObject selectCon;

        public override void BeforeClose()
        {
            transform.Find("Click_Container").gameObject.SetActive(false);
        }

        public override IEnumerator OpenSequence(UIAnimationCallback callback)
        {
            Debug.Log("Message Animation Init");
            selectCon.SetActive(false);

            return base.OpenSequence(() =>
            {
                if (DataManager.GetInstance().tempData.isDiaboxRecover)
                {
                    GetComponent<MessageUIManager>().ShowWindow(
                        new Action(() => { callback(); })
                    );
                    DataManager.GetInstance().tempData.isDiaboxRecover = false;
                }
                else
                {
                    callback();
                }
            });
        }

        public override IEnumerator CloseSequence(UIAnimationCallback callback)
        {
            Debug.Log("Message Animation Before Out");
            selectCon.SetActive(false);
            GetComponent<MessageUIManager>().ClearText();

            return base.CloseSequence(() =>
            {
                callback();
            });
        }

    }
}
