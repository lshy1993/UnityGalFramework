using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Framework.UI
{
    /// <summary>
    /// 侧边存档文件夹按钮
    /// </summary>
    public class SideFolderButton : BasicButton
    {

        protected override void Execute()
        {
            string FolderPath = Application.persistentDataPath + "/Save";
            System.Diagnostics.Process.Start(FolderPath);
        }

    }
}