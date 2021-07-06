using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackNpc : IState
{
    protected EnemyGuardController enemyGuardController;
    protected Animator animator;
    protected Transform playerTarget;

    public AttackNpc(EnemyGuardController enemyGuardController, Animator animator, Transform playerTarget)
    {
        this.enemyGuardController = enemyGuardController;
        this.animator = animator;
        this.playerTarget = playerTarget;
    }

    public void Enter()
    {
        this.playerTarget = enemyGuardController.playerTarget;

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

        if(playerTarget.position.x < enemyGuardController.gameObject.transform.position.x)
        {
            enemyGuardController.MakeNpcFaceLeft();
        }
        else
        {
            enemyGuardController.MakeNpcFaceRight();
        }
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}