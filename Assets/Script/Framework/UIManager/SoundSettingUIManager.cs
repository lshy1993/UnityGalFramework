using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    /// <summary>
    /// 用于控制游戏声音的设置
    /// </summary>
    public class SoundSettingUIManager : MonoBehaviour
    {
        //public GameObject charaGrid;
        //public GameObject onBtn, offBtn, testBtn;

        /// <summary>
        /// 滚动条
        /// </summary>
        public Slider bgmSlider, seSlider, voiceSlider, sysSeSlider;
        //public GameObject volumeSlider;

        public GameObject bgmMuteBtn, seMuteBtn, voiceMuteBtn;

        private bool bgmMuted, seMuted, voiceMuted;
        private float bgmValue, seValue, voiceValue;

        //private int defaultNum;

        private void OnEnable()
        {
            //初始化
            bgmSlider.value = DataManager.GetInstance().configData.userBGMVolume;
            seSlider.value = DataManager.GetInstance().configData.userSEVolume;
            sysSeSlider.value = DataManager.GetInstance().configData.userSysSEVolume;
            voiceSlider.value = DataManager.GetInstance().configData.userVoiceVolume;

            //defaultNum = DataManager.GetInstance().configData.defaultCharaNum;
            //读取相应的数据 并挂到组件上
            //SetCharaButton(defaultNum);
        }

        private void SetRadioPressed(GameObject target)
        {
            target.GetComponent<BasicButton>().enabled = false;
            target.GetComponent<Image>().sprite = null;//target.GetComponent<Button>().hoverSprite2D;
        }
        private void SetRadioAvailable(GameObject target)
        {
            target.GetComponent<BasicButton>().enabled = true;
            target.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/fun_back");
        }

        /// <summary>
        /// 将目标设置为 已按下
        /// </summary>
        /// <param name="target"></param>
        private void SetTogglePressed(GameObject target)
        {
            target.transform.Find("Hover").gameObject.SetActive(true);
            //target.GetComponent<BasicButton>().enabled = false;
            //target.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/mute_o");
        }

        /// <summary>
        /// 将目标设置为 可以按下
        /// </summary>
        /// <param name="target"></param>
        private void SetToggleAvailable(GameObject target)
        {
            target.transform.Find("Hover").gameObject.SetActive(false);
            //target.GetComponent<BasicButton>().enabled = true;
            //target.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/mute");
        }

        private void SetCharaButton(int x)
        {
            //SetRadioPressed(charaGrid.transform.GetChild(x).gameObject);
            ////对应组件
            //float volume = DataManager.GetInstance().configData.charaVoiceVolume[x];
            //bool flag = DataManager.GetInstance().configData.charaVoice[x];
            //SetToggleAvailable(flag ? offBtn : onBtn);
            //SetTogglePressed(flag ? onBtn : offBtn);
            //volumeSlider.SetActive(flag);
            //volumeSlider.GetComponent<Slider>().value = volume;
            //testBtn.SetActive(flag);

            //volumeSlider.GetComponent<UISlider>().enabled = flag;
            //volumeSlider.GetComponent<UIButton>().enabled = flag;
            //
            //testBtn.GetComponent<UIButton>().enabled = flag;
        }

        public void SwitchChara(string str)
        {
            ////切换角色：更改设置的索引？
            //SetRadioAvailable(charaGrid.transform.GetChild(defaultNum).gameObject);
            //switch (str)
            //{
            //    case "Li_Button":
            //        defaultNum = 0;
            //        break;
            //    case "Su_Button":
            //        defaultNum = 1;
            //        break;
            //    case "Miao_Button":
            //        defaultNum = 2;
            //        break;
            //    case "Xi_Button":
            //        defaultNum = 3;
            //        break;
            //}
            //SetCharaButton(defaultNum);
        }

        public void MicTest()
        {
            //播放测试语音
            //Debug.Log(defaultNum);
        }

        public void SwitchVoice()
        {
            //是否开启语音
            //bool[] flag = DataManager.GetInstance().configData.charaVoice;
            //flag[defaultNum] = !flag[defaultNum];
            //DataManager.GetInstance().configData.charaVoice = flag;
            //SetCharaButton(defaultNum);
        }

        public void SetVolume(float volume)
        {
            //DataManager.GetInstance().configData.charaVoiceVolume[defaultNum] = volume;
        }

        public void MuteBGM()
        {
            bgmMuted = !bgmMuted;
            if (bgmMuted)
            {
                bgmValue = bgmSlider.value;
                bgmSlider.value = 0;
                SetToggleAvailable(bgmMuteBtn);
            }
            else
            {
                bgmSlider.value = bgmValue;
                SetTogglePressed(bgmMuteBtn);
            }
        }

        public void MuteSE()
        {
            seMuted = !seMuted;
            if (seMuted)
            {
                seValue = seSlider.value;
                seSlider.value = 0;
                SetTogglePressed(seMuteBtn);
                
            }
            else
            {
                seSlider.value = seValue;
                SetToggleAvailable(seMuteBtn);
            }
        }

        public void MuteVoice()
        {
            voiceMuted = !voiceMuted;
            if (bgmMuted)
            {
                voiceValue = voiceSlider.value;
                voiceSlider.value = 0;
                SetTogglePressed(voiceMuteBtn);
            }
            else
            {
                voiceSlider.value = voiceValue;
                SetToggleAvailable(voiceMuteBtn);
            }
        }
    }
}