using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SSShootingPlayer : IState
{
    protected SSPlayerController playerController;
    protected Animator animator;

    public SSShootingPlayer(SSPlayerController playerController, Animator animator)
    {
        this.playerController = playerController;
        this.animator = animator;
    }

    public void Enter()
    {
        playerController.isIdle = false;
        playerController.isWalking = false;
        playerController.isJumping = false;
        //playerController.isWallSliding = false;
        playerController.isDashing = false;
        playerController.isShooting = true;
        playerController.isFrozen = false;
        playerController.isStunned = false;
        playerController.isDead = false;
        playerController.isSceneControlled = false;

        animator.SetBool("isWalking", false);
        animator.SetBool("isShooting", true);
        animator.SetBool("isWallSliding", false);
        animator.SetBool("isDashing", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("isMeleeAttacking", false);
        animator.SetBool("isStunned", false);
        animator.SetBool("isDead", false);

        playerController.rigidBody.velocity = new Vector3(0f, 0f, 0f);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {

    }
}