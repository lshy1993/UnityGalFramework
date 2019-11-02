using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Framework
{
    /// <summary>
    /// 文字履历块类
    /// </summary>
    public class BacklogText
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string charaName;
        
        /// <summary>
        /// 文本
        /// </summary>
        public string mainContent;

        /// <summary>
        /// 语音路径
        /// </summary>
        public string voicePath;

        /// <summary>
        /// 头像文件名
        /// </summary>
        public string avatarFile;

        public BacklogText(string cName, string mContent, string vFile = "", string aFile = "")
        {
            this.charaName = cName;
            this.mainContent = mContent;
            this.voicePath = vFile;
            this.avatarFile = aFile;
        }

        public override string ToString()
        {
            string str = string.Empty;
            str += "Name: " + charaName+"\n";
            str += "Content: " + mainContent + "\n";
            str += "VoicePath: " + voicePath + "\n";
            str += "AvatarPath: " + avatarFile + "\n";
            return str;
        }
    }
}
