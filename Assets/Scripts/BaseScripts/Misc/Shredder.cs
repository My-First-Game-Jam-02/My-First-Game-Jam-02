using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" || collision.tag == "Projectile")
        {
            collision.gameObject.SetActive(false);
        }

        if(collision.tag == "Player")
        {
            SSPlayerHealth playerHealth = collision.GetComponent<SSPlayerHealth>();
            SSPlayerController playerController = collision.GetComponent<SSPlayerController>();
            if (!playerController.isDead)
            {
                playerHealth.KillPlayer();
            }
           
        }
    }
}
