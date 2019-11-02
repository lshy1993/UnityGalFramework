using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assets.Script.Framework.UI;

using UnityEngine;

namespace Assets.Script.Framework.Node
{
    public class FinNode : GameNode
    {
        private FinUIManager uiManager;

        private GameNode next = null;
        private NodeFactory factory;

        public FinNode(DataManager manager, GameObject root, PanelSwitch ps):
            base(manager, root, ps)
        {
            ps.SwitchTo_VerifyIterative("Fin_Panel");
        }

        public override void Init()
        {
            uiManager = root.transform.Find("Fin_Panel").GetComponent<FinUIManager>();
            factory = NodeFactory.GetInstance();
        }

        public override void Update()
        { }

        public override GameNode NextNode()
        {
            return next;
        }

    }
}
