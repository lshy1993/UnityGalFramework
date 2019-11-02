using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using Assets.Script.Framework.UI;

using UnityEngine;

namespace Assets.Script.Framework.Node
{
    /// <summary>
    /// GameNode
    /// 游戏节点，作为游戏进程推进的最小单位
    /// </summary
    public abstract class GameNode
    {
        /// <summary>
        /// 存储局部变量
        /// </summary>
        //public Hashtable gVars, lVars;
        public DataManager manager;
        public GameObject root;
        public bool end { set; get; }
        public PanelSwitch ps;
        public GameNode(DataManager manager, GameObject root, PanelSwitch ps) //
        {
            //this.gVars = gVars;
            //this.lVars = lVars;
            this.manager = manager;
            this.root = root;
            this.ps = ps;
            end = false;

            Init();
        }

        /// <summary>
        /// 初始化游戏节点
        /// </summary>
        public virtual void Init() { }
        /// <summary>
        /// 单步更新
        /// </summary>
        public abstract void Update();
        /// <summary>
        /// 结束，重写此方法实现将可能需要写入全局的信息写入
        /// </summary>
        public virtual void Finish() { }
        /// <summary>
        /// 下一个节点,重写此方法实现不同游戏节点之间的跳转，
        /// 但是要注意在结束之前处理完结束内容
        /// </summary>
        /// <returns>下一个游戏节点</returns>
        public virtual GameNode NextNode()
        {
            Finish();
            return null;
        }

    }
}

