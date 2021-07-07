using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SSStunnedPlayer : IState
{
    protected SSPlayerController playerController;
    protected Animator animator;
    protected float stunStartTime;

    public SSStunnedPlayer(SSPlayerController playerController, Animator animator)
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
        playerController.isShooting = false;
        playerController.isFrozen = false;
        playerController.isStunned = true;
        playerController.isDead = false;
        playerController.isSceneControlled = false;

        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isShooting", false);
        animator.SetBool("isWallSliding", false);
        animator.SetBool("isDashing", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("isMeleeAttacking", false);
        animator.SetBool("isStunned", true);
        animator.SetBool("isDead", false);

        playerController.rigidBody.velocity = new Vector2(-playerController.playerDirection * playerController.stunForce.x, playerController.stunForce.y);

        stunStartTime = Time.time;
    }

    public void Execute()
    {
        if(stunStartTime + playerController.stunTime < Time.time)
        {
            playerController.ChangeStateToIdle();
        }
    }

    public void Exit()
    {

    }
}
