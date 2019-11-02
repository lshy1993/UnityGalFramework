using System.Collections;
using System.Collections.Generic;
using System;

using Assets.Script.Framework.Effect;
using Assets.Script.Framework.UI;

using UnityEngine;

namespace Assets.Script.Framework
{
    /// <summary>
    /// 全局声音控制
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        /// <summary>
        /// BGM控制器
        /// </summary>
        public AudioPlayer playerBGM;

        /// <summary>
        /// 效果音源
        /// </summary>
        public AudioPlayer playerSE;

        /// <summary>
        /// 语音音源
        /// </summary>
        public AudioPlayer playerVoice;

        /// <summary>
        /// 系统效果音
        /// </summary>
        public AudioPlayer playerSysSE;

        public SideLabelUIManager sideLabel;

        /// <summary>
        /// 主音量
        /// </summary>
        [Range(0, 1), SerializeField]
        public float masterVolume = 1;

        public float userBgmVolume
        {
            get { return dm.configData.userBGMVolume; }
            set
            {
                dm.configData.userBGMVolume = value;
                playerBGM.GroupVolume = value;
            }
        }
        public float userSeVolume
        {
            get { return dm.configData.userSEVolume; }
            set
            {
                dm.configData.userSEVolume = value;
                playerSE.GroupVolume = value;
            }
        }

        public float userVoiceVolume
        {
            get { return dm.configData.userVoiceVolume; }
            set
            {
                dm.configData.userVoiceVolume = value;
                playerVoice.GroupVolume = value;
            }
        }
        public float userSysSEVolume
        {
            get { return dm.configData.userSysSEVolume; }
            set
            {
                dm.configData.userSysSEVolume = value;
                playerSysSE.GroupVolume = value;
            }
        }


        //private Dictionary<string, AudioClip> auDic;
        //private AudioSource aim;

        private DataManager dm;

        private void Start()
        {
            dm = DataManager.GetInstance();
            userBgmVolume = dm.configData.userBGMVolume;
            userSeVolume = dm.configData.userSEVolume;
            userVoiceVolume = dm.configData.userVoiceVolume;
            userSysSEVolume = dm.configData.userSysSEVolume;

        }

        //public void SetAuDic(Dictionary<string, AudioClip> auDic)
        //{
        //    this.auDic = auDic;
        //}

        public void AddBGM(string fileName, bool loop = true)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            //设置资料
            playerBGM.ChangeSound(fileName, 0.5f, loop);
        }

        public void SetBGM(string fileName, bool loop = true)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            //设置资料
            playerBGM.ChangeSound("BGM/" + fileName, 0.5f, loop);
        }
        public void PauseBGM()
        {
            playerBGM.Pause();
        }
        public void PlayBGM()
        {
            playerBGM.UnPause();
        }
        public void StopBGM()
        {
            playerBGM.Stop();
        }

        public void SetSE(string fileName, bool loop = false)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            playerSE.ChangeSound("SE/" + fileName, 0, loop);
        }
        public void StopSE()
        {
            playerSE.Stop();
        }

        public void SetVoice(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            //设置资料
            Debug.Log(fileName);
            playerVoice.ChangeSound("Voice/" + fileName, 0, false);
        }
        public void StopVoice()
        {
            playerVoice.Stop();
        }

        /// <summary>
        /// 播放系统音效
        /// </summary>
        public void SetSystemSE(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            //设置资料
            playerSysSE.ChangeSound("SE/" + fileName, 0f, false);
        }


        public void RunEffect(SoundEffect effect, Action callback)
        {
            //选择操作类型
            switch (effect.operate)
            {
                case SoundEffect.OperateType.Pause:
                    GetPlayer(effect).Pause();
                    callback();
                    break;
                case SoundEffect.OperateType.Unpause:
                    GetPlayer(effect).UnPause();
                    callback();
                    break;
                case SoundEffect.OperateType.Set:
                    GetPlayer(effect).ChangeSound("BGM/" + effect.clip, effect.time, effect.loop);
                    if (effect.target == SoundEffect.SoundType.BGM)
                    {
                        //显示bgm标题哈希表获取标题
                        string title = ""; // dm.staticData.bgmTitleList[effect.clip];
                        sideLabel.ShowBGM(title);
                    }
                    callback();
                    break;
                case SoundEffect.OperateType.Remove:
                    GetPlayer(effect).Stop();
                    callback();
                    break;
                default:
                    StartCoroutine(Run(effect, callback));
                    break;
            }
        }

        private AudioPlayer GetPlayer(SoundEffect effect)
        {
            switch (effect.target)
            {
                case SoundEffect.SoundType.BGM:
                    return playerBGM;
                case SoundEffect.SoundType.SE:
                    return playerSE;
                case SoundEffect.SoundType.Voice:
                    return playerVoice;
                default:
                    return playerBGM;
            }
        }

        public void SaveSoundInfo()
        {
            string bgmName = playerBGM.GetAudioName();
            string seName = playerSE.GetAudioName();
            string voiceName = playerVoice.GetAudioName();
            dm.gameData.BGM = bgmName;
            dm.gameData.SE = seName;
            dm.gameData.Voice = voiceName;
        }

        public bool IsVoicePlaying()
        {
            return playerVoice.IsPlaying();
        }

        public void LoadSoundInfo()
        {
            string bgmName = dm.gameData.BGM;
            string seName = dm.gameData.SE;
            string voiceName = dm.gameData.Voice;
            SetBGM(bgmName);
            SetSE(seName);
            SetVoice(voiceName);
        }

        private IEnumerator Run(SoundEffect effect, Action callback)
        {
            //根据操作设定变化
            AudioPlayer ap = GetPlayer(effect);
            switch (effect.operate)
            {
                case SoundEffect.OperateType.VolumeUp:
                    ap.VolumeUp(effect.time, effect.argus);
                    break;
                case SoundEffect.OperateType.VolumeDown:
                    ap.VolumeDown(effect.time, effect.argus);
                    break;
            }
            while (ap.EffectEnd())
            {
                yield return null;
            }
            callback();
        }

    }
}