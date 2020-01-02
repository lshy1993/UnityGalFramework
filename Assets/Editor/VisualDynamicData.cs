using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using Assets.Script.Framework;

/// <summary>
/// 编辑器插件 查看动态数据
/// </summary>
public class VisualDynamicData : EditorWindow
{
    VisualDynamicData()
    {
        this.titleContent = new GUIContent("DynamicData");
    }

    private string detitle, content;
    private Vector2 scrollPosition1, scrollPosition2, scrollPosition3;
    private bool toggle1, toggle2, toggle3, toggle4, toggle5, toggle6;
    private int index;
    private DataManager dm;

    [MenuItem("Tools/DynamicData")]
    public static void showWindow()
    {
        CreateInstance<VisualDynamicData>().Show();
    }

    private void OnEnable()
    {
        //开启窗口时 加载数据
        dm = DataManager.GetInstance();
        autoRepaintOnSceneChange = true;
        toggle1 = false;
    }

    public void OnGUI()
    {
        //GUI显示
        if (GUILayout.Button("REFRESH"))
        {
            index = -1;
        }
        GUILayout.Space(5);
        EditorGUILayout.LabelField("面板链", ShowChain());
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        //game var
        GUILayout.BeginVertical(GUILayout.MaxWidth(300));
        EditorGUILayout.LabelField("GameVar", "Value");
        EditorGUILayout.LabelField("MODE", dm.gameData.MODE);
        EditorGUILayout.LabelField("当前脚本名", dm.gameData.currentScript);
        EditorGUILayout.LabelField("文字位置", dm.gameData.currentTextPos.ToString());

        EditorGUILayout.LabelField("Effecting", dm.isEffecting.ToString());
        EditorGUILayout.LabelField("ClickBlock", dm.IsClickBlocked().ToString());
        EditorGUILayout.LabelField("RClickBlock", dm.IsRightClickBlocked().ToString());
        EditorGUILayout.LabelField("WheelBlock", dm.IsWheelBlocked().ToString());
        EditorGUILayout.LabelField("LogBlock", dm.IsBacklogBlocked().ToString());
        EditorGUILayout.LabelField("SaveBlock", dm.IsSaveLoadBlocked().ToString());

        EditorGUILayout.LabelField("BGM", dm.gameData.BGM);
        EditorGUILayout.LabelField("SE", dm.gameData.SE);
        EditorGUILayout.LabelField("Voice", dm.gameData.Voice);
        
        if (GUILayout.Button("精灵信息", GUILayout.MaxWidth(100)))
        {
            index = 0;
        }
        if (GUILayout.Button("已读记录", GUILayout.MaxWidth(100)))
        {
            index = 1;
        }
        GUILayout.EndVertical();

        ////temp var
        GUILayout.BeginVertical();
        GUILayout.Label(detitle);
        scrollPosition3 = GUILayout.BeginScrollView(scrollPosition3);
        ReShowContent();
        GUILayout.Label(content);
        GUILayout.EndScrollView();
        GUILayout.EndVertical();

        //end
        GUILayout.EndHorizontal();
    }

    string ShowChain()
    {
        string result = "";
        foreach(string ss in dm.tempData.panelChain)
        {
            result += ss + ">";
        }
        return result;
    }

    void ReShowContent()
    {
        switch (index)
        {
            case -1:
                Repaint();
                break;
            case 0:
                ShowSpriteInfo();
                break;
            case 1:
                ShowReaded();
                break;
            case 2:
                break;
        }
    }

    void ShowSpriteInfo()
    {
        detitle = "精灵信息";
        content = string.Empty;
        Dictionary<int, SpriteState> lss = dm.gameData.bgSprites;
        foreach (KeyValuePair<int, SpriteState> kv in lss)
        {
            content += "sprite" + kv.Key + " : \n";
            content += kv.Value.ToString();
            content += "\n";
        }
    }

    void ShowReaded()
    {
        detitle = "已读信息";
        content = string.Empty;
        Dictionary<string, int> lss = dm.multiData.scriptTable;
        foreach (KeyValuePair<string,int> kv in lss)
        {
            content += string.Format("{0}: {1}\n", kv.Key, kv.Value);
        }
    }

}
