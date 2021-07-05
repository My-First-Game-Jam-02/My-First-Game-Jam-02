using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NpcController : MonoBehaviour
{
    protected StateMachine stateMachine;
    protected IState idleNpc;
    protected IState walkingNpc;
    protected IState frozenNpc;
    protected IState sceneControlledNpc;
    protected SSPlayerController playerController;
    protected Animator animator;
    protected AudioSource audioSource;

    [HideInInspector]
    public Rigidbody2D npcRigidBody;

    [Header("Settings")]
    public float speed;
    public bool isFacingRight { get; protected set; }
    public Transform targetDestination;
    

    [Header("Npc States")]
    public bool isIdle;
    public bool isWalking;
    public bool isFrozen;
    public bool isSceneControlled;
    public bool isGrounded = false;
    public bool isAirBorn = false;
    public bool isTouchingWall = false;
    public bool isChasing = false;
    public bool isAttacking = false;
    public bool isPatrolling = false;
    public bool isDead = false;
    public bool hasReachedDestination = true;
    [HideInInspector]
    public bool previousGrounded;
    [HideInInspector]
    public bool justGrounded;
    [HideInInspector]
    public float velocityBeforeGrounded;
    [HideInInspector]
    public float previousDownwardVelocity;

    public Transform groundCheckCollider;
    public float groundCheckSize;
    public LayerMask groundLayer;

    public virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        npcRigidBody = GetComponent<Rigidbody2D>();
        playerController = FindObjectOfType<SSPlayerController>();

        stateMachine = gameObject.AddComponent<StateMachine>();
        idleNpc = new IdleNpc(this, animator);
        frozenNpc = new FrozenNpc(this, animator);
        walkingNpc = new WalkingNpc(this, animator);
        sceneControlledNpc = new SceneControlledNpc(this, animator);
    }

    public virtual void Update()
    {
        CheckGrounded();
        CheckAirBorn();
        SetFacingDirection();
        
        this.stateMachine.ExecuteStateUpdate();

        if (CheckJustGrounded())
        {
            velocityBeforeGrounded = previousDownwardVelocity;
        }

        previousGrounded = isGrounded;
        if(npcRigidBody != null)
        {
            previousDownwardVelocity = npcRigidBody.velocity.y;
        }
        
    }

    public virtual void FixedUpdate()
    {
        
    }

    public void ChangeStateToIdle()
    {
        this.stateMachine.ChangeState(idleNpc);
    }

    public void ChangeStateToFrozen()
    {
        this.stateMachine.ChangeState(frozenNpc);
    }

    public void ChangeStateToWalking()
    {
        this.stateMachine.ChangeState(walkingNpc);
    }

    public void ChangeStateToSceneControlled()
    {
        this.stateMachine.ChangeState(sceneControlledNpc);
    }

    public virtual void SetFacingDirection()
    {
        if(targetDestination != null)
        {
            if(targetDestination.transform.position.x > transform.position.x)
            {
                MakeNpcFaceRight();
            } else
            {
                MakeNpcFaceLeft();
            }
        }
    }

    public void MakeNpcFaceLeft()
    {
        isFacingRight = false;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
    }

    public void MakeNpcFaceRight()
    {
        isFacingRight = true;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0f, transform.eulerAngles.z);
    }

    public void MoveNpc()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetDestination.position, speed * Time.deltaTime);
    }

    public void CheckGrounded()
    {
        if(groundCheckCollider != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckCollider.position, groundCheckSize, groundLayer);
        } 
    }

    public void CheckAirBorn()
    {
        if (!isGrounded)
        {
            isAirBorn = true;
        }
        else
        {
            isAirBorn = false;
        }
    }

    public bool CheckJustGrounded()
    {
        if (previousGrounded == false && isGrounded)
        {
            justGrounded = true;
            return true;
        }
        else
        {
            justGrounded = false;
            return false;
        }
    }

    public virtual bool CheckIfEndReached()
    {
        if (targetDestination == null)
        {
            return true;
        }

        if(Vector2.Distance(transform.position, targetDestination.position) < 0.1f)
        {
            return true;
        } else
        {
            return false;
        }
        
    }


    public void PrintString(string stringToPrint)
    {
        print(stringToPrint);
    }
}
