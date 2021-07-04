using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SSFrozenPlayer : IState
{
    protected SSPlayerController playerController;
    protected Animator animator;

    public SSFrozenPlayer(SSPlayerController playerController, Animator animator)
    {
        this.playerController = playerController;
        this.animator = animator;
    }

    public void Enter()
    {
        playerController.isIdle = false;
        playerController.isWalking = false;
        playerController.isJumping = false;
        playerController.isWallSliding = false;
        playerController.isDashing = false;
        playerController.isMeleeAttacking = false;
        playerController.isFrozen = true;
        playerController.isStunned = false;
        playerController.isDead = false;
        playerController.isSceneControlled = false;

        animator.SetBool("isWalking", false);
        animator.SetBool("isShooting", false);
        animator.SetBool("isWallSliding", false);
        animator.SetBool("isDashing", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("isMeleeAttacking", false);
        animator.SetBool("isStunned", false);
        animator.SetBool("isDead", false);

        
    }

    public void Execute()
    {
        playerController.rigidBody.velocity = Vector2.zero;
    }

    public void Exit()
    {

    }
}
