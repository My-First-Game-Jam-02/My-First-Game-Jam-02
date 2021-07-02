using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{

    public Image fadeObject;
    public Image whiteOutObject;
    public bool isFadingOut { get; private set; }
    public bool isFadingIn { get; private set; }
    [HideInInspector]
    public float fadeTime;
    public bool useWhiteOut;

    void Awake()
    {
        fadeObject = GameObject.Find("FadeObject").GetComponent<Image>();
        whiteOutObject = GameObject.Find("WhiteOutObject").GetComponent<Image>();
    }

    void Update()
    {
        if (isFadingOut)
        {
            Color objectColor;
            if (useWhiteOut)
            {
                objectColor = whiteOutObject.color;
            } else
            {
                objectColor = fadeObject.color;
            }
            
            float fadeAmount = objectColor.a + (Time.deltaTime / fadeTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);

            if (useWhiteOut)
            {
                whiteOutObject.color = objectColor;
            } else
            {
                fadeObject.color = objectColor;
            }
            

            if (objectColor.a >= 1f)
            {
                isFadingOut = false;
            }
        }

        if (isFadingIn)
        {
            Color objectColor;
            if (useWhiteOut)
            {
                objectColor = whiteOutObject.color;
            }
            else
            {
                objectColor = fadeObject.color;
            }

            float fadeAmount = objectColor.a - (Time.deltaTime / fadeTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);

            if (useWhiteOut)
            {
                whiteOutObject.color = objectColor;
            } else
            {
                fadeObject.color = objectColor;
            }
            
            if (objectColor.a <= 0f)
            {
                isFadingIn = false;
            }
        }
    }

    public void FadeOut(float fadeTime)
    {
        this.fadeTime = fadeTime; 
        isFadingOut = true;
        isFadingIn = false;
    }

    public void FadeIn(float fadeTime)
    {
        this.fadeTime = fadeTime;
        isFadingOut = false;
        isFadingIn = true;
    }

    public void SetScreenToBlack()
    {
        fadeObject.color = new Color(0, 0, 0);
    }

    public void SetScreenToWhite()
    {
        whiteOutObject.color = new Color(1, 1, 1);
    }
}
