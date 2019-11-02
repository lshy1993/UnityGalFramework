using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Script.Framework
{
    /// <summary>
    /// 网络管理器
    /// 用于控制游戏的联网过程
    /// </summary>
    public class NetworkManager : MonoBehaviour
    {
        private const string apiurl_debug = "http://localhost:3000/lt/select/";
        private const string apiurl = "http://api.liantui.xyz/lt/select/";

        public void GetSelect(string id)
        {
            string url = "http://localhost:3000/lt/select/" + id;
            StartCoroutine(HttpGet(url));
        }

        private IEnumerator HttpGet(string url)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.Log("net error");
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
            yield return request.downloadHandler.text;
        }

        public void PostSelect(string id, int select)
        {
            StartCoroutine(HttpPostSelect(id, select));
        }

        private IEnumerator HttpPostSelect(string id, int select)
        {
            string url = apiurl + id;
            WWWForm form = new WWWForm();
            form.AddField("num", select);
            UnityWebRequest request = UnityWebRequest.Post(url, form);
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.Log("net error");
            }
            else
            {
                Debug.Log("post done");
            }
        }
    }
}