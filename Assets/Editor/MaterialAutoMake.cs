using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using System.IO;

public class MaterialAutoMake : MonoBehaviour
{
    
    [MenuItem("Tools/MaterialAutoMake")]
    public static void MakeMaterial()
    {
        string[] allPath = AssetDatabase.FindAssets("t:Shader", new string[] { "Assets/Shaders" });
        for (int i = 0; i < allPath.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(allPath[i]);
            Shader sd = AssetDatabase.LoadAssetAtPath(path, typeof(Shader)) as Shader;
            string fname = Path.GetFileNameWithoutExtension(path);
            //Debug.Log(fname);
            Material mk = new Material(sd);
            string newpath = "Assets/Resources/Material/" + fname + ".mat";
            AssetDatabase.CreateAsset(mk, newpath);
            //AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();
        }
    }

    static string[] GetExistedMaterial()
    {
        string[] allPath = AssetDatabase.FindAssets("t:Material", new string[] { "Assets/Resources/Material" });
        for (int i = 0; i < allPath.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(allPath[i]);

        }
        return allPath;

    }
}
