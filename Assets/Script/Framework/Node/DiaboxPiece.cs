using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.Framework.Node
{
    /// <summary>
    /// 仅用于控制对话框的显示与消失
    /// </summary>
    public class DiaboxPiece : Piece
    {
        private GameObject diabox;
        private bool isopen;
        private float time;

        public DiaboxPiece(int id, GameObject diabox, bool isopen, float time) : base(id)
        {
            this.isopen = isopen;
            this.time = time;
            this.diabox = diabox;
        }

        public override void Exec()
        {
            DialogBoxUIManager uiManger = diabox.GetComponent<DialogBoxUIManager>();
            if (isopen) uiManger.Open(time, new Action(() => { }));
            else uiManger.Close(time, new Action(() => { }));
        }

        public void ExecAuto(Action callback)
        {
            DialogBoxUIManager uiManger = diabox.GetComponent<DialogBoxUIManager>();
            if (isopen) uiManger.Open(time, callback);
            else uiManger.Close(time, callback);
        }
    }
}
