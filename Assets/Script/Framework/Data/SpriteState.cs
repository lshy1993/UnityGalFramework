﻿using System;
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
        public enum SpriteType
        {
            Normal, Movie, Live2d, Spine
        };
        public SpriteType type = 0;
        public int path = 0;
        public float spritePosition_x, spritePosition_y;
        public float spriteAlpha;
        public float scalex, scaley;
        public float rotatex, rotatey, rotatez;

        public SpriteState() { }

        public SpriteState(string name, Vector3 pos, float alpha)
        {
            spriteName = name;
            type = SpriteType.Normal;
            SetPosition(pos);
            spriteAlpha = alpha;
        }

        public void SetType(SpriteType x)
        {
            type = x;
        }

        public void SetSource(string filename)
        {
            spriteName = filename;
        }

        public string GetSource()
        {
            return spriteName;
        }

        public void SetAlpha(float a)
        {
            spriteAlpha = a;
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

        public Vector3 GetScale()
        {
            return new Vector3(scalex, scaley, 1);
        }

        public void SetScale(Vector3 vec)
        {
            scalex = vec.x;
            scaley = vec.y;
        }

        public Quaternion GetRatation()
        {
            return Quaternion.Euler(rotatex, rotatey, rotatez);
        }

        public void SetRatation(Quaternion qt)
        {
            Vector3 vect = qt.eulerAngles;
            rotatex = vect.x;
            rotatey = vect.y;
            rotatez = vect.z;
        }

        public override string ToString()
        {
            string content = string.Empty;
            content += "    type:" + type + " path: " + path + "\n";
            content += "    name: " + spriteName + "\n";
            content += "    alpha: " + spriteAlpha + "\n";
            content += "    pos: " + GetPosition() + "\n";
            content += "    scale: " + GetScale() + "\n";
            content += "    rotation: " + GetRatation() + "\n";
            return content;

            //return string.Format(
            //    "file:{0}, a:{1}, pos:({2}，{3})\n",
            //    spriteName,
            //    spriteAlpha,
            //    spritePosition_x,
            //    spritePosition_y);
        }
    }
}
