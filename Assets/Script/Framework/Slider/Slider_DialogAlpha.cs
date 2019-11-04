using System;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    public class Slider_DialogAlpha : MonoBehaviour
    {
        public Slider slider;
        public Text numlabel, helplabel;
        public TextSettingUIManager uiManager;

        public void OnValueChange()
        {
            int alpha = Convert.ToInt32(slider.value * 100);
            numlabel.text = alpha.ToString("0");
            DataManager.GetInstance().configData.diaboxAlpha = alpha;
            uiManager.SetAlpha();
        }

        void OnHover(bool ishover)
        {
            helplabel.text = ishover ? "设置对话框的透明度" : string.Empty;
        }

    }
}