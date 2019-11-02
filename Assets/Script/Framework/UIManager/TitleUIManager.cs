using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Assets.Script.Framework.Data;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Assets.Script.Framework.UI
{
    /// <summary>
    /// 主界面UI管理
    /// </summary>
    public class TitleUIManager : MonoBehaviour
    {
        public GameManager gm;
        public SystemUIManager sysm;

        //private UIWidget titleCon, extraCon;
        //private UIWidget musicCon, galleryCon, recollectionCon, endingCon;
        private GameObject titleCon;
        private GameObject btnTable, largeCon, particleCon;
        private GameObject bg, videobg;
        private Text verLabel;

        /// <summary>
        /// 鼠标微动背景
        /// </summary>
        public bool mouseFocusBackground = false;

        private Vector2 originPoint;

        /// <summary>
        /// 鼠标偏移
        /// </summary>
        public Vector2 mouseOffset = new Vector2(0.05f, 0.05f);

        /// <summary>
        /// 动态背景
        /// </summary>
        public bool videoBackground = false;

        /// <summary>
        /// 动态粒子特效
        /// </summary>
        public bool dynamicParticle = false;

        private Constants.TITLE_STATUS status;

        void Awake()
        {
            status = Constants.TITLE_STATUS.TITLE;

            titleCon = transform.Find("Title_Container").gameObject;
            //extraCon = transform.Find("ExtraButton_Container").GetComponent<UIWidget>();
            //musicCon = transform.Find("Music_Container").GetComponent<UIWidget>();
            //galleryCon = transform.Find("Gallery_Container").GetComponent<UIWidget>();
            //recollectionCon = transform.Find("Recollection_Container").GetComponent<UIWidget>();
            //endingCon = transform.Find("Ending_Container").GetComponent<UIWidget>();

            //btnTable = titleCon.transform.Find("MainButton_Table").gameObject;
            //largeCon = galleryCon.transform.Find("Large_Container").gameObject;
            //particleCon = titleCon.transform.Find("Particle_Container").gameObject;

            bg = transform.Find("Back_Sprite").gameObject;
            videobg = transform.Find("Video_Sprite").gameObject;
            verLabel = transform.Find("Title_Container/Version_Label").GetComponent<Text>();
            originPoint = bg.GetComponent<RectTransform>().anchoredPosition;
        }

        private void OnEnable()
        {
            //初始化时播放BGM
            //gm.sm.SetBGM("Title");
            verLabel.text = DataManager.GetInstance().version;
            //粒子特效
            //if(dynamicParticle)
            //动态背景
            if (videoBackground)
            {
                bg.SetActive(false);
                videobg.SetActive(true);
                VideoPlayer vp = videobg.GetComponent<VideoPlayer>();
                //vp.url = Path.Combine(Application.streamingAssetsPath, "title_loop.mp4");
                vp.isLooping = true;
                vp.Play();
                RawImage ri = videobg.GetComponent<RawImage>();
                ri.texture = vp.targetTexture;
                Debug.Log("video played!" + Time.time);
            }
            else
            {
                bg.SetActive(true);
                videobg.SetActive(false);
            }
        }

        private void Update()
        {
            if (mouseFocusBackground) DynamicMouse();
        }

        //计算动态背景位置
        private void DynamicMouse()
        {
            //获取鼠标相对偏移位置（中心为0,0）
            float mouseX = GetScreenOffset(Input.mousePosition.x / 1920f - 0.5f);
            float mouseY = GetScreenOffset(Input.mousePosition.y / 1080f - 0.5f);
            //Debug.Log(mouseX.ToString() + mouseY.ToString());
            float bgX = originPoint.x - mouseX * 1920 * mouseOffset.x;
            float bgY = originPoint.y - mouseY * 1080 * mouseOffset.y;
            bg.transform.localPosition = new Vector2(bgX, bgY);
        }

        private float GetScreenOffset(float t)
        {
            if (t < -0.5f) return -0.5f;
            if (t > 0.5f) return 0.5f;
            return t;
        }

        public Constants.TITLE_STATUS GetStatus()
        {
            return status;
        }

        //标题画面右键
        public void RightClick()
        {
            //switch (status)
            //{
            //    case Constants.TITLE_STATUS.EXTRA:
            //        RightClickReturn();
            //        break;
            //    case Constants.TITLE_STATUS.GALLERY:
            //        if (largeCon.activeSelf)
            //        {
            //            //galleryCon.GetComponent<GalleryUIManager>().ClosePic();
            //        }
            //        else
            //        {
            //            CloseGallery();
            //        }
            //        break;
            //    case Constants.TITLE_STATUS.MUSIC:
            //        CloseMusic();
            //        break;
            //    case Constants.TITLE_STATUS.RECOLL:
            //        CloseRecollection();
            //        break;
            //    case Constants.TITLE_STATUS.ENDING:
            //        CloseEnding();
            //        break;
            //    default:
            //        break;
            //}
        }

        #region public Title相关按钮
        public void ClickStart()
        {
            //新游戏start
            gm.sm.StopBGM();
            gm.NewGame();
        }

        public void ClickExtra()
        {
            //打开extra
            status = Constants.TITLE_STATUS.EXTRA;
            //StartCoroutine(OpenExtra());
        }

        public void RightClickReturn()
        {
            status = Constants.TITLE_STATUS.TITLE;
            //StartCoroutine(CloseExtra());
        }

        public void ClickLoad()
        {
            //设计：title不动 sys淡入
            sysm.transform.gameObject.SetActive(true);
            sysm.GetComponent<CanvasGroup>().alpha = 1;
            sysm.OpenLoad(true);
        }

        public void ClickSetting()
        {
            sysm.transform.gameObject.SetActive(true);
            sysm.GetComponent<CanvasGroup>().alpha = 1;
            sysm.OpenSetting();
        }

        public void ClickExit()
        {
            Application.Quit();
        }

        public void OpenSaveFolder()
        {
            string FolderPath = Application.persistentDataPath + "/Save";
            System.Diagnostics.Process.Start(FolderPath);
        }

        public void OpenHomepage()
        {
            string url = "https://github.com/lshy1993/UnityGalFramework";
            Application.OpenURL(url);
        }
        #endregion

        //#region public Extra开关
        //public void OpenMusic()
        //{
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        status = Constants.TITLE_STATUS.MUSIC;
        //        StartCoroutine(FadeOut(extraCon));
        //        StartCoroutine(FadeIn(musicCon));
        //        gm.sm.StopBGM();
        //    }
        //}
        //public void CloseMusic()
        //{
        //    status = Constants.TITLE_STATUS.EXTRA;
        //    StartCoroutine(FadeOut(musicCon));
        //    StartCoroutine(FadeIn(extraCon));
        //    gm.sm.SetBGM("Title");
        //}
        //public void OpenGallery()
        //{
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        status = Constants.TITLE_STATUS.GALLERY;
        //        StartCoroutine(FadeOut(extraCon));
        //        StartCoroutine(FadeIn(galleryCon));
        //    }
        //}
        //public void CloseGallery()
        //{
        //    status = Constants.TITLE_STATUS.EXTRA;
        //    StartCoroutine(FadeOut(galleryCon));
        //    StartCoroutine(FadeIn(extraCon));
        //}
        //public void OpenRecollection()
        //{
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        status = Constants.TITLE_STATUS.RECOLL;
        //        StartCoroutine(FadeOut(extraCon));
        //        StartCoroutine(FadeIn(recollectionCon));
        //    }
        //}
        //public void CloseRecollection()
        //{
        //    status = Constants.TITLE_STATUS.EXTRA;
        //    StartCoroutine(FadeOut(recollectionCon));
        //    StartCoroutine(FadeIn(extraCon));
        //}
        //public void OpenEnding()
        //{
        //    if (Input.GetMouseButtonUp(0))
        //    {
        //        status = Constants.TITLE_STATUS.ENDING;
        //        StartCoroutine(FadeOut(extraCon));
        //        StartCoroutine(FadeIn(endingCon));
        //    }
        //}
        //public void CloseEnding()
        //{
        //    status = Constants.TITLE_STATUS.EXTRA;
        //    StartCoroutine(FadeOut(endingCon));
        //    StartCoroutine(FadeIn(extraCon));
        //}
        //#endregion

        //private IEnumerator OpenExtra()
        //{
        //    StartCoroutine(MoveBG(false));
        //    yield return StartCoroutine(FadeOut(titleCon));
        //    StartCoroutine(FadeIn(extraCon));
        //}
        //private IEnumerator CloseExtra()
        //{
        //    StartCoroutine(MoveBG(true));
        //    yield return StartCoroutine(FadeOut(extraCon));
        //    StartCoroutine(FadeIn(titleCon));
        //}

        //private IEnumerator FadeIn(UIWidget target)
        //{
        //    DataManager.GetInstance().BlockRightClick();
        //    if (target == titleCon) BlockBtn(false);
        //    target.transform.gameObject.SetActive(true);
        //    float x = 0;
        //    while (x < 1)
        //    {
        //        x = Mathf.MoveTowards(x, 1, 1 / 0.3f * Time.deltaTime);
        //        target.alpha = x;
        //        yield return null;
        //    }
        //    if (target == titleCon) BlockBtn(true);
        //    DataManager.GetInstance().UnblockRightClick();
        //}
        //private IEnumerator FadeOut(UIWidget target)
        //{
        //    if (target == titleCon) BlockBtn(false);
        //    float x = 1;
        //    while (x > 0)
        //    {
        //        x = Mathf.MoveTowards(x, 0, 1 / 0.3f * Time.deltaTime);
        //        target.alpha = x;
        //        yield return null;
        //    }
        //    target.transform.gameObject.SetActive(false);
        //    if (target == titleCon) BlockBtn(true);
        //}

        //private IEnumerator MoveBG(bool isback)
        //{
        //    dynamicBackgrund = false;
        //    DataManager.GetInstance().BlockRightClick();
        //    float x = bg.transform.localPosition.x;
        //    float t = 0;
        //    while (t < 1)
        //    {
        //        t = Mathf.MoveTowards(t, 1, 1 / 0.5f * Time.deltaTime);
        //        float y = 630 - 920 * (isback ? (1 - t) : t);
        //        bg.transform.localPosition = new Vector3(x, y);
        //        yield return null;
        //    }
        //    titleCon.transform.gameObject.SetActive(isback);
        //    DataManager.GetInstance().UnblockRightClick();
        //    if (isback) dynamicBackgrund = true;
        //}

        //private void BlockBtn(bool blocked)
        //{
        //    for (int i = 0; i < btnTable.transform.childCount; i++)
        //    {
        //        btnTable.transform.GetChild(i).GetComponent<UIButton>().enabled = blocked;
        //    }
        //}
    }
}