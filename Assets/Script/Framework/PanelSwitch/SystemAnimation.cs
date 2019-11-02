using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.Framework.UI
{
    public class SystemAnimation : MonoBehaviour
    {
        public void OpenSysButton()
        {

        }

        private IEnumerator Open()
        {
            yield return StartCoroutine(Fadein(0.2f));
        }

        private IEnumerator Fadein(float time)
        {
            CanvasGroup panel = transform.GetComponent<CanvasGroup>();
            float f = time == 0 ? 1 : 0;
            panel.alpha = f;
            while (f < 1f)
            {
                f = Mathf.MoveTowards(f, 1f, Time.deltaTime / time);
                panel.alpha = f;
                yield return null;
            }
        }

        private IEnumerator Fadeout(float time, GameObject target)
        {
            CanvasGroup panel = target.GetComponent<CanvasGroup>();
            float f = time == 0 ? 0 : 1;
            panel.alpha = f;
            while (f > 0)
            {
                f = Mathf.MoveTowards(f, 0, Time.deltaTime / time);
                panel.alpha = f;
                yield return null;
            }
            target.SetActive(false);
        }
    }
}
