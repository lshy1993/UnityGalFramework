using System.Collections;

using Assets.Script.Framework.Node;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Script.Framework.UI
{
    public class Click_Next : MonoBehaviour, IPointerClickHandler
    {
        public GameManager gm;
        public MessageUIManager uiManger;
        public ToggleAuto ta;
        
        /// <summary>
        /// 快进模式
        /// </summary>
        private bool skipmode;

        void Update()
        {
            if (skipmode)
            {
                // 跳过所有 或者 已读文本
                if (DataManager.GetInstance().configData.skipAll || DataManager.GetInstance().IsTextRead())
                {
                    Execute();
                }
            }
            else
            {
                //跳过
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                    Execute();
                }
                // 空格 回车 触发左键
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    Execute();
                }
            }
            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //base.OnPointerClick(eventData);
            //if (Input.GetMouseButtonUp(1)) return;
            skipmode = false;
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            Execute();
        }

        public void Execute()
        {
            // 如果锁定点击 则直接返回
            if (DataManager.GetInstance().IsClickBlocked())
            {
                Debug.Log("blocked");
                return;
            }
            // 如果auto模式开启 则关闭计时器
            else if (DataManager.GetInstance().isAuto)
            {
                Debug.Log("关闭计时器");
                ScriptUpdate();
                ta.ResetTimer();
            }
            // 如果对话框被隐藏
            else if (uiManger.IsBoxClosed())
            {
                Debug.Log("恢复对话框显示");
                uiManger.ShowWindow();
                return;
            }
            else
            {
                //Debug.Log("clicked");
                ScriptUpdate();
            }
        }

        private void ScriptUpdate()
        {
            //否则根据Script类型执行Update
            if (typeof(TextScript).IsInstanceOfType(gm.GetCurrentNode()))
            {
                gm.GetCurrentNode().Update();
            }
            else
            {
                Debug.Log("无效点击");
            }
        }

        public void SkipMode()
        {
            skipmode = !skipmode;
        }

    }
}