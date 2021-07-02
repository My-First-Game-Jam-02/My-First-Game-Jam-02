using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityActivatorForShooter : MonoBehaviour
{
    private bool playerNear;
    private bool sfxIsPlaying;

    public GameObject sfxToPlay;

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerNear = true;
            //ActivateAllShooters();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerNear = false;
            //DeactivateAllShooters();
        }
    }

    
    void FixedUpdate()
    {
        
    }

    public void TriggerShootingSfx()
    {
        if(playerNear && !sfxIsPlaying)
        {
            PlaySfx();
            sfxIsPlaying = true;
            print("playing sound effect");
            Invoke("SetSfxNotPlaying", 0.1f);
        }
    }

    public void SetSfxNotPlaying()
    {
        sfxIsPlaying = false;
    }

    public void PlaySfx()
    {
        Instantiate(sfxToPlay, transform.position, transform.rotation);
    }

    //public void ActivateAllShooters()
    //{
    //    for (int i = 0; i < projectileShooters.Length; i++)
    //    {
    //        projectileShooters[i].gameObject.SetActive(true);
    //    }
    //}

    //public void DeactivateAllShooters()
    //{
    //    for (int i = 0; i < projectileShooters.Length; i++)
    //    {
    //        projectileShooters[i].gameObject.SetActive(false);
    //    }
    //}

   
}
