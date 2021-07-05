using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackNpc : IState
{
    protected EnemyGuardController enemyGuardController;
    protected Animator animator;
    protected Transform playerTransform;

    public AttackNpc(EnemyGuardController enemyGuardController, Animator animator, SSPlayerHealth playerHealth)
    {
        this.enemyGuardController = enemyGuardController;
        this.animator = animator;
        this.playerTransform = playerHealth.gameObject.transform;
    }

    public void Enter()
    {
        enemyGuardController.isIdle = false;
        enemyGuardController.isWalking = false;
        enemyGuardController.isFrozen = false;
        enemyGuardController.isSceneControlled = false;
        enemyGuardController.isChasing = false;
        enemyGuardController.isAttacking = true;
        enemyGuardController.isPatrolling = false;
        enemyGuardController.isDead = false;

        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", true);
        animator.SetBool("isDead", false);

        enemyGuardController.horizontalMovement = 0;

        if (enemyGuardController.npcRigidBody != null)
        {
            enemyGuardController.npcRigidBody.velocity = Vector2.zero;
        }

        if(playerTransform.position.x < enemyGuardController.gameObject.transform.position.x)
        {
            enemyGuardController.MakeNpcFaceLeft();
        }
        else
        {
            enemyGuardController.MakeNpcFaceRight();
        }

        enemyGuardController.AttackPlayer();
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}