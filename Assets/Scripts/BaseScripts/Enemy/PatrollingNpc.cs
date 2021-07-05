using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatrollingNpc : IState
{
    protected EnemyGuardController enemyGuardController;
    protected Animator animator;
    protected GameObject player;
    protected GameObject enemy;

    public PatrollingNpc(EnemyGuardController enemyGuardController, Animator animator, SSPlayerHealth playerHealth)
    {
        this.enemyGuardController = enemyGuardController;
        this.animator = animator;
        this.player = playerHealth.gameObject;
        this.enemy = enemyGuardController.gameObject;
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

        if (player.transform.position.x > enemyGuardController.gameObject.transform.position.x)
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

        if (enemyGuardController.isTouchingWall || enemyGuardController.DetectPitfall())
        {
            
            if (enemyGuardController.isFacingRight)
            {
                enemyGuardController.horizontalMovement = -1f;
                enemyGuardController.MakeNpcFaceLeft();
            } else
            {
                enemyGuardController.horizontalMovement = 1f;
                enemyGuardController.MakeNpcFaceRight();
            }
            
        }
    }

    public void Exit()
    {

    }
}