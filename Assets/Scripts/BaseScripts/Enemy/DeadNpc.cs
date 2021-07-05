using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeadNpc : IState
{
    protected EnemyController enemyController;
    protected Animator animator;
    protected CapsuleCollider2D capsuleCollider;

    public DeadNpc(EnemyController enemyController, Animator animator, CapsuleCollider2D capsuleCollider)
    {
        this.enemyController = enemyController;
        this.animator = animator;
        this.capsuleCollider = capsuleCollider;
    }

    public void Enter()
    {
        enemyController.isIdle = false;
        enemyController.isWalking = false;
        enemyController.isFrozen = false;
        enemyController.isSceneControlled = false;
        enemyController.isChasing = false;
        enemyController.isAttacking = false;
        enemyController.isPatrolling = false;
        enemyController.isDead = true;

        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", true);
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}