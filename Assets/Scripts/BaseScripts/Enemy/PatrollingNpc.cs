using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatrollingNpc : IState
{
    protected EnemyGuardController enemyGuardController;
    protected Animator animator;
    protected Transform playerTarget;

    public PatrollingNpc(EnemyGuardController enemyGuardController, Animator animator, Transform playerTarget)
    {
        this.enemyGuardController = enemyGuardController;
        this.animator = animator;
        this.playerTarget = playerTarget;
    }

    public void Enter()
    {
        enemyGuardController.isIdle = false;
        enemyGuardController.isWalking = true;
        enemyGuardController.isFrozen = false;
        enemyGuardController.isSceneControlled = false;
        enemyGuardController.isChasing = false;
        enemyGuardController.isAttacking = false;
        enemyGuardController.isPatrolling = true;
        enemyGuardController.isDead = false;

        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", false);

        if (playerTarget.transform.position.x > enemyGuardController.gameObject.transform.position.x)
        {
            enemyGuardController.horizontalMovement = 1f;
            enemyGuardController.MakeNpcFaceRight();
        } else
        {
            enemyGuardController.horizontalMovement = -1f;
            enemyGuardController.MakeNpcFaceLeft();
        }

        enemyGuardController.attackCoolDownTime = Random.Range(enemyGuardController.coolDownMinTime, enemyGuardController.coolDownMaxTime);
    }

    public void Execute()
    {
        if (enemyGuardController.DetectPlayer())
        {
            enemyGuardController.ChangeStateToChasePlayer();
        }

        enemyGuardController.Patrol();
            
    }

    public void Exit()
    {

    }
}