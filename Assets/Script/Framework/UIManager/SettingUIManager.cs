using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Script.Framework.Data;
using Assets.Script.Framework.Effect;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    public class SettingUIManager : MonoBehaviour
    {
        /// <summary>
        /// 用于控制 和切换旗下的4个设置模块
        /// </summary>
        public GameObject graphicCon, soundCon, textCon, previewCon;

        //public GameObject graphicBtn, soundBtn, textBtn, sysBtn;

        /// <summary>
        /// 提示文字
        /// </summary>
        //public Text hintLabel;

        /// <summary>
        /// 当前玩家所处模块
        /// </summary>
        //private Constants.Setting_Mode settingMode
        //{
        //    get { return DataManager.GetInstance().configData.settingMode; }
        //    set { DataManager.GetInstance().configData.settingMode = value; }
        //}

        //打开此界面时的初始化
        private void OnEnable()
        {
            //打开时自动切换至图像？
            //switch (settingMode)
            //{
            //    case Constants.Setting_Mode.Graphic:
            //        SwitchTab("Graphic_Button");
            //        break;
            //    case Constants.Setting_Mode.Operate:
            //        SwitchTab("Operate_Button");
            //        break;
            //    case Constants.Setting_Mode.Sound:
            //        SwitchTab("Sound_Button");
            //        break;
            //    case Constants.Setting_Mode.Text:
            //        SwitchTab("Sound_Button");
            //        break;
            //    default:
            //        SwitchTab("Graphic_Button");
            //        break;
            //}
            InitCanvas();
            Open();
        }

        private void OnDisable()
        {
            DataManager.GetInstance().SaveConfigData();
        }

        private void Open()
        {
            AnimationCurve ac = AniCurveManager.GetInstance().Curve84;
            // 淡入4个
            GameObject[] goos = new GameObject[4] { graphicCon, textCon, soundCon, previewCon };
            int[] posY = new int[4] {0,0,0,0 };
            for (int i = 0; i < 4; i++)
            {
                GameObject go = goos[i];
                int raw = i / 2;
                int col = i % 2;
                go.GetComponent<CanvasGroup>()
                    .DOFade(1, 0.5f)
                    .SetEase(ac)
                    .SetDelay(0.03f * col + 0.02f * raw);
                go.GetComponent<RectTransform>()
                    .DOAnchorPosY(posY[i], 0.5f)
                    .SetEase(ac)
                    .SetDelay(0.03f * col + 0.02f * raw);
            }
        }

        private void InitCanvas()
        {
            GameObject[] goos = new GameObject[4] { graphicCon, textCon, soundCon, previewCon };

            for (int i = 0; i < 4; i++)
            {
                GameObject go = goos[i];
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -222);
                go.GetComponent<CanvasGroup>().alpha = 0;
            }
        }

        /// <summary>
        /// 切换标签页
        /// </summary>
        /// <param name="target">点击的目标</param>
        //public void SwitchTab(string target)
        //{
        //    //原状态按钮开启
        //    switch (settingMode)
        //    {
        //        case Constants.Setting_Mode.Graphic:
        //            graphicBtn.GetComponent<UIButton>().enabled = true;
        //            graphicBtn.GetComponent<UIButton>().normalSprite = "UI/fun_back";
        //            graphicCon.SetActive(false);
        //            break;
        //        case Constants.Setting_Mode.Sound:
        //            soundBtn.GetComponent<UIButton>().enabled = true;
        //            soundBtn.GetComponent<UIButton>().normalSprite = "UI/fun_back";
        //            soundCon.SetActive(false);
        //            break;
        //        case Constants.Setting_Mode.Text:
        //            textBtn.GetComponent<UIButton>().enabled = true;
        //            textBtn.GetComponent<UIButton>().normalSprite = "UI/fun_back";
        //            textCon.SetActive(false);
        //            break;
        //        case Constants.Setting_Mode.Operate:
        //            sysBtn.GetComponent<UIButton>().enabled = true;
        //            sysBtn.GetComponent<UIButton>().normalSprite = "UI/fun_back";
        //            sysCon.SetActive(false);
        //            break;
        //    }
        //    if (target == "Graphic_Button") settingMode = Constants.Setting_Mode.Graphic;
        //    if (target == "Sound_Button") settingMode = Constants.Setting_Mode.Sound;
        //    if (target == "Text_Button") settingMode = Constants.Setting_Mode.Text;
        //    if (target == "Operate_Button") settingMode = Constants.Setting_Mode.Operate;
        //    //新状态按钮不可用
        //    switch (settingMode)
        //    {
        //        case Constants.Setting_Mode.Graphic:
        //            graphicBtn.GetComponent<UIButton>().enabled = false;
        //            graphicBtn.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>("UI/fun_hover1");
        //            graphicCon.SetActive(true);
        //            break;
        //        case Constants.Setting_Mode.Sound:
        //            soundBtn.GetComponent<UIButton>().enabled = false;
        //            soundBtn.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>("UI/fun_hover2");
        //            soundCon.SetActive(true);
        //            break;
        //        case Constants.Setting_Mode.Text:
        //            textBtn.GetComponent<UIButton>().enabled = false;
        //            textBtn.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>("UI/fun_hover3");
        //            textCon.SetActive(true);
        //            break;
        //        case Constants.Setting_Mode.Operate:
        //            sysBtn.GetComponent<UIButton>().enabled = false;
        //            sysBtn.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>("UI/fun_hover4");
        //            sysCon.SetActive(true);
        //            break;
        //    }
        //}

        /// <summary>
        /// 设置提示文字
        /// </summary>
        /// <param name="target"></param>
        //public void SetHint(string target)
        //{
        //    switch (target)
        //    {
        //        case "Graphic_Button":
        //            hintLabel.text = "游戏图像性能设置";
        //            break;
        //        case "Sound_Button":
        //            hintLabel.text = "游戏声音相关设置";
        //            break;
        //        case "Text_Button":
        //            hintLabel.text = "游戏文本相关设置";
        //            break;
        //        case "Operate_Button":
        //            hintLabel.text = "暂未开放";
        //            break;
        //        default:
        //            hintLabel.text = string.Empty;
        //            break;
        //    }
        //}

    }
}