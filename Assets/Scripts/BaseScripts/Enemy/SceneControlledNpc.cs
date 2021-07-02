using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneControlledNpc : IState
{
    protected NpcController npcController;
    protected Animator animator;

    public SceneControlledNpc(NpcController npcController, Animator animator)
    {
        this.npcController = npcController;
        this.animator = animator;
    }

    public void Enter()
    {
        npcController.isIdle = false;
        npcController.isWalking = false;
        npcController.isFrozen = false;
        npcController.isSceneControlled = true;
        npcController.isAttacking = false;
        npcController.isPatrolling = false;
        npcController.isDead = false;

        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", false);

        npcController.hasReachedDestination = false;
        npcController.SetFacingDirection();
    }

    public void Execute()
    {
        if (npcController.CheckIfEndReached())
        {
            npcController.hasReachedDestination = true;
            npcController.targetDestination = null;
            npcController.ChangeStateToIdle();
            return;
        }

        npcController.MoveNpc();
    }

    public void Exit()
    {
        
    }
}