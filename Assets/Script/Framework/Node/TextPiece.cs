using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework.Node
{
    /// <summary>
    /// TextPiece 
    /// 文字事件，通常是一句话与跳转逻辑的组合
    /// </summary>
    public class TextPiece : Piece
    {
        private enum OperateMode
        {
            Line,PageLine,LineFeed,ClearPage
        }
        private OperateMode operate = 0;

        private string name, dialog, avatar, voice;

        private GameObject diabox;

        //private Text nameLabel, dialogLabel;
        //private Image avatarSprite;

        private int l2dmouth = -1;

        public bool finish = false;

        public TextPiece(int id, GameObject diabox) : base(id)
        {
            this.diabox = diabox;
            this.operate = OperateMode.ClearPage;
        }

        /// <summary>
        /// 最基本的文字段
        /// </summary>
        /// <param name="id">piece id</param>
        /// <param name="diabox">对话框元件</param>
        /// <param name="dialog">对话内容</param>
        /// <param name="name">名字</param>
        /// <param name="avatar">头像图片名</param>
        public TextPiece(int id, GameObject diabox, string dialog, string name = "", string avatar = "", string voice = "", int model = -1) : base(id)
        {
            this.diabox = diabox;
            SetVars(name, dialog, avatar, voice);
            SetModel(model);
        }

        public void RunSimpleLogic(DataManager dm, Func<int> simpleLogic)
        {

        }

        public void RunComplexLogic(DataManager dm, Func<DataManager, int> complexLogic)
        {

        }

        public void RunAction(Action callback)
        {
            //callback();
        }

        public void RunAction(Action<DataManager> callback)
        {
            //callback();
        }

        #region 废弃旧代码
        /// <summary>
        /// 带复杂逻辑的文字段
        /// </summary>
        /// <param name="id">piece id</param>
        /// <param name="diabox">对话框元件</param>
        /// <param name="name">名字</param>
        /// <param name="dialog">对话内容</param>
        /// <param name="avatar">头像图片名</param>
        /// <param name="simpleLogic">简单逻辑，可以用lambda表示</param>
        //public TextPiece(int id, GameObject diabox, string dialog, string name, string voice, string avatar, Func<int> simpleLogic) : base(id, simpleLogic)
        //{
        //    this.diabox = diabox;
        //    SetVars(name, dialog, avatar, voice);
        //}

        /// <summary>
        /// 创建一个拥有复杂逻辑的文字段，可以引用外部变量
        /// </summary>
        /// <param name="id">piece id</param>
        /// <param name="diabox">对话框元件</param>
        /// <param name="manager">数据库</param>
        /// <param name="complexLogic">复杂逻辑</param>
        /// <param name="name">名字</param>
        /// <param name="dialog">对话内容</param>
        /// <param name="avatar">头像图片名</param>
        //public TextPiece(int id,GameObject diabox, DataManager manager, Func<DataManager, int> complexLogic, string name = "", string dialog = "", string avatar = "", string voice = "") : base(id, complexLogic, manager)
        //{
        //    SetVars(name, dialog, avatar, voice);
        //}
        //public TextPiece(int id, GameObject diabox, DataManager manager, Action simpleAction, string name = "", string dialog = "", string avatar = "", string voice = "") :
        //    base(id, simpleAction, manager)
        //{
        //    SetVars(name, dialog, avatar, voice);
        //}
        //public TextPiece(int id, GameObject diabox, DataManager manager, Action<DataManager> complexAction, string name = "", string dialog = "", string avatar = "", string voice = "") :
        //    base(id, complexAction, manager)
        //{
        //    SetVars(name, dialog, avatar, voice);
        //}


        /// <param name="nameLabel">名字标签</param>
        /// <param name="dialogLabel">对话标签</param>
        /// <param name="avatarSprite">头像</param>
        //public TextPiece(int id, UILabel nameLabel, UILabel dialogLabel, UI2DSprite avatarSprite, string name = "", string dialog = "", string avatar ="") : base(id)
        //{
        //    setVars(name, dialog, avatar, nameLabel, dialogLabel, avatarSprite);
        //}


        /// <param name="nameLabel">名字标签</param>
        /// <param name="dialogLabel">对话标签</param>
        /// <param name="avatarSprite">头像</param>
        //public TextPiece(int id,
        //           UILabel nameLabel,
        //           UILabel dialogLabel,
        //           UI2DSprite avatarSprite,
        //           string name, string dialog, string avatar,
        //           Func<int> simpleLogic
        //        ) : base(id, simpleLogic)
        //    {
        //        setVars(name, dialog, avatar, nameLabel, dialogLabel, avatarSprite);
        //    }

        /// <param name="nameLabel">名字标签</param>
        /// <param name="dialogLabel">对话标签</param>
        /// <param name="avatarSprite">头像</param>
        //    public TextPiece(int id,
        //UILabel nameLabel,
        //UILabel dialogLabel,
        //UI2DSprite avatarSprite,
        //DataManager manager,
        //Func<DataManager, int> complexLogic,
        //string name = "", string dialog = "", string avatar = "") : base(id, complexLogic, manager)
        //    {
        //        setVars(name, dialog, avatar, nameLabel, dialogLabel, avatarSprite);

        //    }


        //public TextPiece(int id, UILabel nameLabel, UILabel dialogLabel, UI2DSprite avatarSprite, DataManager manager, Action simpleAction, string name ="", string dialog = "", string avatar = ""):
        //    base(id,simpleAction, manager)
        //{
        //    setVars(name, dialog, avatar, nameLabel, dialogLabel, avatarSprite);
        //}


        //public TextPiece(int id, UILabel nameLabel, UILabel dialogLabel, UI2DSprite avatarSprite, DataManager manager, Action<DataManager> complexAction, string name ="", string dialog = "", string avatar = ""):
        //    base(id,complexAction, manager)
        //{
        //    setVars(name, dialog, avatar, nameLabel, dialogLabel, avatarSprite);
        //}
        #endregion

        public override void Exec()
        {
            ExecAuto(() => { });
        }

        public void ExecAuto(Action callback)
        {
            MessageUIManager uiManager = diabox.GetComponent<MessageUIManager>();
            SoundManager sm = GameObject.Find("GameManager").GetComponent<SoundManager>();
            //判断是否在打字途中点击第二下
            if (uiManager.IsTyping())
            {
                //通过UIManager执行打字结束操作
                uiManager.FinishType();
                //当前模块结束
                finish = true;
            }
            else
            {
                HistoryManager hm = GameObject.Find("GameManager").GetComponent<HistoryManager>();
                hm.AddToTable(new BacklogText(name, dialog, voice, avatar));
                hm.SetCurrentText(name, dialog);

                if (l2dmouth == -1)
                {
                    sm.SetVoice(voice);
                }
                else
                {
                    // liv2d口型部分
                    ImageManager im = GameObject.Find("GameManager").GetComponent<ImageManager>();
                    sm.SetVoiceWithLive2d(voice, im.GetLive2dObject(0));
                }
                //通过UIManager设置文字，并开启打字机
                Debug.Log(operate);
                switch (operate)
                {
                    case OperateMode.Line:
                        uiManager.ClearText();
                        uiManager.SetText(this, dialog, name, avatar);
                        //未结束等待打字机
                        finish = false;
                        break;
                    case OperateMode.PageLine:
                        uiManager.HideNextIcon();
                        uiManager.AddText(this, dialog);
                        //未结束等待打字机
                        finish = false;
                        break;
                    case OperateMode.LineFeed:
                        uiManager.LineFeed();
                        finish = true;
                        callback();
                        break;
                    case OperateMode.ClearPage:
                        uiManager.ClearText();
                        // 已经执行完毕
                        finish = true;
                        callback();
                        break;
                }
            }
        }

        private void Clear()
        {
            MessageUIManager uiManager = diabox.GetComponent<MessageUIManager>();
            uiManager.ClearText();
        }

        public void SetPageLine()
        {
            operate = OperateMode.PageLine;
        }

        public void SetLineFeed()
        {
            operate = OperateMode.LineFeed;
        }

        private void SetVars(string name, string dialog, string avatar, string voice)
        {
            this.name = name;
            this.dialog = dialog;
            this.avatar = avatar;
            this.voice = voice;
        }

        private void SetModel(int x)
        {
            this.l2dmouth = x;
        }

        public override string ToString()
        {
            return base.ToString()+ "name: " + name + "dialog: " + dialog + "\n";
        }
    }
}
