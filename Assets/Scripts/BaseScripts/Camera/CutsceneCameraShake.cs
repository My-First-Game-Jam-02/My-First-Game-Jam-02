using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CutsceneCameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera[] cinemachineVirtualCameras;
    

    void Awake()
    {
        cinemachineVirtualCameras = FindObjectsOfType<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity)
    {
        for (int i = 0; i < cinemachineVirtualCameras.Length; i++)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCameras[i].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        }
    }

    public void StopShakingCamera()
    {
        for (int i = 0; i < cinemachineVirtualCameras.Length; i++)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCameras[i].GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        }
    }
}
