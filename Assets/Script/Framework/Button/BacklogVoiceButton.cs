using UnityEngine;
using System.Collections;

namespace Assets.Script.Framework.UI
{

    public class BacklogVoiceButton : BasicButton
    {

        public string path;

        protected override void Hover(bool ishover)
        {
            //keep empty
        }

        protected override void SE_Click()
        {
            //keep empty
        }

        protected override void Execute()
        {
            //Debug.Log("Repeat Voice" + path);
            GameObject.Find("GameManager").GetComponent<SoundManager>().SetVoice(path);
        }

    }
}