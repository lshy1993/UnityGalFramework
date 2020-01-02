using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    public class AvgUIManager : MonoBehaviour
    {
        private GameObject root;
        public GameManager gm;
        public GameObject avgObject;
        public CanvasGroup avgPanel;

        //// Use this for initialization
        //void Awake()
        //{
        //    avgObject = transform.parent.gameObject;
        //    avgPanel = avgObject.GetComponent<UIPanel>();
        //    Debug.Log("AvgPanel init");
        //}

        /// <summary>
        /// 存储面板名称
        /// </summary>
        public readonly List<string> PANEL_NAMES = new List<string>()
        {
            "Invest",
            "DialogBox",
            "Reasoning",
            "Enquire",
            "Negotiate"
        };

        Dictionary<string, GameObject> panels;
        private string current;

        void Start()
        {


        }

        private void OnDisable()
        {
            // 储存至save0
            DataManager.GetInstance().Save(0);
        }

        // Use this for initialization
        public void Init()
        {
            root = transform.gameObject;
            panels = new Dictionary<string, GameObject>();
            for (int i = 0; i < PANEL_NAMES.Count; i++)
            {
                GameObject panelObj = root.transform.Find(PANEL_NAMES[i] + "_Panel").gameObject;
                panels.Add(PANEL_NAMES[i], panelObj);
            }
            current = "DialogBox";
        }

        /// <summary>
        /// 切换面板
        /// </summary>
        /// <param name="panel">切换目标面板</param>
        /// <param name="fadein">淡入时间，默认0.3s</param>
        /// <param name="fadeout">淡出时间，默认0.3s</param>
        public void SwitchTo(string panel, float fadein = 0.3f, float fadeout = 0.3f)
        {

            if (panels.ContainsKey(panel))
            {
                GameObject currentPanel = panels[current];
                GameObject nextPanel = panels[panel];
                Debug.Log(panel);
                StartCoroutine(Switch(currentPanel, nextPanel, fadein, fadeout));
                current = panel;
            }
            else
            {
                Debug.Log("Can't find panel: " + panel);
            }
        }

        private IEnumerator Switch(GameObject currentPanel, GameObject nextPanel, float fadein, float fadeout)
        {
            PanelFade2 current = currentPanel.GetComponent<PanelFade2>(),
                       next = nextPanel.GetComponent<PanelFade2>();
            current.Close(fadeout);
            yield return new WaitForSeconds(fadeout);
            nextPanel.SetActive(true);
            next.Open(fadein);
        }

    }
}