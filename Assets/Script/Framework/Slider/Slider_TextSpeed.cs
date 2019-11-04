using System.Collections;
using System;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    public class Slider_TextSpeed : MonoBehaviour
    {
        public Slider slider;
        public Text numlabel, helplabel;
        public TextSettingUIManager uiManager;

        private bool clicked = false;

        public void OnValueChange()
        {
            float textSpeed = 5 + Convert.ToInt32(slider.value * 75);
            Debug.Log("TextSpeed : " + textSpeed);
            numlabel.text = (slider.value * 100).ToString("0");
            DataManager.GetInstance().configData.textSpeed = textSpeed;
            clicked = true;
        }

        private void Update()
        {
            if (clicked && Input.GetMouseButtonUp(0))
            {
                uiManager.SetTextSpeed();
                clicked = false;
            }
        }

        void OnHover(bool ishover)
        {
            helplabel.text = ishover ? "设置文本的显示速度" : string.Empty;
        }

    }
}