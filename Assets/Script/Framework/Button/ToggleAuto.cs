using System.Collections;

using Assets.Script.Framework;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    public class ToggleAuto : MonoBehaviour
    {
        public SoundManager sm;
        public MessageUIManager uiManager;
        public Click_Next cn;
        //public UIProgressBar autoBar;

        /// <summary>
        /// 是否开启了auto模式
        /// </summary>
        private bool isAuto
        {
            get { return DataManager.GetInstance().isAuto; }
            set { DataManager.GetInstance().isAuto = value; }
        }

        /// <summary>
        /// 是否正在进行特效
        /// </summary>
        private bool isEffecting
        {
            get { return DataManager.GetInstance().isEffecting; }
        }

        private bool isCounting = false;
        private float currentTime = 0f;
        private float maxTime = 0f;

        private void Update()
        {
            // 检测到auto关闭 直接调用
            if (!isAuto)
            {
                CancelAuto();
                return;
            }
            // 倒计时中
            if (isCounting)
            {
                CountDown();
            }
            else
            {
                InitTimer();
            }
        }

        // 初始化倒计时
        private void InitTimer()
        {
            if (isEffecting) return;
            // 检测当前文字是否打印结束
            if (!uiManager.IsTyping())
            {
                // 用户设置等待时间
                float waitTime = DataManager.GetInstance().configData.waitTime;
                // 等待时间存在语音情况下取大值
                maxTime = Mathf.Max(sm.playerVoice.GetAllTime(), waitTime);
                isCounting = true;
            }
        }

        //计时器
        private void CountDown()
        {
            // 等待计时器数完
            if (currentTime < maxTime)
            {
                currentTime += Time.deltaTime;
                //autoBar.gameObject.SetActive(true);
                //autoBar.value = currentTime / maxTime;
            }
            else
            {
                // 如果还在播放语音则等待语音完成跳过
                if (sm.IsVoicePlaying()) return;
                // 计时器关闭且重置
                ResetTimer();
                // 调用Click
                Debug.Log("计时器调用Click");
                cn.Execute();
            }
        }

        // 重置
        public void ResetTimer()
        {
            //autoBar.gameObject.SetActive(false);
            isCounting = false;
            currentTime = 0f;
        }

        public void OnClick()
        {
            //切换模式
            isAuto = !isAuto;
        }

        public void CancelAuto()
        {
            isAuto = false;
            //autoBar.gameObject.SetActive(false);
            currentTime = 0f;
            //GetComponent<Toggle>().isOn = false;
            //autoBar.gameObject.SetActive(false);
        }
    }
}