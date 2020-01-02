using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Live2D.Cubism.Framework.Expression;

public class expresstest : MonoBehaviour
{
    public CubismExpressionController cec;
    public Text tt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tt.text = cec.CurrentExpressionIndex.ToString();
    }

    public void Prev()
    {
        cec.CurrentExpressionIndex--;
    }

    public void Next()
    {
        cec.CurrentExpressionIndex ++;
    }
}
