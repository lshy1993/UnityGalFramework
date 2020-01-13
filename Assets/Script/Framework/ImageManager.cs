using System.Collections;
using System.Collections.Generic;
using System;

using Assets.Script.Framework.Effect;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

using DG.Tweening;

namespace Assets.Script.Framework
{
    /**
     * ImageManager: 
     * 整个游戏只允许一个，作为GameManager的组件，不能删除
     * 图像处理的方法集合，供其他Manager调用
     * 控制 立绘 与 背景 的切换等等
     */

    //public delegate void MyEffect(UIAnimationCallback callback);

    public class ImageManager : MonoBehaviour
    {
        private const string BG_PATH = "Background/";
        private const string CHARA_PATH = "Character/";
        private const string CG_PATH = "CG/";

        public static readonly Vector3 LEFT = new Vector3(-300, 0, 0);
        public static readonly Vector3 MIDDLE = new Vector3(0, 0, 0);
        public static readonly Vector3 RIGHT = new Vector3(300, 0, 0);

        public GameObject bgPanel, fgPanel;
        public GameObject l2dPanel;
        public MessageUIManager duiManager;

        public Camera mainCam;

        private DataManager dm;

        /// <summary>
        /// 背景图层
        /// </summary>
        //private GameObject bgSprite;
        //private GameObject mvSprite;

        /// <summary>
        /// 背景渐变层
        /// </summary>
        private Image backTransSprite;

        /// <summary>
        /// 储存前景信息【层号，信息】
        /// </summary>
        public Dictionary<int, SpriteState> spriteDic
        {
            get { return DataManager.GetInstance().gameData.bgSprites; }
            set { DataManager.GetInstance().gameData.bgSprites = value; }
        }

        /// <summary>
        /// 临时储存需要一起渐变的图层效果
        /// 【层号，转换效果】
        /// </summary>
        private Dictionary<int, NewImageEffect> transList;

        /// <summary>
        /// 是否处于快进状态
        /// </summary>
        private bool isFast = false;

        void Awake()
        {
            dm = DataManager.GetInstance();
            //bgSprite = bgPanel.transform.Find("BackGround_Sprite").gameObject;
            //mvSprite = bgPanel.transform.Find("BackGround_Movie").gameObject;
            //backTransSprite = bgPanel.transform.Find("Trans_Sprite").gameObject.GetComponent<Image>();
            transList = new Dictionary<int, NewImageEffect>();
        }

        public void SetFast(bool fast) { isFast = fast; }

        public Sprite LoadImage(string fullname)
        {
            Debug.Log(fullname);
            return Resources.Load<Sprite>(fullname);
        }

        /// <summary>
        /// 新建图层
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="recovered">是否需要复原</param>
        private GameObject NewSpriteLayer(NewImageEffect effect)
        {
            int depth = effect.depth;
            // 直接新建视频模板或者图片
            string prefab = effect.movie ? "Prefab/Video_Sprite" : "Prefab/Sprite";
            GameObject go = Resources.Load(prefab) as GameObject;
            go = Instantiate(go);
            go.transform.SetParent(depth < 0 ? bgPanel.transform : fgPanel.transform, false);
            go.transform.name = "sprite" + depth;
            go.transform.SetAsFirstSibling();
            return go;
        }

        private GameObject RenewSpriteLayer(NewImageEffect effect)
        {
            GameObject go = GetSpriteByDepth(effect.depth);
            int sbi = go.transform.GetSiblingIndex();
            float alpha = go.GetComponent<CanvasGroup>().alpha;
            DestroyImmediate(go);
            go = NewSpriteLayer(effect);
            go.transform.SetSiblingIndex(sbi);
            go.GetComponent<CanvasGroup>().alpha = alpha;
            return go;
        }

        /// <summary>
        /// 获取某一层的图片
        /// </summary>
        /// <param name="depth">层编号</param>
        private GameObject GetSpriteByDepth(int depth)
        {
            Transform tr;
            if (depth < 0)
            {
                tr = bgPanel.transform.Find("sprite" + depth);
            }
            else
            {
                tr = fgPanel.transform.Find("sprite" + depth);
            }
            if(tr != null)
            {
                return tr.gameObject;
            }
            return null;
        }

        /// <summary>
        /// 获取某一层的渐变层
        /// </summary>
        /// <param name="depth">层编号</param>
        private GameObject GetTransByDepth(int depth)
        {
            Transform tr;
            if (depth < 0)
            {
                tr = bgPanel.transform.Find("trans" + depth);
            }
            else
            {
                tr = fgPanel.transform.Find("trans" + depth);
            }
            if (tr == null)
            {
                return null;
            }
            return tr.gameObject;
        }

