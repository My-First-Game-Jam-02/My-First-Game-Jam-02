using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{

    public GameObject pauseScreen;

    // Start is called before the first frame update
    void Start()
    {
        DeactivatePauseScreen();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseScreen.activeSelf)
            {
                DeactivatePauseScreen();
            } else
            {
                ActivatePauseScreen();
            }
            
        }
    }

    public void ActivatePauseScreen()
    {
        pauseScreen.SetActive(true);
        GameManager.Instance.pauseMenuActive = true;
        if (!GameManager.Instance.isPaused)
        {
            GameManager.Instance.PauseGame();
        }
    }

    public void DeactivatePauseScreen()
    {
        pauseScreen.SetActive(false);
        GameManager.Instance.pauseMenuActive = false;

        if (GameManager.Instance.isPaused && !GameManager.Instance.inventoryActive)
        {
            GameManager.Instance.UnPauseGame();
        }

        
    }

    public void ResetGameStatistics()
    {
        //GameManager.instance.ResetGameStatistics();
    }

    public void LoadStartScreen()
    {
        Time.timeScale = 1;
        ResetGameStatistics();
        SceneManager.LoadScene("StartScreen");
        
    }
}
