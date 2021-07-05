using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SSWalkingPlayer : IState
{
    protected SSPlayerController playerController;
    protected Animator animator;

    public SSWalkingPlayer(SSPlayerController playerController, Animator animator)
    {
        this.playerController = playerController;
        this.animator = animator;
    }

    public void Enter()
    {
        playerController.isIdle = false;
        playerController.isWalking = true;
        playerController.isJumping = false;
        playerController.isWallSliding = false;
        playerController.isDashing = false;
        playerController.isShooting = false;
        playerController.isFrozen = false;
        playerController.isStunned = false;
        playerController.isDead = false;
        playerController.isSceneControlled = false;

        animator.SetBool("isWalking", true);
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
        playerController.CheckPlayerStateChange();
    }

    public void Exit()
    {

    }
}

