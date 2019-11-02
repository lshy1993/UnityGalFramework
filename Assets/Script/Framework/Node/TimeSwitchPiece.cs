using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Framework.Node
{
    /// <summary>
    /// 时间显示块（从右侧显示）
    /// </summary>
    public class TimeSwitchPiece : Piece
    {
        private GameObject timePanel;
        //private TimeUIManager uiManager;
        private string timeStr, placeStr;
        public bool finished;

        public TimeSwitchPiece(int id, GameObject timepanel, string time, string place) : base(id)
        {
            timePanel = timepanel;
            //uiManager = timePanel.GetComponent<TimeUIManager>();
            timeStr = time;
            placeStr = place;
            finished = false;
        }

        public override void Exec()
        {
            timePanel.SetActive(true);
            //执行UI动作
            //uiManager.SetLabel(timeStr, placeStr, "");
            //uiManager.Show(this);
        }

        public void ExecAuto(Action callback)
        {
            //uiManager.Close(callback);
        }

    }
}
