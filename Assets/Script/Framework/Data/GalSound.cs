using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Framework
{
    /// <summary>
    /// GalSound类
    /// 附在AudioSource原件
    /// </summary>
    public class GalSound : MonoBehaviour
    {
        /// <summary>
        /// 管理的音源
        /// </summary>
        public AudioSource currentSource;

        AudioClip ac0, ac1;

        enum SoundStatus
        {
            None,
            FadeIn,
            Play,
            FadeOut,
            VolumeChange
        };
        [SerializeField]
        private SoundStatus status = SoundStatus.None;

        /// <summary>
        /// 文件源
        /// </summary>
        private string fileName;

        /// <summary>
        /// 是否循环
        /// </summary>
        private bool isLoop;

        /// <summary>
        /// 淡入，淡出的时间
        /// </summary>
        private float fadeIn, fadeOut;

        /// <summary>
        /// 被设定的音量
        /// </summary>
        [SerializeField]
        private float userVolume;

        [SerializeField]
        private float deltaVolume;

        private float deltaTime, finalVolume;

        /// <summary>
        /// 所属的player
        /// </summary>
        public AudioPlayer ap;

        /// <summary>
        /// 设定音乐资料
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="fadeIn">淡入时间</param>
        /// <param name="isLoop">是否循环</param>
        public void SetSoundData(string fileName, float fadeIn, bool isLoop)
        {
            this.fileName = fileName;
            this.fadeIn = fadeIn;
            this.isLoop = isLoop;
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init()
        {
            //Debug.Log("audip clip:"+ fileName);
            //控制：需要中间循环的音乐 准备2个souce
            AudioClip ac0 = Resources.Load<AudioClip>("Audio/" + fileName);
            AudioClip ac1 = Resources.Load<AudioClip>("Audio/" + fileName + "_NoIntro");
            currentSource.clip = ac0;
            currentSource.loop = this.isLoop;
            if (fadeIn > 0)
            {
                FadeIn();
            }
            else
            {
                Play();
            }
        }

        /// <summary>
        /// 设定给audiosource的音量
        /// </summary>
        /// <param name="volume"></param>
        public void SetUserVolume(float volume)
        {
            userVolume = volume;
            currentSource.volume = userVolume;
        }

        /// <summary>
        /// 临时更改音量（用于淡入淡出）
        /// </summary>
        /// <param name="time">变动时间</param>
        /// <param name="change">相对变化百分比(0,1)</param>
        public void ChangeVolume(float time, float change)
        {
            //0+1 恢复至user
            deltaTime = time;
            if (change < 0)
            {
                finalVolume = userVolume * (1 + change);
            }
            else
            {
                finalVolume = userVolume * change;
            }

            deltaVolume = Mathf.Abs(currentSource.volume - finalVolume);
            status = SoundStatus.VolumeChange;
        }

        /// <summary>
        /// 是否播放完
        /// </summary>
        bool IsAudioEnd()
        {
            if (currentSource.clip == null)
            {
                return true;
            }
            if (currentSource.isPlaying)
            {
                return false;
            }
            else
            {
                if (currentSource.clip.length == currentSource.time)
                {
                    return true;
                }
                else if (Mathf.Approximately(currentSource.time, 0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsPlaying()
        {
            return status == SoundStatus.Play;
        }

        void Play()
        {
            currentSource.volume = userVolume;
            currentSource.Play();
            status = SoundStatus.Play;
        }

        public void FadeIn(float time)
        {
            this.fadeIn = time;
            FadeIn();
        }

        void FadeIn()
        {
            currentSource.volume = 0;
            deltaVolume = userVolume;
            Debug.Log(deltaVolume);
            currentSource.Play();
            status = SoundStatus.FadeIn;
        }

        public void FadeOut(float time)
        {
            this.fadeOut = time;
            FadeOut();
        }

        void FadeOut()
        {
            if (this.fadeOut == 0)
            {
                EndPlay();
            }
            else
            {
                deltaVolume = currentSource.volume;
                status = SoundStatus.FadeOut;
            }
        }

        /// <summary>
        /// 自我销毁
        /// </summary>
        public void EndPlay()
        {
            ap.RemoveObject(this.name);
            GameObject.Destroy(this.gameObject);
        }

        void Update()
        {
            switch (status)
            {
                case SoundStatus.Play:
                    UpdatePlay();
                    break;
                case SoundStatus.FadeIn:
                    UpdateFadeIn();
                    break;
                case SoundStatus.FadeOut:
                    UpdateFadeOut();
                    break;
                case SoundStatus.VolumeChange:
                    UpdateVolume();
                    break;
                default:
                    break;
            }
        }

        void UpdatePlay()
        {
            if (!isLoop && IsAudioEnd())
            {
                FadeOut();
            }
        }

        void UpdateFadeIn()
        {
            //控制音量到达userV
            currentSource.volume = Mathf.MoveTowards(currentSource.volume, userVolume, userVolume / fadeIn * Time.deltaTime);
            if (currentSource.volume == userVolume)
            {
                //切换至正常状态
                status = SoundStatus.Play;
            }
        }

        void UpdateFadeOut()
        {
            //控制音量到达0
            currentSource.volume = Mathf.MoveTowards(currentSource.volume, 0, userVolume / fadeOut * Time.deltaTime);
            if (currentSource.volume == 0)
            {
                EndPlay();
            }
        }

        void UpdateVolume()
        {
            currentSource.volume = Mathf.MoveTowards(currentSource.volume, finalVolume, deltaVolume / deltaTime * Time.deltaTime);
            //Debug.Log(currentSource.volume.ToString() + finalVolume.ToString());
            if (currentSource.volume == finalVolume) status = SoundStatus.Play;
        }

    }
}