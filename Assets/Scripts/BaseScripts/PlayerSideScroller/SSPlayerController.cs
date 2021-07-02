using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using System;

public class SSPlayerController : MonoBehaviour
{

    private StateMachine stateMachine;
    private CameraController cameraController;
    private IState idlePlayer;
    private IState walkingPlayer;
    private IState jumpingPlayer;
    private IState fallingPlayer;
    private IState wallSlidingPlayer;
    private IState dashingPlayer;
    private IState meleeAttackPlayer;
    private IState frozenPlayer;
    private IState stunnedPlayer;
    private IState deadPlayer;
    private IState sceneControlledPlayer;
    private AudioSource audioSource;
    private float normalYWallJumpForce;
    private LevelControl levelController;
    private bool isTransitioning;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider;
    public SpringJoint2D joint;

    [HideInInspector]
    public bool ignoreInputs;
    [HideInInspector]
    public bool antiGravityOn;

    [Header("Player States")]
    public bool isFacingRight = false;
    public bool isIdle = false;
    public bool isWalking = false;
    public bool isJumping = false;
    public bool isWallJumping = false;
    public bool isGrounded = false;
    public bool isTouchingWall = false;
    public bool isWallSliding = false;
    public bool isDashing = false;
    public bool isMeleeAttacking = false;
    public bool isFrozen = false;
    public bool isAirBorn = false;
    public bool isInWater = false;
    public bool isStunned = false;
    public bool isDead = false;
    public bool isSceneControlled = false;
    public bool isHidden = false;
    public bool hasReachedDestination = true;
    public bool previousGrounded;
    public bool justGrounded;

    [Header("Running")]
    // Variables used for running
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float airMoveSpeed = 30f;
    [SerializeField] float reducedVelocityFactor;

    public float horizontalMovement = 0f;
    public float verticalMovement = 0f;

    [Header("Jumping")]
    // Variables used for Jumping
    [SerializeField] float waterGravityScale;
    [SerializeField] float normalJumpForce = 10f;
    [SerializeField] float waterJumpForce = 3f;
    [SerializeField] float waterMoveSpeed = 3f;
    [SerializeField] float jumpForce;
    [SerializeField] float smallJumpMultiplier = 0.5f;
    [SerializeField] int extraJumpsValue = 1;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    public int extraJumps;
    


    [Header("Wall Jumping and Wall Sliding")]
    // Variables for wall jumping and wall sliding
    [SerializeField] Transform wallCheckCollider;
    [SerializeField] float wallSlidingSpeed = 1f;
    [SerializeField] float xWallJumpForce = 1f;
    [SerializeField] float yWallJumpForce = 1f;
    [SerializeField] float timeOfWallJump = 0.5f;
    [SerializeField] float wallCheckRadius = 0.3f;
    [SerializeField] float pauseTimeForWallSlide;

    [Header("Dashing")]
    // Variables for dashing
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float dashCoolDownTime;
    [SerializeField] float distanceBetweenImages;
    float dashTimeLeft;
    float lastImageXpos;
    float timeOfLastDash = -100f;

    [Header("Anit Gravity")]
    public float normalGravityScale;
    public float spiritGravityScale;
    [SerializeField] float antiGravityScale;
    [SerializeField] float antiGravityJumpForce;
    [SerializeField] float antiGravityYWallJumpForce = 1f;

    [Header("Stunned")]
    // Variables for player getting stunned.
    public Vector2 stunForce;
    public float stunTime;

    [Header("Audio")]
    public GameObject jumpSound;
    public GameObject fireSound;
    public GameObject slideSound;
    public GameObject hurtSound;
    public GameObject deathSound;
    public GameObject meleeSound;


    [Header("Events")]
    public UnityEvent OnSlide = new UnityEvent();

    [Header("Player States")]
    public bool isPlayer = true;
    public bool isSpirit;
    public bool isEnemy;
    
    public RuntimeAnimatorController playerAnimatorController;
    public RuntimeAnimatorController spiritAnimatorController;
    public RuntimeAnimatorController enemyAnimatorController;

    public GameObject playerAnchor;
    public float maxDistance;

