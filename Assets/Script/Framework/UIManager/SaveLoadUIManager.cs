using System.Collections;
using System.Collections.Generic;

using Assets.Script.Framework.Data;
using Assets.Script.Framework.Node;

using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using Assets.Script.Framework.Effect;

namespace Assets.Script.Framework.UI
{
    /// <summary>
    /// 存读档UI管理器
    /// </summary>
    public class SaveLoadUIManager : MonoBehaviour
    {
        private GameManager gm;

        public SystemUIManager suim;
        public GameObject saveTable, numTable;
        public int saveSlotNum;

        public Vector2 slotSize;
        public Vector2 slotOffset;
        public Vector2 slotPos;

        private int saveID, saveSlot, groupnum = 0;
        private Dictionary<int, SavingInfo> savedic;
        private Dictionary<string, byte[]> savepic;

        private bool saveMode;
        private bool fromTitle;
        private bool copyMode;

        private void Awake()
        {
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            // 初始化存档栏位
            for (int i = 1; i <= saveSlotNum; i++)
            {
                //GameObject go = Resources.Load("Prefab/SaveBox") as GameObject;
                //go = Instantiate(go);
                //go.transform.SetParent(saveTable.transform,false);
                //go.name = "SaveBox" + i;
                GameObject go = transform.Find("SaveSlot_Panel/SaveBox" + i).gameObject;
                var slbtn = go.GetComponent<SaveLoadButton>();
                slbtn.id = i;
                slbtn.uiManager = this;
                var cpbtn = go.transform.Find("Copy_Button").GetComponent<SaveCopyButton>();
                cpbtn.id = i;
                cpbtn.uiManager = this;
                var debtn = go.transform.Find("Delete_Button").GetComponent<SaveDeleteButton>();
                debtn.id = i;
                debtn.uiManager = this;
                // 随机变更图像
                //ChangeImage(i, go);
                ChangeColor(go);
            }
        }

        private void OnEnable()
        {
            DataManager.GetInstance().BlockClick();
            InitSaveSlotPos();
            Open();
        }

        private void OnDisable()
        {
            DataManager.GetInstance().UnblockClick();
            //DataManager.GetInstance().UnblockBacklog();
        }

        private void Open()
        {
            AnimationCurve ac = AniCurveManager.GetInstance().Curve84;
            // 淡入数字行
            numTable.GetComponent<CanvasGroup>().DOFade(1,0.66f);
            // 淡入存档栏
            for(int i = 0; i < saveSlotNum; i++)
            {
                int t = i + 1;
                GameObject go = saveTable.transform.Find("SaveBox" + t).gameObject;
                int raw = i / 2;
                int col = i % 2;
                go.GetComponent<CanvasGroup>()
                    .DOFade(1, 0.5f)
                    .SetEase(ac)
                    .SetDelay(0.03f * col + 0.02f * raw);
                go.GetComponent<RectTransform>()
                    .DOAnchorPosY(slotPos.y - (slotSize.y+slotOffset.y) * raw, 0.5f)
                    .SetEase(ac)
                    .SetDelay(0.03f * col + 0.02f * raw);
            }
        }

        private void ChangeImage(int i, GameObject go)
        {
            int[] fn = new int[6] { 0, 1, 2, 0, 1, 2 };
            string[] sn = new string[3] { "_r", "_g", "" };
            Image im = go.GetComponent<Image>();
            fn[i] = Random.Range(0, 2);
            if (i > 0)
            {
                while (fn[i] == fn[i - 1])
                {
                    fn[i] = Random.Range(0, 3);
                }
            }
            string fname = "UI/save_info" + sn[fn[i]];
            Debug.Log(fname);
            im.sprite = Resources.Load<Sprite>(fname);
        }

        private void ChangeColor(GameObject go)
        {
            Image im = go.GetComponent<Image>();
            im.color = Random.ColorHSV();
        }

        /// <summary>
        /// 初始化存档位置
        /// </summary>
        private void InitSaveSlotPos()
        {
            for (int i = 0; i < saveSlotNum; i++)
            {
                int t = i + 1;
                GameObject go = saveTable.transform.Find("SaveBox" + t).gameObject;
                int raw = i / 2;
                int col = i % 2;
                float x = slotPos.x + (slotSize.x + slotOffset.x) * col;
                float y = slotPos.y - (slotSize.y + slotOffset.y) * raw + 200;
                go.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                go.GetComponent<CanvasGroup>().alpha = 0;
            }
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
                DataManager.GetInstance().gameData.fgSprites = gm.im.SaveImageInfo();
            }
            // 储存
            DataManager.GetInstance().Save(saveID);
            // 更新单一动画
            savedic = DataManager.GetInstance().tempData.saveInfo;
            GameObject go = saveTable.transform.Find("SaveBox" + saveSlot).gameObject;
            ChangeSaveSlop(go, saveMode, saveID, savedic[saveID]);
            AnimationCurve ac = AniCurveManager.GetInstance().Curve84;
            Sequence twq = DOTween.Sequence();
            twq.Pause();
            twq.Append(go.transform.Find("Save_Sprite/Thumb_Sprite").GetComponent<Image>()
                    .DOFade(1, 0.5f)
                    .SetEase(ac))
                .Join(go.transform.Find("Info_Canvas").GetComponent<CanvasGroup>()
                    .DOFade(1,0.5f)
                    .SetEase(ac));
            twq.Play();
            //FreshSaveTable();
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
            //Debug.Log("Delete: " + saveID + " " + saveSlot);
            DataManager.GetInstance().Delete(saveID);
            // 播放单独动画
            savedic = DataManager.GetInstance().tempData.saveInfo;
            GameObject go = saveTable.transform.Find("SaveBox" + saveSlot).gameObject;
            AnimationCurve ac = AniCurveManager.GetInstance().Curve84;
            Sequence twq = DOTween.Sequence();
            twq.Pause();
            twq.Append(go.transform.Find("Save_Sprite/Thumb_Sprite").GetComponent<Image>()
                    .DOFade(0, 0.5f)
                    .SetEase(ac))
                .Join(go.transform.Find("Info_Canvas").GetComponent<CanvasGroup>()
                    .DOFade(0, 0.5f)
                    .SetEase(ac));
            twq.Play().OnComplete(
                () => { ChangeSaveSlop(go, saveMode, saveID, savedic[saveID]); }
            );
            //FreshSaveTable();
        }

