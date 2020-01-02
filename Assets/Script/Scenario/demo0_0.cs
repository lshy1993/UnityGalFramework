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
                f.t("","要跳过序章测试吗？"),
            };
        }

        public override GameNode NextNode()
        {
            Finish();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("跳过", "demo1");
            dic.Add("不跳过", "demo0_1");
            return nodeFactory.GetSelectNode(dic);
        }

    }
}
