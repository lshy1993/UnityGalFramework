using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LitJson;
using Newtonsoft.Json;

//选项类
namespace Assets.Script.Framework.Data
{
    public class Selection
    {
        public string uid;
        public int nums;
        public List<string> select;
        public List<string> entrance;
        public double cd;
        public string cdexit;

        public Selection()
        {
            select = new List<string>();
            entrance = new List<string>();
        }

        public Selection(JsonData data)
        { 
            uid = (string)data["id"];
            nums = (int)data["选项数"];
            select = new List<string>();
            entrance = new List<string>();
            foreach (JsonData jd in data["选项"])
            {
                string text = (string)jd["文字"];
                select.Add(text);
                string ent = (string)jd["入口"];
                entrance.Add(ent);
            }
        }
    }
}
