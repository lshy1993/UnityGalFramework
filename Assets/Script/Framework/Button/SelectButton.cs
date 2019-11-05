using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Framework.UI
{
    public class SelectButton : BasicButton
    {
        private SelectUIManager uiManager;
        private string text;
        private int select_id;

        protected override void Execute()
        {
            uiManager.Select(select_id);
        }

        public void SetUIManager(SelectUIManager ui)
        {
            this.uiManager = ui;
        }
        public void SetID(int x)
        {
            this.select_id = x;
        }
        public void SetText(string text)
        {
            this.text = text;
        }
    }
}