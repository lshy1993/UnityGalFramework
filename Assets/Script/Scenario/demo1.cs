using System.Collections;
using System.Collections.Generic;

using Assets.Script.Framework;
using Assets.Script.Framework.Node;
using Assets.Script.Framework.UI;

using UnityEngine;

namespace Assets.Script.Scenario
{
    public class demo1 : SharpScript
    {
        public demo1(DataManager manager, GameObject root, PanelSwitch ps) : base(manager, root, ps) { }
        public override void InitText()
        {
            pieces = new List<Piece>()
            {
                //——背景 后排视角教室——
                f.FadeinBackground("bg cloud up"),
                f.OpenDialog(),
                f.FadeinCharacterSprite(0,"ming/n-01"),
                f.t("喵星人", "呼——终于忙完了喵！"),
                f.t("李云萧", "<color=#66ccff>（将新的教材搬回教室后，我们把课本分发给了每一位同学。）</color>"),
                f.t("李云萧", "说起来，喵星人，我们的班主任呢？"),
                f.t("喵星人", "工作做完了，估计已经回家去了。"),
                f.t("李云萧", "这么早就回去了！？"),
                f.PreTransSprite(0),
                f.SetSprite(0,"ch3"),
                f.TransCharacterSprite(0),
                f.t("？？？", "各位！大家都拿到课本了吗？"),
                //——CG 苏梦忆讲台演讲——
                f.t("？？？", "既然大家都回来了，那么……"),
                f.t("？？？", "现在就开始新一轮的班委竞选。"),
                f.t("李云萧", "……"),
                f.PreTransSprite(0),
                f.SetSprite(0,"ch4"),
                f.TransCharacterSprite(0),
                f.t("喵星人", "在盯着看什么呢？"),
                f.t("李云萧", "没、没什么……"),
                f.t("喵星人", "啊~我懂了，原来是班长啊~"),
                f.t("李云萧", "班、班长？"),
                f.t("喵星人", "喏，站在讲台上讲话的就是了。"),
                f.MoveCharacterSprite(0,"right",0.2f),
                //f.PreTransCharacterSprite(0,"ch4","right"),
                f.SetSprite(1,"ch3","left"),
                f.FadeinCharacterSprite(1),
                //f.TransAll(),
                f.t("？？？", "你是新来的同学吧，你要参与竞选吗？"),
                f.t("李云萧", "不、不用了吧，我刚来……"),
                f.t("？？？", "嗯，那好吧。"),
                f.t("？？？", "那么等大家发言结束后，请投一下票。"),
                f.t("李云萧", "我知道了。"),
                f.FadeoutCharacterSprite(1),
                f.MoveCharacterSprite(0,"middle",0.3f),
                //f.TransCharacterSprite(0,"ch4"),
                f.t("喵星人", "怎么？看女生看得眼睛都歪了？"),
                f.t("李云萧", "没、没有……"),
                f.t("李云萧", "[66ccff]（虽然，真的有种令人心静的清秀……）[-]"),
                f.t("喵星人", "你都写在脸上了，好喵？"),
                f.t("李云萧", "[66ccff]（不行，得赶紧转换话题！）[-]"),
                f.t("李云萧", "说起来，喵星人，我旁边的这个座位是谁的？"),
                f.t("喵星人", "就是班长她的喵。"),
                f.t("李云萧", "啊！？不会吧……"),
                f.FadeoutAllChara(),
                f.CloseDialog(),
                f.TransBackgroundToImage("sky_day"),
                f.Wait(0.3f),
                f.OpenDialog(),
                f.t("李云萧", "[66ccff]（我并没有心思去听每个人的上台发言……）[-]"),
                f.t("李云萧", "[66ccff]（夏日的午后总是这么悠闲……）[-]"),
                f.t("李云萧", "[66ccff]（悠闲得让人不自觉地发起呆来……）[-]"),
                f.t("喵星人", "李云萧！李云萧！"),
                f.TransBackgroundToImage("classroom"),
                f.FadeinCharacterSprite(0,"ch4"),
                f.t("李云萧", "欸？已经结束了？"),
                f.t("喵星人", "发言早就结束了，现在在投票呢！"),
                f.t("李云萧", "哦哦。"),
                f.t("喵星人", "候选人的名字在黑板上呢。"),
                f.t("李云萧", "[66ccff]（我随便写了几个名字上去。）[-]"),
                f.t("李云萧", "这样行了吧。"),
                f.t("喵星人", "我说，你从刚开始就精神恍惚……"),
                f.t("喵星人", "你不会是真的看上班长了吧？"),
                f.t("李云萧", "你在乱说什么……"),
                f.t("喵星人", "但是，不得不承认，班长她确实漂亮。"),
                f.t("喵星人", "嘿嘿~你就当我什么也没说吧，我们走！"),
                f.t("李云萧", "等等？这不是还没统计出结果吗？"),
                f.t("喵星人", "对于我们这些不参选的人来说，已经结束了。"),
                f.t("喵星人", "再说，班长她也说了解散了。"),
                f.t("李云萧", "是、是吗？"),
                f.t("喵星人", "走啦！我还要带你逛校园喵！"),
                f.FadeoutAll(),
                //——背景 校园地图——
                f.FadeinBackground("school"),
                //*这里进入地图说明 人物考虑用Q版
                f.FadeinCharacterSprite(0,"ch4","right"),
                f.OpenDialog(),
                f.t("喵星人", "先给你介绍一下我们的校园喵。"),
                f.t("喵星人", "这就是我们所在的[ff6600]1号教学楼[-]，整个高中部都在这里。"),
                f.t("喵星人", "然后与之相连的，则是……好麻烦！"),
                f.t("李云萧", "怎么不介绍了？"),
                f.t("喵星人", "因为字太多了，加上这是试玩版，没必要解释这么清楚。"),
                f.t("喵星人", "只要把鼠标移到按钮上，就能看到这个地点的详细介绍。"),
                f.t("喵星人", "好了！讲完了，我们去吧。"),
                f.t("李云萧", "……"),
                f.t("喵星人", "2号寝室楼，别搞错了。")
            };
        }

        public override GameNode NextNode()
        {
            Finish();
            return nodeFactory.FindTextScript("demo_jump");
        }

    }

}
