using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{

    protected Vector3 targetPosition;
    protected bool shouldMove;
    protected bool isMovingToEndPoint;

    [Header("Settings")]
    public float speed;
    public float pauseTime;
    public float pauseTimeBeforeStart;
    public GameObject objectToMove;
    public GameObject startPoint;
    public GameObject endPoint;

    // Start is called before the first frame update
    public virtual void Start()
    {
        objectToMove.transform.position = startPoint.transform.position;
        targetPosition = endPoint.transform.position;
        isMovingToEndPoint = true;

        if (pauseTimeBeforeStart != 0)
        {
            Invoke("EnableMovement", pauseTimeBeforeStart);
        } else
        {
            EnableMovement();
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (shouldMove)
        {
            MoveTowardTargetPosition();
            if (CheckIfReachedDestination())
            {
                if (pauseTime == 0f)
                {
                    ReverseDirection();
                }
                else
                {
                    shouldMove = false;
                    ReverseDirection();
                    Invoke("EnableMovement", pauseTime);
                }
            }
        }
    }

    public void MoveTowardTargetPosition()
    {
        float step = speed * Time.deltaTime;
        objectToMove.transform.position = Vector3.MoveTowards(objectToMove.transform.position, targetPosition, step);
    }

    public bool CheckIfReachedDestination()
    {
        if (Vector3.Distance(objectToMove.transform.position, targetPosition) < 0.001f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ReverseDirection()
    {
        if (isMovingToEndPoint)
        {
            isMovingToEndPoint = false;
            targetPosition = startPoint.transform.position;
        } else
        {
            isMovingToEndPoint = true;
            targetPosition = endPoint.transform.position;
        }

    }

    public void EnableMovement()
    {
        shouldMove = true;
    }

    public void DisableMovement()
    {
        shouldMove = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(startPoint.transform.position, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endPoint.transform.position, 0.2f);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(startPoint.transform.position, endPoint.transform.position);
    }
}
