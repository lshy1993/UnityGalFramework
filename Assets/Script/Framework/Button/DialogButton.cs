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
    public class DialogButton : BasicButton
    {
        public Animator at;

        protected override void Start()
        {
            at.Play("Sprite", 0, 0f);
            at.Update(0f);
        }

        protected override void Hover(bool ishover)
        {
            if (ishover)
            {
                //at.gameObject.SetActive(ishover);
                at.Play(0);
            }
            else
            {
                at.Play("Sprite", 0, 0f);
                at.Update(0f);
            }
        }
    }
}