        public GameObject NewLive2dObject(int depth)
        {
            if (fgPanel.transform.Find("sprite" + depth) != null)
            {
                DestroyImmediate(fgPanel.transform.Find("sprite" + depth).gameObject);
            }
            GameObject go = new GameObject();
            go.transform.SetParent(fgPanel.transform, false);
            go.transform.name = "sprite" + depth;
            go.AddComponent<Live2dModel>();
            go.AddComponent<RawImage>();
            return go;
        }

        /// <summary>
        /// 获取某一live2d层
        /// </summary>
        /// <param name="depth">层编号</param>
        public GameObject GetLive2dObject(int depth)
        {
            Transform tr = fgPanel.transform.Find("sprite" + depth);
            if (tr == null)
            {
                return NewLive2dObject(depth);
            }
            return tr.gameObject;
            //由预设生成一个新的texture
            //GameObject go = Resources.Load("Prefab/Live2DTexture") as GameObject;
            //go = NGUITools.AddChild(fgPanel.gameObject, go);
            //go.transform.name = "Texture" + depth;
            //ui = go.GetComponent<UITexture>();
        }

        /// <summary>
        /// 移除某一层
        /// </summary>
        /// <param name="depth">层编号</param>
        private void RemoveSpriteByDepth(NewImageEffect effect)
        {
            GameObject go = GetSpriteByDepth(effect.depth);
            if(go != null)
            {
                if (effect.delete)
                {
                    // 完全删除
                    DestroyImmediate(go);
                }
                else
                {
                    // 仅将image source删除
                    Image im = go.GetComponent<Image>();
                    if (im != null) im.sprite = null;
                    VideoSprite vs = go.GetComponent<VideoSprite>();
                    if (vs != null) vs.ClearClip();
                }
            }
        }

        private Vector3 SetDefaultPos(GameObject ui, string pstr)
        {
            float x;
            switch (pstr)
            {
                case "left":
                    x = -320;
                    break;
                case "middle":
                    x = 0;
                    break;
                case "right":
                    x = 320;
                    break;
                default:
                    x = 0;
                    break;
            }
            float y = -540 + (ui.GetComponent<RectTransform>().rect.height / 2);
            return new Vector3(x, y);
        }

        /// <summary>
        /// 主特效运行函数
        /// </summary>
        /// <param name="effect">特效参数</param>
        /// <param name="callback">回调</param>
        public void Run(NewImageEffect effect, Action callback)
        {
            spriteDic = SaveImageInfo();
            Debug.Log(effect.ToDebugString());
            //操作模式
            switch (effect.operate)
            {
                case NewImageEffect.OperateMode.Set:
                    SetImage(effect);
                    callback();
                    break;
                case NewImageEffect.OperateMode.SetAlpha:
                    SetImageAlpha(effect);
                    callback();
                    break;
                case NewImageEffect.OperateMode.SetPos:
                    SetImagePos(effect);
                    callback();
                    break;
                case NewImageEffect.OperateMode.SetRotate:
                    SetImageRotate(effect);
                    callback();
                    break;
                case NewImageEffect.OperateMode.Trans:
                    RunTrans(effect, callback);
                    break;
                case NewImageEffect.OperateMode.PreTrans:
                    StartCoroutine(PreTransByDepth(effect, callback));
                    break;
                case NewImageEffect.OperateMode.TransAll:
                    StartCoroutine(TransAll(effect.time, callback));
                    break;
                case NewImageEffect.OperateMode.Remove:
                    RemoveSpriteByDepth(effect);
                    callback();
                    break;
                case NewImageEffect.OperateMode.Wait:
                    StartCoroutine(Wait(effect, callback));
                    break;
                case NewImageEffect.OperateMode.Effect:
                    RunEffect(effect, callback);
                    break;
                case NewImageEffect.OperateMode.Action:
                    RunAction(effect, callback);
                    break;
                case NewImageEffect.OperateMode.Live2d:
                    RunLive2d(effect, callback);
                    break;
                case NewImageEffect.OperateMode.Gallery:
                    dm.UnlockGallery(effect.state.spriteName);
                    callback();
                    break;
            }
        }
        private void RunTrans(NewImageEffect effect, Action callback)
        {
            switch (effect.transmode)
            {
                case NewImageEffect.TransMode.Normal:
                    //ImageSet(effect);
                    StartCoroutine(TransByDepth(effect, callback));
                    break;
                case NewImageEffect.TransMode.CrossFade:
                    StartCoroutine(Fade(effect,callback));
                    break;
                case NewImageEffect.TransMode.Universial:
                    StartCoroutine(Universial(effect, callback));
                    break;
                case NewImageEffect.TransMode.Shutter:
                    StartCoroutine(Shutter(effect, callback));
                    break;
                case NewImageEffect.TransMode.Scroll:
                    StartCoroutine(Scroll(effect, false, callback));
                    break;
                case NewImageEffect.TransMode.ScrollBoth:
                    StartCoroutine(Scroll(effect, true, callback));
                    break;
                case NewImageEffect.TransMode.Circle:
                    StartCoroutine(Circle(effect, callback));
                    break;
                case NewImageEffect.TransMode.RotateFade:
                    StartCoroutine(RotateFade(effect, callback));
                    break;
                case NewImageEffect.TransMode.SideFade:
                    StartCoroutine(SideFade(effect, callback));
                    break;
            }
        }
        private void RunEffect(NewImageEffect effect, Action callback)
        {
            switch (effect.effectmode)
            {
                case NewImageEffect.EffectMode.Blur:
                    StartCoroutine(Blur(effect, true, callback));
                    break;
                case NewImageEffect.EffectMode.Mask:
                    StartCoroutine(Mask(effect, callback));
                    break;
                case NewImageEffect.EffectMode.Mosaic:
                    StartCoroutine(Mosaic(effect, callback));
                    break;
                case NewImageEffect.EffectMode.Gray:
                    StartCoroutine(Gray(effect, callback));
                    break;
                case NewImageEffect.EffectMode.OldPhoto:
                    StartCoroutine(OldPhoto(effect, callback));
                    break;

            }
        }
        private void RunAction(NewImageEffect effect, Action callback)
        {
            switch (effect.actionmode)
            {
                case NewImageEffect.ActionMode.Fade:
                    ImageFade(effect, callback);
                    break;
                case NewImageEffect.ActionMode.Move:
                    if (effect.sync)
                    {
                        //StartCoroutine(Move(effect, () => { }));
                        TweenMove(effect);
                        callback();
                    }
                    else
                    {
                        StartCoroutine(Move(effect, callback));
                    }
                    break;
                case NewImageEffect.ActionMode.Rotate:
                    StartCoroutine(Rotate(effect, callback));
                    break;
                case NewImageEffect.ActionMode.Scale:
                    StartCoroutine(Scale(effect, callback));
                    break;
                case NewImageEffect.ActionMode.WinShake:
                    StartCoroutine(WinShake(effect, callback));
                    break;
                case NewImageEffect.ActionMode.Shake:
                    StartCoroutine(Shake(effect, callback));
                    break;

            }
        }

