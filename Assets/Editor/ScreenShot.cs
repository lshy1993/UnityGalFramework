using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using System.IO;

public class ScreenShoter : MonoBehaviour
{
    
    [MenuItem("Tools/ScreenShot")]
    public static void ScreenShot()
    {
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        TextureScale.Bilinear(tex, Screen.width, Screen.height);
        ScreenCapture.CaptureScreenshot(@"C:\Users\MzlStudio-1022\Desktop\1.png");
        //byte[] imagebytes = tex.EncodeToPNG();
        //FileStream fs = new FileStream(@"C:\Users\MzlStudio-1022\Desktop\1.png", FileMode.Create);
        //fs.Write(imagebytes, 0, imagebytes.Length);
        ////每次读取文件后都要记得关闭文件
        //fs.Close();
    }
}
