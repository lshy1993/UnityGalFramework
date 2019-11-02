using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    /// <summary>
    /// 仅在LOGO阶段有效
    /// </summary>
    public class Click_SkipLogo : MonoBehaviour, IPointerClickHandler
    {
        public LogoUIManager uiManager;

        public void OnPointerClick(PointerEventData eventData)
        {
            //base.OnPointerClick(eventData);
            if (eventData.button != PointerEventData.InputButton.Left)
                return;
            Execute();
        }

        void Execute()
        {
            Debug.Log(name + "click!");
            uiManager.Skip();
        }
    }
}