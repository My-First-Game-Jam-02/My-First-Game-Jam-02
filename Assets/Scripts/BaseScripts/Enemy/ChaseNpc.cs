using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChaseNpc : IState
{
    protected EnemyGuardController enemyGuardController;
    protected Animator animator;
    protected float startChaseTime;

    public ChaseNpc(EnemyGuardController enemyGuardController, Animator animator)
    {
        this.enemyGuardController = enemyGuardController;
        this.animator = animator;
    }

    public void Enter()
    {
        enemyGuardController.isIdle = false;
        enemyGuardController.isWalking = false;
        enemyGuardController.isFrozen = false;
        enemyGuardController.isSceneControlled = false;
        enemyGuardController.isChasing = true;
        enemyGuardController.isAttacking = false;
        enemyGuardController.isPatrolling = false;

        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", false);

        startChaseTime = Time.time;
    }

    public void Execute()
    {
        enemyGuardController.ChasePlayer();

        if (!enemyGuardController.DetectPlayer())
        {
            enemyGuardController.ChangeStateToPatrolling();
        }

        if(enemyGuardController.attackCoolDownTime + startChaseTime < Time.time)
        {
            enemyGuardController.ChangeStateToAttacking();
        }
    }

    public void Exit()
    {

    }
}