    // Cache
    public Rigidbody2D rigidBody;
    public Animator animator;
    public float playerDirection { get; private set; } = 1f;
    public new Collider2D collider;
    public GameObject close;
    public GameObject meleeWeapon;
    public Transform playerDestination;
    public float forceAttackDirection = 0;
    public GameObject restartLevelAction;
    public float timeBeforeRestart;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        if (animator == null)
            animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        collider = GetComponent<Collider2D>();
        levelController = FindObjectOfType<LevelControl>();
        cameraController = FindObjectOfType<CameraController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        stateMachine = gameObject.AddComponent<StateMachine>();
        idlePlayer = new SSIdlePlayer(this, animator);
        walkingPlayer = new SSWalkingPlayer(this, animator);
        jumpingPlayer = new SSJumpingPlayer(this, animator);
        fallingPlayer = new SSFallingPlayer(this, animator);
        wallSlidingPlayer = new SSWallSlidePlayer(this, animator);
        dashingPlayer = new SSDashingPlayer(this, animator);
        meleeAttackPlayer = new SSMeleeAttack(this, animator);
        frozenPlayer = new SSFrozenPlayer(this, animator);
        stunnedPlayer = new SSStunnedPlayer(this, animator);
        deadPlayer = new SSDeadPlayer(this, animator);
        sceneControlledPlayer = new SSSceneControlledPlayer(this, animator);

