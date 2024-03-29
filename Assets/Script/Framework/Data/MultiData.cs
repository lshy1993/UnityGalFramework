﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Framework.Data
{
    /// <summary>
    /// 多周目全局数据 单独存档
    /// </summary>
    public class MultiData
    {
        /// <summary>
        /// 记录剧本块已读位置
        /// </summary>
        public Dictionary<string, int> scriptTable;

        /// <summary>
        /// 音乐浏览开启表
        /// </summary>
        public Dictionary<string, bool> musicTable;

        /// <summary>
        /// CG浏览表
        /// </summary>
        public Dictionary<string, bool> cgTable;

        /// <summary>
        /// 结局开启表
        /// </summary>
        public Dictionary<int, bool> endingTable;

        /// <summary>
        /// 案件回顾开启表
        /// </summary>
        public Dictionary<int, bool> caseTable;

        public MultiData()
        {
            scriptTable = new Dictionary<string, int>();
            musicTable = new Dictionary<string, bool>();
            cgTable = new Dictionary<string, bool>();
            endingTable = new Dictionary<int, bool>();
            caseTable = new Dictionary<int, bool>();
        }
    }
}
