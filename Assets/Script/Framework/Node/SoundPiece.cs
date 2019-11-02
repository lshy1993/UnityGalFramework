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
    public class SoundPiece : Piece
    {
        private Queue<SoundEffect> effects;
        //public Action callback;
        public SoundPiece(int id, Queue<SoundEffect> effects) : base(id)
        {
            this.effects = effects;
        }

        public override void Exec()
        {
            RunEffects();
        }

        public void ExecAuto(Action callback)
        {
            RunEffects(effects, callback);
        }

        private void RunEffects()
        {
            RunEffects(effects, () => { });
        }

        private void RunEffects(Queue<SoundEffect> effectQueue, Action callback)
        {
            if (effectQueue.Count == 0 || effectQueue.Peek() == null)
            {
                //Debug.Log("sp callback");
                callback();
            }
            else
            {
                SoundEffect effect = effectQueue.Dequeue();
                GameObject.Find("GameManager").GetComponent<SoundManager>().RunEffect(effect, new Action(() =>
                {
                    RunEffects(effectQueue, callback);
                }));
            }
        }
    }
}
