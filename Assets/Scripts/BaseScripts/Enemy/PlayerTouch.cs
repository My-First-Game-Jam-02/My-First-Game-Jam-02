using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouch : MonoBehaviour
{

    public EnemyGuardController enemyGuardController;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "PlayerHealth")
        {
            enemyGuardController.isTouchingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "PlayerHealth")
        {
            enemyGuardController.isTouchingPlayer = false;
        }
    }
}
