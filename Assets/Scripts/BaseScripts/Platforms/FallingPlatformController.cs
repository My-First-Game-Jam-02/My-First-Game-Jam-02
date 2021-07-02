using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformController : MoveObject
{
    protected BoxCollider2D boxCollider;
    protected bool hasBeenTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!hasBeenTriggered)
            {
                Invoke("StartMoving", pauseTimeBeforeStart);
            }
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        objectToMove.transform.position = startPoint.transform.position;
        targetPosition = endPoint.transform.position;
        isMovingToEndPoint = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (shouldMove)
        {
            MoveTowardTargetPosition();
            if (CheckIfReachedDestination())
            {
                DisableMovement();
            }
        }
    }

    public void DisableTrigger()
    {
        boxCollider.enabled = false;
        hasBeenTriggered = true;
    }

    public void EnableTrigger()
    {
        boxCollider.enabled = true;
        hasBeenTriggered = false;
    }

    public void StartMoving()
    {
        DisableTrigger();
        EnableMovement();
    }

    public void ResetPlatform()
    {
        objectToMove.transform.position = startPoint.transform.position;
        EnableTrigger();
        DisableMovement();
    }
}