        private void RunLive2d(NewImageEffect effect, Action callback)
        {
            switch (effect.l2dmode)
            {
                case NewImageEffect.Live2dMode.SetLive2D:
                    SetLive2dModel(effect);
                    callback();
                    break;
                case NewImageEffect.Live2dMode.ChangeMotion:
                    //SetLive2dExpression(effect);
                    callback();
                    break;
                case NewImageEffect.Live2dMode.ChangeExpression:
                    SetLive2dExpression(effect);
                    callback();
                    break;
            }
        }

        #region live2d相关
        private void SetLive2dModel(NewImageEffect effect)
        {
            GameObject go = NewLive2dObject(effect.depth);
            go.GetComponent<Live2dModel>().depth = effect.depth;
            go.GetComponent<Live2dModel>().LoadModel(effect.state.spriteName, l2dPanel);
            //go.GetComponent<Live2dModel>().LoadModelWithCamera(effect.state.spriteName);
            RawImage ui = go.GetComponent<RawImage>();
            ui.material = null;
        }

        private void SetLive2dExpression(NewImageEffect effect)
        {
            GameObject go = GetLive2dObject(effect.depth);
            go.GetComponent<Live2dModel>().ChangeExpress(effect.freq);
        }

        #endregion

        /// <summary>
        /// 新建图片
        /// </summary>
        /// <param name="effect"></param>
        private void SetImage(NewImageEffect effect)
        {
            // 获取图层 不存在则从模板新建
            GameObject go = GetSpriteByDepth(effect.depth);
            if (go == null) go = NewSpriteLayer(effect);
            // 判断是否需要替换
            VideoSprite vs = go.GetComponent<VideoSprite>();
            Image im = go.GetComponent<Image>();
            if (effect.movie)
            {
                if(vs == null) go = RenewSpriteLayer(effect);
                // 是动态 RawImage
                go.GetComponent<VideoSprite>().LoadClip(effect.state.spriteName);
            }
            else
            {
                if (im == null) go = RenewSpriteLayer(effect);
                // 静态图层
                go.GetComponent<Image>().sprite = LoadImage(GetFullPath(effect));
                go.GetComponent<Image>().SetNativeSize();
            }
            // 材质
            go.GetComponent<MaskableGraphic>().material = null;
        }

