using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolScript : NpcController
{

    private bool isMoving;
    private bool isPaused;
    private bool hasDiscoveredPlayer;

    private ActionController actionController;
    

    public SingleAction actionToActivate;
    public Transform startDestination;
    public Transform endDesitnation;
    public float idleTime;

    

    public override void Awake()
    {
        base.Awake();
        actionController = FindObjectOfType<ActionController>();
        PlaceNpcAtStart();
        targetDestination = endDesitnation;
        ChangeStateToSceneControlled();
        isMoving = true;
        isPaused = false;
        hasDiscoveredPlayer = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (hasDiscoveredPlayer) { return; }

        if (collision.tag == "Player")
        {
            if (!playerController.isHidden)
            {
                hasDiscoveredPlayer = true;

                PatrolScript[] henchpersonPatrollers = FindObjectsOfType<PatrolScript>();
                for (int i = 0; i < henchpersonPatrollers.Length; i++)
                {
                    henchpersonPatrollers[i].hasDiscoveredPlayer = true;
                    henchpersonPatrollers[i].ChangeStateToIdle();

                    if (playerController.transform.position.x < henchpersonPatrollers[i].gameObject.transform.position.x)
                    {
                        henchpersonPatrollers[i].MakeNpcFaceLeft();
                    }
                    else
                    {
                        henchpersonPatrollers[i].MakeNpcFaceRight();
                    }
                }
                

                if(actionToActivate != null)
                {
                    ActivateSpecificAction();
                }
            }
        }
    }
    public override void Update()
    {
        if (hasDiscoveredPlayer){return;}

        if (CheckIfEndReached() && isMoving)
        {
            PauseMovement();
        }
        base.Update();
    }

    private void PlaceNpcAtStart()
    {
        this.gameObject.transform.position = startDestination.position;
    }

    public override bool CheckIfEndReached()
    {

        if (Vector2.Distance(transform.position, targetDestination.position) < 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void PauseMovement()
    {
        isPaused = true;
        isMoving = false;
        ChangeStateToIdle();
        Invoke("MakeNpcChangeDirection", idleTime);
    }

    private void MakeNpcChangeDirection()
    {
        if(hasDiscoveredPlayer) { return; }
        if(targetDestination == endDesitnation)
        {
            targetDestination = startDestination;
        } else
        {
            targetDestination = endDesitnation;
        }

        isPaused = false;
        isMoving = true;
        ChangeStateToSceneControlled();
    }

    private void ActivateSpecificAction()
    {
        actionController.DeactivateAllActions();
        actionToActivate.gameObject.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startDestination.position, 1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endDesitnation.position, 1f);
    }
}
