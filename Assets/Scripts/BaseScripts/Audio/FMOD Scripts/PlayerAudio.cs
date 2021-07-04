using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] SSPlayerController m_PlayerController;
    [SerializeField] bool m_IsPlayingSpiritSounds;

    private void Awake()
    {
        if(m_PlayerController == null)
        {
            m_PlayerController = GetComponent<SSPlayerController>();
        }
    }

    private void Update()
    {
        TriggerSpiritSounds();
    }

    private void TriggerSpiritSounds()
    {   
        if(m_PlayerController.isSpirit && !m_IsPlayingSpiritSounds)
        {
            Debug.Log("Play Spirit Sounds");
            m_IsPlayingSpiritSounds = true;
        }


        if(!m_PlayerController.isSpirit && m_IsPlayingSpiritSounds)
        {
            m_IsPlayingSpiritSounds = false;
        }

       
    }
}
