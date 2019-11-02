using System.Collections;

using UnityEngine;

namespace Assets.Script.Framework.UI
{
    public class SaveCopyButton : BasicButton
    {
        public SaveLoadUIManager uiManager;
        public int id;

        protected override void SE_Click()
        {
            //null
        }

        protected override void Execute()
        {
            Debug.Log("SaveLoad");
            uiManager.SelectSave(id);
        }


    }
}