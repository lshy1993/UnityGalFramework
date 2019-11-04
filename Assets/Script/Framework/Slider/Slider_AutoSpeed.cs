using System.Collections;
using System;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    public class Slider_AutoSpeed : MonoBehaviour
    {
        public Slider slider;
        public Text numlabel, helplabel;
        public TextSettingUIManager uiManager;

        private bool clicked = false;

        public void OnValueChange()
        {
            float speed = 5 - slider.value * 5;
            //Debug.Log("AutoSpeed : " + speed);
            int i = (int)(speed * 100);
            speed = (float)(i * 1.0) / 100;
            numlabel.text = speed.ToString("0.0s");// (slider.value * 100).ToString("0.0");
            DataManager.GetInstance().configData.waitTime = speed;
            clicked = true;
        }

        private void Update()
        {
            if (clicked && Input.GetMouseButtonUp(0))
            {
                uiManager.SetAutoSpeed();
                clicked = false;
            }
        }

        void OnHover(bool ishover)
        {
            helplabel.text = ishover ? "自动模式下，文本的等待的时间" : string.Empty;
        }

    }
}