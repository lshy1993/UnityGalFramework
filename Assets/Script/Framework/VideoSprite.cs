using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Assets.Script.Framework
{
    [RequireComponent( typeof(VideoPlayer))]
    public class VideoSprite : RawImage
    {
        public VideoPlayer vp;
        //public RawImage rui;
        private bool played;

        //private void Update()
        //{
        //    if (!played && vp.isPrepared)
        //    {
        //        vp.Play();
        //    }
        //}

        public void LoadClip(string fname, bool isloop = true)
        {
            vp.clip = Resources.Load<VideoClip>("Video/" + fname);
            vp.Prepare();
            int w = Convert.ToInt32(vp.clip.width);
            int h = Convert.ToInt32(vp.clip.height);
            var rtt = new RenderTexture(w, h, (int)Screen.dpi, RenderTextureFormat.ARGB32);
            vp.targetTexture = rtt;
            texture = rtt;
            vp.isLooping = isloop;
            vp.Play();
            played = false;
        }

        public void ClearClip()
        {
            vp.clip = null;
            texture = null;
        }

    }
}
