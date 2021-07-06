using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMeleeAttack : MonoBehaviour
{
    private float attackStartTime;

    public EnemyGuardController enemyGuardController;
    public float timeBetweenAttacks;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //enemyController.isTouchingPlayer = true;
            if (attackStartTime + timeBetweenAttacks < Time.time && !enemyGuardController.isDead)
            {
                attackStartTime = Time.time;
                enemyGuardController.ChangeStateToAttacking();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //enemyController.isTouchingPlayer = false;
            if (!enemyGuardController.isAttacking && !enemyGuardController.isDead)
            {
                enemyGuardController.ChangeStateToChasePlayer();
            }
        }
    }
}
