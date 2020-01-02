using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using Live2D.Cubism.Framework.LookAt;

namespace Assets.Script.Framework
{
    public class ScreenMouse : MonoBehaviour, ICubismLookTarget
    {
        public Camera cm;
        private Sequence twq;
        public GameObject cursor;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                float X = Input.mousePosition.x - Screen.width / 2f;
                float Y = Input.mousePosition.y - Screen.height / 2f;
                Vector2 screenPosition = new Vector2(X, Y);
                cursor.transform.localPosition = screenPosition;
                ShowCusor();
            }
            
        }

        public Vector3 GetPosition()
        {
            if (!Input.GetMouseButton(0)) return Vector3.zero;
            var targetPosition = Input.mousePosition;
            targetPosition = (cm.ScreenToViewportPoint(targetPosition) * 2) - Vector3.one;
            Debug.Log(targetPosition);
            return targetPosition;
        }

        public bool IsActive()
        {
            return true;
        }

        void ShowCusor()
        {
            if (twq != null)
            {
                twq.Complete();
                twq.Kill();
            }
            twq = DOTween.Sequence();
            twq.Append(cursor.GetComponent<Image>().DOFade(1, 0.25f))
                .Join(cursor.GetComponent<RectTransform>().DOScale(1, 0.25f))
                .Append(cursor.GetComponent<Image>().DOFade(0, 0.25f))
                .Join(cursor.GetComponent<RectTransform>().DOScale(0, 0.25f))
                .Play();
        }
    }
}