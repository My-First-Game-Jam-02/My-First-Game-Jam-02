using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    public bool preventSwitching;
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera anchorCamera;

    // Start is called before the first frame update
    void Start()
    {
        UsePlayerCamera();
        preventSwitching = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UsePlayerCamera()
    {
        playerCamera.Priority = 10;
        anchorCamera.Priority = 0;
    }

    public void UseAnchorCamera()
    {
        playerCamera.Priority = 0;
        anchorCamera.Priority = 10;
    }
}
