using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Script.Framework.UI
{
    /**
     * PanelSwitch: 
     * 整个游戏只允许一个，作为GameManager的组件，不能删除
     * 保存其他模块间切换函数，供其他Manager调用
     * 组件示意：
     * Avg_Panel 文字模式大块
     * Title_Panel 开始界面大块
     * System_Panel 系统大块
     * ===后续添加===
     * Map_Panel 地图模式大块
     * Edu_Panel 养成模式大块
     * Exam_Panel 考试模式
     * Shop_Panel 购物系统
     * Phone_Panel 电子手册大块
     */
    public class PanelSwitch : MonoBehaviour
    {
        private GameObject root;

        /// <summary>
        /// 存储面板名称
        /// </summary>
        public readonly List<string> PANEL_NAMES = new List<string>()
    {
        "Avg",
        "Title",
        "System"
        //"Map",
        //"Edu",
        //"Exam",
        //"Shop",
        //"Phone",
        //"Fin",
        //"Hardness"
    };

        Dictionary<string, GameObject> panels;
        private string current;
        //记录当前已开启的面板
        private List<string> currentPanelPath
        {
            get { return DataManager.GetInstance().tempData.panelChain; }
            set { DataManager.GetInstance().tempData.panelChain = value; }
        }

        private FadeTreeIterator iterator;

        public void HidePanel(string name)
        {
            panels[name].GetComponent<CanvasGroup>().alpha = 0;
        }

        /// <summary>
        ///  用于验证新的PanelSwitch想法的函数
        /// </summary>
        /// <param name="next"></param>
        public void SwitchTo_IdeaVerify(string next)
        {
            // TODO 想办法直接获取相应的符合接口的component
            PanelTreeInterface currentFade = panels[current].GetComponent<PanelTreeInterface>(),
                nextFade = panels[next].GetComponent<PanelTreeInterface>();

            panels[current].GetComponent<CanvasGroup>().alpha = 1;
            currentFade.Close(() =>
            {
                panels[current].SetActive(false);
                panels[next].GetComponent<CanvasGroup>().alpha = 0;
                panels[next].SetActive(true);
                nextFade.Open();
            });
        }

        public void SwitchTo_VerifyIterative(string next)
        {
            SwitchTo_VerifyIterative(next, () => { }, () => { });
        }

        public void SwitchTo_VerifyIterative(string next, UIAnimationCallback closeCallback)
        {
            SwitchTo_VerifyIterative(next, closeCallback, () => { });
        }

        public void SwitchTo_VerifyIterative_WithOpenCallback(string next, UIAnimationCallback openCallback)
        {
            SwitchTo_VerifyIterative(next, () => { }, openCallback);
        }

        public void SwitchTo_VerifyIterative(string next, UIAnimationCallback closeCallback,
            UIAnimationCallback openCallback)
        {
            //Debug.Log(next);
            //计算板块链：共有链，关闭链，开启链
            //例：Avg切换到Enqurie：关闭DialogBox 打开Enquire 共有Avg
            List<string>[] result = GetListIntersectAndDifference(currentPanelPath, iterator.pathTable[next]);
            List<string> sameChain = result[0], closeChain = result[1], openChain = result[2];

            //自己切换自己的情形
            if (openChain.Count == 0 && closeChain.Count == 0)
            {
                closeChain = sameChain.GetRange(sameChain.Count - 1, 1);
                openChain = sameChain.GetRange(sameChain.Count - 1, 1);
            }

            //Debug.Log("currentPath:" + ConvertToStringPath(currentPanelPath));
            //Debug.Log("sameChain:" + ConvertToStringPath(sameChain));
            //Debug.Log("closeChain:" + ConvertToStringPath(closeChain));
            //Debug.Log("openChain:" + ConvertToStringPath(openChain));

            CloseChain(new Stack<string>(closeChain), () =>
            {
                SetActiveChain(false, closeChain);
                SetActiveChain(true, openChain); //可能需要设置打开时初始条件
            closeCallback();
                OpenChain(new Queue<string>(openChain), openCallback);
            });
        }

        private void SetActiveChain(bool isOpne, List<string> chain)
        {
            //按照开启链，打开/关闭GameObject，开启时设置alpha为0
            foreach (string s in chain)
            {
                GameObject go = iterator.satellightTable[s].gameObject;
                if (isOpne)
                {
                    CanvasGroup up = go.GetComponent<CanvasGroup>();
                    if (up != null) up.alpha = 0;
                    Debug.Log(string.Format("{0} SetActive({1}),Alpha: {2}",
                    go.name, isOpne, up.alpha));
                }

                go.SetActive(isOpne);
            }
        }

        private void CloseChain(Stack<string> closeStack, UIAnimationCallback closeFinishCallback)
        {
            if (closeStack.Count == 0 || closeStack.Peek() == null)
            {
                //已将所有面板关闭
                closeFinishCallback();
                return;
            }
            else
            {
                string close = closeStack.Pop();
                currentPanelPath.Remove(close);
                //Debug.Log("path update:" + ConvertToStringPath(currentPanelPath));
                iterator.satellightTable[close].Close(() =>
                {
                    CloseChain(closeStack, closeFinishCallback);
                });
            }

        }

        private void OpenChain(Queue<string> openQueue, UIAnimationCallback openFinishCallback)
        {
            if (openQueue.Count == 0 || openQueue.Peek() == null)
            {
                //已将所有面板开启
                openFinishCallback();
                return;
            }
            else
            {
                string open = openQueue.Dequeue();
                currentPanelPath.Add(open);
                //Debug.Log("path update:" + ConvertToStringPath(currentPanelPath));
                iterator.satellightTable[open].Open(() =>
                {
                    OpenChain(openQueue, openFinishCallback);
                });
            }
        }

        private string ConvertToStringPath(List<string> strs)
        {
            string path = "";
            foreach (string str in strs)
            {
                path += str + ">";
            }
            return path;
        }

        private List<string>[] GetListIntersectAndDifference(List<string> fst, List<string> snd)
        {
            List<string>[] difference = new List<string>[3];

            difference[0] = fst.Intersect(snd).ToList();
            difference[1] = fst.Except(snd).ToList();
            difference[2] = snd.Except(fst).ToList();

            return difference;
        }


        // Use this for initialization
        public void Init()
        {
            root = GameObject.Find("UI Root");
            //搜索到所有的Panel原件
            panels = new Dictionary<string, GameObject>();
            for (int i = 0; i < PANEL_NAMES.Count; i++)
            {
                GameObject panelObj = root.transform.Find(PANEL_NAMES[i] + "_Panel").gameObject;
                panels.Add(PANEL_NAMES[i], panelObj);
            }
            //游戏初始界面打开情况
            current = "Title";
            currentPanelPath = new List<string>()
            {
                "UI Root"
            };
            try
            {
                PanelTreeInterface rt = root.GetComponent<PanelTreeInterface>();
                iterator = new FadeTreeIterator(rt);
                iterator.Init();
                //Debug.Log("打印Panel树");
                //iterator.PrintTree();
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(NullReferenceException))
                {
                    string emsg = string.Format("未找到TreeInterface组件！请检查 {0}！", name);
                    Debug.LogError(emsg);
                }
                else
                {
                    Debug.LogError(e);
                }
            }
        }

        //右键界面操作 全局函数
        public void RightClick()
        {
            if (panels["Title"].activeSelf)
            {
                //标题画面被打开时
                TitleUIManager tm = panels["Title"].GetComponent<TitleUIManager>();
                tm.RightClick();
            }

            if (panels["System"].activeSelf)
            {
                //系统菜单打开时
                GameObject wc = panels["System"].transform.Find("Warning_Panel").gameObject;
                if (wc != null && wc.activeSelf)
                {
                    //警告窗口开启 则关闭
                    wc.SetActive(false);
                }
                else
                {
                    //否则关闭菜单
                    CloseMenu();
                }
            }
            else
            {
                //其他情形
                if (panels["Avg"].activeSelf)
                {
                    OpenMenu();
                }
            }

        }

        //直接打开菜单
        public void OpenMenu()
        {
            //Auto模式下先取消
            if (DataManager.GetInstance().isAuto)
            {
                DataManager.GetInstance().isAuto = false;
                return;
            }
            Debug.Log("Open Menu!");
            //预截图
            StartCoroutine(ScreenShot());
        }
        private IEnumerator ScreenShot()
        {
            // 开始截图
            yield return new WaitForEndOfFrame();
            Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            TextureScale.Bilinear(tex, 540, 303);
            byte[] imagebytes = tex.EncodeToPNG();
            DataManager.GetInstance().SetTempVar("缩略图", imagebytes);
            Destroy(tex);
            // 隐去dialog
            GameObject diabox = panels["Avg"].transform.Find("Message_Panel").gameObject;
            diabox.GetComponent<DialogBoxUIManager>().HideWindow();
            // 打开菜单
            panels["System"].GetComponent<SystemUIManager>().quickOpen = true;
            panels["System"].SetActive(true);
            panels["System"].GetComponent<SystemUIManager>().Open();
        }

        //关闭菜单
        private void CloseMenu()
        {
            Debug.Log("Close Menu");
            panels["System"].GetComponent<SystemUIManager>().Close();
        }

        //滚轮向上操作
        public void MouseUpScroll(float delta)
        {
            //仅纯avg状态下可以打开BackLog
            if (panels["Avg"].activeSelf)
            {
                GameObject backCon = panels["System"].transform.Find("Backlog_Panel").gameObject;
                //Backlog关闭时 且 未被锁定才能打开
                if (!backCon.activeSelf && !DataManager.GetInstance().IsBacklogBlocked())
                {
                    OpenBacklog();
                }
                else
                {
                    //如果已经打开了Backlog 则向上滚动条
                    BacklogUIManager uim = backCon.GetComponent<BacklogUIManager>();
                    uim.UpScroll(delta);
                }
            }
        }

        //开启BackLog
        public void OpenBacklog()
        {
            if (DataManager.GetInstance().isAuto) return;
            panels["Avg"].transform.Find("Message_Panel").GetComponent<DialogBoxUIManager>().HideWindow();
            panels["System"].GetComponent<SystemUIManager>().quickOpen = true;
            panels["System"].SetActive(true);
            panels["System"].GetComponent<CanvasGroup>().alpha = 1;
            panels["System"].GetComponent<SystemUIManager>().OpenBacklog();
        }

        //滚轮朝下操作
        public void MouseDownScroll(float delta)
        {
            //仅当avg开启时有效
            if (panels["Avg"].activeSelf)
            {
                GameObject backCon = panels["System"].transform.Find("Backlog_Panel").gameObject;
                //当Backlog打开时
                if (backCon.activeSelf)
                {
                    BacklogUIManager uim = backCon.GetComponent<BacklogUIManager>();
                    uim.DownScroll(delta,()=> { CloseMenu(); });
                }
                else
                {
                    //否则相当于左键
                    GameObject clickCon = panels["Avg"].transform.Find("Message_Panel").gameObject;
                    clickCon.GetComponent<Click_Next>().Execute();
                }

            }
        }

        ///--------------------以下是旧函数----------------------------------
        /// <summary>
        /// 切换面板,TODO:可能需要对每个Panel做一个切换特效？
        /// </summary>
        /// <param name="panel">切换目标面板</param>
        /// <param name="fadein">淡入时间，默认0.3s</param>
        /// <param name="fadeout">淡出时间，默认0.3s</param>
        public void SwitchTo(string panel, float fadein = 0.3f, float fadeout = 0.3f)
        {
            Debug.Log("从：" + current + "切换:" + panel);

            if (panels.ContainsKey(panel))
            {
                GameObject currentPanel = panels[current];
                GameObject nextPanel = panels[panel];
                //Debug.Log(panel);
                //nextPanel.SetActive(true);
                //nextPanel.GetComponent<UIPanel>().alpha = 0;
                //yield return new WaitForSeconds(1);

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
            yield return new WaitForSeconds(fadeout + 1f);
            nextPanel.SetActive(true);
            next.Open(fadein);

        }

        IEnumerator Fadein(float time, GameObject target)
        {
            CanvasGroup panel = target.GetComponent<CanvasGroup>();
            float f = time == 0 ? 1 : 0;
            panel.alpha = f;
            target.SetActive(true);
            while (f < 1f)
            {
                f = Mathf.MoveTowards(f, 1f, Time.deltaTime / time);
                panel.alpha = f;
                yield return null;
            }
        }

        IEnumerator DelaySec(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action();
        }
        IEnumerator Fadeout(float time, GameObject target)
        {
            CanvasGroup panel = target.GetComponent<CanvasGroup>();
            float f = time == 0 ? 0 : 1;
            panel.alpha = f;
            while (f > 0)
            {
                f = Mathf.MoveTowards(f, 0, Time.deltaTime / time);
                panel.alpha = f;
                yield return null;
            }
            target.SetActive(false);
        }

    }
}