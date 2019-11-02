using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

namespace Assets.Script.Framework.UI
{
    /// <summary>
    /// 侧边章节BGM标签UI管理器
    /// </summary>
    public class SideLabelUIManager : MonoBehaviour
    {
        //制作淡入淡出与时间等待器
        public GameObject bgmCon, chapterCon;
        public Text bgmLabel, chapterLabel;

        private const int originX_bgm = -1110;
        private const int X_bgm = 300;
        private const int originY_bgm = 495;
        //private const int finalX2 = finalX1 - 300;

        private void OnEnable()
        {
            //复位
            bgmCon.transform.localPosition = new Vector3(originX_bgm, originY_bgm);
            //chapterCon.GetComponent<UIWidget>().alpha = 0;
        }

        /// <summary>
        /// 飞入BGM名称
        /// </summary>
        public void ShowBGM(string name)
        {
            float waitTime = DataManager.GetInstance().configData.BGMTime;
            if (waitTime == 0) return;
            gameObject.SetActive(true);
            bgmLabel.text = name;
            StartCoroutine(MainRoutine(true, waitTime));
        }

        /// <summary>
        /// 飞入章节名
        /// </summary>
        public void ShowChapter(string str)
        {
            float waitTime = DataManager.GetInstance().configData.chapterTime;
            if (waitTime == 0) return;
            gameObject.SetActive(true);
            if (str == string.Empty)
            {
                chapterLabel.text = DataManager.GetInstance().gameData.currentScript;
            }
            else
            {
                chapterLabel.text = str;
            }
            StartCoroutine(MainRoutine(false, waitTime));
        }

        private IEnumerator MainRoutine(bool isbgm, float waitTime)
        {
            //1.显示
            if (isbgm)
            {
                yield return StartCoroutine(MoveBGM(true));
            }
            else
            {
                yield return StartCoroutine(MoveChapter(true));
            }
            //2.等待秒数
            yield return new WaitForSeconds(waitTime);
            //3.消失
            if (isbgm)
            {
                yield return StartCoroutine(MoveBGM(false));
            }
            else
            {
                yield return StartCoroutine(MoveChapter(false));
            }
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 移动BGM标签
        /// </summary>
        /// <param name="isMoveIn">是否飞入</param>
        /// <returns></returns>
        private IEnumerator MoveBGM(bool isMoveIn)
        {
            float x0 = isMoveIn ? originX_bgm : originX_bgm - X_bgm;

            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / 0.3f * Time.deltaTime);
                float x = isMoveIn ? x0 + t * X_bgm : x0 - t * X_bgm;
                bgmCon.transform.localPosition = new Vector2(x, bgmCon.transform.localPosition.y);
                yield return null;
            }
        }

        private IEnumerator MoveChapter(bool forward)
        {
            float t = 0;
            //float X0 = forward ? finalX2 + 300 : finalX2;
            //float X1 = !forward ? finalX2 + 300 : finalX2;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / 0.3f * Time.deltaTime);
                //float x = X0 + t * (X1 - X0);
                //chapterCon.transform.localPosition = new Vector2(x, chapterCon.transform.localPosition.y);
                //chapterCon.GetComponent<UIWidget>().alpha = forward ? t : 1 - t;
                yield return null;
            }
        }


    }
}