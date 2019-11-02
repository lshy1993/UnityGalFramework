using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BacklogUIManager : MonoBehaviour {

    public GameObject table;
    public ScrollRect rect;

	void Awake ()
    {
        table = transform.Find("Scroll View/Viewport/Content").gameObject;
        rect = transform.Find("Scroll View").GetComponent<ScrollRect>();
    }

    void OnEnable()
    {
        //刷新位置到最底部
        //table.GetComponent<ContentSizeFitter>();
        Debug.Log("Enabled! "+ rect.verticalNormalizedPosition);
        if (IsEnoughRow())
        {
            rect.verticalNormalizedPosition = 0;
        }
        
    }

    /// <summary>
    /// 已经有足够撑满view的内容块
    /// </summary>
    public bool IsEnoughRow()
    {
        return table.transform.childCount > 4;
    }

    /// <summary>
    /// 向上滚动
    /// </summary>
    /// <param name="delta"></param>
    public void UpScroll(float delta)
    {
        float final = rect.verticalNormalizedPosition + delta;
        rect.verticalNormalizedPosition = final > 1 ? 1f : final;
    }

    /// <summary>
    /// 向下滚动
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="callback"></param>
    public void DownScroll(float delta, Action callback)
    {
        float value = rect.verticalNormalizedPosition;
        if (!IsEnoughRow() || value <= 0)
        {
            //当文本不足 或 滚动条到底时 关闭backlog
            callback();
            //CloseMenu();
        }
        else
        {
            rect.verticalNormalizedPosition += delta;
        }
    }

}
