using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Framework.Node
{
    /// <summary>
    /// 数据更改块
    /// </summary>
    public class ExecPiece : Piece
    {
        public delegate void Execute(DataManager manager);
        //private DataManager manager;

        private Execute exec;
        public ExecPiece(int id, DataManager manager, Execute exec):base(id)
        {
            this.manager = manager;
            this.exec = exec;
        }
        public override void Exec()
        {
            exec(manager);
        }
   }
}
