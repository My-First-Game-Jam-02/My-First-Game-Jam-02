using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SSFallingPlayer : IState
{
    protected SSPlayerController playerController;
    protected Animator animator;

    public SSFallingPlayer(SSPlayerController playerController, Animator animator)
    {
        this.playerController = playerController;
        this.animator = animator;
    }

    public void Enter()
    {
        playerController.isIdle = false;
        playerController.isWalking = false;
        playerController.isJumping = true;
        playerController.isWallSliding = false;
        playerController.isDashing = false;
        playerController.isShooting = false;
        playerController.isFrozen = false;
        playerController.isStunned = false;
        playerController.isDead = false;
        playerController.isSceneControlled = false;

        animator.SetBool("isWalking", false);
        animator.SetBool("isShooting", false);
        animator.SetBool("isWallSliding", false);
        animator.SetBool("isDashing", false);
        animator.SetBool("isFalling", true);
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
