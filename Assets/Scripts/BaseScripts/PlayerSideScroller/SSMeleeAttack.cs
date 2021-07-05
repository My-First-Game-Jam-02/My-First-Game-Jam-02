using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SSMeleeAttack : IState
{
    protected SSPlayerController playerController;
    protected Animator animator;
    protected float startAttackTime;
    protected float timeToEndAttack = 0.3f;

    public SSMeleeAttack(SSPlayerController playerController, Animator animator)
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
        playerController.isShooting = true;
        playerController.isFrozen = false;
        playerController.isStunned = false;
        playerController.isDead = false;
        playerController.isSceneControlled = false;

        animator.SetBool("isWalking", false);
        animator.SetBool("isShooting", false);
        animator.SetBool("isWallSliding", false);
        animator.SetBool("isDashing", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("isMeleeAttacking", true);
        animator.SetBool("isStunned", false);
        animator.SetBool("isDead", false);

        startAttackTime = Time.time;

    }

    public void Execute()
    {
        if(startAttackTime + timeToEndAttack < Time.time)
        {
            playerController.ChangeStateToIdle();
        }
    }

    public void Exit()
    {
        playerController.DeactivateMeleeWeapon();
    }
}
