using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SSDashingPlayer : IState
{
    protected SSPlayerController playerController;
    protected Animator animator;

    public SSDashingPlayer(SSPlayerController playerController, Animator animator)
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
        playerController.isDashing = true;
        playerController.isMeleeAttacking = false;
        playerController.isFrozen = false;
        playerController.isStunned = false;
        playerController.isDead = false;
        playerController.isSceneControlled = false;

        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isShooting", false);
        animator.SetBool("isWallSliding", false);
        animator.SetBool("isDashing", true);
        animator.SetBool("isFalling", false);
        animator.SetBool("isMeleeAttacking", false);
        animator.SetBool("isStunned", false);
        animator.SetBool("isDead", false);

        playerController.PlaySlideSound();
    }

    public void Execute()
    {
        playerController.CheckPlayerStateChange();
        playerController.Dash();
    }

    public void Exit()
    {

    }
}
