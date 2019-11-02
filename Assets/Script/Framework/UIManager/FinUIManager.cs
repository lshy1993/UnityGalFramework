using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    public class FinUIManager : MonoBehaviour
    {
        public SoundManager sm;
        public PanelSwitch ps;
        public GameObject picPanel;
        public GameObject castTable;

        private int maxHeight = 3360;

        private void Update()
        {
            //按下左键则跳过
            //if (Input.GetMouseButton(0))
            //{
            //    sm.StopBGM();
            //    StopAllCoroutines();
            //    ps.SwitchTo_VerifyIterative("Title_Panel");
            //}
        }

        private void OnEnable()
        {
            picPanel.GetComponent<Image>().sprite = null;
            castTable.transform.localPosition = new Vector3(0, -540);
            StartCoroutine(OpenAnimate());
            StartCoroutine(ShowPic());
        }

        /// <summary>
        /// CAST表
        /// </summary>
        private IEnumerator OpenAnimate()
        {
            sm.SetBGM("ed");
            yield return new WaitForSeconds(1f);
            //预设总时长
            float tall = 60f;

            //初始位置
            float t = -540;
            while (t < maxHeight)
            {
                t = Mathf.MoveTowards(t, maxHeight, maxHeight / tall * Time.deltaTime);
                castTable.transform.localPosition = new Vector3(0, t);
                yield return null;
            }
            yield return new WaitForSeconds(5f);
            //TODO:等待点击
            StopAllCoroutines();
            ps.SwitchTo_VerifyIterative("Title_Panel");
        }

        /// <summary>
        /// 显示图片
        /// </summary>
        private IEnumerator ShowPic()
        {
            float t = 0;
            yield return new WaitForSeconds(2f);
            Sprite[] spAll = Resources.LoadAll<Sprite>("Background/");
            while (true)
            {
                //随机显示图片
                picPanel.GetComponent<Image>().sprite = spAll[UnityEngine.Random.Range(0, spAll.Count())];
                //picPanel.GetComponent<UI2DSprite>().sprite2D = Resources.Load<Sprite>("Background/themepark_day");
                while (t < 1)
                {
                    t = Mathf.MoveTowards(t, 1, 1 / 0.5f * Time.deltaTime);
                    picPanel.GetComponent<Image>().color = new Color(0, 0, 0, t);
                    yield return null;
                }
                yield return new WaitForSeconds(5f);
                while (t > 0)
                {
                    t = Mathf.MoveTowards(t, 0, 1 / 0.5f * Time.deltaTime);
                    picPanel.GetComponent<Image>().color = new Color(0, 0, 0, t);
                    yield return null;
                }
            }

            //2

            //picPanel.GetComponent<Image>().sprite = Resources.Load<Sprite>("Background/school_day");
            //while (t < 1)
            //{
            //    t = Mathf.MoveTowards(t, 1, 1 / 0.5f * Time.deltaTime);
            //    picPanel.GetComponent<Image>().color = new Color(0, 0, 0, t);
            //    yield return null;
            //}
            //yield return new WaitForSeconds(5f);
            //while (t > 0)
            //{
            //    t = Mathf.MoveTowards(t, 0, 1 / 0.5f * Time.deltaTime);
            //    picPanel.GetComponent<Image>().color = new Color(0, 0, 0, t);
            //    yield return null;
            //}
            //yield return null;
        }

    }
}
