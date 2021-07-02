using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IdleNpc : IState
{
    protected NpcController npcController;
    protected Animator animator;

    public IdleNpc(NpcController npcController, Animator animator)
    {
        this.npcController = npcController;
        this.animator = animator;
    }

    public void Enter()
    {
        npcController.isIdle = true;
        npcController.isWalking = false;
        npcController.isFrozen = false;
        npcController.isSceneControlled = false;
        npcController.isChasing = false;
        npcController.isAttacking = false;
        npcController.isPatrolling = false;
        npcController.isDead = false;

        animator.SetBool("isIdle", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", false);
    }

    public void Execute()
    {
        
    }

    public void Exit()
    {

    }
}
