using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Framework.UI
{
    public class PanelFade2 : MonoBehaviour
    {
        internal bool updating;
        internal float fadeSpeed;
        private bool close, open;

        private CanvasGroup panel;

        public void Close(float fadeOutTime)
        {
            this.fadeSpeed = 1 / fadeOutTime;

            this.close = true;
            this.open = false;
            
        }

        public void Open(float fadeInTime)
        {
            this.fadeSpeed = 1 / fadeInTime;
            panel.alpha = 0;

            
            this.open = true;
            this.close = false;
        }

        void Awake()
        {
            updating = false;
            fadeSpeed = 1 / 0.5f;
            panel = this.GetComponentInParent<CanvasGroup>();
            //panel = this.transform.GetComponent<UIPanel>();
            Debug.Log("PanelFade2: " + panel.name);
        }

        void FixedUpdate()
        {
            if (close)
            {
                updating = true;
                panel.alpha = Mathf.MoveTowards(panel.alpha, 0, fadeSpeed * Time.fixedDeltaTime);

                if (panel.alpha < 0.0000001)
                {
                    updating = false;
                    close = false;
                    panel.transform.gameObject.SetActive(false);
                }
            }

            if (open)
            {
                updating = true;

                panel.alpha = Mathf.MoveTowards(panel.alpha, 1, fadeSpeed * Time.fixedDeltaTime);

                if (panel.alpha > 0.9999999)
                {
                    updating = false;
                    open = false;
                }
            }
        }
    }
}
