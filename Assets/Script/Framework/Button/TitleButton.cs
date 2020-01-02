using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Assets.Script.Framework.UI
{
    public class TitleButton : BasicButton
    {
        public Animator at;
        public Image underline;
        private Sequence tw;

        protected override void Hover(bool ishover)
        {
            if (tw != null)
            {
                tw.Kill();
            }
            //Debug.Log("hover:" + ishover);
            if (ishover)
            {
                at.gameObject.SetActive(ishover);
                at.Play(0);
                
                tw = DOTween.Sequence();
                tw.Pause();
                tw.Append(at.gameObject.GetComponent<SpriteRenderer>().DOFade(1, 0))
                .Join(underline.GetComponent<Image>().DOFade(1, 0))
                .Join(underline.GetComponent<RectTransform>().DOScaleX(1, 0.33f));
                tw.Play();
            }
            else
            {
                tw = DOTween.Sequence();
                tw.Pause();
                tw.Append(at.gameObject.GetComponent<SpriteRenderer>().DOFade(0, 0.33f))
                .Join(underline.GetComponent<Image>()
                        .DOFade(0, 0.33f)
                        .OnComplete(() =>
                        {
                            underline.GetComponent<RectTransform>().localScale = new Vector3(0, 1, 1);
                        }));
                tw.Play();
            }
        }
    }
}