        public void HoverSave(int x,bool ishover)
        {
            saveSlot = x;
            GameObject go = saveTable.transform.Find("SaveBox" + saveSlot).gameObject;
            AnimationCurve ac = AniCurveManager.GetInstance().Curve84;
            go.transform.Find("Image").GetComponent<Outline>()
                    .DOFade(ishover ? 1 : 0, 0.5f)
                    .SetEase(ac);
        }

        /// <summary>
        /// 【档位按钮】按下存档/读档
        /// </summary>
        /// <param name="x">存档栏位置</param>
        public void SelectSave(int x)
        {
            saveSlot = x;
            saveID = groupnum * saveSlotNum + x;
            if (copyMode)
            {
                // 复制模式
                if (savedic.ContainsKey(saveID))
                {
                    //弹出警告
                    suim.OpenWarning(Constants.WarningMode.Copy);
                }
                else
                {
                    SaveData();
                }
            }
            else if (saveMode)
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
            saveSlot = x;
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
            copyMode = !copyMode;
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
                    ChangeSaveSlop(go, saveMode, saveid, savedic[saveid]);
                    go.transform.Find("Save_Sprite/Thumb_Sprite").GetComponent<Image>().color = new Color(1, 1, 1, 1);
                }
                else
                {
                    //若为空 仅存档可以开按钮
                    ChangeSaveSlop(go, saveMode, saveid, new SavingInfo(), true);
                }
            }
        }

        private void ChangeSaveSlop(GameObject go, bool isSave, int id, SavingInfo si, bool isEmpty = false)
        {
            savepic = DataManager.GetInstance().GetTempVar<Dictionary<string, byte[]>>("存档缩略图");
            go.transform.GetComponent<SaveLoadButton>().enabled = isSave || !isEmpty;
            go.transform.Find("No_Label").GetComponent<Text>().text = id.ToString("D2");
            //go.transform.Find("Name_Label").GetComponent<Text>().text = si.currentName;
            go.transform.Find("Info_Label").GetComponent<Text>().text = si.currentText;
            //go.transform.Find("Date_Label").GetComponent<Text>().text = si.saveDate;
            go.transform.Find("Time_Label").GetComponent<Text>().text = si.saveDate + si.saveTime;
            //go.transform.Find("Info_Canvas").GetComponent<CanvasGroup>().alpha = 1;
            if (savepic.ContainsKey(si.picPath))
            {
                Texture2D texture = new Texture2D(342, 192);
                texture.LoadImage(savepic[si.picPath]);
                Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                go.transform.Find("Save_Sprite").GetComponent<Image>().sprite = sp;
            }
            else
            {
                go.transform.Find("Save_Sprite").GetComponent<Image>().sprite = null;
            }
            go.transform.Find("Save_Sprite").GetComponent<Image>().color = new Color(1, 1, 1, 0);
            go.transform.Find("Copy_Button").gameObject.SetActive(!isEmpty);
            go.transform.Find("Delete_Button").gameObject.SetActive(!isEmpty);
            //go.transform.Find("Info_Sprite/Delete_Button").GetComponent<SaveDeleteButton>().enabled = isEmpty;
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
                case "选择分歧":
                    string selectID = DataManager.GetInstance().gameData.selectID;
                    //界面复原
                    gm.im.LoadImageInfo();
                    gm.sm.LoadSoundInfo();
                    gm.node = NodeFactory.GetInstance().GetSelectNode(selectID);
                    break;
                case "Avg模式":
                    //string textName = DataManager.GetInstance().gameData.currentScript;
                    ////界面复原
                    //gm.im.LoadImageInfo();
                    //gm.sm.LoadSoundInfo();
                    //gm.node = NodeFactory.GetInstance().FindTextScriptNoneInit(textName);
                    //break;
                default:
                    string textName = DataManager.GetInstance().gameData.currentScript;
                    //界面复原
                    gm.im.LoadImageInfo();
                    gm.sm.LoadSoundInfo();
                    gm.node = NodeFactory.GetInstance().FindTextScriptNoneInit(textName);
                    break;
            }
        }

    }
}