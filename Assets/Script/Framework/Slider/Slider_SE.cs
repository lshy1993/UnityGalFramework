using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    public class Slider_SE : MonoBehaviour
    {

        //AudioSource bgm
        public SoundManager sm;
        public Slider slider;
        public Text numlabel, helplabel;

        public void OnValueChange()
        {
            sm.userSeVolume = slider.value;
            numlabel.text = slider.value.ToString("00%");
        }

        void OnHover(bool ishover)
        {
            helplabel.text = ishover ? "设置效果音的大小" : string.Empty;
        }

    }
}