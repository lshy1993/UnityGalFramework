using System.Collections;
using System.Collections.Generic;

using Assets.Script.Framework;
using Assets.Script.Framework.Node;
using Assets.Script.Framework.UI;

using UnityEngine;

namespace Assets.Script.Scenario
{
    public class demo0_1 : SharpScript
    {
        public demo0_1(DataManager manager, GameObject root, PanelSwitch ps):base(manager, root, ps) { }
        public override void InitText()
        {
            pieces = new List<Piece>()
            {
                f.FadeinBackground("classroom"),
                //f.SetLive2dSprite(0,"ming/mingmeng"),
                f.OpenDialog(),
                f.t("转场测试","单侧渐变-左"),
                f.SideFade("corridor"),
                f.t("转场测试","单侧渐变-右"),
                f.SideFade("classroom","right"),
                f.t("转场测试","旋转渐变-顺时针"),
                f.RotateFade("corridor",false,1.5f),
                f.t("转场测试","旋转渐变-逆时针"),
                f.RotateFade("classroom",true,1.5f),
                f.t("转场测试","圆-由内向外"),
                f.Circle("corridor",false,1.0f),
                f.t("","圆-逆向（由外向内）"),
                f.Circle("classroom",true,1.0f),
                f.t("","滚动-左侧进入"),
                f.Scroll("corridor"),
                f.t("","滚动-右侧进入"),
                f.Scroll("classroom","right"),
                f.t("","百叶窗-左侧反转"),
                f.Shutter("corridor"),
                f.t("","百叶窗-右侧反转"),
                f.Shutter("classroom","right"),
                f.t("","Rule 文件1"),
                f.Universial("corridor","1"),
                f.t("","Rule 文件2"),
                f.Universial("classroom","2"),
                f.t("","Rule 文件3"),
                f.Universial("corridor","3"),
                f.t("","Rule 文件4"),
                f.Universial("classroom","4"),
                f.t("","Rule 文件5"),
                f.Universial("corridor","5"),
                f.t("","Rule 文件6"),
                f.Universial("classroom","6"),
                f.t("","Rule 文件7"),
                f.Universial("corridor","7"),
                f.t("","Rule 文件8"),
                f.Universial("classroom","8"),
                f.t("","Rule 文件9"),
                f.Universial("corridor","9"),
                f.t("","Rule 文件10"),
                f.Universial("classroom","10"),
                f.t("","Rule 文件11"),
                f.Universial("corridor","11"),
                f.t("","Rule 文件12"),
                f.Universial("classroom","12"),
                f.t("","Rule 文件13"),
                f.Universial("corridor","13"),
                f.t("","Rule 文件14"),
                f.Universial("classroom","14"),
                f.t("","Rule 文件15"),
                f.Universial("corridor","15"),
                f.t("","Rule 文件16"),
                f.Universial("classroom","16"),
                f.t("","Rule 文件17"),
                f.Universial("corridor","17"),
                f.t("","Rule 文件18"),
                f.Universial("classroom","18"),
                f.t("","Rule 文件19"),
                f.Universial("corridor","19"),
                f.t("","Rule 文件20"),
                f.Universial("classroom","20"),
                f.t("","Rule 文件21"),
                f.Universial("corridor","21"),
                f.t("","Rule 文件22"),
                f.Universial("classroom","22"),
                f.t("","Rule 文件23"),
                f.Universial("corridor","23"),
                f.t("","Rule 文件24"),
                f.Universial("classroom","24"),
                f.t("","Rule 文件25"),
                f.Universial("corridor","25"),
                f.t("","Rule 文件26"),
                f.Universial("classroom","26"),
                f.t("","Rule 文件27"),
                f.Universial("corridor","27"),
                f.t("","Rule 文件28"),
                f.Universial("classroom","28"),
                f.t("","Rule 文件29"),
                f.Universial("corridor","29"),
                f.t("","Rule 文件30"),
                f.Universial("classroom","30"),
                f.t("","Rule 文件31"),
                f.Universial("corridor","31"),
                f.t("","Rule 文件32"),
                f.Universial("classroom","32"),
                f.t("","Rule 文件33"),
                f.Universial("corridor","33"),
                f.t("","Rule 文件34"),
                f.Universial("classroom","34"),
                f.t("","Rule 文件35"),
                f.Universial("corridor","35"),
                f.t("","Rule 文件36"),
                f.Universial("classroom","36"),
                f.t("","Rule 文件37"),
                f.Universial("corridor","37"),
                f.t("","Rule 文件38"),
                f.Universial("classroom","38"),
                f.FadeoutAll()
            };
        }

        public override GameNode NextNode()
        {
            Finish();
            return nodeFactory.FindTextScript("avg00");
        }

    }
}