        private string GetFullPath(NewImageEffect effect)
        {
            string path = effect.state.path == -1 ? CG_PATH : effect.state.path == 0 ? BG_PATH : "";
            return path + effect.state.spriteName;
        }

        private void SetImageAlpha(NewImageEffect effect)
        {
            GameObject go = GetSpriteByDepth(effect.depth);
            // 初始alpha
            go.GetComponent<MaskableGraphic>().color = new Color(1, 1, 1, effect.state.spriteAlpha);
        }

        private void SetImagePos(NewImageEffect effect)
        {
            GameObject go = GetSpriteByDepth(effect.depth);
            Vector3 pos = string.IsNullOrEmpty(effect.defaultpos) ?
                effect.state.GetPosition() : SetDefaultPos(go, effect.defaultpos);
            //go.GetComponent<RectTransform>().anchoredPosition = pos;
            go.transform.localPosition = pos;
        }

        private void SetImageRotate(NewImageEffect effect)
        {
            GameObject go = GetSpriteByDepth(effect.depth);
            go.transform.localRotation = Quaternion.Euler(effect.angle);
        }


        private void ImageFade(NewImageEffect effect, Action callback)
        {
            if (effect.target == NewImageEffect.ImageType.All)
            {
                StartCoroutine(FadeAll(effect, callback, true, true));
            }
            else if (effect.target == NewImageEffect.ImageType.AllChara)
            {
                StartCoroutine(FadeAll(effect, callback, false, false));
            }
            else if (effect.target == NewImageEffect.ImageType.AllPic)
            {
                StartCoroutine(FadeAll(effect, callback, true, false));
            }
            else
            {
                StartCoroutine(Fade(effect, callback));
            }
        }

