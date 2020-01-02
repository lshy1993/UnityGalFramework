using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Framework
{
    public class PanoramicCameraRotate : MonoBehaviour
    {
        public Camera cm;
        public Vector2 mouseSens = Vector2.one;

        public GameObject sphere;

        [SerializeField]
        private Vector3 camAng;
        private bool draging;
        private int ctn;

        // Start is called before the first frame update
        void Start()
        {
            camAng = cm.transform.eulerAngles;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShowCoord();
            }
            if (Input.GetMouseButtonDown(1))
            {
                draging = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                draging = false;
            }
            if (draging) Rotate();
        }

        void Rotate()
        {
            float y = Input.GetAxis("Mouse X");
            float x = Input.GetAxis("Mouse Y");
            camAng.x -= x * mouseSens.x;
            camAng.y += y * mouseSens.y;
            cm.transform.eulerAngles = camAng;
        }

        public void ChangeTexture()
        {
            string[] aa = new string[3] { "李岚的房间-全景-有光照白天-1", "李岚的房间-全景-晚上", "李岚的房间-全景-有光照白天-1" };
            int a = ctn;
            while (ctn == a)
            {
                ctn = Random.Range(0, 3);
            }
            string fn = "Background/" + aa[ctn];
            Texture2D tx = Resources.Load(fn) as Texture2D;
            sphere.GetComponent<MeshRenderer>().material.mainTexture = tx;
        }

        void ShowCoord()
        {
            RaycastHit hit;
            Vector3 vv = Input.mousePosition;
            Ray ray = cm.ScreenPointToRay(vv);
            //Debug.Log(ray.origin);
            //Debug.Log(ray.direction);
            ray.origin = ray.GetPoint(5);
            ray.direction = -ray.direction;
            //Debug.DrawRay(ray.origin, ray.direction*3f, Color.red);
            sphere.GetComponent<MeshCollider>().Raycast(ray, out hit, Mathf.Infinity);
            //Debug.Log(hit.point);
            Debug.Log(hit.textureCoord);

        }
    }
}