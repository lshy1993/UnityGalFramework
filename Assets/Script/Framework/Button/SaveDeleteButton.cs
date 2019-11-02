using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Script.Framework.UI
{
    public class SaveDeleteButton : BasicButton
    {

        public SaveLoadUIManager uiManager;
        public int id;

        protected override void SE_Click()
        {
            //null
        }

        protected override void SE_Hover()
        {
            //null
        }

        protected override void Execute()
        {
            Debug.Log("SaveDelete");
            uiManager.SelectDelete(id);
        }
    }
}