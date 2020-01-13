using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using DG.Tweening;

namespace Assets.Script.Framework.Effect
{
    public class SpriteTweener : MonoBehaviour
    {
        public Tween maintw;
        [SerializeField]
        private Vector3 origin;
        [SerializeField]
        private Vector3 final;
        [SerializeField]
        private float curT, allT;

        private void Update()
        {
            if(curT < allT)
            {
                curT += Time.deltaTime;
                float t = curT / allT;
                transform.localPosition = Vector3.Lerp(origin, final, t);
            }
        }

        public void StopTween()
        {
            maintw.Kill();
        }

        public void Move(NewImageEffect effect)
        {
            origin = transform.localPosition;
            final = effect.state.GetPosition();
            if (!string.IsNullOrEmpty(effect.defaultpos))
            {
                final = SetDefaultPos(gameObject, effect.defaultpos);
            }
            curT = 0;
            allT = effect.time;
        }


        private Vector3 SetDefaultPos(GameObject ui, string pstr)
        {
            float x;
            switch (pstr)
            {
                case "left":
                    x = -320;
                    break;
                case "middle":
                    x = 0;
                    break;
                case "right":
                    x = 320;
                    break;
                default:
                    x = 0;
                    break;
            }
            float y = -540 + (ui.GetComponent<RectTransform>().rect.height / 2);
            return new Vector3(x, y);
        }

    }
}