        #region 背景 shader 特效相关
        private IEnumerator Blur(NewImageEffect effect, bool isBlur, Action callback)
        {
            //设置shader
            MaskableGraphic ui = GetSpriteByDepth(effect.depth).GetComponent<MaskableGraphic>();
            ui.material = Resources.Load("Material/Blur") as Material;
            //ui.material = new Material(Shader.Find("Custom/Blur"));
            //效果
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / 0.1f * Time.deltaTime);
                float value = isBlur ? t * 0.005f : (1 - t) * 0.005f;
                //Debug.Log(value);
                ui.material.SetFloat("uvOffset", value);
                yield return null;
            }
            callback();
        }

        private IEnumerator Mask(NewImageEffect effect, Action callback)
        {
            yield return null;
        }

        private IEnumerator Mosaic(NewImageEffect effect, Action callback)
        {
            //设置shader
            MaskableGraphic ui = GetSpriteByDepth(effect.depth).GetComponent<MaskableGraphic>();
            ui.material = Resources.Load("Material/Mosaic") as Material;
            yield return null;
            callback();
        }

        private IEnumerator Gray(NewImageEffect effect, Action callback)
        {
            //设置shader
            MaskableGraphic ui = GetSpriteByDepth(effect.depth).GetComponent<MaskableGraphic>();
            ui.material = Resources.Load("Material/Gray") as Material;
            yield return null;
            callback();
        }

        private IEnumerator OldPhoto(NewImageEffect effect, Action callback)
        {
            //设置shader
            MaskableGraphic ui = GetSpriteByDepth(effect.depth).GetComponent<MaskableGraphic>();
            ui.material = Resources.Load("Material/OldPhoto") as Material;
            yield return null;
            callback();
        }

        #endregion

        /// <summary>
        /// 获取方向四元向量
        /// </summary>
        /// <param name="direction">方向枚举</param>
        private Vector4 GetDirectVector(int direction)
        {
            switch (direction)
            {
                case 0:
                    //"left"
                    return new Vector4(1, 0, 0, 0);
                case 1:
                    //"right"
                    return new Vector4(-1, 0, 0, 0);
                case 2:
                    //"top"
                    return new Vector4(0, 1, 0, 0);
                case 3:
                    //"bottom"
                    return new Vector4(0, -1, 0, 0);
                default:
                    return new Vector4(1, 0, 0, 0);
            }
        }

        #region 渐变Trans相关

        /// <summary>
        /// 等待所有Trans完成
        /// </summary>
        private IEnumerator TransAll(float t, Action callback)
        { 
            foreach (KeyValuePair<int, NewImageEffect> kv in transList)
            {
                Debug.Log(kv.Key);
                kv.Value.time = t;
                StartCoroutine(TransByDepth(kv.Value, () => { }));
            }
            yield return new WaitForSeconds(t);
            // 清空list
            transList.Clear();
            callback();
        }

        /// <summary>
        /// 渐变预处理
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="callback"></param>
        private IEnumerator PreTransByDepth(NewImageEffect effect, Action callback)
        {
            //向待渐变TransList添加
            if (transList.ContainsKey(effect.depth))
            {
                string msg = string.Format("发现未完成的渐变，已替换图层{0}的渐变", effect.depth);
                Debug.LogWarning(msg);
                transList[effect.depth] = effect;
            }
            else
            {
                transList.Add(effect.depth, effect);
            }
            GameObject origin = GetSpriteByDepth(effect.depth);
            if(origin == null)
            {
                // 没有原图的时候
                Debug.LogWarning("未发现精灵，depth: " + effect.depth);
                // 创建隐藏的原图层用于后面展示
                origin = NewSpriteLayer(effect);
                origin.GetComponent<CanvasGroup>().alpha = 0;
                origin.transform.SetAsFirstSibling();
            }
            else
            {
                GameObject trans = GetTransByDepth(effect.depth);
                if (trans == null)
                {
                    Debug.Log("将原图复制成trans");
                    //复制一层变成Trans
                    trans = Instantiate(origin.gameObject) as GameObject;
                    trans.transform.SetParent(origin.transform.parent, false);
                    trans.transform.localPosition = origin.transform.localPosition;
                    trans.transform.localScale = origin.transform.localScale;
                    trans.transform.name = "trans" + effect.depth;
                }
                // 渐变层遮挡
                trans.transform.SetAsLastSibling();
                // 原图隐藏
                origin.GetComponent<CanvasGroup>().alpha = 0;
                origin.transform.SetAsFirstSibling();
            }

            yield return null;

            callback();
        }


        /// <summary>
        /// 单一渐变图层（-1为背景层）
        /// </summary>
        /// <param name="effect">特效</param>
        /// <param name="callback">回调</param>
        private IEnumerator TransByDepth(NewImageEffect effect, Action callback)
        {
            GameObject origin = GetSpriteByDepth(effect.depth);
            GameObject trans = GetTransByDepth(effect.depth);
            bool flag1 = origin != null;
            bool flag2 = trans != null;
            //将trans淡出同时淡入原ui
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                if (flag1)
                {
                    //originSprite.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, t));
                    origin.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, t);
                }
                if (flag2)
                {
                    //transSprite.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, t));
                    trans.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, t);
                }
                yield return null;
            }
            //删除trans
            DeleteTransLayer(trans, effect);
            callback();
        }

        private IEnumerator RotateFade(NewImageEffect effect, Action callback)
        {
            GameObject origin = GetSpriteByDepth(effect.depth);
            GameObject trans = GetTransByDepth(effect.depth);
            MaskableGraphic originSprite = origin.GetComponent<MaskableGraphic>();
            MaskableGraphic transSprite = trans.GetComponent<MaskableGraphic>();
            //设置shader
            transSprite.material = Resources.Load("Material/RotateFade") as Material;
            //Texture2D te = LoadBackground(effect.state.spriteName).texture;
            Texture2D te = LoadImage(GetFullPath(effect)).texture;
            transSprite.material.SetTexture("_NewTex", te);
            //正反向
            transSprite.material.SetInt("inverse", effect.inverse ? 1 : 0);
            //效果
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                transSprite.material.SetFloat("currentT", t);
                yield return null;
            }
            DeleteTransLayer(trans, effect);
            callback();
        }

        private IEnumerator SideFade(NewImageEffect effect, Action callback)
        {
            GameObject origin = GetSpriteByDepth(effect.depth);
            GameObject trans = GetTransByDepth(effect.depth);
            MaskableGraphic originSprite = origin.GetComponent<MaskableGraphic>();
            MaskableGraphic transSprite = trans.GetComponent<MaskableGraphic>();
            //设置shader
            transSprite.material = Resources.Load("Material/SideFade") as Material;
            //Texture2D te = LoadBackground(effect.state.spriteName).texture;
            Texture2D te = LoadImage(GetFullPath(effect)).texture;
            transSprite.material.SetTexture("_NewTex", te);
            transSprite.material.SetInt("_Direction", (int)effect.direction);
            //效果
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                transSprite.material.SetFloat("currentT", t);
                yield return null;
            }
            DeleteTransLayer(trans, effect);
            callback();
        }

        private IEnumerator Circle(NewImageEffect effect, Action callback)
        {
            GameObject origin = GetSpriteByDepth(effect.depth);
            GameObject trans = GetTransByDepth(effect.depth);
            MaskableGraphic originSprite = origin.GetComponent<MaskableGraphic>();
            MaskableGraphic transSprite = trans.GetComponent<MaskableGraphic>();
            //设置shader
            transSprite.material = Resources.Load("Material/CircleHole") as Material;
            //ui.material = new Material(Shader.Find("Custom/CircleHole"));
            Debug.Log(effect.ToString());
            //Texture2D te = LoadBackground(effect.state.spriteName).texture;
            Texture2D te = LoadImage(GetFullPath(effect)).texture;
            transSprite.material.SetTexture("_NewTex", te);
            //正反向
            transSprite.material.SetInt("inverse", effect.inverse ? 1 : 0);
            //效果
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                transSprite.material.SetFloat("currentT", t);
                yield return null;
            }
            DeleteTransLayer(trans, effect);
            callback();
        }

        private IEnumerator Scroll(NewImageEffect effect, bool isBoth, Action callback)
        {
            GameObject origin = GetSpriteByDepth(effect.depth);
            GameObject trans = GetTransByDepth(effect.depth);
            MaskableGraphic originSprite = origin.GetComponent<MaskableGraphic>();
            MaskableGraphic transSprite = trans.GetComponent<MaskableGraphic>();
            //设置shader
            transSprite.material = Resources.Load("Material/ScrollBoth") as Material;
            //Texture2D te = LoadBackground(effect.state.spriteName).texture;
            Texture2D te = LoadImage(GetFullPath(effect)).texture;
            transSprite.material.SetTexture("_NewTex", te);
            transSprite.material.SetInt("_Direction", (int)effect.direction);
            //效果
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                transSprite.material.SetFloat("currentT", t);
                yield return null;
            }
            DeleteTransLayer(trans, effect);
            callback();
        }

        /// <summary>
        /// 百叶窗
        /// </summary>
        private IEnumerator Shutter(NewImageEffect effect, Action callback)
        {
            GameObject origin = GetSpriteByDepth(effect.depth);
            GameObject trans = GetTransByDepth(effect.depth);
            MaskableGraphic originSprite = origin.GetComponent<MaskableGraphic>();
            MaskableGraphic transSprite = trans.GetComponent<MaskableGraphic>();
            //设置shader
            transSprite.material = Resources.Load("Material/Shutter") as Material;
            //Texture2D te = LoadBackground(effect.state.spriteName).texture;
            Texture2D te = LoadImage(GetFullPath(effect)).texture;
            transSprite.material.SetTexture("_NewTex", te);
            transSprite.material.SetInt("_Direction", (int)effect.direction);
            //效果
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                transSprite.material.SetFloat("currentT", t);
                yield return null;
            }
            DeleteTransLayer(trans, effect);
            callback();
        }

        /// <summary>
        /// 通用渐变
        /// </summary>
        private IEnumerator Universial(NewImageEffect effect, Action callback)
        {
            GameObject origin = GetSpriteByDepth(effect.depth);
            GameObject trans = GetTransByDepth(effect.depth);
            MaskableGraphic originSprite = origin.GetComponent<MaskableGraphic>();
            MaskableGraphic transSprite = trans.GetComponent<MaskableGraphic>();
            //设置shader
            transSprite.material = Resources.Load("Material/Mask") as Material;
            //Texture2D te = LoadBackground(effect.state.spriteName).texture;
            Texture2D te = LoadImage(GetFullPath(effect)).texture;
            transSprite.material.SetTexture("_NewTex", te);
            Texture2D mk = LoadImage("Rule/" + effect.maskImage).texture;
            transSprite.material.SetTexture("_MaskTex", mk);
            //效果
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                transSprite.material.SetFloat("currentT", t);
                yield return null;
            }
            DeleteTransLayer(trans, effect);
            callback();
        }

        private void DeleteTransLayer(GameObject trans,NewImageEffect effect)
        {
            Destroy(trans);
            if (transList.ContainsKey(effect.depth))
            {
                transList.Remove(effect.depth);
            }
        }
        #endregion

        private IEnumerator Fade(NewImageEffect effect, Action callback)
        {
            GameObject go = GetSpriteByDepth(effect.depth);

            MaskableGraphic ui = go.GetComponent<MaskableGraphic>();
            float origin = ui.color.a;
            float final = effect.state.spriteAlpha;

            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                ui.color = new Color(1, 1, 1, Mathf.Lerp(origin, final, t));
                yield return null;
            }
            callback();
        }

        private void TweenMove(NewImageEffect effect)
        {
            GameObject ui = GetSpriteByDepth(effect.depth);
            ui.GetComponent<SpriteTweener>().Move(effect);
            //ui.transform.DOLocalMove(effect.state.GetPosition(), effect.time)
            //    .SetEase(Ease.Linear);
        }

        private IEnumerator Move(NewImageEffect effect, Action callback)
        {
            GameObject ui = GetSpriteByDepth(effect.depth);
            Vector3 origin = ui.transform.localPosition;
            Vector3 final = effect.state.GetPosition();
            if (!string.IsNullOrEmpty(effect.defaultpos))
            {
                final = SetDefaultPos(ui.gameObject, effect.defaultpos);
            }
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                ui.transform.localPosition = Vector3.Lerp(origin, final, t);
                yield return null;
            }
            callback();
        }

        private IEnumerator Rotate(NewImageEffect effect, Action callback)
        {
            GameObject ui = GetSpriteByDepth(effect.depth);
            Quaternion origin = ui.transform.localRotation;
            Quaternion final = Quaternion.Euler(effect.angle);
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                ui.transform.localRotation = Quaternion.Lerp(origin, final, t);
                yield return null;
            }
            callback();
        }

        private IEnumerator Scale(NewImageEffect effect, Action callback)
        {
            GameObject ui = GetSpriteByDepth(effect.depth);
            Vector3 origin = ui.transform.localScale;
            Vector3 final = effect.scale;
            if(effect.time == 0)
            {
                ui.transform.localScale = final;
                callback();
            }
            else
            {
                float t = 0;
                while (t < 1)
                {
                    t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                    ui.transform.localScale = Vector3.Lerp(origin, final, t);
                    yield return null;
                }
                callback();
            }

        }

        private IEnumerator Wait(NewImageEffect effect, Action callback)
        {
            yield return new WaitForSeconds(effect.time);
            callback();
        }

        private IEnumerator FadeAll(NewImageEffect effect, Action callback, bool includeBack, bool includeDiabox)
        {
            Dictionary<int, float> originAlpha = new Dictionary<int, float>();
            foreach (int i in GetDepthNum())
            {
                if(i>=0 || includeBack)
                {
                    MaskableGraphic ui = GetSpriteByDepth(i).GetComponent<MaskableGraphic>();
                    originAlpha[i] = ui.color.a;
                }
            }
            if (includeDiabox)
            {
                duiManager.Close(effect.time, () => { });
                originAlpha[-2] = duiManager.mainContainer.GetComponent<CanvasGroup>().alpha;
                duiManager.clickContainer.SetActive(false);
            }
            float t = 0;
            float final = effect.state.spriteAlpha;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                if (includeDiabox)
                {
                    float origin = originAlpha[-2];
                    float alpha = Mathf.Lerp(origin, final, t);
                    duiManager.mainContainer.GetComponent<CanvasGroup>().alpha = alpha;
                }
                foreach (int i in GetDepthNum())
                {
                    if(i>=0 || includeBack)
                    {
                        float alpha = Mathf.Lerp(originAlpha[i], final, t);
                        GetSpriteByDepth(i).GetComponent<MaskableGraphic>().color = new Color(1, 1, 1, alpha);

                    }
                }
                yield return null;
            }
            //删除
            foreach (int i in GetDepthNum())
            {
                DestroyImmediate(GetSpriteByDepth(-1));
            }
            if (includeBack)
            {
                //Destroy(GetSpriteByDepth(-1));
                DestroyImmediate(GetSpriteByDepth(-1));
                //GetSpriteByDepth(-1).GetComponent<Image>().sprite = null;
            }
            //if (includeDiabox)duiManager.mainContainer.SetActive(false);
            callback();
        }

        private IEnumerator Shake(NewImageEffect effect, Action callback)
        {
            //Debug.Log(effect.depth);
            float shakeDelta = effect.v;
            Image sp = GetSpriteByDepth(effect.depth).GetComponent<Image>();
            Vector3 oriPos = sp.transform.localPosition;
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                float x = oriPos.x + shakeDelta * UnityEngine.Random.Range(-1, 1);
                float y = oriPos.y + shakeDelta * UnityEngine.Random.Range(-1, 1);
                sp.transform.localPosition = new Vector3(x, y);
                yield return null;
            }
            sp.transform.localPosition = oriPos;
            yield return null;
            callback();
        }

        private IEnumerator WinShake(NewImageEffect effect, Action callback)
        {
            float shakeDelta = effect.v;
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                mainCam.rect = new Rect(shakeDelta * (-1.0f + 2.0f * UnityEngine.Random.value), shakeDelta * (-1.0f + 2.0f * UnityEngine.Random.value), 1.0f, 1.0f);
                yield return null;
            }
            mainCam.rect = new Rect(0f, 0f, 1.0f, 1.0f);
            callback();
        }

        private List<int> GetDepthNum()
        {
            List<int> nums = new List<int>();
            foreach (Transform child in bgPanel.transform)
            {
                int x = Convert.ToInt32(child.name.Remove(0, 6));
                nums.Add(x);
            }
            foreach (Transform child in fgPanel.transform)
            {
                int x = Convert.ToInt32(child.name.Remove(0, 6));
                nums.Add(x);
            }
            nums.Sort();
            return nums;
        }

        private List<int> GetBackDepthNum()
        {
            List<int> nums = new List<int>();
            foreach (Transform child in bgPanel.transform)
            {
                int x = Convert.ToInt32(child.name.Remove(0, 6));
                nums.Add(x);
            }
            nums.Sort();
            return nums;
        }

        #region 存储 读取 前背景画面
        public Dictionary<int, SpriteState> SaveImageInfo()
        {
            // 字典
            Dictionary<int, SpriteState> charaDic = new Dictionary<int, SpriteState>();
            // 储存当前的背景与立绘信息
            foreach (Transform child in bgPanel.transform)
            {
                if (child.name.Contains("trans")) continue;
                string t = child.name.Replace("sprite", "");
                int depth = Convert.ToInt32(t);
                charaDic.Add(depth, GetSpriteInfo(depth, child));
            }
            foreach (Transform child in fgPanel.transform)
            {
                if (child.name.Contains("trans")) continue;
                string t = child.name.Replace("sprite", "");
                int depth = Convert.ToInt32(t);
                charaDic.Add(depth, GetSpriteInfo(depth, child));
            }
            return charaDic;
        }

        private SpriteState GetSpriteInfo(int depth, Transform child)
        {
            // 精灵信息
            SpriteState ss = new SpriteState();
            MaskableGraphic ui = child.GetComponent<MaskableGraphic>();
            ss.spriteAlpha = ui.color.a;
            ss.SetPosition(child.localPosition);
            ss.SetScale(child.localScale);
            ss.SetRatation(child.localRotation);
            if (child.GetComponent<VideoSprite>() != null)
            {
                ss.SetType(SpriteState.SpriteType.Movie);
                ss.SetSource(child.GetComponent<VideoPlayer>().clip.name);
            }
            else if (child.GetComponent<Live2dModel>() != null)
            {
                ss.SetType(SpriteState.SpriteType.Live2d);
                ss.SetSource(child.GetComponent<Live2dModel>().modelName);
            }
            else
            {
                ss.SetType(SpriteState.SpriteType.Normal);
                Image img = child.GetComponent<Image>();
                ss.SetSource(img.sprite == null ? "" : img.sprite.name);
            }
            return ss;
        }

        public void LoadImageInfo()
        {
            Dictionary<int, SpriteState> spdic = dm.gameData.fgSprites;
            //遍历当前的背景图删除
            foreach (Transform child in bgPanel.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in fgPanel.transform)
            {
                Destroy(child.gameObject);
            }
            //遍历存储字典 替换内容
            foreach (KeyValuePair<int, SpriteState> child in spdic)
            {
                int depth = child.Key;
                SpriteState ss = child.Value;
                GameObject go = GetSpriteByDepth(depth);
                // 如果有则删除 否则直接新建
                if (go != null)
                {
                    DestroyImmediate(go);
                }
                string prefab = "";
                if(ss.type == SpriteState.SpriteType.Live2d)
                {
                    prefab = ss.GetSource();
                    go = GetLive2dObject(depth);
                    go.GetComponent<Live2dModel>().depth = depth;
                    go.GetComponent<Live2dModel>().LoadModel(ss.spriteName, l2dPanel);
                    //go.GetComponent<Live2dModel>().LoadModelWithCamera(ss.spriteName);
                    // TODO: 表情动作
                }
                else if(ss.type == SpriteState.SpriteType.Movie)
                {
                    prefab = "Prefab/Video_Sprite";
                    go = Resources.Load(prefab) as GameObject;
                    go = Instantiate(go);
                    go.transform.SetParent(depth < 0 ? bgPanel.transform : fgPanel.transform, false);
                    go.transform.name = "sprite" + depth;
                    // 是动态 RawImage
                    go.GetComponent<VideoSprite>().LoadClip(ss.spriteName);
                    // TODO: 记录时间？
                }
                else
                {
                    prefab = "Prefab/Sprite";
                    go = Resources.Load(prefab) as GameObject;
                    go = Instantiate(go);
                    go.transform.SetParent(depth < 0 ? bgPanel.transform : fgPanel.transform, false);
                    go.transform.name = "sprite" + depth;
                    Image ui = go.GetComponent<Image>();
                    //ui.sprite = depth < 0 ? LoadBackground(ss.spriteName): LoadImage(ss.spriteName);
                    ui.sprite = LoadImage(ss.spriteName);
                    ui.SetNativeSize();
                }
                //设置位置等
                go.transform.localPosition = ss.GetPosition();
                go.transform.localScale = ss.GetScale();
                go.transform.localRotation = ss.GetRatation();
                go.GetComponent<MaskableGraphic>().color = new Color(1, 1, 1, ss.spriteAlpha);
                go.GetComponent<MaskableGraphic>().material = null;

            }
            // 恢复对话框
            //duiManager.ClearText();
            //duiManager.ShowWindow();
        }
        #endregion

    }
}