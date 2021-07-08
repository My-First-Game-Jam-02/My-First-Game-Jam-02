using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayOneShotFmod : MonoBehaviour
{ 
    [SerializeField] OneShotActions m_ActionOnAwake = new OneShotActions();

    private void Awake()
    {

        PlaySoundOnAwake();
    }

    public void PlaySoundOnAwake()
    {
        switch(m_ActionOnAwake)
        {
            case OneShotActions.None:
                return; 
            case OneShotActions.Fire:
                FMODManager.instance.PlayShortSounds("Fire");
                break;
            case OneShotActions.Laser:
                FMODManager.instance.PlayShortSounds("Laser");
                break;
            case OneShotActions.Explosion:
                FMODManager.instance.PlayShortSounds("Explosion");
                break;
        }
    }

    public void PlayOneShotSound()
    {
        
    }

    public void PlayFootsteps()
    {
        FMODManager.instance.PlayShortSounds("Footsteps");
    }
    public void PlayJumpSound()
    {
        FMODManager.instance.PlayShortSounds("Jump");
    }

    public void PlaySwitchSound()
    {
        FMODManager.instance.PlayShortSounds("Switch");
        
    }


}
