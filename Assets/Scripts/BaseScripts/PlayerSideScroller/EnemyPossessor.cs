using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPossessor : MonoBehaviour
{
    private SSPlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<SSPlayerController>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            playerController.possessableEnemies.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            playerController.possessableEnemies.Remove(collision.gameObject);
        }
    }
    void Update()
    {
        
    }
}
