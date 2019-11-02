using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Framework.UI
{
    public delegate void UIAnimationCallback();
    public class PanelTreeInterface : MonoBehaviour
    {
        /// <summary>
        /// 当前面板
        /// </summary>
        public GameObject panel;

        /// <summary>
        /// 子面板
        /// </summary>
        public PanelTreeInterface[] subPanels;

        /// <summary>
        /// 面板默认动画
        /// 继承：PanelAnimation
        /// </summary>
        private PanelAnimation satellight;

        public Dictionary<string, PanelTreeInterface> childrenDictionary;

        public void Init()
        {
            //satellight = this;
            try
            {
                satellight = transform.GetComponent<PanelAnimation>();
                satellight.Init();
            }
            catch(Exception e)
            {
                if(e.GetType()== typeof(NullReferenceException))
                {
                    string emsg = string.Format("未检测到PanelAnimation组件！请检查 {0}", name);
                    Debug.LogError(emsg);
                }
                else
                {
                    Debug.LogError(e);
                }
                
            }
            
            childrenDictionary = new Dictionary<string, PanelTreeInterface>();
            if (!(subPanels == null  
                || subPanels.Length == 0 ))
            {
                for (int i = 0; i < subPanels.Length; i++)
                {
                    //Debug.Log(subPanels[i].name);
                    childrenDictionary.Add(subPanels[i].name, subPanels[i]);
                }
                
            }
        }

        public PanelTreeInterface GetChildByKey(string key)
        {
            if (childrenDictionary.ContainsKey(key)) return childrenDictionary[key];
            else return null;
        }

        public bool IsLeaf()
        {
            return (childrenDictionary == null || childrenDictionary.Count == 0);
        }


        public void Close(UIAnimationCallback callback)
        {
            StopAllCoroutines();
            satellight.BeforeClose();
            StartCoroutine(satellight.CloseSequence(callback));
        }

        public void Open(UIAnimationCallback callback)
        {
            StopAllCoroutines();
            StartCoroutine(satellight.OpenSequence(callback));
        }

        public void Open()
        {
            Open(() => { });
        }

        public void Close()
        {
            Close(() => { });
        }
    }

    public delegate void UIFadeIteratorFunc(PanelTreeInterface node);

    public class FadeTreeIterator
    {
        private PanelTreeInterface root;
        public Dictionary<string, List<string>> pathTable;
        public Dictionary<string, PanelTreeInterface> satellightTable;
        public FadeTreeIterator(PanelTreeInterface root)
        {
            this.root = root;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            RecursiveInit(root);
            pathTable = new Dictionary<string, List<string>>();
            satellightTable = new Dictionary<string, PanelTreeInterface>();
            BuildTreeTable(root, new List<string>());
        }

        /// <summary>
        /// 递归初始化
        /// </summary>
        /// <param name="node">当前节点</param>
        public void RecursiveInit(PanelTreeInterface node)
        {
            node.Init();

            if (node.IsLeaf())
            {
                return;
            }
            else
            {
                foreach (PanelTreeInterface child in node.childrenDictionary.Values)
                {
                    RecursiveInit(child);
                }
            }
        }

        public void PrintTree()
        {
            //return;            
            foreach (KeyValuePair<string, List<string>> kv in pathTable)
            {
                string info = kv.Key;
                info += ":";
                foreach (string s in kv.Value)
                {
                    info += "/" + s;
                }
                Debug.Log(info);
            }
        }



        private void BuildTreeTable(PanelTreeInterface node, List<string> pathArr)
        {
            pathArr.Add(node.name);

            pathTable.Add(node.name, pathArr);

            satellightTable.Add(node.name, node);
            
            if (node.IsLeaf()) return;
            else
            {
                foreach (PanelTreeInterface child in node.childrenDictionary.Values)
                {
                    BuildTreeTable(child, new List<string>(pathArr));
                }
            }
        }

        public void RecursiveExecute(PanelTreeInterface node, UIFadeIteratorFunc func)
        {
            RecursiveExecute(node, func, func);
        }

        public void RecursiveExecute(PanelTreeInterface node, UIFadeIteratorFunc nodeFunc, UIFadeIteratorFunc finishFunc)
        {
            nodeFunc(node);
            if (node.IsLeaf())
            {
                finishFunc(node);
            }
            else
            {
                foreach (PanelTreeInterface child in node.childrenDictionary.Values)
                {
                    RecursiveExecute(child, nodeFunc, finishFunc);
                }
            }
        }
    }
}
