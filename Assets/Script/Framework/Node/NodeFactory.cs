using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Script.Framework.UI;

using UnityEngine;

namespace Assets.Script.Framework.Node
{
    /// <summary>
    /// 游戏节点生成器
    /// </summary>
    public class NodeFactory
    {
        private static NodeFactory instance = new NodeFactory();

        private DataManager dm;
        private GameObject root;
        private PanelSwitch ps;
        //private AvgPanelSwitch avgPanelSwitch;
        private static readonly string SCRIPT_PATH = "Assets.Script.Scenario";
        private NodeFactory() { }

        public static NodeFactory GetInstance()
        {
            //if (instance == null) { instance = new NodeFactory(); }
            return instance;
        }

        public void Init(DataManager manager, GameObject root, PanelSwitch ps)
        {
            this.dm = manager;
            this.root = root;
            this.ps = ps;
        }


        /// <summary>
        /// 选择分歧
        /// </summary>
        /// <param name="dic">分歧项</param>
        /// <param name="cd">倒计时</param>
        /// <param name="cdexit">时间为零时出口</param>
        /// <returns></returns>
        public SelectNode GetSelectNode(Dictionary<string, string> dic, float cd = 0f, string cdexit="")
        {
            dm.gameData.MODE = "特殊选择分歧";
            return new SelectNode(dm, root, ps, dic, cd, cdexit);
        }

        /// <summary>
        /// 选择分歧（预设含网络）
        /// </summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        public SelectNode GetSelectNode(string id)
        {
            dm.gameData.MODE = "选择分歧";
            dm.gameData.selectID = id;
            return new SelectNode(dm, root, ps, id);
        }

        /// <summary>
        /// 结束游戏NODE
        /// </summary>
        /// <returns></returns>
        public GameNode GetFinNode()
        {
            return new FinNode(dm, root, ps);
        }

        /// <summary>
        /// 寻找脚本文件(自动重置文本位置)
        /// </summary>
        /// <param name="name">脚本名</param>
        /// <returns></returns>
        public SharpScript FindTextScript(string name)
        {
            dm.gameData.currentTextPos = 0;
            dm.gameData.currentScript = name;
            dm.gameData.MODE = "Avg模式";
            return FindScript(name);
        }

        /// <summary>
        /// 寻找脚本文件(不重置文字位置)
        /// </summary>
        /// <param name="name">脚本名</param>
        /// <returns></returns>
        public SharpScript FindTextScriptNoneInit(string name)
        {
            dm.gameData.currentScript = name;
            dm.gameData.MODE = "Avg模式";
            DataManager.GetInstance().tempData.isDiaboxRecover = true;
            return FindScript(name);
        }

        private SharpScript FindScript(string name)
        {
            string classStr = SCRIPT_PATH + "." + name;
            try
            {
                Type t = Type.GetType(classStr);
                Debug.Log(t);
                object[] args = new object[] { dm, root, ps };
                SharpScript script = (SharpScript)Activator.CreateInstance(t, args);
                return script;
            }
            catch(Exception e)
            {
                Debug.LogError(e);
                Debug.LogError("未找到对应入口文件！" + classStr);
                return null;
            }
            //string classStr = SCRIPT_PATH + "." + name;
            //Type t = Type.GetType(classStr);
            //object[] args = new object[] { dm, root, ps };
            //TextScript script = (TextScript)Activator.CreateInstance(t, args);
            //return script;
        }

    }
}
