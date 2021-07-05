using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlaySoundsInFmod : MonoBehaviour
{
    [SerializeField] SSPlayerController m_PlayerController;
    [SerializeField] bool m_isSoundPlaying;
    [SerializeField] List<EventsInFmod> m_EventsInFmod;
    [SerializeField] Transform m_Position;
    private void Awake()
    {
        
    }

    private void Update()
    {
        if(m_PlayerController.isSpirit && !m_isSoundPlaying)
        {
            RuntimeManager.PlayOneShot(m_EventsInFmod[0].Path, m_Position.position);
            m_isSoundPlaying = true;
        }

        if(!m_PlayerController.isSpirit && m_isSoundPlaying)
        {
            RuntimeManager.MuteAllEvents(true);
            m_isSoundPlaying = false;
        }
    }
}

[System.Serializable]
public class EventsInFmod
{
    [SerializeField] public string Name;
    [SerializeField] public string Path;
}
