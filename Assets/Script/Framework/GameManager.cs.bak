using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

//using Assets.Script.Framework.Effect;
using Assets.Script.Framework.Data;
using Assets.Script.Framework.Node;
using Assets.Script.Framework.UI;

using UnityEngine;

namespace Assets.Script.Framework
{
    /// <summary>
    /// GameManager: 
    /// 整个游戏只允许一个，且不能由于场景切换而被删除
    /// 游戏的控制核心，逻辑控制
    /// 与其他Manager交互
    /// 储存主要游戏数据，提供其他Manager访问
    /// 提供方法供其他Manager使用，同时控制其他Manager
    /// 从而实现游戏进程的推进
    /// TODO: 存储读档机制
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        // win32
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);
        //private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        //设置此窗体为活动窗体
        //[DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        //public static extern bool SetForegroundWindow(IntPtr hWnd);
        private IntPtr handler;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        //const int GWL_STYLE = -16;
        //const long WS_POPUP = 0x80000000L;
        //const int WS_BORDER = 1;
        const uint SWP_SHOWWINDOW = 0x0040;



        // UI root
        private GameObject root;

        /// <summary>
        /// PanelSwitch，控制面板切换
        /// </summary>
        public PanelSwitch ps;

        /// <summary>
        /// ImageManager, 控制图像处理
        /// </summary>
        public ImageManager im;

        /// <summary>
        /// SoundManager，控制音效
        /// </summary>
        public SoundManager sm;

        /// <summary>
        /// 事件管理器
        /// </summary>
        //public EventManager em;

        /// <summary>
        /// 创建Node的工厂
        /// </summary>
        public NodeFactory nodeFactory;

        /// <summary>
        /// 当前游戏节点
        /// </summary>
        public GameNode node;

        /// <summary>
        /// 数据管理
        /// </summary>
        public DataManager dm;

        //test for the init node
        private bool startNewGame = false;

        public string version = "UnityGalgameFrame v0.1";
        public string entryNode = "demo0_0";

        void Awake()
        {
            //初始化整个游戏系统
            InitSystem();
        }

        void Update()
        {
            //右键事件绑定 Esc 充当右键功能
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                if (dm.isEffecting || dm.IsRightClickBlocked())
                {
                    return;
                }
                else
                {
                    ps.RightClick();
                }

            }

            //滚轮作用绑定事件
            if (!dm.IsWheelBlocked())
            {
                float wheeloffset = Input.GetAxis("Mouse ScrollWheel");
                if (wheeloffset > 0)
                {
                    //Debug.Log("Up " + wheeloffset);
                    ps.MouseUpScroll(wheeloffset);
                }
                else if (wheeloffset < 0)
                {
                    //Debug.Log("Down " + wheeloffset);
                    ps.MouseDownScroll(wheeloffset);
                }
            }

            if (node == null)
            {
                //Debug.LogError("Game End, node null");
                // 游戏结束返回标题画面
                //ps.SwitchTo_VerifyIterative("Title_Panel");
            }
            else if (node.end)
            {
                SwitchNode();
            }
        }

        private void SwitchNode()
        {
            //转换前节点名
            string[] str = node.GetType().ToString().Split('.');
            string output1 = str[str.Length - 1];

            node = node.NextNode();
            //转换后节点名
            str = node.GetType().ToString().Split('.');
            string output2 = str[str.Length - 1];
            Debug.Log("转换节点：由" + output1 + "至" + output2);
        }

        public void ReturnTitle()
        {
            sm.StopBGM();
            sm.StopSE();
            sm.StopVoice();
            ps.SwitchTo_VerifyIterative("Title_Panel");
        }

        /// <summary>
        /// 新游戏入口
        /// </summary>
        public void NewGame()
        {
            sm.StopBGM();
            //重置gameVar
            dm.InitGame();
            node = nodeFactory.FindTextScript(entryNode);
            //清空原先的文字记录
            dm.ClearHistory();
        }

        /// <summary>
        /// 继续游戏入口
        /// </summary>
        public void Continue()
        {
            Dictionary<int, SavingInfo> savedic = DataManager.GetInstance().tempData.saveInfo;
            if (savedic.ContainsKey(0) && savedic[0] != null)
            {
                Debug.Log("load data 0");
                DataManager.GetInstance().Load(0);
                sm.StopBGM();
                //执行切换界面
                string textName = DataManager.GetInstance().gameData.currentScript;
                //界面复原
                im.LoadImageInfo();
                sm.LoadSoundInfo();
                node = nodeFactory.FindTextScriptNoneInit(textName);
            }
        }

        /// <summary>
        /// 快速存档
        /// </summary>
        public void QuickSave()
        {
            DataManager.GetInstance().Save(-1);
        }

        /// <summary>
        /// 快速读档
        /// </summary>
        public void QuickLoad()
        {
            DataManager.GetInstance().Load(-1);
        }

        public GameNode GetCurrentNode()
        {
            return node;
        }

        /// <summary>
        /// 初始化系统
        /// </summary>
        private void InitSystem()
        {
            root = GameObject.Find("UI Root");
            //ps初始化
            if (ps == null)
            {
                ps = transform.GetComponent<PanelSwitch>();
                Debug.Log("PanelSwitch 再获取");
            }
            ps.Init();
            //im初始化
            if (im == null)
            {
                im = transform.GetComponent<ImageManager>();
                Debug.Log("ImageManager 再获取");
            }
            //dm初始化
            dm = DataManager.GetInstance();
            dm.version = this.version;
            //em初始化
            // em = EventManager.GetInstance();
            //eb初始化
            //NewEffectBuilder.Init(im, sm); //, CharacterManager.GetInstance()
            //nf初始化
            nodeFactory = NodeFactory.GetInstance();
            nodeFactory.Init(dm, root, ps);
        }

        public void SetTopMostWindow(bool topped)
        {
            string name = Application.productName;
            handler = FindWindow(null, name);
            if (handler == IntPtr.Zero)
            {
                return;
            }
            Debug.Log(String.Format("{0} {1}", handler, topped ? "to top" : "to notop"));
            RECT rect1;
            GetWindowRect(new HandleRef(this, handler), out rect1);
            bool result = SetWindowPos(handler, topped ? -2 : -1,
                (int)rect1.left, (int)rect1.top,
                (int)(rect1.right - rect1.left), (int)(rect1.bottom - rect1.top),
                SWP_SHOWWINDOW);
        }

        void OnApplicationQuit()
        {
            //DataManager.GetInstance().Save(0);
            DataManager.GetInstance().SaveMultiData();
            DataManager.GetInstance().SaveConfigData();
            Debug.Log("Application ending after " + Time.time + " seconds");
        }
    }
}