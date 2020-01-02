using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using Assets.Script.Framework.Data;
using Assets.Script.Framework.Node;

using UnityEngine;

//using Mono.Data.Sqlite;
//using LitJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


/// <summary>
/// 游戏数据管理
/// </summary>
namespace Assets.Script.Framework
{
    public class DataManager
    {
        private static DataManager instance = new DataManager();

        public static readonly DateTime START_DAY = new DateTime(2014, 8, 31);

        public static DataManager GetInstance()
        {
            //if (instance == null)
            //{
            //    Debug.Log("new datamanager");
            //    instance = new DataManager();
            //}
            return instance;
        }

        //各类数据管理器
        public GameData gameData = new GameData();
        //public InTurnData inturnData = new InTurnData();
        //public StaticData staticData = new StaticData();
        public ConfigData configData = new ConfigData();
        public MultiData multiData = new MultiData();
        public TempData tempData = new TempData();

        /// <summary>
        /// 是否自动模式
        /// </summary>
        public bool isAuto = false;

        /// <summary>
        /// 是否正在进行特效
        /// </summary>
        public bool isEffecting = false;

        /// <summary>
        /// 禁用右键菜单(询问/地点转换)
        /// </summary>
        private bool blockRightClick = false;

        /// <summary>
        /// 禁用滚轮
        /// </summary>
        private bool blockWheel = false;

        /// <summary>
        /// 禁用快进
        /// </summary>
        private bool blockClick = false;

        /// <summary>
        /// 锁定文字履历，不添加新文字 (菜单/询问)
        /// </summary>
        private bool blockBacklog = false;

        /// <summary>
        /// 禁用存读档(询问)
        /// </summary>
        private bool blockSaveLoad = false;

        /// <summary>
        /// 当前游戏版本
        /// </summary>
        public string version;

        public DataManager()
        {
            Init();
        }

        public void Init()
        {
            InitTemp();
            InitStatic();
            InitSystem();
            InitMultiplay();
            InitSaving();
            InitGame();
        }

        #region 游戏数据类初始化
        /// <summary>
        /// 游戏临时数据的清空
        /// </summary>
        private void InitTemp()
        {
            tempData.backLog = new Queue<BacklogText>();
        }

        /// <summary>
        /// 游戏数据初始化 跟随存档
        /// </summary>
        public void InitGame()
        {
            gameData.MODE = string.Empty;
            gameData.currentScript = string.Empty;
            gameData.currentTextPos = 0;
            gameData.bgSprites = new Dictionary<int, SpriteState>();
            gameData.fgSprites = new Dictionary<int, SpriteState>();
            //选过的选项清空
            gameData.selectionSwitch = new List<string>();
        }
        #endregion
        
        #region 游戏设置 ConfigData 初始化
        private void InitSystem()
        {
            string filename = "config.sav";
            string savepath = SaveLoadTool.GetSavePath(filename);
            if (!SaveLoadTool.IsFileExists(savepath))
            {
                //若不存在则创建默认数据
                Debug.Log("setting date donot exist");
                ResetSysConfig();
                string toSave = JsonConvert.SerializeObject(configData);
                SaveLoadTool.SaveFile(savepath, toSave);
            }
            else
            {
                try
                {
                    //读取config
                    Debug.Log("Loading config file");
                    string toLoad = SaveLoadTool.LoadFile(savepath);
                    configData = JsonConvert.DeserializeObject<ConfigData>(toLoad);
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                    Debug.Log("存档文件不符，已重置");
                    //出错则采用默认 并覆盖原文件
                    ResetSysConfig();
                    string toSave = JsonConvert.SerializeObject(configData);
                    SaveLoadTool.SaveFile(savepath, toSave);
                }
                
            }
        }

        public void ResetSysConfig()
        {
            //游戏的默认设置
            configData = new ConfigData();
        }

        #endregion

