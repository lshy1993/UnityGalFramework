using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.Framework.Effect
{
    public class SoundBuilder
    {
        static SoundManager soundManager;
        //public static AudioSource currentBGM, currentSE;

        public static void Init(SoundManager sm)
        {
            soundManager = GameObject.Find("GameManager").GetComponent<SoundManager>();
        }

        private SoundEffect soundEffect;

        public SoundBuilder Source(SoundEffect.SoundType mode)
        {
            soundEffect = new SoundEffect();
            soundEffect.target = mode;
            return this;
        }

        public SoundBuilder Operater(SoundEffect.OperateType mode)
        {
            soundEffect.operate = mode;
            return this;
        }

        public SoundBuilder Clip(string fname)
        {
            soundEffect.clip = fname;
            return this;
        }

        public SoundBuilder Loop(bool loop)
        {
            soundEffect.loop = loop;
            return this;
        }

        public SoundBuilder Argus(float p)
        {
            soundEffect.argus = p;
            return this;
        }

        public SoundBuilder TotalTime(float time)
        {
            soundEffect.time = time;
            return this;
        }

        public SoundEffect Get() { return soundEffect; }

        public static SoundEffect SetBGM(string fname, float time, bool loop)
        {
            SoundBuilder builder = new SoundBuilder();
            SoundEffect e = builder.Source(SoundEffect.SoundType.BGM)
                .Operater(SoundEffect.OperateType.Set)
                .TotalTime(time)
                .Clip(fname)
                .Loop(loop)
                .Get();
            return e;
        }

        public static SoundEffect RemoveBGM()
        {
            SoundBuilder builder = new SoundBuilder();
            SoundEffect e = builder.Source(SoundEffect.SoundType.BGM)
                .Operater(SoundEffect.OperateType.Remove)
                .Get();
            return e;
        }

        public static SoundEffect VolumeUpBGM(float time, float change)
        {
            SoundBuilder builder = new SoundBuilder();
            SoundEffect e = builder.Source(SoundEffect.SoundType.BGM)
                .Operater(SoundEffect.OperateType.VolumeUp)
                .TotalTime(time)
                .Argus(change)
                .Get();
            return e;
        }

        public static SoundEffect VolumeDownBGM(float time, float change)
        {
            SoundBuilder builder = new SoundBuilder();
            SoundEffect e = builder.Source(SoundEffect.SoundType.BGM)
                .Operater(SoundEffect.OperateType.VolumeDown)
                .TotalTime(time)
                .Argus(change)
                .Get();
            return e;
        }

        public static SoundEffect PauseBGM()
        {
            SoundBuilder builder = new SoundBuilder();
            SoundEffect e = builder.Source(SoundEffect.SoundType.BGM)
                .Operater(SoundEffect.OperateType.Pause)
                .Get();
            return e;
        }

        public static SoundEffect UnpauseBGM()
        {
            SoundBuilder builder = new SoundBuilder();
            SoundEffect e = builder.Source(SoundEffect.SoundType.BGM)
                .Operater(SoundEffect.OperateType.Unpause)
                .Get();
            return e;
        }

        public static SoundEffect SetSE(string fname, float time, bool loop)
        {
            SoundBuilder builder = new SoundBuilder();
            SoundEffect e = builder.Source(SoundEffect.SoundType.SE)
                .Operater(SoundEffect.OperateType.Set)
                .TotalTime(time)
                .Loop(loop)
                .Get();
            return e;
        }

        public static SoundEffect RemoveSE()
        {
            SoundBuilder builder = new SoundBuilder();
            SoundEffect e = builder.Source(SoundEffect.SoundType.SE)
                .Operater(SoundEffect.OperateType.Remove)
                .Get();
            return e;
        }

        public static SoundEffect VolumeUpSE(float time, float change)
        {
            SoundBuilder builder = new SoundBuilder();
            SoundEffect e = builder.Source(SoundEffect.SoundType.SE)
                .Operater(SoundEffect.OperateType.VolumeUp)
                .Argus(change)
                .TotalTime(time)
                .Get();
            return e;
        }

        public static SoundEffect VolumeDownSE(float time, float change)
        {
            SoundBuilder builder = new SoundBuilder();
            SoundEffect e = builder.Source(SoundEffect.SoundType.SE)
                .Operater(SoundEffect.OperateType.VolumeDown)
                .TotalTime(time)
                .Argus(change)
                .Get();
            return e;
        }
    }
    
}
