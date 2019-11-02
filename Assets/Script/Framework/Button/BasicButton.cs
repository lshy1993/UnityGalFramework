using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace Assets.Script.Framework.UI
{
    /// <summary>
    /// 作为游戏内所有按钮的基类
    /// </summary>
    public class BasicButton : Selectable
    {
        protected SoundManager sm;

        [SerializeField]
        public UnityEvent onClick;
 
        [SerializeField]
        public UnityEvent onHover;

        /// <summary>
        /// 默认悬停音效
        /// </summary>
        public string File_Hover = "SE_hover";

        /// <summary>
        /// 默认点击音效
        /// </summary>
        public string File_Click = "decision3";

        protected override void Awake()
        {
            sm = GameObject.Find("GameManager").GetComponent<SoundManager>();
        }

        /// <summary>
        /// 按钮悬停效果触发
        /// </summary>
        public override void OnPointerEnter(PointerEventData eventData)
        {
            SE_Hover();
            base.OnPointerEnter(eventData);
            Hover(true);
        }

        /// <summary>
        /// 按钮悬停效果结束
        /// </summary>
        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            Hover(false);
        }

        /// <summary>
        /// 按钮点击的音效触发
        /// </summary>
        public override void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log("pressed");
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            base.OnPointerUp(eventData);
            SE_Click();
            Execute();
        }


        /// <summary>
        /// 修改默认悬停音效
        /// </summary>
        protected virtual void SE_Hover()
        {
            if (string.IsNullOrEmpty(File_Hover)) return;
            sm.SetSystemSE(File_Hover);
        }

        /// <summary>
        /// 修改默认点击音效
        /// </summary>
        protected virtual void SE_Click()
        {
            if (string.IsNullOrEmpty(File_Click)) return;
            sm.SetSystemSE(File_Click);
        }

        /// <summary>
        /// 悬停触发内容
        /// </summary>
        protected virtual void Hover(bool ishover)
        {
            onHover.Invoke();
        }

        /// <summary>
        /// 点击触发内容
        /// </summary>
        protected virtual void Execute()
        {
            onClick.Invoke();
        }
    }
}