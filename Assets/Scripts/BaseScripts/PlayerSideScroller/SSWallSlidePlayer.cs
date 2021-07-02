using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SSWallSlidePlayer : IState
{
    protected SSPlayerController playerController;
    protected Animator animator;

    public SSWallSlidePlayer(SSPlayerController playerController, Animator animator)
    {
        this.playerController = playerController;
        this.animator = animator;
    }

    public void Enter()
    {
        playerController.isIdle = false;
        playerController.isWalking = false;
        playerController.isJumping = false;
        playerController.isWallSliding = true;
        playerController.isDashing = false;
        playerController.isMeleeAttacking = false;
        playerController.isFrozen = false;
        playerController.isStunned = false;
        playerController.isDead = false;
        playerController.isSceneControlled = false;

        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isShooting", false);
        animator.SetBool("isWallSliding", true);
        animator.SetBool("isDashing", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("isMeleeAttacking", false);
        animator.SetBool("isStunned", false);
        animator.SetBool("isDead", false);
    }

    public void Execute()
    {
        playerController.CheckPlayerStateChange();
        playerController.WallSlide();
    }

    public void Exit()
    {

    }
}