using System.Collections;
using System.Collections.Generic;
using System;

using Assets.Script.Framework.Effect;

using UnityEngine;
using UnityEngine.UI;

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

        public static readonly Vector3 LEFT = new Vector3(-300, 0, 0);
        public static readonly Vector3 MIDDLE = new Vector3(0, 0, 0);
        public static readonly Vector3 RIGHT = new Vector3(300, 0, 0);

        public GameObject bgPanel, fgPanel; //, eviPanel;
                                            //public DialogBoxUIManager duiManager;

        public Camera mainCam;

        private DataManager dm;

        /// <summary>
        /// 背景图层
        /// </summary>
        private Image bgSprite;

        /// <summary>
        /// 背景渐变层
        /// </summary>
        private Image backTransSprite;

        /// <summary>
        /// 储存前景信息【层号，信息】
        /// </summary>
        private Dictionary<int, SpriteState> spriteDic;

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
            bgSprite = bgPanel.transform.Find("BackGround_Sprite").gameObject.GetComponent<Image>();
            //backTransSprite = bgPanel.transform.Find("Trans_Sprite").gameObject.GetComponent<Image>();
            spriteDic = new Dictionary<int, SpriteState>();
            transList = new Dictionary<int, NewImageEffect>();
        }

        public void SetFast(bool fast) { isFast = fast; }

        public Sprite LoadImage(string path, string name)
        {
            return Resources.Load<Sprite>(path + name);
        }

        public Sprite LoadBackground(string name)
        {
            return LoadImage(BG_PATH, name);
        }
        public Sprite LoadCharacter(string name)
        {
            return LoadImage(CHARA_PATH, name);
        }


        /// <summary>
        /// 获取某一层的图片
        /// </summary>
        /// <param name="depth">层编号</param>
        private GameObject GetSpriteByDepth(int depth)
        {
            GameObject go;
            if (depth == -1) return bgSprite.gameObject;
            if (fgPanel.transform.Find("sprite" + depth) != null)
            {
                go = fgPanel.transform.Find("sprite" + depth).gameObject;
            }
            else
            {
                go = Resources.Load("Prefab/Character") as GameObject;
                go = Instantiate(go);
                go.transform.SetParent(fgPanel.transform, false);
                //go.transform.localScale = Vector3.one;
                go.transform.name = "sprite" + depth;
            }
            return go;
        }

        /// <summary>
        /// 获取某一层的渐变层
        /// </summary>
        /// <param name="depth">层编号</param>
        private GameObject GetTransByDepth(int depth)
        {
            GameObject go;
            if (depth == -1) return bgSprite.gameObject; //backTransSprite;
            if (fgPanel.transform.Find("trans" + depth) != null)
            {
                go = fgPanel.transform.Find("trans" + depth).gameObject;
            }
            else
            {
                GameObject originSprite = GetSpriteByDepth(depth);
                //复制一个变成Trans
                go = Instantiate(originSprite.gameObject) as GameObject;
                go.transform.SetParent(originSprite.transform.parent, false);
                go.transform.localPosition = originSprite.transform.localPosition;
                go.transform.localScale = originSprite.transform.localScale;
                go.transform.name = "trans" + depth;
            }
            return go;
        }

        /// <summary>
        /// 获取某一live2d层
        /// </summary>
        /// <param name="depth">层编号</param>
        private GameObject GetLive2dObject(int depth)
        {
            GameObject go;
            if (fgPanel.transform.Find("sprite" + depth) != null)
            {
                go = fgPanel.transform.Find("sprite" + depth).gameObject;
            }
            else
            {
                go = new GameObject();
                go.transform.SetParent(fgPanel.transform, false);
                go.transform.name = "sprite" + depth;
                go.AddComponent<Live2dModel>();

                //由预设生成一个新的texture
                //GameObject go = Resources.Load("Prefab/Live2DTexture") as GameObject;
                //go = NGUITools.AddChild(fgPanel.gameObject, go);
                //go.transform.name = "Texture" + depth;
                //ui = go.GetComponent<UITexture>();
            }
            return go;
        }

        /// <summary>
        /// 移除某一层
        /// </summary>
        /// <param name="depth">层编号</param>
        private void RemoveSpriteByDepth(int depth)
        {
            if (fgPanel.transform.Find("sprite" + depth) != null)
            {
                Destroy(fgPanel.transform.Find("sprite" + depth).gameObject);
            }
        }

        private Vector3 SetDefaultPos(Image ui, string pstr)
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
        public void RunEffect(NewImageEffect effect, Action callback)
        {
            //操作模式
            switch (effect.operate)
            {
                case NewImageEffect.OperateMode.SetSprite:
                    ImageSet(effect, true, false, false);
                    callback();
                    break;
                case NewImageEffect.OperateMode.SetAlpha:
                    ImageSet(effect, false, true, false);
                    callback();
                    break;
                case NewImageEffect.OperateMode.SetPos:
                    ImageSet(effect, false, false, true);
                    callback();
                    break;
                case NewImageEffect.OperateMode.Trans:
                    StartCoroutine(TransByDepth(effect, callback));
                    break;
                case NewImageEffect.OperateMode.PreTrans:
                    StartCoroutine(PreTransByDepth(effect, callback));
                    break;
                case NewImageEffect.OperateMode.TransAll:
                    float t = 0;//等待时长
                    foreach (KeyValuePair<int, NewImageEffect> kv in transList)
                    {
                        if (kv.Value.time > t) t = kv.Value.time;
                        StartCoroutine(TransByDepth(kv.Value, () => { }));
                    }
                    StartCoroutine(TransAll(t, callback));
                    break;
                case NewImageEffect.OperateMode.Fade:
                    ImageFade(effect, callback);
                    break;
                case NewImageEffect.OperateMode.Move:
                    StartCoroutine(Move(effect, callback));
                    break;
                case NewImageEffect.OperateMode.Delete:
                    break;
                case NewImageEffect.OperateMode.Wait:
                    StartCoroutine(Wait(effect, callback));
                    break;
                case NewImageEffect.OperateMode.Blur:
                    StartCoroutine(Blur(effect, true, callback));
                    break;
                case NewImageEffect.OperateMode.Shutter:
                    StartCoroutine(Shutter(effect, callback));
                    break;
                case NewImageEffect.OperateMode.Mask:
                    StartCoroutine(Mask(effect, callback));
                    break;
                case NewImageEffect.OperateMode.Scroll:
                    StartCoroutine(Scroll(effect, false, callback));
                    break;
                case NewImageEffect.OperateMode.ScrollBoth:
                    StartCoroutine(Scroll(effect, true, callback));
                    break;
                case NewImageEffect.OperateMode.Circle:
                    StartCoroutine(Circle(effect, callback));
                    break;
                case NewImageEffect.OperateMode.RotateFade:
                    StartCoroutine(RotateFade(effect, callback));
                    break;
                case NewImageEffect.OperateMode.SideFade:
                    StartCoroutine(SideFade(effect, callback));
                    break;
                case NewImageEffect.OperateMode.Mosaic:
                    StartCoroutine(Mosaic(effect, callback));
                    break;
                case NewImageEffect.OperateMode.Gray:
                    StartCoroutine(Gray(effect, callback));
                    break;
                case NewImageEffect.OperateMode.OldPhoto:
                    StartCoroutine(OldPhoto(effect, callback));
                    break;
                case NewImageEffect.OperateMode.WinShake:
                    StartCoroutine(WinShake(effect, callback));
                    break;
                case NewImageEffect.OperateMode.Shake:
                    StartCoroutine(Shake(effect, callback));
                    break;
                case NewImageEffect.OperateMode.SetLive2D:
                    Live2dSpriteSet(effect);
                    callback();
                    break;
                case NewImageEffect.OperateMode.ChangeMotion:
                    break;
            }
        }

        #region live2d相关
        private void Live2dSpriteSet(NewImageEffect effect)
        {
            GameObject go = GetLive2dObject(effect.depth);
            //go.GetComponent<Live2dModel>().LoadModel(effect.state.spriteName);
            go.GetComponent<Live2dModel>().depth = effect.depth;
            go.GetComponent<Live2dModel>().LoadModelWithCamera(effect.state.spriteName);
            RawImage ui = go.GetComponent<RawImage>();
            ui.material = null;
        }

        #endregion
        
        private void ImageSet(NewImageEffect effect, bool isSprite, bool isAlpha, bool isPos)
        {
            //决定操作对象
            
            GameObject go = GetSpriteByDepth(effect.depth);
            Image ui = go.GetComponent<Image>();
            RawImage rui = go.GetComponent<RawImage>();

            //传入了图片名
            if (isSprite)
            {
                if (ui == null && rui != null)
                {
                    // 是动态 RawImage
                }
                else
                {
                    // 静态图层
                    ui.sprite = LoadBackground(effect.state.spriteName);
                    if (effect.target == NewImageEffect.ImageType.Fore)
                    {
                        ui.sprite = LoadCharacter(effect.state.spriteName);
                    }
                    ui.SetNativeSize();
                }
                
            }
            // 材质
            go.GetComponent<MaskableGraphic>().material = null;
            // 初始alpha
            if (isAlpha)
            {
                go.GetComponent<MaskableGraphic>().color = new Color(1, 1, 1, effect.state.spriteAlpha);
                //ui.color = new Color(1, 1, 1, effect.state.spriteAlpha);
                //rui.color = new Color(1, 1, 1, effect.state.spriteAlpha);
            }
            if (isPos)
            {
                Vector3 pos = string.IsNullOrEmpty(effect.defaultpos) ? effect.state.GetPosition() : SetDefaultPos(ui, effect.defaultpos);
                //go.GetComponent<RectTransform>().anchoredPosition = pos;
                go.transform.localPosition = pos;
            }
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
        
        private IEnumerator RotateFade(NewImageEffect effect, Action callback)
        {
            //设置shader
            MaskableGraphic ui = GetSpriteByDepth(effect.depth).GetComponent<MaskableGraphic>();
            ui.material = Resources.Load("Material/RotateFade") as Material;
            //ui.material = new Material(Shader.Find("Custom/RotateFade"));

            Texture2D te = LoadBackground(effect.state.spriteName).texture;
            ui.material.SetTexture("_NewTex", te);
            //正反向
            ui.material.SetInt("inverse", effect.inverse ? 1 : 0);
            //效果
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                ui.material.SetFloat("currentT", t);
                yield return null;
            }
            callback();
        }

        private IEnumerator SideFade(NewImageEffect effect, Action callback)
        {
            //设置shader
            MaskableGraphic ui = GetSpriteByDepth(effect.depth).GetComponent<MaskableGraphic>();
            ui.material = Resources.Load("Material/SideFade") as Material;
            //ui.material = new Material(Shader.Find("Custom/SideFade"));
            Texture2D te = LoadBackground(effect.state.spriteName).texture;
            ui.material.SetTexture("_NewTex", te);
            ui.material.SetInt("_Direction", (int)effect.direction);
            //效果
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                ui.material.SetFloat("currentT", t);
                yield return null;
            }
            callback();
        }

        private IEnumerator Circle(NewImageEffect effect, Action callback)
        {
            //设置shader
            MaskableGraphic ui = GetSpriteByDepth(effect.depth).GetComponent<MaskableGraphic>();
            ui.material = Resources.Load("Material/CircleHole") as Material;
            //ui.material = new Material(Shader.Find("Custom/CircleHole"));
            Debug.Log(effect.ToString());
            Texture2D te = LoadBackground(effect.state.spriteName).texture;
            ui.material.SetTexture("_NewTex", te);
            //Shader.SetGlobalTexture();
            //正反向
            ui.material.SetInt("inverse", effect.inverse ? 1 : 0);
            //Shader.SetGlobalInt();
            //效果
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                ui.material.SetFloat("currentT", t);
                //Shader.SetGlobalFloat("currentT", t);
                yield return null;
            }
            callback();
        }

        private IEnumerator Scroll(NewImageEffect effect, bool isBoth, Action callback)
        {
            //设置shader
            MaskableGraphic ui = GetSpriteByDepth(effect.depth).GetComponent<MaskableGraphic>();
            ui.material = Resources.Load("Material/ScrollBoth") as Material;
            //ui.material = new Material(Shader.Find(isBoth ? "Custom/ScrollBoth" : "Custom/Scroll"));
            
            Texture2D te = LoadBackground(effect.state.spriteName).texture;
            ui.material.SetTexture("_NewTex", te);
            ui.material.SetInt("_Direction", (int)effect.direction);
            //效果
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                ui.material.SetFloat("currentT", t);
                yield return null;
            }
            callback();
        }

        private IEnumerator Shutter(NewImageEffect effect, Action callback)
        {
            //设置shader
            MaskableGraphic ui = GetSpriteByDepth(effect.depth).GetComponent<MaskableGraphic>();
            ui.material = Resources.Load("Material/Shutter") as Material;
            //ui.material = new Material(Shader.Find("Custom/Shutter"));

            Texture2D te = LoadBackground(effect.state.spriteName).texture;
            ui.material.SetTexture("_NewTex", te);
            ui.material.SetInt("_Direction", (int)effect.direction);
            //效果
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                ui.material.SetFloat("currentT", t);
                yield return null;
            }
            callback();
        }

        private IEnumerator Mask(NewImageEffect effect, Action callback)
        {
            //设置shader
            MaskableGraphic ui = GetSpriteByDepth(effect.depth).GetComponent<MaskableGraphic>();
            ui.material = Resources.Load("Material/Mask") as Material;
            //ui.material = new Material(Shader.Find("Custom/Mask"));

            Texture2D te = LoadBackground(effect.state.spriteName).texture;
            ui.material.SetTexture("_NewTex", te);
            //Shader.SetGlobalTexture();
            Texture2D mk = LoadImage("Rule/", effect.maskImage).texture;
            ui.material.SetTexture("_MaskTex", mk);
            //Shader.SetGlobalTexture();
            //效果
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                ui.material.SetFloat("currentT", t);
                yield return null;
            }
            callback();
        }

        private IEnumerator Mosaic(NewImageEffect effect, Action callback)
        {
            //设置shader
            MaskableGraphic ui = GetSpriteByDepth(effect.depth).GetComponent<MaskableGraphic>();
            ui.material = Resources.Load("Material/Mosaic") as Material;
            //ui.material = new Material(Shader.Find("Custom/Mosaic"));
            yield return null;
            callback();
        }

        private IEnumerator Gray(NewImageEffect effect, Action callback)
        {
            //设置shader
            MaskableGraphic ui = GetSpriteByDepth(effect.depth).GetComponent<MaskableGraphic>();
            ui.material = Resources.Load("Material/Gray") as Material;
            //ui.material = new Material(Shader.Find("Custom/Gray"));
            yield return null;
            callback();
        }

        private IEnumerator OldPhoto(NewImageEffect effect, Action callback)
        {
            //设置shader
            MaskableGraphic ui = GetSpriteByDepth(effect.depth).GetComponent<MaskableGraphic>();
            ui.material = Resources.Load("Material/OldPhoto") as Material;
            //ui.material = new Material(Shader.Find("Custom/OldPhoto"));
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
            yield return new WaitForSeconds(t);
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
            //向TransList添加层数
            transList.Add(effect.depth, effect);
            Image originSprite = GetSpriteByDepth(effect.depth).GetComponent<Image>();
            Image transSprite = GetTransByDepth(effect.depth).GetComponent<Image>();
            //复制本体给Trans层
            transSprite.sprite = originSprite.sprite;
            //if (effect.depth != -1) transSprite.MakePixelPerfect();

            transSprite.color = originSprite.color;
            // transSprite.depth = originSprite.depth + 1;
            yield return null;
            //更换本体层内容
            originSprite.color = new Color(1, 1, 1, 0);
            if (effect.depth != -1)
            {
                originSprite.sprite = LoadCharacter(effect.state.spriteName);
                originSprite.SetNativeSize();
                //originSprite.MakePixelPerfect();
            }
            else
            {
                originSprite.sprite = LoadBackground(effect.state.spriteName);
            }
            if (!string.IsNullOrEmpty(effect.defaultpos))
            {
                originSprite.transform.localPosition = SetDefaultPos(originSprite, effect.defaultpos);
            }
            else
            {
                originSprite.transform.localPosition = effect.state.GetPosition();
            }
            callback();
        }


        /// <summary>
        /// 单一渐变图层（-1为背景层）
        /// </summary>
        /// <param name="effect">特效</param>
        /// <param name="callback">回调</param>
        private IEnumerator TransByDepth(NewImageEffect effect, Action callback)
        {
            Image ui = GetSpriteByDepth(effect.depth).GetComponent<Image>();
            Image trans = GetTransByDepth(effect.depth).GetComponent<Image>();
            //将trans淡出同时淡入原ui
            float t = 0;
            float origin = trans.color.a;//.alpha;
            float final = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                float a = origin + t * (final - origin);
                trans.color = new Color(1, 1, 1, a);
                ui.color = new Color(1, 1, 1, t);
                yield return null;
            }
            //删除trans
            if (effect.depth != -1)
            {
                Destroy(trans.gameObject);
            }
            if (transList.ContainsKey(effect.depth))
            {
                transList.Remove(effect.depth);
            }

            callback();
        }
        #endregion

        private IEnumerator Fade(NewImageEffect effect, Action callback)
        {
            Image ui = GetSpriteByDepth(effect.depth).GetComponent<Image>();
            //ui.shader = Shader.Find("Unity/Transparent Colored");
            float origin = ui.color.a; //alpha;
            float final = effect.state.spriteAlpha;

            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                float a = origin + t * (final - origin);
                ui.color = new Color(1, 1, 1, a);
                // ui.alpha = origin + t * (final - origin);
                yield return null;
            }
            callback();
        }

        private IEnumerator Move(NewImageEffect effect, Action callback)
        {
            Image ui = GetSpriteByDepth(effect.depth).GetComponent<Image>();
            Vector3 origin = ui.transform.localPosition;
            Vector3 final = effect.state.GetPosition();
            if (!string.IsNullOrEmpty(effect.defaultpos))
            {
                final = SetDefaultPos(ui, effect.defaultpos);
            }
            else
            {
                final = effect.state.GetPosition();
            }
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                ui.transform.localPosition = origin + t * (final - origin);
                yield return null;
            }
            callback();
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
                Image ui = GetSpriteByDepth(i).GetComponent<Image>();
                originAlpha[i] = ui.color.a; // alpha;
            }
            if (includeBack) originAlpha[-1] = bgSprite.color.a; // alpha;
            if (includeDiabox)
            {
                //duiManager.Close(effect.time, () => { });
                //originAlpha[-2] = duiManager.mainContainer.GetComponent<UIWidget>().alpha;
                //duiManager.clickContainer.SetActive(false);
            }
            float t = 0;
            float final = effect.state.spriteAlpha;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / effect.time * Time.deltaTime);
                if (includeDiabox)
                {
                    //float origin = originAlpha[-2];
                    //float alpha = origin + t * (final - origin);

                    //duiManager.mainContainer.GetComponent<UIWidget>().alpha = alpha;
                }
                foreach (int i in GetDepthNum())
                {
                    Image ui = GetSpriteByDepth(i).GetComponent<Image>();
                    float origin = originAlpha[i];
                    float alpha = origin + t * (final - origin);
                    ui.color = new Color(1, 1, 1, alpha);
                    //ui.GetComponent<UIRect>().alpha = alpha;
                }
                if (includeBack)
                {
                    float origin = originAlpha[-1];
                    float alpha = origin + t * (final - origin);
                    bgSprite.color = new Color(1, 1, 1, alpha);
                    //bgSprite.GetComponent<UIRect>().alpha = alpha;
                }
                yield return null;
            }
            //删除
            foreach (int i in GetDepthNum())
            {
                RemoveSpriteByDepth(i);
            }
            if (includeBack) bgSprite.sprite = null;
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
            foreach (Transform child in fgPanel.transform)
            {
                int x = Convert.ToInt32(child.name.Remove(0, 6));
                nums.Add(x);
            }
            nums.Sort();
            return nums;
        }


        #region 存储 读取 前背景画面
        public void SaveImageInfo()
        {
            //储存当前的背景
            Dictionary<int, SpriteState> charaDic = new Dictionary<int, SpriteState>();
            //和立绘信息
            foreach (Transform child in fgPanel.transform)
            {
                //int depth = Convert.ToInt32(child.name.Substring(6, child.name.Length - 6));
                int depth = Convert.ToInt32(child.name.Substring(6));
                Image ui = child.GetComponent<Image>();
                string sprite = ui.sprite == null ? "" : ui.sprite.name;
                charaDic.Add(depth, new SpriteState(sprite, child.localPosition, ui.color.a));
            }
            if (bgSprite.sprite == null)
            {
                dm.gameData.bgSprite = string.Empty;
            }
            else
            {
                dm.gameData.bgSprite = bgSprite.sprite.name;
            }

            dm.gameData.fgSprites = charaDic;
        }

        public void LoadImageInfo()
        {
            string bgname = dm.gameData.bgSprite;
            bgSprite.sprite = LoadBackground(bgname);
            Dictionary<int, SpriteState> fgdic = dm.gameData.fgSprites;

            //遍历当前的前景图 不在字典内的删除
            foreach (Transform child in fgPanel.transform)
            {
                int depth = Convert.ToInt32(child.name.Substring(6));
                if (!fgdic.ContainsKey(depth))
                {
                    RemoveSpriteByDepth(depth);
                }
            }
            //遍历存储字典 替换内容
            foreach (KeyValuePair<int, SpriteState> child in fgdic)
            {
                Image ui;
                if (fgPanel.transform.Find("sprite" + child.Key) == null)
                {
                    GameObject go = Resources.Load("Prefab/Character") as GameObject;
                    //go = NGUITools.AddChild(fgPanel.gameObject, go);
                    go = Instantiate(go);
                    go.transform.parent = fgPanel.transform;
                    go.transform.name = "sprite" + child.Key;
                    ui = go.GetComponent<Image>();
                }
                else
                {
                    ui = fgPanel.transform.Find("sprite" + child.Key).GetComponent<Image>();
                }
                //设置位置等
                ui.sprite = LoadCharacter(child.Value.spriteName);
                ui.transform.localPosition = child.Value.GetPosition();
                ui.color = new Color(1, 1, 1, child.Value.spriteAlpha);
                //ui.alpha = child.Value.spriteAlpha;
                ui.SetNativeSize();
                //ui.MakePixelPerfect();
            }
        }
        #endregion

    }
}