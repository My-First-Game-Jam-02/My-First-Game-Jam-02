using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuardController : EnemyController
{
    protected IState chaseNpc;
    protected IState attackNpc;
    protected IState patrollingNpc;
    public Transform playerTarget;

    public bool isTouchingPlayer;
    public bool isTouchingAnchor;

    [HideInInspector]
    public float attackStartTime;
    [HideInInspector]
    public float attackCoolDownTime;

    public bool canJump;
    public float jumpDectorLength;
    [SerializeField] Transform jumpDetector;
    public float jumpForce;
    [SerializeField] Transform wallCheckCollider;
    [SerializeField] float checkRadius = 0.3f;
    public LayerMask playerLayer;
    public LayerMask possessedEnemyLayer;
    public int playerDetectorRaycasts;
    public float playerDetectorLength;
    public float distanceBetweenRaycasts;
    public Transform raycastsStartPoint;
    public float horizontalMovement = 0f;

    public override void Awake()
    {
        base.Awake();
        playerTarget = playerController.transform;
        chaseNpc = new ChaseNpc(this, animator);
        attackNpc = new AttackNpc(this, animator, playerTarget);
        patrollingNpc = new PatrollingNpc(this, animator, playerTarget);

    }

    private void OnEnable()
    {
        if (playerHealth.gameObject.transform.position.x > transform.position.x)
        {
            horizontalMovement = 1f;
        }
        else
        {
            horizontalMovement = -1f;
        }
        print("changing state to patrolling");
        ChangeStateToPatrolling();
    }

    public override void Update()
    {
        CheckTouchingWall();
        base.Update();
        SetAnimatorForChasing();
    }

    public override void FixedUpdate()
    {
        HorizontalMovement();
    }

    private void HorizontalMovement()
    {
        if (isDead || isFrozen || !npcRigidBody || isAirBorn) return;

        if (isGrounded)
        {
            npcRigidBody.velocity = new Vector2(horizontalMovement * speed, npcRigidBody.velocity.y);
        }
        else
        {
            npcRigidBody.velocity = new Vector2((horizontalMovement * speed / 2), npcRigidBody.velocity.y);
        }
    }

    public override void SetFacingDirection()
    {
        if (isChasing)
        {
            if (horizontalMovement < 0)
            {
                MakeNpcFaceLeft();
            }
            else if (horizontalMovement > 0)
            {
                MakeNpcFaceRight();
            }
        }
    }

    public void ChasePlayer()
    {
        //if (isGrounded && canJump && DetectPitfall())
        //{
        //    MakeNpcJump();
        //}

        if (!isGrounded)
        {
            npcRigidBody.velocity = new Vector2(0f, npcRigidBody.velocity.y);
            return;
        }

        if (DetectPlayer())
        {
            if(playerTarget.gameObject.layer == 15)
            {
                if (!isTouchingAnchor && !isTouchingPlayer)
                {
                    if (playerTarget.position.x > transform.position.x)
                    {
                        if (!DetectPitfall())
                        {
                            horizontalMovement = 1f;
                        }
                        else
                        {
                            horizontalMovement = 0f;
                        }
                    }
                    else
                    {
                        if (!DetectPitfall())
                        {
                            horizontalMovement = -1f;
                        }
                        else
                        {
                            horizontalMovement = 0f;
                        }
                    }
                }
                else
                {
                    horizontalMovement = 0f;
                }
            }
            if (playerTarget.gameObject.layer == 18)
            {
                if (!isTouchingPlayer)
                {
                    if (playerTarget.position.x > transform.position.x)
                    {
                        if (!DetectPitfall())
                        {
                            horizontalMovement = 1f;
                        }
                        else
                        {
                            horizontalMovement = 0f;
                        }
                    }
                    else
                    {
                        if (!DetectPitfall())
                        {
                            horizontalMovement = -1f;
                        }
                        else
                        {
                            horizontalMovement = 0f;
                        }
                    }
                }
                else
                {
                    horizontalMovement = 0f;
                }
            }
        }

        
    }

    public void Patrol()
    {
        if (!isGrounded)
        {
            npcRigidBody.velocity = new Vector2(0f, npcRigidBody.velocity.y);
            return;
        }
       
            if (isTouchingWall || DetectPitfall())
            {

                if (isFacingRight)
                {
                    horizontalMovement = -1f;
                    MakeNpcFaceLeft();
                }
                else
                {
                    horizontalMovement = 1f;
                    MakeNpcFaceRight();
                }

            }
    }
    public void ShootWeapon()
    {
        Bullet spawnedBullet = Instantiate(pfBullet, bulletSpawnPoint.transform.position, transform.rotation);

        Vector2 shootDirection = isFacingRight ? Vector2.right : Vector2.left;
        spawnedBullet.OnObjectSpawn(shootDirection);
    }

    public void SetAnimatorForChasing()
    {
        if (isChasing)
        {
            if (isGrounded)
            {
                animator.SetBool("isAirborn", false);
                if (Mathf.Abs(horizontalMovement) > 0f)
                {
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isIdle", false);
                }
                else
                {
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isIdle", true);
                }
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", false);
                animator.SetBool("isAirborn", true);
            }
        }

    }

    public bool DetectPlayer()
    {

        bool hitTypeOfPlayer = false;
        bool hitPlayerHealth = DetectPlayerHealth();
        bool hitPossessedEnemy = DetectPossessedEnemy();

        if (hitPlayerHealth)
        {
            playerTarget = playerHealth.gameObject.transform;
        }

        if (hitPossessedEnemy)
        {
            playerTarget = playerController.gameObject.transform;
        }

        
        if(hitPlayerHealth || hitPossessedEnemy)
        {
            hitTypeOfPlayer = true;
        }

        return hitTypeOfPlayer;
    }

    public bool DetectPlayerHealth()
    {
        bool hitPlayer = false;
        Vector3 rayCastOrigin = new Vector3(raycastsStartPoint.position.x + (playerDetectorLength / 2), raycastsStartPoint.position.y, raycastsStartPoint.position.z);

        for (int i = 0; i < playerDetectorRaycasts; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, Vector2.left, playerDetectorLength, playerLayer);
            if (hit.collider)
            {
                SSPlayerHealth playerHealth = hit.collider.gameObject.GetComponent<SSPlayerHealth>();
                if (playerHealth != null)
                {
                    hitPlayer = true;
                }
            }

            rayCastOrigin = new Vector3(rayCastOrigin.x, rayCastOrigin.y + distanceBetweenRaycasts, rayCastOrigin.z);
        }

        return hitPlayer;
    }

    public bool DetectPossessedEnemy()
    {
        bool hitPossessedEnemy = false;
        Vector3 rayCastOrigin = new Vector3(raycastsStartPoint.position.x + (playerDetectorLength / 2), raycastsStartPoint.position.y, raycastsStartPoint.position.z);

        for (int i = 0; i < playerDetectorRaycasts; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, Vector2.left, playerDetectorLength, possessedEnemyLayer);
            if (hit.collider)
            {
                hitPossessedEnemy = true;
            }

            rayCastOrigin = new Vector3(rayCastOrigin.x, rayCastOrigin.y + distanceBetweenRaycasts, rayCastOrigin.z);
        }

        return hitPossessedEnemy;
    }

    public bool DetectPitfall()
    {
        RaycastHit2D hit = Physics2D.Raycast(jumpDetector.transform.position, Vector2.down, jumpDectorLength, groundLayer);
        if (hit.collider == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void MakeNpcJump()
    {
        npcRigidBody.velocity = new Vector2(npcRigidBody.velocity.x, jumpForce);
    }

    public void ChangeStateToChasePlayer()
    {
        //print("changing state to chasing");
        this.stateMachine.ChangeState(chaseNpc);
    }

    public void ChangeStateToPatrolling()
    {
        //print("changing state to patrolling");
        this.stateMachine.ChangeState(patrollingNpc);
    }

    public void ChangeStateToAttacking()
    {
        //print("changing state to attacking");
        this.stateMachine.ChangeState(attackNpc);
    }

    public void CheckTouchingWall()
    {
        if (wallCheckCollider != null)
        {
            isTouchingWall = Physics2D.OverlapCircle(wallCheckCollider.position, checkRadius, groundLayer);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckCollider.position, groundCheckSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(jumpDetector.transform.position, new Vector3(jumpDetector.transform.position.x, jumpDetector.transform.position.y - jumpDectorLength, jumpDetector.position.z));
        Gizmos.color = Color.red;

        Vector3 rayCastOrigin = new Vector3(raycastsStartPoint.position.x + (playerDetectorLength / 2), raycastsStartPoint.position.y, raycastsStartPoint.position.z);

        for (int i = 0; i < playerDetectorRaycasts; i++)
        {
            Gizmos.DrawLine(rayCastOrigin, new Vector3(rayCastOrigin.x - playerDetectorLength, rayCastOrigin.y, rayCastOrigin.z));
            rayCastOrigin = new Vector3(rayCastOrigin.x, rayCastOrigin.y + distanceBetweenRaycasts, rayCastOrigin.z);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheckCollider.position, checkRadius);
    }
}
