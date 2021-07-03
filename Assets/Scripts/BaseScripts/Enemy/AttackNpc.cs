using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackNpc : IState
{
    protected EnemyController enemyController;
    protected Animator animator;
    protected Transform playerTransform;

    public AttackNpc(EnemyController enemyController, Animator animator, SSPlayerController playerController)
    {
        this.enemyController = enemyController;
        this.animator = animator;
        this.playerTransform = playerController.gameObject.transform;
    }

    public void Enter()
    {
        enemyController.isIdle = false;
        enemyController.isWalking = false;
        enemyController.isFrozen = false;
        enemyController.isSceneControlled = false;
        enemyController.isChasing = false;
        enemyController.isAttacking = true;
        enemyController.isPatrolling = false;
        enemyController.isDead = false;

        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);
        animator.SetBool("isDead", false);

        enemyController.horizontalMovement = 0;

        if (enemyController.npcRigidBody != null)
        {
            enemyController.npcRigidBody.velocity = Vector2.zero;
        }

        if(playerTransform.position.x < enemyController.gameObject.transform.position.x)
        {
            enemyController.MakeNpcFaceLeft();
        }
        else
        {
            enemyController.MakeNpcFaceRight();
        }
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}