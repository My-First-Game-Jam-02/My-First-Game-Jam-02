using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlaySoundsInFmod : MonoBehaviour
{
    [SerializeField] SSPlayerController m_PlayerController;
    [SerializeField] bool m_isSoundPlaying;
    [SerializeField] List<EventInFmod> m_EventsInFmod;
    [SerializeField] Transform m_Position;
    FMOD.Studio.EventInstance playerState;

    private void Awake()
    {
        
    }

    private void Update()
    {
        PlaySpiritSounds();
    }

    void PlaySpiritSounds()
    {
        if (m_PlayerController.isSpirit && !m_isSoundPlaying)
        {    
            m_isSoundPlaying = true;
        }

        if (!m_PlayerController.isSpirit && m_isSoundPlaying)
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_EventsInFmod[0].m_Path, this.transform.position);
            m_isSoundPlaying = false;
        }
    }

}
