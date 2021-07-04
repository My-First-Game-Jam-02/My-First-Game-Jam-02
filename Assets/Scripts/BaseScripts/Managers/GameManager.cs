using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public List<string> inventoryItemsList = new List<string>(10);

    public bool isPaused;
    public bool inventoryActive;
    public bool pauseMenuActive;
    public bool dialogueActive;

    public GameObject playerTargetObject;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if(Instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        isPaused = false;
        Time.timeScale = 1;
    }
}
