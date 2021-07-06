using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShotFmod : MonoBehaviour
{
    [SerializeField] List<EventInFmod>  m_EventToPlaySteeps = new List<EventInFmod>();


    private void Awake()
    {
        
    }


    public void PlayOneShotSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(m_EventToPlaySteeps[0].m_Path, this.transform.position);
    }
}
