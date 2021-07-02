using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningSequence : MonoBehaviour
{
    private CutsceneCameraShake cameraShake;

    public float cameraShakeIntensity;
    public GameObject boulderSfx;
    public GameObject electricSfx;

    // Start is called before the first frame update
    void Start()
    {
        cameraShake = GetComponent<CutsceneCameraShake>();
    }

    public void CutSceneShakeCamera()
    {
        cameraShake.ShakeCamera(cameraShakeIntensity);
    }

    public void CutSceneStopCameraShake()
    {
        cameraShake.StopShakingCamera();
    }

    public void PlayBoulderSfx()
    {
        Instantiate(boulderSfx, transform.position, transform.rotation);
    }

    public void PlayElectricSfx()
    {
        Instantiate(electricSfx, transform.position, transform.rotation);
    }
}
