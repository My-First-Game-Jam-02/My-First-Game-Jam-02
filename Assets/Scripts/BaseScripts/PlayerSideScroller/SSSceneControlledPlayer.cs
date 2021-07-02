using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SSSceneControlledPlayer : IState
{
    protected SSPlayerController playerController;
    protected Animator animator;

    public SSSceneControlledPlayer(SSPlayerController playerController, Animator animator)
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
        playerController.isSceneControlled = true;

        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);
        animator.SetBool("isShooting", false);
        animator.SetBool("isWallSliding", false);
        animator.SetBool("isDashing", true);
        animator.SetBool("isFalling", false);
        animator.SetBool("isMeleeAttacking", false);
        animator.SetBool("isStunned", false);
        animator.SetBool("isDead", false);

        playerController.hasReachedDestination = false;
        if (playerController.transform.position.x < playerController.playerDestination.position.x)
        {
            playerController.MakePlayerFaceRight();
        }
        else
        {
            playerController.MakePlayerFaceLeft();
        }
    }

    public void Execute()
    {
        if (playerController.CheckIfEndReached())
        {
            playerController.hasReachedDestination = true;
            playerController.playerDestination = null;
            playerController.ChangeStateToFrozen();
            return;
        }

        playerController.MoveSceneControlledPlayer();
    }

    public void Exit()
    {

    }
}