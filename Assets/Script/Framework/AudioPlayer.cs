using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Assets.Script.Framework
{
    /// <summary>
    /// 音频播放器 管理组下audiosource
    /// </summary>
    public class AudioPlayer : MonoBehaviour
    {
        public SoundManager sm;

        /// <summary>
        /// 该组别整体音量
        /// </summary>
        public float GroupVolume
        {
            set
            {
                this.groupVolume = value;
                ChangeGroupVolume();
            }
            get { return groupVolume; }
        }
        [Range(0, 1), SerializeField]
        private float groupVolume = 1;

        /// <summary>
        /// 当前旗下得音源
        /// </summary>
        [SerializeField]
        private Dictionary<string, GameObject> audioObjects;

        private List<string> currentSyncList;

        private void Awake()
        {
            audioObjects = new Dictionary<string, GameObject>();
            currentSyncList = new List<string>();
        }

        /// <summary>
        /// 添加新的音源
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fadeIn"></param>
        /// <param name="isLoop"></param>
        public void AddSound(string fileName, float fadeIn, bool isLoop)
        {
            //不存在才添加
            if (!audioObjects.ContainsKey(fileName))
            {
                GameObject go = Resources.Load("Prefab/GalAudio") as GameObject;
                //go = NGUITools.AddChild(this.gameObject, go);
                go = Instantiate(go);
                go.transform.parent = this.transform;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                go.name = fileName;
                audioObjects.Add(fileName, go);
            }
            //直接获取
            GalSound gs = audioObjects[fileName].GetComponent<GalSound>();
            SetSoundData(gs, fileName, fadeIn, isLoop);
        }

        /// <summary>
        /// 替换现有音源
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fadeIn"></param>
        /// <param name="isLoop"></param>
        public void ChangeSound(string fileName, float fadeIn, bool isLoop)
        {
            if (audioObjects.Count > 0)
            {
                string key = audioObjects.FirstOrDefault().Key;
                audioObjects[key].GetComponent<GalSound>().EndPlay();
                //audioObjects.Remove(key);
            }
            AddSound(fileName, fadeIn, isLoop);
        }

        private void SetSoundData(GalSound gs, string fileName, float fadeIn, bool isLoop)
        {
            gs.ap = this;
            gs.SetUserVolume(sm.masterVolume * groupVolume);
            gs.SetSoundData(fileName, fadeIn, isLoop);
        }

        private void ChangeGroupVolume()
        {
            foreach (KeyValuePair<string, GameObject> kv in audioObjects)
            {
                if (kv.Value != null) currentSyncList.Add(kv.Key);
            }
            foreach (string str in currentSyncList)
            {
                audioObjects[str].GetComponent<GalSound>().SetUserVolume(groupVolume);
            }
            currentSyncList.Clear();
        }

        public void VolumeUp(float time, float change)
        {
            foreach (KeyValuePair<string, GameObject> kv in audioObjects)
            {
                if (kv.Value != null) currentSyncList.Add(kv.Key);
            }
            foreach (string str in currentSyncList)
            {
                audioObjects[str].GetComponent<GalSound>().ChangeVolume(time, change);
            }
            currentSyncList.Clear();
        }

        public void VolumeDown(float time, float change)
        {
            foreach (KeyValuePair<string, GameObject> kv in audioObjects)
            {
                if (kv.Value != null) currentSyncList.Add(kv.Key);
            }
            foreach (string str in currentSyncList)
            {
                audioObjects[str].GetComponent<GalSound>().ChangeVolume(time, -change);
            }
            currentSyncList.Clear();
        }


        /// <summary>
        /// 暂停某个音乐
        /// </summary>
        public void Pause(string name)
        {
            if (audioObjects.ContainsKey(name))
            {
                audioObjects[name].GetComponent<AudioSource>().Pause();
            }
            else
            {
                string result = string.Format("No {0} AudioObject in Groups", name);
                Debug.LogError(result);
            }
        }

        /// <summary>
        /// 暂停所有的音乐
        /// </summary>
        public void Pause()
        {
            foreach (KeyValuePair<string, GameObject> kv in audioObjects)
            {
                if (kv.Value != null) currentSyncList.Add(kv.Key);
            }
            foreach (string str in currentSyncList)
            {
                audioObjects[str].GetComponent<AudioSource>().Pause();
            }
            currentSyncList.Clear();
        }

        /// <summary>
        /// 恢复某个音乐
        /// </summary>
        public void UnPause(string name)
        {
            if (audioObjects.ContainsKey(name))
            {
                audioObjects[name].GetComponent<AudioSource>().UnPause();
            }
            else
            {
                string result = string.Format("No {0} AudioObject in Groups", name);
                Debug.LogError(result);
            }
        }

        /// <summary>
        /// 恢复所有的音乐
        /// </summary>
        public void UnPause()
        {
            //currentSyncList.Clear();
            foreach (KeyValuePair<string, GameObject> kv in audioObjects)
            {
                if (kv.Value != null) currentSyncList.Add(kv.Key);
            }
            foreach (string str in currentSyncList)
            {
                audioObjects[str].GetComponent<AudioSource>().UnPause();
            }
            currentSyncList.Clear();
        }

        /// <summary>
        /// 直接停止所控制的音乐
        /// </summary>
        public void Stop(string name)
        {
            if (audioObjects.ContainsKey(name))
            {
                audioObjects[name].GetComponent<GalSound>().EndPlay();
            }
            else
            {
                string result = string.Format("No {0} AudioObject in Groups", name);
                Debug.LogError(result);
            }
        }

        /// <summary>
        /// 直接停止所控制的所有音乐
        /// </summary>
        public void Stop()
        {
            //currentSyncList.Clear();
            foreach (KeyValuePair<string, GameObject> kv in audioObjects)
            {
                if (kv.Value != null) currentSyncList.Add(kv.Key);
            }
            foreach (string str in currentSyncList)
            {
                if (!audioObjects.ContainsKey(str)) continue;
                audioObjects[str].GetComponent<GalSound>().EndPlay();
            }
            currentSyncList.Clear();
        }

        public void RemoveObject(string name)
        {
            if (!audioObjects.ContainsKey(name)) return;
            audioObjects.Remove(name);
        }

        public string GetAudioName()
        {
            if (audioObjects.Count == 0) return "";
            return audioObjects.Keys.FirstOrDefault();
        }

        public float GetPlayedTime()
        {
            GameObject go = audioObjects.Values.FirstOrDefault();
            return go.GetComponent<AudioSource>().time;
        }

        public float GetAllTime()
        {
            //GameObject go = audioObjects.Values.FirstOrDefault();
            List<float> lens = audioObjects.Values.Select(x => x.GetComponent<AudioSource>().clip.length).ToList();
            return Mathf.Max(lens.ToArray());
        }

        public bool EffectEnd()
        {
            return !IsPlaying();
            //return false;
        }

        public bool IsPlaying()
        {
            if (audioObjects.Count == 0) return false;
            foreach (KeyValuePair<string, GameObject> kv in audioObjects)
            {
                currentSyncList.Clear();
                if (kv.Value != null) currentSyncList.Add(kv.Key);
            }
            foreach (string str in currentSyncList)
            {
                if (audioObjects[str].GetComponent<GalSound>().IsPlaying()) return true;
            }
            return false;
        }
    }
}