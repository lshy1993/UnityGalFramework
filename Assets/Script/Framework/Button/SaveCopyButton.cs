using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    public class SaveCopyButton : Toggle
    {
        public SaveLoadUIManager uiManager;
        public int id;

        //protected override void SE_Click()
        //{
        //    //null
        //}

        public override void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Save Copy");
            uiManager.SelectCopy(id);
            base.OnPointerClick(eventData);
        }

        //protected override void Execute()
        //{
            
        //}


    }
}