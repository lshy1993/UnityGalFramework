﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Script.Framework.Effect;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    ///用于控制对话框的文字设置
    public class TextSettingUIManager : MonoBehaviour
    {
        /// <summary>
        /// 透明度滑动条
        /// </summary>
        public Slider diaBoxAlpha;

        /// <summary>
        /// 文字显示速度
        /// </summary>
        public Slider textSpeed;

        /// <summary>
        /// 自动等待速度
        /// </summary>
        public Slider autoSpeed;

        /// <summary>
        /// 预览文字
        /// </summary>
        public Text previewLabel;
        /// <summary>
        /// 预览框
        /// </summary>
        public Image previewBack;

        /// <summary>
        /// 对话文字
        /// </summary>
        public Text textLabel;
        /// <summary>
        /// 对话框
        /// </summary>
        public Image diaBack;

        private bool previewFlag = false;

        [SerializeField]
        private bool aniFlag = false;
        private float currentTime = 0f;

        private int alpha
        {
            get { return DataManager.GetInstance().configData.diaboxAlpha; }
        }
        private float tsp
        {
            get { return DataManager.GetInstance().configData.textSpeed; }
        }
        private float waitTime
        {
            get { return DataManager.GetInstance().configData.waitTime; }
        }

        private void Awake()
        {
            ResetTextSpeed();
            ResetAutoSpeed();
            ResetAlpha();
        }

        private void Update()
        {
            if (aniFlag)
            {
                //等待计时器
                if (currentTime < waitTime)
                {
                    currentTime += Time.deltaTime;
                    //Debug.Log(currentTime);
                }
                else
                {
                    //计时器关闭且重置
                    currentTime = 0f;
                    aniFlag = false;
                    previewFlag = !previewFlag;
                    ResetTypewriter();
                }
            }

        }

        //打开此界面时的初始化
        private void OnEnable()
        {
            aniFlag = false;
            previewFlag = false;
            SetTextSpeed();
            SetAutoSpeed();
            SetAlpha();
        }

        private void ResetTypewriter()
        {
            string ss = previewFlag ? "请调节合适的文字显示速度\r\n请调节合适的自动等待时间" : "Unity Galgame Framework by Lshy1993\r\nXianZhuo Soft ©COPYRIGHT";
            //previewLabel.text = ss;
            previewLabel.GetComponent<TypeWriter>().ResetToBeginning(ss);
        }

        private void ResetAlpha()
        {
            diaBoxAlpha.value = (float)alpha / 100;
        }

        private void ResetTextSpeed()
        {
            textSpeed.value = (tsp - 20) / 50;
        }

        private void ResetAutoSpeed()
        {
            autoSpeed.value = (5 - waitTime) / 5;
        }

        //##################以下为public方法#####################

        //设置【对话框】透明度
        public void SetAlpha()
        {
            previewBack.color = new Color(1, 1, 1, alpha / 100f);
            diaBack.color = new Color(1, 1, 1, alpha / 100f);
        }

        //设置【预览框/对话框】的文字显示速度
        public void SetTextSpeed()
        {
            Debug.Log("text speed changed:" + tsp);
            previewLabel.GetComponent<TypeWriter>().charsPerSecond = tsp;
            textLabel.GetComponent<TypeWriter>().charsPerSecond = tsp;
            ResetTypewriter();
        }

        //设置【预览框】切换速度
        public void SetAutoSpeed()
        {
            ResetTypewriter();
        }

        //打字机完成后调用重置定时器
        public void SwitchText()
        {
            aniFlag = true;
        }

    }
}
