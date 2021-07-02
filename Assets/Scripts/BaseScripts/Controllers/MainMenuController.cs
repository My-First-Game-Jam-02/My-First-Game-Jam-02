using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    protected bool creditsActivated;

    public string firstLevelName;
    public string startScreenMusic;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject credits;

    void Start()
    {
        Invoke("ActivateMainMenu", 0.01f);
        ResetGame();
        SoundManager.Instance.FadeAndPlayMusic(startScreenMusic, 0.1f);
    }

    private void Update()
    {
        if (creditsActivated == true && Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return) || Input.GetButtonDown("Fire1") || Input.GetMouseButtonDown(0))
        {
            ActivateMainMenu();
        }    
    }

    public void ActivateMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        credits.SetActive(false);
        creditsActivated = false;
    }

    public void ActivateOptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        credits.SetActive(false);
        creditsActivated = false;
    }

    public void ActivateCredits()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        credits.SetActive(true);
        Invoke("SetCreditsActivatedBool", 1f);
    }

    public void SetCreditsActivatedBool()
    {
        creditsActivated = true;
    }

    public void ResetGame()
    {
        QuestManager.Instance.ResetAllTasks();
    }

    public void ExitAndQuitGame()
    {
        Application.Quit();
    }

    public void LoadUnlockedLevel()
    {
        //GameManager.instance.ResetGameStatistics();
        //switch (GameManager.instance.unlockedLevel)
        //{
        //    case 1:
        //        SceneManager.LoadScene("Stage_01");
        //        break;
        //    case 2:
        //        SceneManager.LoadScene("Stage_02");
        //        break;
        //    case 3:
        //        SceneManager.LoadScene("Stage_03");
        //        break;
        //    case 4:
        //        SceneManager.LoadScene("Stage_04");
        //        break;
        //    case 5:
        //        SceneManager.LoadScene("Stage_05");
        //        break;
        //    case 6:
        //        SceneManager.LoadScene("Stage_06");
        //        break;
        //    case 7:
        //        SceneManager.LoadScene("Stage_07");
        //        break;
        //    case 8:
        //        SceneManager.LoadScene("Stage_08");
        //        break;
        //    default:
        //        print("no such level.");
        //        break;
        //}
    }

    public void LoadFirstLevel()
    {
        //GameManager.instance.ResetGameStatistics();
        SceneManager.LoadScene(firstLevelName);
    }
}
