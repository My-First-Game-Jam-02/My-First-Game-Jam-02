using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FMODManager : MonoBehaviour
{
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
                FMODUnity.RuntimeManager.PlayOneShot("event:/Sfx/MC/Movement/FootstepsConcrete");
                break;
            case "Jump":
                FMODUnity.RuntimeManager.PlayOneShot("event:/Sfx/MC/Movement/PlayerJump");
                break;

        }
    }


}
