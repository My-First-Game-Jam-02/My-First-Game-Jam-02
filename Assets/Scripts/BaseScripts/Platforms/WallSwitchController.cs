using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSwitchController : MonoBehaviour
{

    protected bool hasBeenActivated;
    protected Animator switchAnimator;
    protected BoxCollider2D boxCollider;
    protected AudioSource audioSource;

    public WallSwitch[] wallSwitchesToActivate;
    public WallSwitch[] wallSwitchesToDeactivate;
    public AudioClip wallSwitchSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!hasBeenActivated)
            {
                SetSwitchToOff();
                ActivateWalls();
                DeactivateWalls();
                hasBeenActivated = true;
            }
        }
    }

    void Start()
    {
        switchAnimator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        ResetWalls();
    }

    public void ActivateWalls()
    {
        audioSource.PlayOneShot(wallSwitchSFX);
        if(wallSwitchesToActivate.Length > 0)
        {
            for (int i = 0; i < wallSwitchesToActivate.Length; i++)
            {
                wallSwitchesToActivate[i].ActivateWall();
            }
        } 
    }

    public void DeactivateWalls()
    {
        audioSource.PlayOneShot(wallSwitchSFX);
        if (wallSwitchesToDeactivate.Length > 0)
        {
            for (int i = 0; i < wallSwitchesToDeactivate.Length; i++)
            {
                wallSwitchesToDeactivate[i].DeactivateWall();
            }
        }
    }

    public void ResetWalls()
    {
        hasBeenActivated = false;
        SetSwitchToOn();

        if (wallSwitchesToActivate.Length > 0)
        {
            for (int i = 0; i < wallSwitchesToActivate.Length; i++)
            {
                wallSwitchesToActivate[i].DeactivateWall();
            }
        }

        if (wallSwitchesToDeactivate.Length > 0)
        {
            for (int i = 0; i < wallSwitchesToDeactivate.Length; i++)
            {
                wallSwitchesToDeactivate[i].ActivateWall();
            }
        }
    }

    public void SetSwitchToOn()
    {
        boxCollider.enabled = true;
        switchAnimator.SetBool("isOn", true);
    }

    public void SetSwitchToOff()
    {
        boxCollider.enabled = false;
        switchAnimator.SetBool("isOn", false);
    }
}
