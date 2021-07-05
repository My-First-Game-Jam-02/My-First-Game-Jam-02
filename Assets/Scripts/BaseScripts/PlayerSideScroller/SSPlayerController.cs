﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using UnityEngine.UI;
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
    private IState shootingPlayer;
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
    private HealthFollow healthFollow;
    private bool canPossess;
    private bool isPossessing;
    private GameObject currentlyPossessedEnemy;
    private Slider spiritBar;
    private SpringJoint2D joint;

    [HideInInspector]
    public bool ignoreInputs;
    [HideInInspector]
    public bool antiGravityOn;

    [Header("Player States")]
    public bool isFacingRight = true;
    public bool isIdle = false;
    public bool isWalking = false;
    public bool isJumping = false;
    public bool isWallJumping = false;
    public bool isGrounded = false;
    public bool isTouchingWall = false;
    public bool isWallSliding = false;
    public bool isDashing = false;
    public bool isShooting = false;
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


    [Header("Events")]
    public UnityEvent OnSlide = new UnityEvent();

    [Header("Player States")]
    public bool isPlayer = true;
    public bool isSpirit;
    public bool isGuardBot;
    public bool isDroneBot;
    public bool isRollerBot;
    
    public RuntimeAnimatorController playerAnimatorController;
    public RuntimeAnimatorController spiritAnimatorController;
    public RuntimeAnimatorController enemyGuardAnimatorController;
    public RuntimeAnimatorController enemyDroneAnimatorController;
    public GameObject playerAnchor;
    public int maxSpiritHealth;
    public int currentSpiritHealth;
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
    public List<GameObject> possessableEnemies = new List<GameObject>();
    public string poolToSpawnFrom;
    public Transform gunEndPosition;
    public float possessionOffset;

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
        healthFollow = FindObjectOfType<HealthFollow>();
        spiritBar = GameObject.Find("spiritBar").GetComponent<Slider>();
        stateMachine = gameObject.AddComponent<StateMachine>();
        idlePlayer = new SSIdlePlayer(this, animator);
        walkingPlayer = new SSWalkingPlayer(this, animator);
        jumpingPlayer = new SSJumpingPlayer(this, animator);
        fallingPlayer = new SSFallingPlayer(this, animator);
        wallSlidingPlayer = new SSWallSlidePlayer(this, animator);
        dashingPlayer = new SSDashingPlayer(this, animator);
        shootingPlayer = new SSShootingPlayer(this, animator);
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
        SwitchToPlayerBeing();
    }

    void Update()
    {
        
        CheckGrounded();
        CheckAirBorn();
        CheckIfFalling();
        CheckTouchingWall();
        //UpdateSpringJoint();

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
            if (Mathf.Abs(horizontalMovement) < 0.2f && !isAirBorn && !isJumping && !isIdle && !isShooting)
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

        if (Input.GetKeyUp(KeyCode.E))
        {
            HandleSwitchingSpirit();
            return;
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            if (isPossessing)
            {
                DepossessEnemy();
            }
            else if(possessableEnemies.Count > 0)
            {
                PossessEnemy();
            }
            
        }

        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        if (isPlayer || isGuardBot)
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                if (extraJumps > 0)
                {
                    ChangeStateToJumping();
                }
            }
        }

        
        if (Input.GetButtonDown("Fire1") && !isShooting)
        {
            if (isGuardBot)
            {
                if (isGrounded)
                {
                    ChangeStateToShooting();
                    Invoke("ChangeStateToIdle", 0.5f);
                }
            }

            if (isDroneBot)
            {
                ChangeStateToShooting();
                Invoke("ChangeStateToIdle", 0.5f);
            }
        }
        

        if (isSpirit)
        {
            //if (Input.GetKeyDown(KeyCode.LeftShift))
            //{
            //    if (Time.time >= (timeOfLastDash + dashCoolDownTime))
            //    {
            //        AttemptToDash();
            //    }
            //}
        }
    }

    private void HorizontalMovement()
    {
        if (isDashing || isDead) return;

        if (isPlayer || isGuardBot)
        {
            if (isShooting) { return; }

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

        if (isSpirit || isDroneBot)
        {
            
            rigidBody.velocity = new Vector2(horizontalMovement * movementSpeed, rigidBody.velocity.y);
            
        }

    }

    private void VerticalMovement()
    {
        if (isFrozen)
        {
            return;
        }

        if (isSpirit || isDroneBot)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, verticalMovement * movementSpeed);
        }
    }

    public void MoveSceneControlledPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerDestination.position, movementSpeed * Time.deltaTime);
    }

    public void ShootGun()
    {

        Vector3 shootDir = Vector3.left;
        if (isGuardBot)
        {
            if (isFacingRight)
            {
                shootDir = Vector3.right;
            }
        }

        if (isDroneBot)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            shootDir = (mousePosition - gunEndPosition.position).normalized;
        }
        animator.SetBool("isShooting", true);
        ObjectPooler.Instance.SpawnFromPool(poolToSpawnFrom, gunEndPosition.position, shootDir, Quaternion.identity);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;
        return worldPosition;
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

    public void HandleSwitchingSpirit()
    {
        if (isTransitioning) { return; }

        if (isPlayer)
        {
            StartTransitionToSpirit();
            return;
        }

        if (isSpirit)
        {
            StartTransitionToPlayer();
            return;
        }
    }

    private void StartTransitionToSpirit()
    {
        isTransitioning = true;
        SetPlayerAnchor();
        ChangeStateToFrozen();
        SwitchToSpiritBeing();
        healthFollow.MakeHealthFollowAnchor();
        Invoke("StopTransition", .5f);
        Invoke("ChangeStateToIdle", .5f);
    }

    private void StartTransitionToPlayer()
    {
        isTransitioning = true;
        ChangeStateToFrozen();
        SwitchToPlayerBeing();
        cameraController.UseAnchorCamera();
        spriteRenderer.enabled = false;
        capsuleCollider.enabled = false;
        Invoke("StopTransition", .5f);
        Invoke("ActivatePlayer", .5f);
    }

    public void SwitchToPlayerBeing()
    {
        isPlayer = true;
        isSpirit = false;
        isGuardBot = false;
        isDroneBot = false;
        isRollerBot = false;

        currentSpiritHealth = maxSpiritHealth;
        UpdateSpiritHealth();

        animator.runtimeAnimatorController = playerAnimatorController;
        rigidBody.gravityScale = normalGravityScale;
        gameObject.layer = 6;
    }

    public void SwitchToSpiritBeing()
    {

        if (isDroneBot)
        {
            
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + possessionOffset, transform.position.z);
        }

        isPlayer = false;
        isSpirit = true;
        isGuardBot = false;
        isDroneBot = false;
        isRollerBot = false;

        StartCoroutine(DecreaseSpiritHealth());
        animator.runtimeAnimatorController = spiritAnimatorController;
        animator.SetBool("isExiting", true);
        animator.SetBool("isIdle", false);
        rigidBody.gravityScale = spiritGravityScale;
        
        gameObject.layer = 7;
    }

    public void SwitchToGuardBot()
    {
        isPlayer = false;
        isSpirit = false;
        isGuardBot = true;
        isDroneBot = false;
        isRollerBot = false;

        transform.position = currentlyPossessedEnemy.transform.position;
        currentlyPossessedEnemy.SetActive(false);
        animator.runtimeAnimatorController = enemyGuardAnimatorController;
        rigidBody.gravityScale = normalGravityScale;
        gameObject.layer = 6;
    }

    public void SwitchToDroneBot()
    {
        isPlayer = false;
        isSpirit = false;
        isGuardBot = false;
        isDroneBot = true;
        isRollerBot = false;

        transform.position = currentlyPossessedEnemy.transform.position;
        currentlyPossessedEnemy.SetActive(false);
        animator.runtimeAnimatorController = enemyDroneAnimatorController;
        rigidBody.gravityScale = spiritGravityScale;
        gameObject.layer = 7;
    }

    private void PossessEnemy()
    {
        isPossessing = true;
        animator.SetBool("isExiting", false);
        animator.SetBool("isIdle", false);
        ChangeStateToFrozen();

        GameObject closestEnemy = FindClosestEnemy();
        currentlyPossessedEnemy = closestEnemy;
        EnemyController enemyController = currentlyPossessedEnemy.GetComponent<EnemyController>();
        if (enemyController.enemyType == EnemyController.EnemyType.GuardBot)
        {
            EnemyGuardController enemyGuardController = enemyController.GetComponent<EnemyGuardController>();
            enemyGuardController.ChangeStateToFrozen();
            Invoke("SwitchToGuardBot", .5f);
            Invoke("ChangeStateToIdle", .5f);
        }
        else if (enemyController.enemyType == EnemyController.EnemyType.DroneBot)
        {
            EnemyFlyingController enemyFlyingController = enemyController.GetComponent<EnemyFlyingController>();
            enemyFlyingController.isPossessed = true;
            Invoke("SwitchToDroneBot", .5f);
            Invoke("ChangeStateToIdle", .5f);
        }
    }

    private void DepossessEnemy()
    {
        isPossessing = false;
        isTransitioning = true;
        animator.SetBool("isExiting", true);
        ChangeStateToFrozen();
        SwitchToSpiritBeing();

        EnemyController enemyController = currentlyPossessedEnemy.GetComponent<EnemyController>();
        if (enemyController.enemyType == EnemyController.EnemyType.GuardBot)
        {
            currentlyPossessedEnemy.transform.position = new Vector3(transform.position.x, transform.position.y - possessionOffset, transform.position.z);

            EnemyGuardController enemyGuardController = enemyController.GetComponent<EnemyGuardController>();
            enemyGuardController.ChangeStateToPatrolling();
            Invoke("StopTransition", .5f);
            Invoke("ChangeStateToIdle", .5f);
        } else if (enemyController.enemyType == EnemyController.EnemyType.DroneBot)
        {
            currentlyPossessedEnemy.transform.position = transform.position;

            EnemyFlyingController enemyFlyingController = enemyController.GetComponent<EnemyFlyingController>();
            enemyFlyingController.isPossessed = false;
            Invoke("StopTransition", .5f);
            Invoke("ChangeStateToIdle", .5f);
        }

        currentlyPossessedEnemy.SetActive(true);
        currentlyPossessedEnemy = null;

        
        
    }

    private GameObject FindClosestEnemy()
    {
        if (possessableEnemies.Count <= 0) { return null; }
        GameObject closestEnemy = possessableEnemies[0];
        for (int i = 0; i < possessableEnemies.Count; i++)
        {
            if (Vector2.Distance(possessableEnemies[i].transform.position, transform.position) < Vector2.Distance(closestEnemy.transform.position, transform.position))
            {
                closestEnemy = possessableEnemies[i];
            }
        }

        return closestEnemy;
    }

    private IEnumerator DecreaseSpiritHealth()
    {
        currentSpiritHealth -= 1;
        UpdateSpiritHealth();
        yield return new WaitForSeconds(1f);

        if(currentSpiritHealth <= 0)
        {
            StartTransitionToPlayer();
            yield break;
        }

        if (isSpirit)
        {
            StartCoroutine(DecreaseSpiritHealth());
        }
    }

    private void UpdateSpiritHealth()
    {
        spiritBar.value = currentSpiritHealth;
    }

    public void StopTransition()
    {
        isTransitioning = false;
    }

    private void ActivatePlayer()
    {
        playerAnchor.SetActive(false);
        cameraController.UsePlayerCamera();
        healthFollow.MakeHealthFollowPlayer();
        transform.position = playerAnchor.transform.position;
        spriteRenderer.enabled = true;
        capsuleCollider.enabled = true;
        ChangeStateToIdle();
    }

    public void StartDeathSequence()
    {

        if (isSpirit || isGuardBot || isDroneBot || isRollerBot)
        {
            animator.runtimeAnimatorController = playerAnimatorController;
            cameraController.UseAnchorCamera();
            spriteRenderer.enabled = false;
            Invoke("FinishDeathSequence", .5f);
            return;
        }

        FinishDeathSequence();
        
    }

    private void FinishDeathSequence()
    {
        if (isSpirit || isGuardBot || isDroneBot || isRollerBot)
        {
            transform.position = playerAnchor.transform.position;
        }
        animator.SetBool("isDead", true);
        spriteRenderer.enabled = true;
        playerAnchor.SetActive(false);
        rigidBody.gravityScale = 0f;
        rigidBody.velocity = Vector2.zero;
        capsuleCollider.enabled = false;
        RestartLevel();
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

    private void SetPlayerAnchor()
    {
        if (!isFacingRight)
        {
            playerAnchor.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        } else
        {
            playerAnchor.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }

        playerAnchor.transform.position = transform.position;
        playerAnchor.SetActive(true);
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

    public void ChangeStateToShooting()
    {
        this.stateMachine.ChangeState(shootingPlayer);
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
        Gizmos.DrawWireSphere(gunEndPosition.position, .2f);
    }
}
