using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class FMODManager : MonoBehaviour
{
    #region FMOD_MUSIC_Tracks
    [Header("Music Tracks")]
    [SerializeField] string m_MainMenu;
    [SerializeField] string m_GamePlay;
    #endregion

    #region FMOD_Events_Paths
    [Header("Fmod Events Paths")]
    [SerializeField] string m_Footsteps;
    [SerializeField] string m_Jump;
    [SerializeField] string m_LaserShot;
    [SerializeField] string m_FireBall;
    [SerializeField] string m_Switch;
    #endregion



    public static FMODManager instance;

	void Awake()
	{
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        MusicPlayer();
    }

    private void MusicPlayer()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Scene_01":
                FMODUnity.RuntimeManager.PlayOneShot(m_GamePlay);
                break;
        }
    }      

        
   

    public void PlayShortSounds(string eventName)
    {
        switch(eventName)
        {
            case "Footsteps":
                FMODUnity.RuntimeManager.PlayOneShot(m_Footsteps);             
                break;
            case "Jump":
                FMODUnity.RuntimeManager.PlayOneShot(m_Jump);
                break;
            case "Laser":
                FMODUnity.RuntimeManager.PlayOneShot(m_LaserShot);
                break;
            case "Switch":
                FMODUnity.RuntimeManager.PlayOneShot(m_Switch);
                Debug.Log("Switch Off");
                break;
        }
    }


}
