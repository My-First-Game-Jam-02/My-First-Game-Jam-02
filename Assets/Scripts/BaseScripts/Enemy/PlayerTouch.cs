using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouch : MonoBehaviour
{

    public EnemyController enemyController;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "PlayerHealth")
        {
            enemyController.isTouchingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "PlayerHealth")
        {
            enemyController.isTouchingPlayer = false;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
