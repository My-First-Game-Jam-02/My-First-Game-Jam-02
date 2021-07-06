using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public bool canBeUsed;
    protected SSPlayerController playerController;
    protected Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(playerController.isGuardBot || playerController.isPlayer)
            {
                canBeUsed = true;
            }
        }    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canBeUsed = false;
        }
    }

    public virtual void Start()
    {
        playerController = FindObjectOfType<SSPlayerController>();
        animator = GetComponent<Animator>();
    }

}
