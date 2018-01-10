using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoadText : MonoBehaviour
{

    public Canvas canvasRef;
    public Text title;
    public Text subTitle;
    float displayTime;

    // Use this for initialization
    void Start()
    {
        displayTime = 2.5f;
        canvasRef.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasRef.enabled)
        {
            Color col = title.color;
            displayTime -= Time.deltaTime;
            if (displayTime <= 0)
            {
                col.a -= 0.35f * Time.deltaTime;
                title.color = col;
                subTitle.color = col;
            }
            if (col.a <= 0)
            {
                col.a = 1;
                title.color = col;
                subTitle.color = col;
                canvasRef.enabled = false;
            }
        }
    }
}