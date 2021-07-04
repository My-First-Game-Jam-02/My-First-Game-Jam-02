using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : NpcController
{
    protected IState chaseNpc;
    protected IState attackNpc;
    protected IState patrollingNpc;
    protected IState deadNpc;
    private CapsuleCollider2D capsuleCollider;
    private SSPlayerHealth playerHealth;

    public bool isTouchingPlayer;

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
    public int playerDectorRaycasts;
    public float playerDectorLength;
    public float distanceBetweenRaycasts;
    public Transform raycastsStartPoint;
    public float horizontalMovement = 0f;
    public Transform bulletSpawnPoint;
    public Bullet pfBullet;
    public float coolDownMinTime;
    public float coolDownMaxTime;
   

    public override void Awake()
    {
        base.Awake();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerHealth = FindObjectOfType<SSPlayerHealth>();
        chaseNpc = new ChaseNpc(this, animator);
        attackNpc = new AttackNpc(this, animator, playerHealth);
        patrollingNpc = new PatrollingNpc(this, animator, playerHealth);
        deadNpc = new DeadNpc(this, animator, capsuleCollider);

        if (playerHealth.gameObject.transform.position.x > transform.position.x)
        {
            horizontalMovement = 1f;
        }
        else
        {
            horizontalMovement = -1f;
        }

       
        ChangeStateToPatrolling();
        
    }

    private void OnEnable()
    {
        ChangeStateToPatrolling();
    }

    public override void Update()
    {
        CheckTouchingWall();
        base.Update();
        SetAnimator();
    }

    public void FixedUpdate()
    {
        HorizontalMovement();
    }

    private void HorizontalMovement()
    {
        if (isDead || isFrozen || !npcRigidBody) return;

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

        if (DetectPlayer())
        {
            if (!isTouchingPlayer)
            {
                if (playerHealth.gameObject.transform.position.x > transform.position.x)
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

    public void AttackPlayer()
    {
        attackStartTime = Time.time;

        Bullet spawnedBullet = Instantiate(pfBullet, bulletSpawnPoint.transform.position, transform.rotation);

        Vector2 shootDirection = isFacingRight ? Vector2.right : Vector2.left;
        spawnedBullet.OnObjectSpawn(shootDirection);
    }

    public void SetAnimator()
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
        bool hitPlayer = false;
        Vector3 rayCastOrigin = new Vector3(raycastsStartPoint.position.x + (playerDectorLength / 2), raycastsStartPoint.position.y, raycastsStartPoint.position.z);

        for (int i = 0; i < playerDectorRaycasts; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayCastOrigin, Vector2.left, playerDectorLength, playerLayer);
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

    public void ChangeStateToDead()
    {
        this.stateMachine.ChangeState(deadNpc);
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

        Vector3 rayCastOrigin = new Vector3(raycastsStartPoint.position.x + (playerDectorLength / 2), raycastsStartPoint.position.y, raycastsStartPoint.position.z);

        for (int i = 0; i < playerDectorRaycasts; i++)
        {
            Gizmos.DrawLine(rayCastOrigin, new Vector3(rayCastOrigin.x - playerDectorLength, rayCastOrigin.y, rayCastOrigin.z));
            rayCastOrigin = new Vector3(rayCastOrigin.x, rayCastOrigin.y + distanceBetweenRaycasts, rayCastOrigin.z);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheckCollider.position, checkRadius);
    }
}