        normalGravityScale = rigidBody.gravityScale;
        normalJumpForce = jumpForce;
        extraJumps = extraJumpsValue;
        normalYWallJumpForce = yWallJumpForce;
        playerAnchor.SetActive(false);
        ChangeStateToIdle();
    }

    void Update()
    {
        
        CheckGrounded();
        CheckAirBorn();
        CheckIfFalling();
        CheckTouchingWall();
        UpdateSpringJoint();

        Inputs();
        

        ResetExtraJumps();

        this.stateMachine.ExecuteStateUpdate();

        CheckJustGrounded();
        previousGrounded = isGrounded;


    }

    private void FixedUpdate()
    {
        if (isStunned || isFrozen || isSceneControlled || isDead)
        {
            return;
        }
        HorizontalMovement();
        VerticalMovement();
        SetFacingDirection();

        if (!isDashing)
        {
            if (Mathf.Abs(horizontalMovement) < 0.2f && !isAirBorn && !isJumping && !isIdle)
            {
                ChangeStateToIdle();
            }
        }
    }

    private void Inputs()
    {
        if (isStunned || isFrozen || isDead || isDashing || isSceneControlled)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            HandleSwitchingBeings();
            return;
        }
       
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        if (isPlayer)
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                if (extraJumps > 0)
                {
                    ChangeStateToJumping();
                }
            }
        }


        if (isSpirit)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (Time.time >= (timeOfLastDash + dashCoolDownTime))
                {
                    AttemptToDash();
                }
            }
        }

       

        //if (Input.GetButtonDown("Jump") && !isInWater && canJump)
        //{
        //    if (isWallSliding)
        //    {
        //        isWallJumping = true;
        //        ChangeStateToJumping();
        //    }
        //}
    }

    private void HorizontalMovement()
    {
        if (isDashing || isDead) return;

        if (isPlayer)
        {
            if (isGrounded)
            {
                rigidBody.velocity = new Vector2(horizontalMovement * movementSpeed, rigidBody.velocity.y);
            }

            // Gives player control of the character in the air
            else if (!isGrounded && !isWallSliding && !isWallJumping && horizontalMovement != 0)
            {
                rigidBody.AddForce(new Vector2(airMoveSpeed * horizontalMovement, 0));
                if (Mathf.Abs(rigidBody.velocity.x) > movementSpeed)
                {
                    rigidBody.velocity = new Vector2(horizontalMovement * movementSpeed, rigidBody.velocity.y);
                }
            }
            // Allows player to fall faster when walking off a cliff.
            else if (!isGrounded && !isWallSliding && horizontalMovement == 0)
            {
                if (rigidBody.velocity.x > 0)
                {
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y);
                }
                else if (rigidBody.velocity.x < 0)
                {
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y);
                }

                if (Mathf.Abs(rigidBody.velocity.x) < 0.01f)
                {
                    rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
                }
            }

            if (isTouchingWall)
            {
                rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
            }
        }

        if (isSpirit)
        {
            
            rigidBody.velocity = new Vector2(horizontalMovement * movementSpeed, rigidBody.velocity.y);
            

            //if(Vector2.Distance(playerAnchor.transform.position, transform.position) < maxDistance)
            //{
            //    rigidBody.velocity = new Vector2(horizontalMovement * movementSpeed, rigidBody.velocity.y);
            //}
            //else
            //{
            //    if(transform.position.x > playerAnchor.transform.position.x && horizontalMovement < 0f)
            //    {
            //        rigidBody.velocity = new Vector2(horizontalMovement * movementSpeed, rigidBody.velocity.y);
            //    }
            //    else if (transform.position.x < playerAnchor.transform.position.x && horizontalMovement > 0f)
            //    {
            //        rigidBody.velocity = new Vector2(horizontalMovement * movementSpeed, rigidBody.velocity.y);
            //    } else
            //    {
            //        rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
            //    }
            //}
        }

    }

    private void VerticalMovement()
    {
        if (isSpirit)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, verticalMovement * movementSpeed);
            //if (Vector2.Distance(playerAnchor.transform.position, transform.position) < maxDistance)
            //{
            //    rigidBody.velocity = new Vector2(rigidBody.velocity.x, verticalMovement * movementSpeed);
            //}
            //else
            //{
            //    if (transform.position.y > playerAnchor.transform.position.y && verticalMovement < 0f)
            //    {
            //        rigidBody.velocity = new Vector2(rigidBody.velocity.x, verticalMovement * movementSpeed);
            //    }
            //    else if (transform.position.y < playerAnchor.transform.position.y && verticalMovement > 0f)
            //    {
            //        rigidBody.velocity = new Vector2(rigidBody.velocity.x, verticalMovement * movementSpeed);
            //    }
            //    else
            //    {
            //        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
            //    }
            //}
        }
    }

    public void MoveSceneControlledPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerDestination.position, movementSpeed * Time.deltaTime);
    }

    public void ShootGun()
    {
       
    }

    public void SetFacingDirection()
    {
        if (horizontalMovement < 0)
        {
            MakePlayerFaceLeft();
            playerDirection = -1f;
        }
        else if (horizontalMovement > 0)
        {
            MakePlayerFaceRight();
            playerDirection = 1f;
        }
    }

    public void MakePlayerFaceLeft()
    {
        isFacingRight = false;
        transform.eulerAngles = new Vector3(0, 180, 0);
    }

    public void MakePlayerFaceRight()
    {
        isFacingRight = true;
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void HandleSwitchingBeings()
    {
        if (isTransitioning) { return; }
        isTransitioning = true;

        if (isPlayer)
        {
            ChangeStateToIdle();
            SwitchToSpiritBeing();
            playerAnchor.transform.position = transform.position;
            playerAnchor.SetActive(true);
            Invoke("StopTransition", .5f);
            return;
        }

        if (isSpirit)
        {
            ChangeStateToFrozen();
            SwitchToPlayerBeing();
            cameraController.UseAnchorCamera();
            spriteRenderer.enabled = false;
            capsuleCollider.enabled = false;
            Invoke("StopTransition", .5f);
            Invoke("ActivatePlayer", .5f);
            return;
        }
    }

    public void StopTransition()
    {
        isTransitioning = false;
    }

    private void ActivatePlayer()
    {
        playerAnchor.SetActive(false);
        cameraController.UsePlayerCamera();
        transform.position = playerAnchor.transform.position;
        spriteRenderer.enabled = true;
        capsuleCollider.enabled = true;
        ChangeStateToIdle();
    }

    public void SwitchToPlayerBeing()
    {
        isPlayer = true;
        isSpirit = false;
        isEnemy = false;
        animator.runtimeAnimatorController = playerAnimatorController;
        rigidBody.gravityScale = normalGravityScale;
        gameObject.layer = 6;
    }

    public void SwitchToSpiritBeing()
    {
        isPlayer = false;
        isSpirit = true;
        isEnemy = false;
        animator.runtimeAnimatorController = spiritAnimatorController;
        rigidBody.gravityScale = spiritGravityScale;
        gameObject.layer = 7;
    }

    public void SwitchToEnemyBeing()
    {
        isPlayer = false;
        isSpirit = false;
        isEnemy = true;
        animator.runtimeAnimatorController = enemyAnimatorController;
        rigidBody.gravityScale = normalGravityScale;
    }

    public void UpdateSpringJoint()
    {
        if(Vector2.Distance(transform.position, playerAnchor.transform.position) > maxDistance)
        {
            joint.enabled = true;
        }
        else
        {
            joint.enabled = false;
        }
    }

    public void CheckPlayerStateChange()
    {
        if (isDead || isDashing) return;

        if (isIdle)
        {
            if (Mathf.Abs(horizontalMovement) > 0.2f && isGrounded)
            {
                ChangeStateToWalking();
            }
        }

        
        if (justGrounded)
        {
            if (Mathf.Abs(horizontalMovement) > 0.2f)
            {
                ChangeStateToWalking();
            }
            else
            {
                ChangeStateToIdle();
            }
        }
        

        //if (isTouchingWall)
        //{
        //    if (isFacingRight && horizontalMovement > 0.2f && !isGrounded && canWallSlide && !pausedForGroundedWallJump)
        //    {
        //        ChangeStateToWallSliding();
        //    }
        //    else if (!isFacingRight && horizontalMovement < 0.2f && !isGrounded && canWallSlide && !pausedForGroundedWallJump)
        //    {
        //        ChangeStateToWallSliding();
        //    }

        //    if (Mathf.Abs(horizontalMovement) < 0.2f)
        //    {
        //        ChangeStateToFalling();
        //    }
        //}
        //else
        //{
        //    if (!isGrounded && !isDashing)
        //    {
        //        ChangeStateToFalling();
        //    }
        //} 
    }

    public void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckCollider.position, groundCheckRadius, groundLayer);
    }

    public void CheckTouchingWall()
    {
        isTouchingWall = Physics2D.OverlapCircle(wallCheckCollider.position, wallCheckRadius, groundLayer);
    }

    public void CheckAirBorn()
    {
        if (!isGrounded && !isWallSliding)
        {
            isAirBorn = true;
            animator.SetBool("isAirBorn", true);
        }
        else
        {
            isAirBorn = false;
            animator.SetBool("isAirBorn", false);
        }
    }

    public void CheckIfInWater()
    {
        if (isInWater)
        {
            rigidBody.gravityScale = waterGravityScale;
            jumpForce = waterJumpForce;
            movementSpeed = waterMoveSpeed;
        }
        else
        {
            rigidBody.gravityScale = normalGravityScale;
            jumpForce = normalJumpForce;
            //movementSpeed = normalMoveSpeed;
        }
    }

    public bool CheckIfFalling()
    {

        bool isFalling = false;

        if (rigidBody.velocity.y < 0)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }

        return isFalling;
    }

    public bool CheckIfEndReached()
    {
        if (playerDestination == null)
        {
            return true;
        }

        if (Vector2.Distance(transform.position, playerDestination.position) < 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void ResetExtraJumps()
    {
        if (justGrounded || isDashing)
        {
            extraJumps = extraJumpsValue;
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

    public void Jump()
    {
        if (isTouchingWall)
        {
            if(Mathf.Abs(horizontalMovement) > 0.2f)
            {
                rigidBody.velocity = new Vector2(0f, jumpForce + 2.8f);
            }
            else
            {
                rigidBody.velocity = new Vector2(0f, jumpForce);
            }
        }
        else
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        }
        
        extraJumps--;

        //else if (isJumping && isGrounded && extraJumps == 0 || isJumping && isInWater)
        //{
        //    rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce * 100 * Time.fixedDeltaTime);
        //}
    }

    public void WallJump()
    {
        if (isFacingRight)
        {
            rigidBody.velocity = new Vector2(-xWallJumpForce, yWallJumpForce);
            MakePlayerFaceLeft();
        }
        else
        {
            rigidBody.velocity = new Vector2(xWallJumpForce, yWallJumpForce);
            MakePlayerFaceRight();
        }

        Invoke("StopWallJump", timeOfWallJump);
    }

    public void StopWallJump()
    {
        isWallJumping = false;
    }

    public void WallSlide()
    {
        extraJumps = extraJumpsValue;
        if (antiGravityOn)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, wallSlidingSpeed);
        }
        else
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, -wallSlidingSpeed);
        }
        
    }

    public void AttemptToDash()
    {
        dashTimeLeft = dashTime;
        timeOfLastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;

        ChangeStateToDashing();
    }

    public void Dash()
    {
        if (dashTimeLeft > 0)
        {
            rigidBody.velocity = new Vector2(dashSpeed * playerDirection, 0f);
            dashTimeLeft -= Time.deltaTime;

            if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImageXpos = transform.position.x;
            }
        }

        if (dashTimeLeft <= 0 || isTouchingWall)
        {
            ChangeStateToIdle();
        }
    }

    public void PlayFootstepSFX(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void SetPlayerToDead()
    {
        collider.enabled = false;
        animator.SetBool("isDead", true);
        isDead = true;
    }

    public void RevivePlayer()
    {
        collider.enabled = true;
        animator.SetBool("isDead", false);
        isDead = false;
    }

    public void PlayJumpSound()
    {
        if (jumpSound != null)
            Instantiate(jumpSound, transform.position, transform.rotation);
    }

    public void PlaySlideSound()
    {
        if (slideSound != null)
            Instantiate(slideSound, transform.position, transform.rotation);
    }

    public void PlayFireSound()
    {
        if (fireSound != null)
            Instantiate(fireSound, transform.position, transform.rotation);
    }

    public void PlayHurtSound()
    {
        if (hurtSound != null)
            Instantiate(hurtSound, transform.position, transform.rotation);
    }

    public void PlayDeathSound()
    {
        if (deathSound != null)
            Instantiate(deathSound, transform.position, transform.rotation);
    }

    public void PlayMeleeSound()
    {
        if (meleeSound != null)
            Instantiate(meleeSound, transform.position, transform.rotation);
    }

    public void RestoreGravity()
    {
        antiGravityOn = false;
        rigidBody.gravityScale = normalGravityScale;

        if (levelController.antiyGravityOn)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z);
        }
        transform.localScale = new Vector3(1, 1, 1);
        jumpForce = normalJumpForce;
        yWallJumpForce = normalYWallJumpForce;

        levelController.antiyGravityOn = false;
    }

    public void ReverseGravity()
    {
        antiGravityOn = true;

        rigidBody.gravityScale = antiGravityScale;
        
        if (!levelController.antiyGravityOn)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
        }
        
        transform.localScale = new Vector3(1, -1, 1);
        jumpForce = antiGravityJumpForce;
        yWallJumpForce = antiGravityYWallJumpForce;

        levelController.antiyGravityOn = true;
    }

    public void RestartLevel()
    {
        restartLevelAction.SetActive(true);
    }

    public void ActivateMeleeWeapon()
    {
        meleeWeapon.SetActive(true);
    }

    public void DeactivateMeleeWeapon()
    {
        meleeWeapon.SetActive(false);
    }

    #region State Changes
    public void ChangeStateToIdle()
    {
        //print("changing state to idle");
        this.stateMachine.ChangeState(idlePlayer);
    }

    public void ChangeStateToWalking()
    {
        //print("changing state to walking");
        this.stateMachine.ChangeState(walkingPlayer);
    }

    public void ChangeStateToJumping()
    {
        //print("changing state to jumping");
        this.stateMachine.ChangeState(jumpingPlayer);
    }

    public void ChangeStateToFalling()
    {
        this.stateMachine.ChangeState(fallingPlayer);
    }

    public void ChangeStateToWallSliding()
    {
        this.stateMachine.ChangeState(wallSlidingPlayer);
    }

    public void ChangeStateToDashing()
    {
        this.stateMachine.ChangeState(dashingPlayer);
    }

    public void ChangeStateToMeleeAttacking()
    {
        this.stateMachine.ChangeState(meleeAttackPlayer);
    }

    public void ChangeStateToFrozen()
    {
        this.stateMachine.ChangeState(frozenPlayer);
    }

    public void ChangeStateToStunned()
    {
        this.stateMachine.ChangeState(stunnedPlayer);
    }

    public void ChangeStateToDead()
    {
        this.stateMachine.ChangeState(deadPlayer);
    }

    public void ChangeStateToSceneControlled()
    {
        this.stateMachine.ChangeState(sceneControlledPlayer);
    }
    #endregion

    public void PrintString (string stringToPrint)
    {
        print(stringToPrint);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckCollider.position, groundCheckRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheckCollider.position, wallCheckRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(playerAnchor.transform.position, maxDistance);
    }
}
