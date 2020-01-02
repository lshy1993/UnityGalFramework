using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Script.Framework.Effect;

using UnityEngine;

namespace Assets.Script.Framework.Node
{
    /// <summary>
    /// SoundPiece
    /// 音乐Piece 在游戏中用于音乐/音效控制
    /// </summary>
    public class MoviePiece : Piece
    {
        public string filename;
        //public Action callback;
        public MoviePiece(int id, string fname) : base(id)
        {
            filename = fname;
        }

        public override void Exec()
        {
            RunEffects();
        }

        public void ExecAuto(Action callback)
        {
            RunEffects(callback);
        }

        private void RunEffects()
        {
            RunEffects(() => { });
        }

        private void RunEffects(Action callback)
        {
            //GameObject.Find("GameManager").GetComponent<SoundManager>().
            //    RunEffect(fname, callback);
        }
    }
}
