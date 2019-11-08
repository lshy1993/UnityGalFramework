using System;
using System.IO;

using Live2D.Cubism.Core;
using Live2D.Cubism.Rendering;
using Live2D.Cubism.Framework.Json;
using Live2D.Cubism.Framework.Expression;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework
{
    class Live2dModel : MonoBehaviour
    {
        private CubismModel model;
        private Camera live2DCamera;
        public RawImage image;
        public int depth;

        private void Start()
        {
            
        }

        public void LoadModel(string modelName)
        {
            GameObject camera = GameObject.Find("Live2dCamera");
            live2DCamera = camera.GetComponent<Camera>();
            model = load(modelName);
            setRenderTexture();
        }

        public void LoadModelWithCamera(string modelName)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(this.transform);
            live2DCamera = go.AddComponent<Camera>();
            model = load(modelName);
            setCamera();
            setRenderTexture();
        }

        CubismModel load(string modelName)
        {
            string fullPath = "Live2dmodel/" + modelName;
            //Debug.Log(fullPath);
            GameObject md = Resources.Load(fullPath) as GameObject;
            md = Instantiate(md);
            md.transform.SetParent(live2DCamera.transform, false);
            md.transform.localPosition = new Vector3(-0.3f, 0, -10f * depth);
            return md.GetComponent<CubismModel>();
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


        public void SetAnimation(string aniname)
        {
            RuntimeAnimatorController rc = Resources.Load("Live2dmodel/" + aniname) as RuntimeAnimatorController;
            //Debug.Log(rc);
            model.GetComponent<Animator>().runtimeAnimatorController = rc;
        }

        public void SetExpressionList(string listname)
        {
            CubismExpressionList el = Resources.Load("Live2dmodel/"+listname) as CubismExpressionList;
            model.GetComponent<CubismExpressionController>().ExpressionsList = el;
        }

        /// <summary>
        /// 挂载live2d相机
        /// </summary>
        private void setCamera()
        {
            live2DCamera.orthographic = true;
            var modelBounds = model
                    .GetComponent<CubismRenderController>()
                    .Renderers
                    .GetMeshRendererBounds();
            Debug.Log(model.CanvasInformation.CanvasHeight);
            Debug.Log(model.CanvasInformation.CanvasWidth);

            Debug.Log(modelBounds.size);
            live2DCamera.transform.position = new Vector3(0f, 0f, -10f);
            //live2DCamera.orthographicSize = modelBounds.extents.y * 1.1f;
            live2DCamera.orthographicSize = 1.2f;
            live2DCamera.cullingMask = 1 << 8;
            live2DCamera.nearClipPlane = -10 * depth - 5;
            live2DCamera.farClipPlane = -10 * depth;
        }

        
        private void setRenderTexture()
        {
            image = gameObject.AddComponent<RawImage>();
            var w = model.CanvasInformation.CanvasWidth;// 2400;
            var h = model.CanvasInformation.CanvasHeight;// 3200;
            var target = new RenderTexture((int)w, (int)h, (int)Screen.dpi, RenderTextureFormat.ARGB32);
            live2DCamera.targetTexture = target;
            image.texture = target;
            //そのままだと大きすぎるので
            image.SetNativeSize();
            image.rectTransform.localScale = Vector3.one;
            image.rectTransform.localPosition = Vector3.zero;
            image.rectTransform.sizeDelta = new Vector2(w / 4f, h / 4f);
        }

    }
}
