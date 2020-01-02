using System;
using System.IO;

using Live2D.Cubism.Core;
using Live2D.Cubism.Rendering;
using Live2D.Cubism.Framework.Json;
using Live2D.Cubism.Framework.LookAt;
using Live2D.Cubism.Framework.Expression;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework
{
    class Live2dModel : MonoBehaviour
    {
        public CubismModel model;

        private Camera live2DCamera;
        //private RawImage image;

        public int depth;
        public string modelName;

        /// <summary>
        /// 读取模型prefab
        /// </summary>
        /// <param name="modelName"></param>
        public void LoadModel(string modelName, GameObject l2dCon)
        {
            LoadModelWithCamera(modelName);
            return;
            // 形成自带相机
            GameObject go = new GameObject();
            go.transform.SetParent(l2dCon.transform, false);
            live2DCamera = go.AddComponent<Camera>();
            load(modelName);
        }

        /// <summary>
        /// 模型自带相机
        /// </summary>
        /// <param name="modelName"></param>
        public void LoadModelWithCamera(string modelName)
        {
            this.modelName = modelName;
            // 形成自带相机
            GameObject go = new GameObject();
            go.transform.SetParent(this.transform, false);
            live2DCamera = go.AddComponent<Camera>();
            load(modelName);
        }

        public void ChangeExpress(int i)
        {
            model.GetComponent<CubismExpressionController>().CurrentExpressionIndex = i;
        }

        private void load(string modelName)
        {
            string fullPath = "Live2dmodel/" + modelName;
            //Debug.Log(fullPath);
            GameObject md = Resources.Load<GameObject>(fullPath);
            if(md == null)
            {
                string err = string.Format("Live2d Model NOT Found! path:{0}", fullPath);
                Debug.LogError(err);
                return;
            }
            md = Instantiate(md);
            md.transform.SetParent(live2DCamera.transform);
            md.transform.localPosition = new Vector3(0, 0, -10f * depth);
            model =  md.GetComponent<CubismModel>();
            setCamera();
            setRenderTexture();
            //SetLookAt();
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
            RawImage image = gameObject.GetComponent<RawImage>();
            var w = model.CanvasInformation.CanvasWidth;// 2400;
            var h = model.CanvasInformation.CanvasHeight;// 3200;
            RenderTexture target = new RenderTexture((int)w, (int)h, (int)Screen.dpi, RenderTextureFormat.ARGB32);
            live2DCamera.targetTexture = target;
            image.texture = target;
            //そのままだと大きすぎるので
            image.SetNativeSize();
            image.rectTransform.localScale = Vector3.one;
            image.rectTransform.localPosition = Vector3.zero;
            image.SetNativeSize();
            //image.rectTransform.sizeDelta = new Vector2(w / 6f, h / 6f);
        }

        private void SetLookAt()
        {
            CubismLookController la = model.gameObject.AddComponent<CubismLookController>();
            la.Target = GameObject.Find("UI Root/Cursor").gameObject;
        }

    }
}
