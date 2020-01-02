using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shadertest : MonoBehaviour
{
    public Shader sd;
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Image>().material = new Material(sd);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Material GetMaterial()
    {
        GetComponent<Image>().material = new Material(sd);
        return GetComponent<Image>().material;
    }
}
