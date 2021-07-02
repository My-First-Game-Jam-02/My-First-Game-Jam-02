using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMeleeAttack : MonoBehaviour
{
    private float attackStartTime;

    public EnemyController enemyController;
    public float timeBetweenAttacks;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            enemyController.isTouchingPlayer = true;
            if (attackStartTime + timeBetweenAttacks < Time.time && !enemyController.isDead)
            {
                attackStartTime = Time.time;
                enemyController.ChangeStateToAttacking();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enemyController.isTouchingPlayer = false;
            if (!enemyController.isAttacking && !enemyController.isDead)
            {
                enemyController.ChangeStateToChasePlayer();
            }
        }
    }
}
