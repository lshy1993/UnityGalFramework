using System;
using System.Collections;
using System.Collections.Generic;

using Assets.Script.Framework;
using Assets.Script.Framework.Node;
using Assets.Script.Framework.UI;

using UnityEngine;

namespace Assets.Script.Scenario
{
    public class demo0_0 : TextScript
    {
        public demo0_0(DataManager manager, GameObject root, PanelSwitch ps):base(manager, root, ps) { }
        public override void InitText()
        {
            pieces = new List<Piece>()
            {
                f.OpenDialog(),
                f.t("","[00ff00]要跳过序章吗？（跳过无法自定义主人公姓名）[-]"),
            };
        }

        public override GameNode NextNode()
        {
            Finish();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("对峙系统测试", "demo0_1");
            dic.Add("侦探案件测试", "demo01");
            dic.Add("ED动画测试", "demo_fin");
            dic.Add("选项网络统计测试", "demo0_2");
            dic.Add("观看序章", "demo0_3");
            dic.Add("跳过", "demo_jump");
            return nodeFactory.GetSelectNode(dic);
        }

    }
}
