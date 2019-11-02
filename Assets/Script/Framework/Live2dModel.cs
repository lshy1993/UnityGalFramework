using System;
using System.IO;

using Live2D.Cubism.Core;
using Live2D.Cubism.Rendering;
using Live2D.Cubism.Framework.Json;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework
{
    class Live2dModel : MonoBehaviour
    {
        private CubismModel model;
        private Camera live2DCamera;
        public RawImage image;

        private void Start()
        {
            
        }

        public void LoadModel(string modelName)
        {
            string fullPath = Application.dataPath + "/Resources/Live2dmodel/" + modelName + ".model3.json";
            CubismModel3Json ModelJson = CubismModel3Json.LoadAtPath(fullPath, LoadAsset);
            model = ModelJson.ToModel();

            //string fullPath = "Prefab/" + modelName + ".prefab";
            //Debug.Log(fullPath);
            //GameObject go = Resources.Load(fullPath) as GameObject;
            ////model =  as CubismModel;
            //model = Instantiate(model).GetComponent<CubismModel>();

            GameObject go = GameObject.Find("Live2dCamera");
            live2DCamera = go.GetComponent<Camera>();
            RuntimeAnimatorController rc = Resources.Load("Live2dmodel/ming/motions/test.controller") as RuntimeAnimatorController;
            
            Debug.Log(Resources.Load("Live2dmodel/mingmeng_winter.controller"));
            model.GetComponent<Animator>().runtimeAnimatorController = rc;
            model.transform.SetParent(go.transform);
            model.transform.localScale = new Vector3(1, 1, 1);
            model.transform.localPosition = new Vector3(0, 0, 10f)
                ;
            //setCamera();
            setRenderTexture();
        }

        public static object LoadAsset(Type assetType, string absolutePath)
        {
            if (assetType == typeof(byte[]))
            {
                return File.ReadAllBytes(absolutePath);
            }
            else if (assetType == typeof(string))
            {
                return File.ReadAllText(absolutePath);
            }
            else if (assetType == typeof(Texture2D))
            {
                var texture = new Texture2D(1, 1);
                texture.LoadImage(File.ReadAllBytes(absolutePath));
                return texture;
            }
            // Fail hard.
            throw new NotSupportedException();
        }


        void setCamera()
        {
            live2DCamera.orthographic = true;
            var modelBounds = model
                    .GetComponent<CubismRenderController>()
                    .Renderers
                    .GetMeshRendererBounds();
            live2DCamera.transform.position = new Vector3(0f, 0f, -10f);
            live2DCamera.orthographicSize = modelBounds.extents.y * 1.1f;
            //live2DCamera.cullingMask = 0;
            live2DCamera.nearClipPlane = -1;
        }

        
        void setRenderTexture()
        {
            image = gameObject.AddComponent<RawImage>();
            var w = 2400;// model.CanvasInformation.CanvasWidth;
            var h = 3200;//model.CanvasInformation.CanvasHeight;
            var target = new RenderTexture((int)w, (int)h, (int)Screen.dpi, RenderTextureFormat.ARGB32);
            live2DCamera.targetTexture = target;
            image.texture = target;
            //そのままだと大きすぎるので
            image.SetNativeSize();
            image.rectTransform.localScale = Vector3.one;
            image.rectTransform.localPosition = Vector3.zero;
            //image.rectTransform.sizeDelta = new Vector2(w / 4f, h / 4f);
        }

    }
}
