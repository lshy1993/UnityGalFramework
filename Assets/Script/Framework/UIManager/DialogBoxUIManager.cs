using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Assets.Script.Framework.Effect;
using Assets.Script.Framework.Node;
using DG.Tweening;
//using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Framework
{
    public class DialogBoxUIManager : MonoBehaviour
    {
        //[HideInInspector]
        public GameObject mainContainer, clickContainer;
        private Text dialogLabel;
        private Text nameLabel;
        private TypeWriter te;

        private GameObject nextIcon;
        private Toggle toggleAuto;
        private Image avatarSprite;

        // 玩家自定义后的姓名
        private string xing, ming;

        private TextPiece currentPiece;

        private bool typewriting = false;
        [SerializeField]
        private bool closedbox = false;

        private Regex rx = new Regex(@"\[[^\]]+\]");

        void Awake()
        {
            dialogLabel = mainContainer.transform.Find("Dialog_Panel/Content_Text").GetComponent<Text>();
            nameLabel = mainContainer.transform.Find("Dialog_Panel/Name_Text").GetComponent<Text>();

            nextIcon = dialogLabel.transform.Find("NextIcon_Image").gameObject;

            te = dialogLabel.transform.GetComponent<TypeWriter>();
            te.enabled = false;

            toggleAuto = mainContainer.transform.Find("QuickButton_Panel/Auto_Toggle").GetComponent<Toggle>();

            avatarSprite = mainContainer.transform.Find("Avatar_Panel/Avatar_Image").GetComponent<Image>();

            //SetHeroName();
        }

        //public void SetHeroName()
        //{
        //    xing = DataManager.GetInstance().gameData.heroXing;
        //    ming = DataManager.GetInstance().gameData.heroMing;
        //}

        //将文字数据应用到UI上
        public void SetText(TextPiece currentPiece, string name, string dialog, string voice, string avatar = "")
        {
            //设置成禁用右键和滚轮？
            DataManager.GetInstance().BlockRightClick();
            DataManager.GetInstance().BlockWheel();
            this.currentPiece = currentPiece;
            //添加颜色
            //nameLabel.text = AddColor(name);
            //替换已读文本
            dialogLabel.text = ChangeName(dialog);
            float a = DataManager.GetInstance().IsTextRead(currentPiece) ? 0.5f : 1f;
            float r = dialogLabel.color.r;
            float g = dialogLabel.color.g;
            float b = dialogLabel.color.b;
            dialogLabel.color = new Color(r, g, b, a);
            //打字机
            te.enabled = true;
            te.ResetToBeginning();
            typewriting = true;

            //去掉颜色标签符号
            DataManager.GetInstance().tempData.currentText = rx.Replace(dialogLabel.text, "");
            //头像
            SetAvatar(avatar);
        }

        /// <summary>
        /// 设置头像
        /// </summary>
        /// <param name="str"></param>
        private void SetAvatar(string str)
        {
            if (str == string.Empty)
            {
                avatarSprite.sprite = null;
            }
            else
            {
                avatarSprite.sprite = Resources.Load<Sprite>("Character/" + str);
                if (str.Contains("Icon"))
                {
                    avatarSprite.rectTransform.sizeDelta = new Vector2(150, 150);
                }
                else
                {
                    avatarSprite.SetNativeSize();
                }
                //顶边中心靠上
                float h = avatarSprite.rectTransform.rect.height;
                avatarSprite.transform.localPosition = new Vector3(0, -h / 2 + 140);
            }

        }


        //瞬间完成打字
        public void FinishType()
        {
            te.Finish();
        }

        //隐藏对话框
        public void HideWindow()
        {
            if (!gameObject.activeSelf) return;
            if (DataManager.GetInstance().isAuto)
            {
                //toggleAuto.CancelAuto();
                return;
            }
            Close(0.1f, () => { 
                clickContainer.SetActive(true);
            });
            mainContainer.SetActive(false);
            closedbox = true;
        }
        //显示对话框
        public void ShowWindow()
        {
            if (!gameObject.activeSelf) return;
            StartCoroutine(OpenUI(0.1f, () => { }));
            mainContainer.SetActive(true);
            closedbox = false;
        }

        //隐藏/显示下一页图标
        public void HideNextIcon()
        {
            nextIcon.SetActive(false);
        }

        //清空文字框内容
        public void ClearText()
        {
            HideNextIcon();
            nameLabel.text = "";
            dialogLabel.text = "";
            avatarSprite.sprite = null;
        }

        //打字机完成后 调用此函数
        public void ShowNextIcon()
        {
            te.enabled = false;
            if (string.IsNullOrEmpty(dialogLabel.text) && string.IsNullOrEmpty(nameLabel.text)) return;
            DataManager.GetInstance().UnblockRightClick();
            DataManager.GetInstance().UnblockWheel();
            typewriting = false;
            //最后一个文字定位
            Vector3 vec = GetLastCharacterPos(dialogLabel.gameObject);
            //Debug.Log(vec);
            //nextIcon.GetComponent<TweenPosition>();
            DOTween.Kill(nextIcon.GetComponent<RectTransform>());
            nextIcon.GetComponent<RectTransform>().anchoredPosition = new Vector3(vec.x + 12, vec.y - 2);
            Tween tw = nextIcon.GetComponent<RectTransform>().DOAnchorPosY(vec.y + 2, 0.25f);
            tw.SetLoops(-1, LoopType.Yoyo);
            nextIcon.SetActive(true);
            if (currentPiece != null) currentPiece.finish = true;
        }

        private Vector3 GetLastCharacterPos(GameObject go)
        {
            Text label = go.GetComponent<Text>();
            string text = Regex.Replace(label.text, "<(.*?)>", "");
            if (string.IsNullOrEmpty(text)) text = " ";
            float fullSize = label.fontSize;
            float x = 0f, y = fullSize;
            RectTransform transform = label.GetComponent<RectTransform>();
            float offx = 0;//trasform.offsetMin.x;
            float offy = 0;// transform.offsetMax.y;
            Debug.Log(offx + " " + offy);

            for (int i = 0; i < text.Length; i++)
            {
                var ch = text[i];
                if (ch == '\n')
                {
                    y += fullSize + label.lineSpacing;
                    x = 0f;
                    continue;
                }
                else
                {
                    CharacterInfo charInfo;
                    label.font.GetCharacterInfo(ch, out charInfo, label.fontSize);
                    x += charInfo.advance;
                }
            }

            return new Vector3(offx + x, offy - y, 0);
        }

        //public 属性获取方法
        public bool IsTyping()
        {
            return typewriting;
        }
        public bool IsBoxClosed()
        {
            return closedbox;
        }

        //预清空 且 开启对话框
        public void Open(float time, Action callback)
        {
            ClearText();
            closedbox = false;
            StartCoroutine(OpenUI(time, callback));
        }

        /// <summary>
        /// 关闭对话框
        /// </summary>
        /// <param name="time"></param>
        /// <param name="callback"></param>
        public void Close(float time, Action callback)
        {
            StartCoroutine(CloseUI(time, callback));
        }

        /// <summary>
        /// 替换自定义姓名
        /// </summary>
        private string ChangeName(string origin)
        {
            return origin.Replace("李云萧", xing + ming);
        }

        private string AddColor(string name)
        {
            if (name.Contains("李云萧"))
            {
                //return "[33ff00]李[-]  云萧";
                return "[33ff00]" + xing + "[-]  " + ming;
            }
            if (name.Contains("喵星人"))
            {
                return "喵  [ffcc33]星[-]人";
            }
            if (name.Contains("西门吹"))
            {
                return "西门  [0099ff]吹[-]";
            }
            if (name.Contains("苏梦忆"))
            {
                return "苏  [ff3399]梦[-]忆";
            }
            return name;
        }

        private IEnumerator OpenUI(float time, Action callback)
        {
            DataManager.GetInstance().isEffecting = true;
            mainContainer.SetActive(true);
            float t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, 1 / time * Time.deltaTime);
                mainContainer.GetComponent<CanvasGroup>().alpha = t;
                yield return null;
            }
            DataManager.GetInstance().isEffecting = false;
            clickContainer.SetActive(true);
            callback();
        }

        private IEnumerator CloseUI(float time, Action callback)
        {
            DataManager.GetInstance().isEffecting = true;
            clickContainer.SetActive(false);
            float t = 1;
            while (t > 0)
            {
                t = Mathf.MoveTowards(t, 0, 1 / time * Time.deltaTime);
                mainContainer.GetComponent<CanvasGroup>().alpha = t;
                yield return null;
            }
            DataManager.GetInstance().isEffecting = false;
            mainContainer.SetActive(false);
            callback();
        }

    }
}