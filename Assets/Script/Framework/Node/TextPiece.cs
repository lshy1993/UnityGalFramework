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
        private string name, dialog, avatar, voice;

        private GameObject diabox;

        private Text nameLabel, dialogLabel;
        private Image avatarSprite;

        public bool finish = false;

        /// <summary>
        /// 最基本的文字段
        /// </summary>
        /// <param name="id">piece id</param>
        /// <param name="diabox">对话框元件</param>
        /// <param name="name">名字</param>
        /// <param name="dialog">对话内容</param>
        /// <param name="avatar">头像图片名</param>
        public TextPiece(int id, GameObject diabox, string name = "", string dialog = "", string avatar = "", string voice = "") : base(id)
        {
            this.diabox = diabox;
            setVars(name, dialog, avatar, voice);
        }

        /// <summary>
        /// 带复杂逻辑的文字段
        /// </summary>
        /// <param name="id">piece id</param>
        /// <param name="diabox">对话框元件</param>
        /// <param name="name">名字</param>
        /// <param name="dialog">对话内容</param>
        /// <param name="avatar">头像图片名</param>
        /// <param name="simpleLogic">简单逻辑，可以用lambda表示</param>
        public TextPiece(int id, GameObject diabox, string name, string dialog, string avatar, string voice, Func<int> simpleLogic) : base(id, simpleLogic)
        {
            this.diabox = diabox;
            setVars(name, dialog, avatar, voice);
        }

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
        public TextPiece(int id,GameObject diabox, DataManager manager, Func<DataManager, int> complexLogic, string name = "", string dialog = "", string avatar = "", string voice = "") : base(id, complexLogic, manager)
        {
            setVars(name, dialog, avatar, voice);
        }
        public TextPiece(int id, GameObject diabox, DataManager manager, Action simpleAction, string name = "", string dialog = "", string avatar = "", string voice = "") :
            base(id, simpleAction, manager)
        {
            setVars(name, dialog, avatar, voice);
        }
        public TextPiece(int id, GameObject diabox, DataManager manager, Action<DataManager> complexAction, string name = "", string dialog = "", string avatar = "", string voice = "") :
            base(id, complexAction, manager)
        {
            setVars(name, dialog, avatar, voice);
        }

        #region 废弃旧代码
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
            DialogBoxUIManager uiManager = diabox.GetComponent<DialogBoxUIManager>();
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
                //通过UIManager设置文字，并开启打字机
                uiManager.SetText(this, name, dialog, voice, avatar);
                sm.SetVoice(voice);
                //模块设为未结束
                finish = false;
            }

        }

        public void Clear()
        {
            DialogBoxUIManager uiManager = diabox.GetComponent<DialogBoxUIManager>();
            uiManager.ClearText();
        }

        private void setVars(string name, string dialog, string avatar, string voice)
        {
            this.name = name;
            this.dialog = dialog;
            this.avatar = avatar;
            this.voice = voice;
        }

        public override string ToString()
        {
            return base.ToString()+ "name: " + name + "dialog: " + dialog + "\n";
        }
    }
}