        #region 存档类初始化
        /// <summary>
        /// 初始化存档
        /// </summary>
        private void InitSaving()
        {
            Dictionary<int, SavingInfo> list = new Dictionary<int, SavingInfo>();
            string filename = "datasv.sav";
            string savepath = SaveLoadTool.GetSavePath(filename);
            if (SaveLoadTool.IsFileExists(savepath))
            {
                //读取存档列表
                Debug.Log("Loading save file");
                string toLoad = SaveLoadTool.LoadFile(savepath);
                list = JsonConvert.DeserializeObject<Dictionary<int, SavingInfo>>(toLoad);

                //预处理玩家存档
                foreach(int i in list.Keys)
                {
                    string save = "data" + i + ".sav";
                    try{
                        string str = SaveLoadTool.LoadFile(SaveLoadTool.GetSavePath(save));
                        gameData = JsonConvert.DeserializeObject<GameData>(str);
                    }
                    catch
                    {
                        string emsg = string.Format("存档文件 {0} 损坏", filename);
                        Debug.LogError(emsg);
                        return;
                    }
                }
            }
            tempData.saveInfo = list;
            RefreshSavePic();
        }

        /// <summary>
        /// 读取存档的图片并写入Data
        /// </summary>
        public void RefreshSavePic()
        {
            Dictionary<int, SavingInfo> list = tempData.saveInfo;

            Dictionary<string, byte[]> savepic = new Dictionary<string, byte[]>();
            foreach (KeyValuePair<int, SavingInfo> kv in list)
            {
                if (kv.Key == 0) continue;
                string picname = "data" + kv.Key + ".png";
                string picpath = SaveLoadTool.GetSavePath(picname);
                FileStream fs = new FileStream(picpath, FileMode.Open, FileAccess.Read);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, (int)fs.Length);
                fs.Close();
                savepic.Add(kv.Value.picPath, bytes);
            }
            tempData.WriteTempVar("存档缩略图", savepic);
        }
        #endregion

        #region 多周目数据初始化
        /// <summary>
        /// 初始化多周目数据
        /// </summary>
        private void InitMultiplay()
        {
            string filename = "datamp.sav";
            string savepath = SaveLoadTool.GetSavePath(filename);

            //判断是否含有datamp文件
            if (!SaveLoadTool.IsFileExists(savepath))
            {
                //若不存在 则生成默认数据表 
                DefaultMultiData();
                //并写入本地文件
                string toSave = JsonConvert.SerializeObject(multiData);
                SaveLoadTool.SaveFile(savepath, toSave);
            }
            else
            {
                try
                {
                    //若文件存在 则读取二周目数据
                    string toLoad = SaveLoadTool.LoadFile(savepath);
                    multiData = JsonConvert.DeserializeObject<MultiData>(toLoad);
                }
                catch
                {
                    Debug.LogError("存档文件不符，已重置");
                    //出差错则覆盖本地文件
                    DefaultMultiData();
                    string toSave = JsonConvert.SerializeObject(multiData);
                    SaveLoadTool.SaveFile(savepath, toSave);
                }
                
            }

        }

        /// <summary>
        /// 生成默认的多周目数据
        /// </summary>
        private void DefaultMultiData()
        {
            multiData = new MultiData();

            //foreach (KeyValuePair<int, string> kv in staticData.cgInfo)
            //{
            //    multiData.cgTable.Add(kv.Key, false);
            //}

            //multiData.cgTable[0] = true;
            //multiData.cgTable[1] = true;
            //multiData.cgTable[2] = true;
            //multiData.cgTable[3] = true;
            //multiData.endingTable[0] = true;
            //multiData.endingTable[1] = true;
            //multiData.endingTable[2] = false;
            //multiData.endingTable[3] = false;
            //multiData.musicTable[0] = true;
            //multiData.musicTable[1] = true;
        }

        #endregion

        #region 游戏静态数据初始化
        private void InitStatic()
        {
            //TODO:静态表格 例如cginfo 记录键值与文件全路径
            Dictionary<int, string> cgInfo = new Dictionary<int, string>();
            cgInfo.Add(0, "Background/sky_day");
            cgInfo.Add(1, "Background/classroom");
            cgInfo.Add(2, "Background/corridor");
            cgInfo.Add(3, "Background/gate");
            cgInfo.Add(4, "Background/sky_evening");
            //staticData.cgInfo = cgInfo;
        }


        //public Dictionary<string, string> GetBGMTitle()
        //{
        //    Sqlite sql = new Sqlite("liantui.db");
        //    SqliteDataReader dataReader = sql.SelectDataBase("BgmList");
        //    Dictionary<string, string> bgmTitle = new Dictionary<string, string>();
        //    while (dataReader.Read())
        //    {
        //        //读取ID
        //        int id = dataReader.GetInt32(dataReader.GetOrdinal("ID"));
        //        //读取Name
        //        string filename = dataReader.GetString(dataReader.GetOrdinal("FileName"));
        //        string title = dataReader.GetString(dataReader.GetOrdinal("Title"));
        //        bgmTitle.Add(filename, title);
        //    }
        //    sql.CloseDataBase();
        //    return bgmTitle;
        //}

