using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouch : MonoBehaviour
{

    public EnemyGuardController enemyGuardController;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            enemyGuardController.isTouchingPlayer = true;
        }

        if(collision.tag == "PlayerAnchor")
        {
            enemyGuardController.isTouchingAnchor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            enemyGuardController.isTouchingPlayer = false;
        }

        if (collision.tag == "PlayerAnchor")
        {
            enemyGuardController.isTouchingAnchor = false;
        }
    }
}
