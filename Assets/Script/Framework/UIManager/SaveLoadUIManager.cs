using System.Collections;
using System.Collections.Generic;

using Assets.Script.Framework.Data;
using Assets.Script.Framework.Node;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.UI
{
    /// <summary>
    /// 存读档UI管理器
    /// </summary>
    public class SaveLoadUIManager : MonoBehaviour
    {
        private GameManager gm;

        public SystemUIManager suim;
        public GameObject saveTable;
        public int saveSlotNum;

        private int saveID, groupnum = 0;
        private Dictionary<int, SavingInfo> savedic;
        private Dictionary<string, byte[]> savepic;

        private bool saveMode;
        private bool fromTitle;

        private void Awake()
        {
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            // 初始化存档栏位
            for (int i = 1; i <= saveSlotNum; i++)
            {
                //GameObject go = Resources.Load("Prefab/SaveBox") as GameObject;
                //go = Instantiate(go);
                //go.name = "SaveBox" + i;
                GameObject go = transform.Find("SaveSlot_Panel/SaveBox" + i).gameObject;
                go.transform.SetParent(saveTable.transform);
                var slbtn = go.GetComponent<SaveLoadButton>();
                slbtn.id = i;
                slbtn.uiManager = this;
                var cpbtn = go.transform.Find("Copy_Button").GetComponent<SaveCopyButton>();
                cpbtn.id = i;
                cpbtn.uiManager = this;
                var debtn = go.transform.Find("Delete_Button").GetComponent<SaveDeleteButton>();
                debtn.id = i;
                debtn.uiManager = this;
                
            }
        }

        private void OnEnable()
        {
            DataManager.GetInstance().BlockClick();
        }

        private void OnDisable()
        {
            DataManager.GetInstance().UnblockClick();
            //DataManager.GetInstance().UnblockBacklog();
        }

        public void SetSaveMode()
        {
            saveMode = true;
            FreshSaveTable();
        }

        public void SetLoadMode(bool ori)
        {
            fromTitle = ori;
            saveMode = false;
            FreshSaveTable();
        }


        public void SaveData()
        {
            string mode = DataManager.GetInstance().gameData.MODE;
            if (mode == "Avg模式" || mode == "侦探模式" || mode == "选项分歧" || mode == "特殊选择分歧")
            {
                gm.sm.SaveSoundInfo();
                gm.im.SaveImageInfo();
            }
            DataManager.GetInstance().Save(saveID);
            FreshSaveTable();
        }

        public void LoadData()
        {
            Debug.Log("load data"+saveID);
            DataManager.GetInstance().Load(saveID);
            gm.sm.StopBGM();
            //执行切换界面
            StartCoroutine(FadeOutAndLoad());
        }

        public void DeleteData()
        {
            DataManager.GetInstance().Delete(saveID);
            FreshSaveTable();
        }

        /// <summary>
        /// 【档位按钮】按下存档/读档
        /// </summary>
        /// <param name="x">存档栏位置</param>
        public void SelectSave(int x)
        {
            saveID = groupnum * saveSlotNum + x;
            if (saveMode)
            {
                //存档模式
                if (savedic.ContainsKey(saveID))
                {
                    //弹出警告
                    suim.OpenWarning(Constants.WarningMode.Save);
                }
                else
                {
                    SaveData();
                }
            }
            else
            {
                //读取模式
                if (savedic.ContainsKey(saveID))
                {
                    suim.OpenWarning(Constants.WarningMode.Load);
                }
            }

        }

        /// <summary>
        /// 【档位按钮】按下删除
        /// </summary>
        /// <param name="x">存档栏位置</param>
        public void SelectDelete(int x)
        {
            saveID = groupnum * saveSlotNum + x;
            if (savedic.ContainsKey(saveID))
            {
                suim.OpenWarning(Constants.WarningMode.Delete);
            }
        }

        /// <summary>
        /// 【档位按钮】按下复制
        /// </summary>
        /// <param name="x">存档栏位置</param>
        public void SelectCopy(int x)
        {

        }

        /// <summary>
        /// 【按钮】变更存档栏组号 1-9
        /// </summary>
        public void SetFileNum()
        {
            //if (!Toggle.current.value) return;
            GameObject go = transform.Find("NumTab_Panel").gameObject;
            for (int i = 0; i < go.transform.childCount; i++)
            {
                if (go.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    groupnum = i;
                    break;
                }
            }
            FreshSaveTable();
        }

        /// <summary>
        /// 刷新存档表格
        /// </summary>
        private void FreshSaveTable()
        {
            transform.Find("SL_Label").GetComponent<Text>().text = saveMode ? "SAVE" : "LOAD";
            savedic = DataManager.GetInstance().tempData.saveInfo;
            savepic = DataManager.GetInstance().GetTempVar<Dictionary<string, byte[]>>("存档缩略图");
            for (int i = 1; i <= saveSlotNum; i++)
            {
                int saveid = groupnum * saveSlotNum + i;
                GameObject go = saveTable.transform.Find("SaveBox" + i).gameObject;
                //检查 存档位 是否为空
                if (savedic.ContainsKey(saveid) && savedic[saveid] != null)
                {
                    //若为非空 则开启按钮
                    go.transform.GetComponent<SaveLoadButton>().enabled = true;
                    go.transform.Find("No_Label").GetComponent<Text>().text = "NO." + saveid.ToString("D2");
                    go.transform.Find("Info_Label").GetComponent<Text>().text = savedic[saveid].saveText;
                    go.transform.Find("Time_Label").GetComponent<Text>().text = savedic[saveid].saveTime;
                    Texture2D texture = new Texture2D(540, 303);
                    if (savepic.ContainsKey(savedic[saveid].picPath))
                    {
                        texture.LoadImage(savepic[savedic[saveid].picPath]);
                        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                        go.transform.Find("Save_Sprite").GetComponent<Image>().sprite = sp;
                    }
                    go.transform.Find("Copy_Button").GetComponent<BasicButton>().enabled = true;
                    go.transform.Find("Delete_Button").GetComponent<BasicButton>().enabled = true;
                }
                else
                {
                    //若为空 仅存档可以开按钮
                    go.transform.GetComponent<SaveLoadButton>().enabled = saveMode;
                    go.transform.Find("No_Label").GetComponent<Text>().text = "NO." + saveid.ToString("D2");
                    go.transform.Find("Info_Label").GetComponent<Text>().text = "";
                    go.transform.Find("Time_Label").GetComponent<Text>().text = "";
                    if (saveMode)
                    {
                        go.transform.Find("Save_Sprite").GetComponent<Image>().sprite = Resources.Load<Sprite>("Background/Title");
                    }
                    else
                    {
                        go.transform.Find("Save_Sprite").GetComponent<Image>().sprite = Resources.Load<Sprite>("Background/Title");
                    }
                    go.transform.Find("Copy_Button").GetComponent<BasicButton>().enabled = false;
                    go.transform.Find("Delete_Button").GetComponent<BasicButton>().enabled = false;
                }
            }
        }

        /// <summary>
        /// 修改存档标签
        /// </summary>
        /// <param name="i">存档栏位置1-saveSlotNum</param>
        public void ChangeSaveInfo(int i)
        {
            Debug.Log(i);
            int saveid = groupnum * saveSlotNum + i;
            GameObject go = saveTable.transform.Find("SaveBox" + i).gameObject;
            string str = go.transform.Find("Info_Label").GetComponent<Text>().text;
            savedic[saveid].saveText = str;
            DataManager.GetInstance().ChangeSave();
            FreshSaveTable();
        }


        private IEnumerator FadeOutAndLoad(float time = 0.5f)
        {
            CanvasGroup panel = suim.transform.GetComponent<CanvasGroup>();
            float x = 1;
            while (x > 0)
            {
                x = Mathf.MoveTowards(x, 0, 1 / time * Time.deltaTime);
                panel.alpha = x;
                yield return null;
            }
            //关闭读档界面
            this.gameObject.SetActive(false);
            //关闭警告框
            suim.warningContainer.SetActive(false);
            //关闭上级的SystemUI
            suim.gameObject.SetActive(false);
            //Node复原
            string modeName = DataManager.GetInstance().gameData.MODE;
            Debug.Log("读档前往：" + modeName);
            switch (modeName)
            {
                case "大地图模式":
                    //gm.node = NodeFactory.GetInstance().GetMapNode();
                    break;
                case "养成模式":
                    //gm.node = NodeFactory.GetInstance().GetEduNode();
                    break;
                case "侦探模式":
                    //string currentDetect = DataManager.GetInstance().inturnData.currentDetectEvent;
                    ////界面复原
                    //gm.im.LoadImageInfo();
                    //gm.sm.LoadSoundInfo();
                    //gm.node = NodeFactory.GetInstance().GetDetectJudgeNode(currentDetect);
                    break;
                case "Avg模式":
                    string textName = DataManager.GetInstance().gameData.currentScript;
                    //界面复原
                    gm.im.LoadImageInfo();
                    gm.sm.LoadSoundInfo();
                    gm.node = NodeFactory.GetInstance().FindTextScriptNoneInit(textName);
                    break;
                case "选择分歧":
                    string selectID = DataManager.GetInstance().gameData.selectID;
                    //界面复原
                    gm.im.LoadImageInfo();
                    gm.sm.LoadSoundInfo();
                    gm.node = NodeFactory.GetInstance().GetSelectNode(selectID);
                    break;
            }
        }

    }
}