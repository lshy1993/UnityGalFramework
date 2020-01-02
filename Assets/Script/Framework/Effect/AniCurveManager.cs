using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Framework.Effect
{
    public class AniCurveManager
    {
        private static AniCurveManager instance = new AniCurveManager();

        public AnimationCurve Curve84;
        public AnimationCurve Curve52;
        public AnimationCurve Curve42;

        private AniCurveManager()
        {
            SetCurve84();
            SetCurve52();
            SetCurve42();
        }

        public static AniCurveManager GetInstance()
        {
            return instance;
        }

        private void SetCurve84()
        {
            Keyframe[] ks = new Keyframe[2];
            ks[0] = new Keyframe(0, 0);
            ks[0].outWeight = 0.84f;
            ks[0].weightedMode = WeightedMode.Out;
            ks[1] = new Keyframe(1f, 1f);
            ks[1].inWeight = 0.84f;
            ks[1].weightedMode = WeightedMode.In;
            Curve84 = new AnimationCurve(ks);
        }

        private void SetCurve52()
        {
            Keyframe[] ks = new Keyframe[2];
            ks[0] = new Keyframe(0, 0);
            ks[0].outWeight = 0.52f;
            ks[0].weightedMode = WeightedMode.Out;
            ks[1] = new Keyframe(1f, 1f);
            ks[1].inWeight = 0.48f;
            ks[1].weightedMode = WeightedMode.In;
            Curve52 = new AnimationCurve(ks);
        }

        private void SetCurve42()
        {
            Keyframe[] ks = new Keyframe[2];
            ks[0] = new Keyframe(0, 0);
            ks[0].outWeight = 0.42f;
            ks[0].weightedMode = WeightedMode.Out;
            ks[1] = new Keyframe(1f, 1f);
            ks[1].inWeight = 0.58f;
            ks[1].weightedMode = WeightedMode.In;
            Curve42 = new AnimationCurve(ks);
        }

    }
}
