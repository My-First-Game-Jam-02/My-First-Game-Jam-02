using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    private SSPlayerHealth playerHealth;
    private SSPlayerController playerController;

    public int damageAmount;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !playerController.isDead)
        {
            playerHealth.Damage(damageAmount);
        }
    }
   
    void Start()
    {
        playerHealth = FindObjectOfType<SSPlayerHealth>();
        playerController = playerHealth.GetComponent<SSPlayerController>();
    }
}
