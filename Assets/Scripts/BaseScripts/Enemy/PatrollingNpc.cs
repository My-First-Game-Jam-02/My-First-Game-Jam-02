using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatrollingNpc : IState
{
    protected EnemyController enemyController;
    protected Animator animator;
    protected GameObject player;
    protected GameObject enemy;

    public PatrollingNpc(EnemyController enemyController, Animator animator, SSPlayerHealth playerHealth)
    {
        this.enemyController = enemyController;
        this.animator = animator;
        this.player = playerHealth.gameObject;
        this.enemy = enemyController.gameObject;
    }

    public void Enter()
    {
        enemyController.isIdle = false;
        enemyController.isWalking = true;
        enemyController.isFrozen = false;
        enemyController.isSceneControlled = false;
        enemyController.isChasing = false;
        enemyController.isAttacking = false;
        enemyController.isPatrolling = true;
        enemyController.isDead = false;

        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", false);

        if (player.transform.position.x > enemyController.gameObject.transform.position.x)
        {
            enemyController.horizontalMovement = 1f;
            enemyController.MakeNpcFaceRight();
        } else
        {
            enemyController.horizontalMovement = -1f;
            enemyController.MakeNpcFaceLeft();
        }

        enemyController.attackCoolDownTime = Random.Range(enemyController.coolDownMinTime, enemyController.coolDownMaxTime);
    }

    public void Execute()
    {
        if (enemyController.DetectPlayer())
        {
            enemyController.ChangeStateToChasePlayer();
        }

        if (enemyController.isTouchingWall || enemyController.DetectPitfall())
        {
            
            if (enemyController.isFacingRight)
            {
                enemyController.horizontalMovement = -1f;
                enemyController.MakeNpcFaceLeft();
            } else
            {
                enemyController.horizontalMovement = 1f;
                enemyController.MakeNpcFaceRight();
            }
            
        }
    }

    public void Exit()
    {

    }
}