using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    private SSPlayerHealth playerHealth;
    private SSPlayerController playerController;
    public bool isLaserBarrier;

    void Start()
    {
        playerHealth = FindObjectOfType<SSPlayerHealth>();
        playerController = FindObjectOfType<SSPlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Projectile")
        {
            collision.gameObject.SetActive(false);
        }

        if(collision.tag == "Enemy")
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.Kill();
                return;
            }

            EnemyRollerController enemyRollerController = collision.gameObject.GetComponent<EnemyRollerController>();
            if(enemyRollerController != null)
            {
                enemyRollerController.PlaceAtOriginalPosition();
            }
        }

        if(collision.tag == "PlayerHealth")
        {
            SSPlayerHealth playerHealth = collision.GetComponent<SSPlayerHealth>();
            playerHealth.KillPlayer();
        }

        if(collision.tag == "Player")
        {
            if (playerController.isPossessing)
            {
                PlayerEnemyHealth playerEnemyHealth = collision.GetComponent<PlayerEnemyHealth>();
                playerEnemyHealth.KillPossessedEnemy();

                //Puts the player back at the anchor position;
                if (!isLaserBarrier)
                {
                    collision.gameObject.transform.position = playerHealth.gameObject.transform.position;
                }
            }
        }
    }
}