        #endregion

        #region Block 函数
        /// <summary>
        /// 禁用右键功能
        /// </summary>
        public void BlockRightClick()
        {
            blockRightClick = true;
        }
        /// <summary>
        /// 禁用左键点击功能（含滚轮）
        /// </summary>
        public void BlockClick()
        {
            blockClick = true;
        }
        /// <summary>
        /// 禁用鼠标滚轮
        /// </summary>
        public void BlockWheel()
        {
            blockWheel = true;
        }
        /// <summary>
        /// 锁定文字履历（不添加新条目）
        /// </summary>
        public void BlockBacklog()
        {
            blockBacklog = true;
        }
        /// <summary>
        /// 禁用存读档
        /// </summary>
        public void BlockSaveLoad()
        {
            blockSaveLoad = true;
        }
        /// <summary>
        /// 解锁右键功能
        /// </summary>
        public void UnblockRightClick()
        {
            blockRightClick = false;
        }
        /// <summary>
        /// 解锁左键点击
        /// </summary>
        public void UnblockClick()
        {
            blockClick = false;
        }
        /// <summary>
        /// 解锁鼠标滚轮
        /// </summary>
        public void UnblockWheel()
        {
            blockWheel = false;
        }
        /// <summary>
        /// 解锁文字履历
        /// </summary>
        public void UnblockBacklog()
        {
            blockBacklog = false;
        }
        /// <summary>
        /// 解锁存读档
        /// </summary>
        public void UnblockSaveLoad()
        {
            blockSaveLoad = false;
        }

        public bool IsClickBlocked()
        {
            return blockClick;
        }
        public bool IsRightClickBlocked()
        {
            return blockRightClick;
        }
        public bool IsBacklogBlocked()
        {
            return blockBacklog;
        }
        public bool IsSaveLoadBlocked()
        {
            return blockSaveLoad;
        }
        public bool IsWheelBlocked()
        {
            return blockWheel;
        }
        #endregion
        
        #region public 方法
        /// <summary>
        /// 清空文字记录
        /// </summary>
        public void ClearHistory()
        {
            tempData.backLog.Clear();
        }

        /// <summary>
        /// 添加文字记录
        /// </summary>
        /// <param name="blt">文本记录块</param>
        public void AddHistory(BacklogText blt)
        {
            Queue<BacklogText> history = tempData.backLog;
            if (history.Count > 100)
            {
                history.Dequeue();
            }
            history.Enqueue(blt);
        }

        public Queue<BacklogText> GetHistory()
        {
            return tempData.backLog;
        }

        /// <summary>
        /// 判定是否已经读过文本
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public bool IsTextRead()
        {
            string script = gameData.currentScript;
            int id = gameData.currentTextPos;
            Dictionary<string, int> dic = multiData.scriptTable;
            if (dic.ContainsKey(script))
            {
                return dic[script] > id;
            }
            return false;
        }


        #endregion

        #region Get / Set 方法
        public void SetTempVar(string key, object value)
        {
            tempData.WriteTempVar(key, value);
        }

        public T GetTempVar<T>(string key)
        {
            return (T)tempData.GetTempVar(key);
        }

        /* demo 1.22 去除
        public void SetGameVar(string key, object value)
        {
            datapool.WriteGameVar(key, value);
        }

        public void SetInTurnVar(string key, object value)
        {
            datapool.WriteInTurnVar(key, value);
        }

        public void SetSystemVar(string key, object value)
        {
            datapool.WriteSystemVar(key, value);
        }
        public T GetGameVar<T>(string key)
        {
            return (T)datapool.GetGameVar(key);
        }

        public T GetInTurnVar<T>(string key)
        {
            return (T)datapool.GetInTurnVar(key);
        }

        public T GetSystemVar<T>(string key)
        {
            return (T)datapool.GetSystemVar(key);
        }

        public bool ContainsGameVar(string key) { return datapool.GetGameVarTable().ContainsKey(key); }

        public bool ContainsInTurnVar(string key) { return datapool.GetInTurnVarTable().ContainsKey(key); }

        DataPool GetDataPool() { return datapool; }

        public Hashtable GetGameVars()
        {
            return datapool.GetGameVarTable();
        }
        public Hashtable GetInTurnVars()
        {
            return datapool.GetInTurnVarTable();
        }
        public Hashtable GetSystemVars()
        {
            return datapool.GetSystemTable();
        }
        */
        #endregion

