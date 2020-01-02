using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Assets.Script.Framework.UI;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework
{
    /// <summary>
    /// 历史记录管理
    /// 用于记录文字语音选择分支
    /// </summary>
    public class HistoryManager : MonoBehaviour
    {

        // 文字履历表
        public GameObject table;

        // 滚动条
        public Scrollbar bar;


        private Regex rx = new Regex(@"\[[^\]]+\]");


        /// <summary>
        /// 清空记录块
        /// </summary>
        public void ClearHistoryObject()
        {
            for (int i = 0; i < table.transform.childCount; i++)
            {
                Destroy(table.transform.GetChild(i).gameObject);
            }

        }
        /// <summary>
        /// 加入文字履历
        /// </summary>
        /// <param name="bt"></param>
        public void AddToTable(BacklogText bt)
        {
            //获取系统数据储存的数目
            Queue<BacklogText> history = DataManager.GetInstance().GetHistory();
            if (history.Count == 0)
            {
                ClearHistoryObject();
            }
            //添加U最新一行界面
            GameObject go = Resources.Load("Prefab/Backlog") as GameObject;
            go = Instantiate(go);
            go.transform.SetParent(table.transform);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;

            //table.GetComponent<VerticalLayoutGroup>().SetLayoutVertical();

            float w = table.GetComponent<RectTransform>().sizeDelta.x;
            table.GetComponent<RectTransform>().sizeDelta = new Vector2(w, table.transform.childCount * 200);
            
            //绑定数据
            go.transform.Find("Name_Label").GetComponent<Text>().text = bt.charaName;
            go.transform.Find("Content_Label").GetComponent<Text>().text = bt.mainContent;
            if (!string.IsNullOrEmpty(bt.avatarFile))
            {
                GameObject ai = go.transform.Find("Avatar_Image").gameObject;
                ai.SetActive(true);
                ai.GetComponent<Image>().sprite = Resources.Load(bt.avatarFile) as Sprite;
            }
            if (!string.IsNullOrEmpty(bt.voicePath))
            {
                GameObject vb = go.transform.Find("Voice_Button").gameObject;
                vb.SetActive(true);
                vb.GetComponent<BacklogVoiceButton>().path = bt.voicePath;
            }
            if (table.transform.childCount > 100)
            {
                //删除第一个
                Destroy(table.transform.GetChild(0).gameObject);
            }
            DataManager.GetInstance().AddHistory(bt);
        }

        /// <summary>
        /// 记录当前的文本
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dialog"></param>
        public void SetCurrentText(string name, string dialog)
        {
            // 去掉颜色标签符号
            DataManager.GetInstance().tempData.currentText = rx.Replace(dialog, "");
            DataManager.GetInstance().tempData.currentName = rx.Replace(name, "");
            // 更已读
            SetReaded();
        }

        public void SetReaded()
        {
            string script = DataManager.GetInstance().gameData.currentScript;
            int id = DataManager.GetInstance().gameData.currentTextPos;

            Dictionary<string, int> dic = DataManager.GetInstance().multiData.scriptTable;
            if (dic.ContainsKey(script))
            {
                dic[script] = Mathf.Max(dic[script], id);
            }
            else
            {
                dic.Add(script, id);
            }
        }
    }

}