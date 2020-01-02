using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Script.Framework.Node
{
    /// <summary>
    /// 仅用于控制对话框的显示与消失
    /// </summary>
    public class DiaboxPiece : Piece
    {
        private GameObject diabox;

        private enum OperateMode
        {
            Set, Show
        }
        /// <summary>
        /// 模式
        /// </summary>
        private OperateMode operate = OperateMode.Set;

        /// <summary>
        /// 默认背景图片
        /// </summary>
        public string filename = string.Empty;

        /// <summary>
        /// 默认背景颜色
        /// </summary>
        public Color color = new Color(1, 1, 1, 0);

        /// <summary>
        /// 默认纯色时图层大小
        /// </summary>
        public Vector2 size = new Vector2(1920, 1080);

        /// <summary>
        /// 默认图层位置
        /// </summary>
        public Vector2 pos = Vector2.zero;

        /// <summary>
        /// 默认文字内框
        /// </summary>
        public Vector4 rect = new Vector4(40, 40, 40, 40);

        //字符间距
        public float xint = 0;
        public float yint = 0;

        /// <summary>
        /// 关闭打开
        /// </summary>
        public bool isopen;

        /// <summary>
        /// 持续时长
        /// </summary>
        public float time;

        public DiaboxPiece(int id, GameObject diabox) : base(id)
        {
            this.diabox = diabox;
            this.operate = OperateMode.Set;
        }

        public DiaboxPiece(int id, GameObject diabox, bool isopen, float time) : base(id)
        {
            this.diabox = diabox;
            this.isopen = isopen;
            this.time = time;
            this.operate = OperateMode.Show;
        }

        public override void Exec()
        {
            //MessageUIManager uiManger = diabox.GetComponent<MessageUIManager>();
            //if (isopen) uiManger.Open(time, new Action(() => { }));
            //else uiManger.Close(time, new Action(() => { }));
            ExecAuto(() => { });
        }

        public void ExecAuto(Action callback)
        {
            MessageUIManager uiManger = diabox.GetComponent<MessageUIManager>();
            switch (operate)
            {
                case OperateMode.Set:
                    uiManger.InitDialogBox(filename, color, pos, size);
                    uiManger.InitDialogLabel(rect.x, rect.y, rect.z, rect.w);
                    callback();
                    break;
                case OperateMode.Show:
                    if (isopen)
                    {
                        uiManger.Open(time, callback);
                    }
                    else
                    {
                        uiManger.Close(time, callback);
                    }
                    break;
                default:
                    break;
            }

        }
    }
}
