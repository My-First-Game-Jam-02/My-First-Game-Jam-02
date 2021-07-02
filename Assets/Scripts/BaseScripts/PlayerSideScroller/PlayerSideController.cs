using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;


public class PlayerSideController : MonoBehaviour
{
    [Header("Player States")]
    public bool isGrounded = false;
    public bool isJumping = false;
    public bool smallJump = false;
    public bool isTouchingWall = false;
    public bool wallSliding = false;
    public bool wallJumping = false;
    public bool isDashing = false;
    public bool isStunned = false;
    public bool isFrozen = false;
    public bool isAirBorn = false;
    public bool isInWater = false;
    public bool isDead = false;
    public bool hasBomb = false;

    [Header("Running")]
    // Variables used for running
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] float airMoveSpeed = 30f;
    [SerializeField] float reducedVelocityFactor;
    float horizontalMovement = 0f;


    [Header("Jumping")]
    // Variables used for Jumping
    public float normalGravityScale;
    [SerializeField] float waterGravityScale;
    [SerializeField] float normalJumpForce = 10f;
    [SerializeField] float waterJumpForce = 3f;
    [SerializeField] float normalMoveSpeed = 7f;
    [SerializeField] float waterMoveSpeed = 3f;
    [SerializeField] float jumpForce;
    [SerializeField] float smallJumpMultiplier = 0.5f;
    [SerializeField] int extraJumpsValue = 1;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;
    public int extraJumps;
    [SerializeField] float checkRadius = 0.3f;


    [Header("Wall Jumping and Wall Sliding")]
    // Variables for wall jumping and wall sliding
    [SerializeField] Transform wallCheckCollider;
    [SerializeField] float wallSlidingSpeed = 1f;
    [SerializeField] float xWallJumpForce = 1f;
    [SerializeField] float yWallJumpForce = 1f;
    float wallJumpDirection = -1f;


    [Header("Dashing")]
    // Variables for dashing
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float dashCoolDownTime;
    [SerializeField] float distanceBetweenImages;
    float dashTimeLeft;
    float lastImageXpos;
    float lastDash = -100f;


    [Header("Stunned")]
    // Variables for player getting stunned.
    [SerializeField] Vector2 stunForce;
    [SerializeField] float stunTime;

    [Header("Audio")]
    public GameObject jumpSound;

    public bool previousGrounded;
    public bool justGrounded;
    public float velocityBeforeGrounded;
    public float previousDownwardVelocity;

    // Cache
    public Rigidbody2D rigidBody;
    Animator animator;
    float playerDirection = 1f;
    bool facingRight = false;
    AudioSource audioSource;
    BoxCollider2D boxCollider;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider2D>();
        extraJumps = extraJumpsValue;
    }

    void Update()
    {

        Inputs();
        WorldCheck();
        AnimationControl();
        CheckIfInWater();
        CheckIfFalling();

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
            WallJump();
        }

        if (isDead)
        {
            rigidBody.velocity = Vector2.zero;
        }

        if (CheckJustGrounded())
        {
            velocityBeforeGrounded = previousDownwardVelocity;
        }

        previousGrounded = isGrounded;
        previousDownwardVelocity = rigidBody.velocity.y;
    }

    private void FixedUpdate()
    {
        if (isStunned || isFrozen || isDead)
        {
            return;
        }
        HorizontalMovement();
        WallSliding();
        PlayerDash();
    }

    private void Inputs()
    {
        if (isStunned || isFrozen || isDead)
        {
            return;
        }
        // Horizontal input
        horizontalMovement = Input.GetAxisRaw("Horizontal");

        // Flips player direction
        if (horizontalMovement < 0 && facingRight)
        {
            FlipSprite();
            playerDirection = -1f;
        }
        else if (horizontalMovement > 0 && !facingRight)
        {
            FlipSprite();
            playerDirection = 1f;
        }

        // Jump input
        if (Input.GetButtonDown("Jump") && !isFrozen && !isStunned)
        {
            isJumping = true;
            if (extraJumps > 0)
            {
                PlayJumpSound();
            }
        }
        else if (Input.GetButtonUp("Jump") && !isFrozen)
        {
            smallJump = true;
        }

        // Check if player is wall jumping
        if (Input.GetButtonDown("Jump") && wallSliding)
        {
            wallJumping = true;
        }

        // Dash input and cooldown logic
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Time.time >= (lastDash + dashCoolDownTime))
                AttemptToDash();
        }
    }

    private void WorldCheck()
    {
        // Check if player is touching ground
        isGrounded = Physics2D.OverlapCircle(groundCheckCollider.position, checkRadius, groundLayer);

        // Check if player is touching wall
        isTouchingWall = Physics2D.OverlapCircle(wallCheckCollider.position, checkRadius, groundLayer);

        // double jumps
        if (justGrounded)
        {
            extraJumps = extraJumpsValue;
        }

        if (!isGrounded && !wallSliding)
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
            movementSpeed = normalMoveSpeed;
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

    private void HorizontalMovement()
    {
        if (isGrounded)
        {
            rigidBody.velocity = new Vector2(horizontalMovement * movementSpeed, rigidBody.velocity.y);

        }
        // Gives player control of the character in the air
        else if (!isGrounded && !wallSliding && horizontalMovement != 0)
        {
            rigidBody.AddForce(new Vector2(airMoveSpeed * horizontalMovement, 0));
            if (Mathf.Abs(rigidBody.velocity.x) > movementSpeed)
            {
                rigidBody.velocity = new Vector2(horizontalMovement * movementSpeed, rigidBody.velocity.y);
            }
        }
        else if (!isGrounded && !wallSliding && horizontalMovement == 0)
        {
            if (rigidBody.velocity.x > 0)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x - reducedVelocityFactor, rigidBody.velocity.y);
            }
            else if (rigidBody.velocity.x < 0)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x + reducedVelocityFactor, rigidBody.velocity.y);
            }

            if (Mathf.Abs(rigidBody.velocity.x) < 0.01f)
            {
                rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);
            }
        }


    }

    private void Jump()
    {
        if (isFrozen || isStunned)
        {
            rigidBody.velocity = Vector2.zero;
            return;
        }
        // Double Jump
        if (isJumping && extraJumps > 0)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce * 100 * Time.fixedDeltaTime);
            print(extraJumps);
            extraJumps--;
            print(extraJumps);
        }
        // Regular Jump
        else if (isJumping && isGrounded && extraJumps == 0 || isJumping && isInWater)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce * 100 * Time.fixedDeltaTime);
        }
        // Allows player to small jump
        else if (smallJump && rigidBody.velocity.y > 0)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x,
                jumpForce * smallJumpMultiplier);
        }

        smallJump = false;
        isJumping = false;
    }

    private void WallSliding()
    {
        // Check if player is wall sliding
        if (isTouchingWall && !isGrounded && rigidBody.velocity.y < 0)
        {
            if (facingRight && Input.GetAxisRaw("Horizontal") > 0)
            {
                wallSliding = true;
            }
            else if (!facingRight && Input.GetAxisRaw("Horizontal") < 0)
            {
                wallSliding = true;
            }
            else
            {
                wallSliding = false;
            }
        }
        else
        {
            wallSliding = false;
        }
        if (wallSliding)
        {
            extraJumps = extraJumpsValue;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x,
            Mathf.Clamp(rigidBody.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
    }

    private void WallJump()
    {
        if (wallJumping && !isInWater)
        {
            rigidBody.velocity = new Vector2(xWallJumpForce * wallJumpDirection, yWallJumpForce);
            FlipSprite();
            wallJumping = false;
        }
        else
        {
            wallJumping = false;
        }
    }

    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    private void PlayerDash()
    {
        if (isDashing)
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
                isDashing = false;
            }
        }

    }

    private void FlipSprite()
    {
        if (!isStunned)
        {
            wallJumpDirection *= -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    public void GetStunned()
    {
        isStunned = true;
        wallSliding = false;
        isDashing = false;
        animator.SetBool("isStunned", true);
        rigidBody.velocity = new Vector2(-playerDirection * stunForce.x, stunForce.y);
        Invoke("StopStunned", stunTime);
    }

    public void StopStunned()
    {
        isStunned = false;
        animator.SetBool("isStunned", false);
    }

    private void AnimationControl()
    {
        if (isFrozen)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isAirBorn", false);
            animator.SetBool("isDashing", false);
            animator.SetBool("isWallSliding", false);
            return;
        }

        if (CheckIfFalling())
        {
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }

        // running
        if (rigidBody.velocity.x != 0 && isGrounded && horizontalMovement != 0)
            animator.SetBool("isRunning", true);
        else
            animator.SetBool("isRunning", false);

        // airborn
        if (isAirBorn || isInWater && !isGrounded)
        {
            animator.SetBool("isAirBorn", true);
            animator.SetBool("isRunning", false);
        }
        else if (!isAirBorn || isGrounded)
        {
            animator.SetBool("isAirBorn", false);
        }


        // dash
        if (isDashing)
        {
            animator.SetBool("isDashing", true);
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isDashing", false);
        }


        // wall slide
        if (wallSliding)
        {
            animator.SetBool("isWallSliding", true);
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isWallSliding", false);
        }


        // stunned
        if (isStunned)
            animator.SetBool("isStunned", true);
        else
            animator.SetBool("isStunned", false);

    }

    public void PlayFootstepSFX(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void FreezePlayer()
    {
        isFrozen = true;
        rigidBody.velocity = Vector2.zero;
    }

    public void UnFreezePlayer()
    {
        isFrozen = false;
        rigidBody.velocity = Vector2.zero;
    }

    public void SetPlayerToDead()
    {
        boxCollider.enabled = false;
        animator.SetBool("isDead", true);
        isDead = true;
    }

    public void RevivePlayer()
    {
        boxCollider.enabled = true;
        animator.SetBool("isDead", false);
        isDead = false;
    }

    public void PlayJumpSound()
    {
        Instantiate(jumpSound, transform.position, transform.rotation);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckCollider.position, checkRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheckCollider.position, checkRadius);
    }
}

