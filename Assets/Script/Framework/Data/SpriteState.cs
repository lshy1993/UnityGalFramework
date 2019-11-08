using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Script.Framework
{
    // 状态
    public class SpriteState
    {
        public string spriteName;
        public float spritePosition_x, spritePosition_y;
        public float spriteAlpha;

        public SpriteState() { }

        public SpriteState(string name, Vector3 pos, float alpha)
        {
            spriteName = name;
            SetPosition(pos);
            spriteAlpha = alpha;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(spritePosition_x, spritePosition_y);
        }

        public void SetPosition(Vector3 pos)
        {
            spritePosition_x = pos.x;
            spritePosition_y = pos.y;
        }

        public override string ToString()
        {
            return string.Format(
                "file:{0}, a:{1}, pos:({2}，{3})\n",
                spriteName,
                spriteAlpha,
                spritePosition_x,
                spritePosition_y);
        }
    }
}
