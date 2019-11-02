using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Framework.Node
{
    public abstract class Piece
    {
        private int id;
        private Func<DataManager, int> complexLogic;
        internal DataManager manager;
        private Func<int> simpleLogic;
        private Action simpleAction;
        private Action<DataManager> complexAction;


        /// <summary>
        /// 创建一个基本的Piece对象,默认的下一步返回自增的id
        /// </summary>
        /// <param name="id">PieceID,一般由Factory自动分配,仅关联于该Factory</param>
        public Piece(int id)
        {
            this.id = id;
            simpleLogic = null;
            complexLogic = null;
        }

        /// <summary>
        /// 创建一个包含简单跳转的Piece对象
        /// </summary>
        /// <param name="id">PieceID,一般由Factory自动分配,仅关联于该Factory</param>
        /// <param name="simpleLogic">简单逻辑，不需要依赖于外部变量</param>
        public Piece(int id, Func<int> simpleLogic) : this(id)
        {
            this.simpleLogic = simpleLogic;
        }

        /// <summary>
        /// 创建一个可以引用外部变量,包含复杂跳转逻辑的Piece
        /// </summary>
        /// <param name="id">PieceID,一般由Factory自动分配,仅关联于该Factor</param>
        /// <param name="complexLogic">可以引用外部变量的复杂逻辑</param>
        public Piece(int id, Func<DataManager, int> complexLogic,
            DataManager manager) : this(id)
        {
            this.complexLogic = complexLogic;
            this.manager = manager;
        }

        public Piece(int id, Action action, DataManager manager) : this(id)
        {
            this.simpleAction = action;
            this.manager = manager;
        }

        public Piece(int id, Action<DataManager> action, DataManager manager) : this(id)
        {
            this.complexAction = action;
            this.manager = manager;
        }


        public abstract void Exec();

        public virtual int Next()
        {
            if (simpleLogic == null && complexLogic == null)
            {
                if (simpleAction != null) { simpleAction(); }
                else if (complexAction != null) { complexAction(this.manager); }
                return id + 1;
            }
            else if (simpleLogic != null)
            {
                return simpleLogic();
            }
            else //if(complexLogic != null)
            {
                return complexLogic(manager);
            }
        }
    }
}
