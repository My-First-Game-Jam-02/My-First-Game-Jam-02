using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximitySoundPlayer : MonoBehaviour
{

    private AudioSource audiosource;

    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();    
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (!audiosource.isPlaying)
            {
                audiosource.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            audiosource.Stop();
        }
    }

}