        #region 存读档用
        public void Save(int i)
        {
            string toSave = DataToJsonString();
            string filename = "data" + i + ".sav";
            SaveLoadTool.SaveFile(SaveLoadTool.GetSavePath(filename), toSave);

            //更新存档信息
            Dictionary<int, SavingInfo> savedic = tempData.saveInfo;
            //TODO: 获取状态
            SavingInfo info = new SavingInfo();
            info.gameMode = gameData.MODE;
            info.saveTime = DateTime.Now.ToString("HH:mm");
            info.saveDate = DateTime.Now.ToString("yyyy/MM/dd");
            info.saveText = "存档了！";
            info.currentName = tempData.currentName;
            info.currentText = tempData.currentText;
            if (i > 0)
            {
                //储存截图
                string picname = "data" + i + ".png";
                byte[] picdata = (byte[])tempData.GetTempVar("缩略图");
                SaveLoadTool.CreatByteFile(SaveLoadTool.GetSavePath(picname), picdata);
                info.picPath = picname;
            }
            //SavingInfo info = new SavingInfo(gamemode, savetime, customtext, picname);
            if (savedic.ContainsKey(i))
            {
                savedic[i] = info;
            }
            else
            {
                savedic.Add(i, info);
            }
            //写入系统存档
            string sysSave = JsonConvert.SerializeObject(savedic);
            SaveLoadTool.SaveFile(SaveLoadTool.GetSavePath("datasv.sav"), sysSave);
            RefreshSavePic();
        }

        /// <summary>
        /// 储存配置
        /// </summary>
        public void SaveConfigData()
        {
            string filename = "config.sav";
            string savepath = SaveLoadTool.GetSavePath(filename);
            string toSave = JsonConvert.SerializeObject(configData);
            SaveLoadTool.SaveFile(savepath, toSave);
        }

        /// <summary>
        /// 储存多周目数据
        /// </summary>
        public void SaveMultiData()
        {
            string filename = "datamp.sav";
            string savepath = SaveLoadTool.GetSavePath(filename);
            //并写入本地文件
            string toSave = JsonConvert.SerializeObject(multiData);
            SaveLoadTool.SaveFile(savepath, toSave);
        }

        private string DataToJsonString()
        {
            //储存内容序列化
            return JsonConvert.SerializeObject(gameData);
        }

        /// <summary>
        /// 读取特定编号存档
        /// </summary>
        /// <param name="i"></param>
        public void Load(int i)
        {
            string filename = "data" + i + ".sav";
            string toLoad = SaveLoadTool.LoadFile(SaveLoadTool.GetSavePath(filename));
            LoadDataFromJson(toLoad);
        }

        private void LoadDataFromJson(string str)
        {
            gameData = JsonConvert.DeserializeObject<GameData>(str);
            //对临时变量重置？
            ClearHistory();
            // event 重置
            UnblockClick();
            UnblockRightClick();
            UnblockWheel();
            //UnblockSaveLoad();
            //UnblockBacklog();
        }

        public void ChangeSave()
        {
            string sysSave = JsonConvert.SerializeObject(tempData.saveInfo);
            SaveLoadTool.SaveFile(SaveLoadTool.GetSavePath("datasv.sav"), sysSave);
            RefreshSavePic();
        }

        public void Delete(int i)
        {
            //删除存档
            string filename = "data" + i + ".sav";
            SaveLoadTool.DeleteFile(SaveLoadTool.GetSavePath(filename));
            //删除截图
            string picname = "data" + i + ".png";
            SaveLoadTool.DeleteFile(SaveLoadTool.GetSavePath(picname));
            //更新存档信息
            Dictionary<int, SavingInfo> savedic = tempData.saveInfo;

            if (savedic.ContainsKey(i))
            {
                savedic.Remove(i);
            }
            //写入系统存档
            string sysSave = JsonConvert.SerializeObject(savedic);
            SaveLoadTool.SaveFile(SaveLoadTool.GetSavePath("datasv.sav"), sysSave);
            RefreshSavePic();
        }
        #endregion

    }
}   
