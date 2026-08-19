using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNew : MonoBehaviour
{
    [Header("GameObjects Import")] 
    public Rigidbody2D rb;
    public ParticleSystem walkFX;
    
    private Animator _animator;
    private GameObject _coin;
    
    [Header("Movement Properties")] 
    public float defaultSpeed = 5f;
    
    private float _horizontalInput;
    private float _movementSpeed;
    
    [Header("Running Properties")] 
    public float runSpeed = 7f;

    private bool _isRunning = false;

    [Header("Dashing Properties")] 
    public float dashPower = 10f;
    public float dashTime = 0.5f;
    public float dashWait = 1f;
    public bool canDash = true;

    private bool _isDashing;
    
    [Header("Jumping Properties")]
    public bool isJumping;
    public float jumpForce = 10f;
    public int maxJump = 2;

    private int _jumpRemaining;
    private bool _isJumping;

    [Header("WallJumping Properties")] 
    public Vector2 wallJumpPower = new Vector2(5f, 10f);
    public float wallJumpDirection;
    public bool isWallJumping;
    public float wallJumpTime = 0.5f;
    public float wallJumpTimer;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask groundCheckMask;
    public bool isGrounded;
    
    [Header("WallCheck")] 
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask wallCheckMask;
    public bool isWalled;
    
    [Header("Gravity")] 
    public float defaultGravity = 2f;
    public float fallGravity = 4f;
    public float downFall = 6f;
    public float fallSpeed = -18f;
    public float walledFallSpeed = -1;

    private bool _isDowning;
    
    [Header("Character Flip")] 
    public float playerScale;
    private bool _isFacingRight;

    [Header("Spawn")] 
    public float spawnTimer = 1f;

    [Header("Respawn")] 
    public Vector2 spawnPoint;
    public float mapLimit = -20f;

    [Header("Player Help")] 
    public float coyoteTime = 0.2f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        _movementSpeed = defaultSpeed;
        spawnPoint = gameObject.transform.position;
    }

    // FixedUpdate updates every 50 frames
    void FixedUpdate()
    {
        Gravity();
        WallSlide();
        GroundCheck();
        WallCheck();
        WallJump();
        Animations();
        Spawn();
        FallDeath();
        Effects();
        if (_isDashing) Dashing();
        
        if (!isWallJumping && !_isDashing && spawnTimer <= 0)
        {
            Movement();
            FlipChar();
        }
    }

    private void Effects()
    {
        if (Mathf.Abs(rb.linearVelocityX) > 0 && isGrounded) walkFX.Play();
        else walkFX.Pause();
    }
    
    public void MoveInput(InputAction.CallbackContext context)
    {
        _horizontalInput = context.ReadValue<Vector2>().x; // Takes the input on the X axis so Q and D
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed && _jumpRemaining > 0 && !isWalled) // Takes the input of Space
        {
            isJumping = true;
            Jump();
            _jumpRemaining--;
        }

        else if (context.canceled) // Checks if the input has been canceled
        {
            isJumping = false; 
            rb.linearVelocityY *= 0.5f; // If it is, we reduce the jump height
            _jumpRemaining--;
        }

        if (context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0f;

            if (transform.localScale.x != wallJumpDirection)
            {
                _isFacingRight = !_isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1;
                transform.localScale = ls;
            }
            
            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
        }
    }

    public void RunInput(InputAction.CallbackContext context)
    {
        if (context.performed) // If the player is holding Shift
        {
            _movementSpeed = runSpeed; // They run
            _isRunning = true;
        }

        if (context.canceled) // If the player stopped holding Shift
        {
            _movementSpeed = defaultSpeed; // They walk
            _isRunning = false;
        }
    }

    public void DashInput(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(nameof(Dash));
        }
    }

    public void DownInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isDowning = true;
        }
        else if (context.canceled)
        {
            _isDowning = false;
        }
    }

    private void Movement()
    {
        rb.linearVelocityX = _horizontalInput * _movementSpeed;
    }

    private void Jump()
    {
        rb.linearVelocityY = jumpForce;
    }

    private void Gravity()
    {
        rb.gravityScale = defaultGravity;
        
        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = fallGravity;
            rb.linearVelocityY = Mathf.Max(rb.linearVelocityY, fallSpeed);
        }

        if (_isDowning)
        {
            rb.gravityScale = downFall;
            rb.linearVelocityY = Mathf.Max(rb.linearVelocityY, fallSpeed);
        }
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundCheckMask))
        {
            isGrounded = true;
            _isJumping = false;
            _jumpRemaining = maxJump;
        }
        else
        {
            _isJumping = true;
            isGrounded = false;
        }
        
    }

    private bool WallCheck()
    {
        return (Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallCheckMask) && !isGrounded);
    }

    private void WallSlide()
    {
        if (!isGrounded && WallCheck() && _horizontalInput != 0)
        {
            isWalled = true;
            rb.linearVelocityY = Mathf.Max(rb.linearVelocityY, walledFallSpeed);
        }
        else isWalled = false;
    }

    private void WallJump()
    {
        if (isWalled)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = coyoteTime;
            CancelInvoke();
        }
        else if (wallJumpTimer > 0f) wallJumpTimer -= Time.deltaTime;
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        _isDashing = true;
        yield return new WaitForSeconds(dashTime);
        _isDashing = false;
        yield return new WaitForSeconds(dashWait);
        canDash = true;
    }

    private void Dashing()
    {
        rb.linearVelocity = new Vector2(transform.localScale.x * dashPower, 0);
    }
    
    private void Spawn()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0) spawnTimer = 0;
    }

    private void Respawn()
    {
        spawnTimer = 0.25f;
        gameObject.transform.position = spawnPoint;
    }

    private void FallDeath()
    {
        if (gameObject.transform.position.y <= mapLimit)
        {
            Respawn();
        }
    }

    private void FlipChar()
    {
        if (_horizontalInput > 0 && _isFacingRight || _horizontalInput < 0 && !_isFacingRight)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Trap"))
        {
            Respawn();
        }
    }

    private void Animations()
    {
        _animator.SetFloat("Horizontal Speed", Mathf.Abs(rb.linearVelocityX));
        _animator.SetFloat("Vertical Speed", rb.linearVelocityY);
        _animator.SetInteger("Jumps", _jumpRemaining);
        _animator.SetBool("IsGrounded", isGrounded);
        _animator.SetBool("IsRunning", _isRunning);
        _animator.SetBool("IsJumping", _isJumping);
        _animator.SetBool("IsWallSliding", isWalled);
        _animator.SetBool("IsWallJumping", isWallJumping);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}