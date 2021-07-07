using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FMODManager : MonoBehaviour
{
    #region FMOD_Events_Paths
    [Header("Fmod Events Paths")]
    [SerializeField] string m_Footsteps;
    [SerializeField] string m_Jump;
    [SerializeField] string m_LaserShot;
    [SerializeField] string m_FireBall;
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
        }
    }


}
