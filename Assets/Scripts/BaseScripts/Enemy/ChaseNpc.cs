using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChaseNpc : IState
{
    protected EnemyController enemyController;
    protected Animator animator;

    public ChaseNpc(EnemyController enemyController, Animator animator)
    {
        this.enemyController = enemyController;
        this.animator = animator;
    }

    public void Enter()
    {
        enemyController.isIdle = false;
        enemyController.isWalking = false;
        enemyController.isFrozen = false;
        enemyController.isSceneControlled = false;
        enemyController.isChasing = true;
        enemyController.isAttacking = false;
        enemyController.isPatrolling = false;

        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", false);
    }

    public void Execute()
    {
        enemyController.ChasePlayer();
        if (!enemyController.DetectPlayer())
        {
            enemyController.ChangeStateToPatrolling();
        }
    }

    public void Exit()
    {

    }
}