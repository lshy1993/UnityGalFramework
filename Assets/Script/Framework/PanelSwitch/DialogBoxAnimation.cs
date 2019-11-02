using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Framework.UI
{
    public class DialogBoxAnimation : PanelAnimation
    {
        public override void BeforeClose()
        {
            transform.Find("Click_Container").gameObject.SetActive(false);
        }

        public override IEnumerator OpenSequence(UIAnimationCallback callback)
        {
            return base.OpenSequence(() =>
            {
                //transform.Find("Main_Container").gameObject.SetActive(true);
                if (DataManager.GetInstance().tempData.isDiaboxRecover)
                {
                    GetComponent<DialogBoxUIManager>().ShowWindow();
                    DataManager.GetInstance().tempData.isDiaboxRecover = false;
                }
                //transform.Find("Click_Container").gameObject.SetActive(true);
                callback();
            });
        }

        public override IEnumerator CloseSequence(UIAnimationCallback callback)
        {
            return base.CloseSequence(() =>
            {
                transform.Find("Main_Container").gameObject.SetActive(false);
                callback();
            });
        }
    }
}
