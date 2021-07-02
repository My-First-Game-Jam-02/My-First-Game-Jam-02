using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadNextScene : MonoBehaviour
{

    protected bool isFadingOut;
    
    public Image fadeObject;
    public float fadeTime;
    public string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            FadeOut();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadingOut)
        {
            Color objectColor = fadeObject.color;
            float fadeAmount = objectColor.a + (Time.deltaTime / fadeTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            fadeObject.color = objectColor;

            if(objectColor.a >= 1f)
            {
                isFadingOut = false;
                LoadScene();
            }
        }    
    }

    public void FadeOut()
    {
        isFadingOut = true;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
