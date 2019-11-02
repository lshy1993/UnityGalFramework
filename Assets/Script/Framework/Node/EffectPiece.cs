using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Script.Framework.Effect;

using UnityEngine;

namespace Assets.Script.Framework.Node
{
    /// <summary>
    /// EffectPiece【图像处理块】
    /// 特效Piece,在游戏中用于切换立绘，背景等。
    /// </summary>
    public class EffectPiece : Piece
    {
        //private Queue<ImageEffect> effects;
        private Queue<NewImageEffect> effects;
        private bool syn;

        public EffectPiece(int id, Queue<NewImageEffect> effects, bool syn = false) : base(id)
        {
            this.effects = effects;
            this.syn = syn;
        }

        public override void Exec()
        {
            RunEffects(effects, () => { });
        }

        public void ExecAuto(Action callback)
        {
            RunEffects(effects, callback);
        }

        private void RunEffects(Queue<NewImageEffect> effectQueue, Action callback)
        {
            if (syn)
            {
                RunEffectsSyn(effectQueue, callback);
            }
            else
            {
                RunEffectsAsyn(effectQueue, callback);
            }
        }

        //同步特效递归
        private void RunEffectsSyn(Queue<NewImageEffect> effectQueue, Action callback)
        {
            if (effectQueue.Count == 2)
            {
                //Debug.Log("Syn End");
                RunEffectsAsyn(effectQueue, callback);
            }
            else
            {
                NewImageEffect effect = effectQueue.Dequeue();
                GameObject.Find("GameManager").GetComponent<ImageManager>().RunEffect(effect, new Action(() => { }));
                RunEffectsSyn(effectQueue, callback);
            }
        }

        //异步特效递归
        private void RunEffectsAsyn(Queue<NewImageEffect> effectQueue, Action callback)
        {
            //Debug.Log(effectQueue.Count);
            if (effectQueue.Count == 0 || effectQueue.Peek() == null)
            {
                callback();
            }
            else
            {
                NewImageEffect effect = effectQueue.Dequeue();
                GameObject.Find("GameManager").GetComponent<ImageManager>().RunEffect(effect, new Action(() => { RunEffectsAsyn(effectQueue, callback); }));
            }
        }

    }
}
