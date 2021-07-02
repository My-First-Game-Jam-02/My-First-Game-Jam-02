using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FrozenNpc : IState
{
    protected NpcController npcController;
    protected Animator animator;

    public FrozenNpc(NpcController npcController, Animator animator)
    {
        this.npcController = npcController;
        this.animator = animator;
    }

    public void Enter()
    {
        npcController.isIdle = false;
        npcController.isWalking = false;
        npcController.isFrozen = true;
        npcController.isSceneControlled = false;
        npcController.isChasing = false;
        npcController.isAttacking = false;
        npcController.isPatrolling = false;
        npcController.isDead = false;

        animator.SetBool("isIdle", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", false);

        if (npcController.npcRigidBody != null)
        {
            npcController.npcRigidBody.velocity = Vector2.zero;
        }
    }

    public void Execute()
    {

    }

    public void Exit()
    {

    }
}