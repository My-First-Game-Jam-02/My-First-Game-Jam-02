using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{

    private Animator[] flickeringLightanimators;
    private bool isFlickering;

    void Start()
    {
        flickeringLightanimators = GetComponentsInChildren<Animator>();
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && !isFlickering)
        {
            FlickerTheLights();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && isFlickering)
        {
            StopFlickeringLights();
        }
    }
    public void FlickerTheLights()
    {
        isFlickering = true;
        for (int i = 0; i < flickeringLightanimators.Length; i++)
        {
            flickeringLightanimators[i].SetBool("isFlickering", true);
        }
    }

    public void StopFlickeringLights()
    {
        isFlickering = false;
        for (int i = 0; i < flickeringLightanimators.Length; i++)
        {
            flickeringLightanimators[i].SetBool("isFlickering", false);
        }
    }
}
