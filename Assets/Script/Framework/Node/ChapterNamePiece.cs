using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Script.Framework.UI;

using UnityEngine;

namespace Assets.Script.Framework.Node
{
    /// <summary>
    /// 章节名显示块
    /// </summary>
    public class ChapterNamePiece : Piece
    {
        private GameObject sideLabelPanel;
        private string chapterName;
        public bool finished;

        public ChapterNamePiece(int id, GameObject panel, string str) : base(id)
        {
            sideLabelPanel = panel;
            chapterName = str;
        }

        public override void Exec()
        {
            sideLabelPanel.SetActive(true);
            SideLabelUIManager uiManager = sideLabelPanel.GetComponent<SideLabelUIManager>();
            uiManager.ShowChapter(chapterName);
        }

    }
}