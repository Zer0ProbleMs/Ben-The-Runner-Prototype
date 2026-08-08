using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNew : MonoBehaviour
{
    [Header("GameObjects Import")] 
    public Rigidbody2D rb;
    
    [Header("Movement Properties")] 
    public float defaultSpeed = 5f;
    
    private float _horizontalInput;
    private float _movementSpeed;
    
    [Header("Running Properties")] 
    public float runSpeed = 7f;
    
    [Header("Jumping Properties")]
    public bool isJumping;
    public float jumpForce = 10f;
    public int maxJump = 2;

    private int _jumpRemaining;

    [Header("WallJumping Properties")] 
    public Vector2 wallJumpPower = new Vector2(5f, 10f);
    public float wallJumpDirection;
    public bool isWallJumping = false;
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
    public float fallSpeed = -18f;
    public float walledFallSpeed = -1;
    public float defaultFallSpeed;

    [Header("Character Flip")] 
    public float playerScale;
    private bool _isFacingRight;
    
    
    private void Start()
    {
        _movementSpeed = defaultSpeed;
        defaultFallSpeed = fallSpeed;
    }

    // FixedUpdate updates every 50 frames
    void FixedUpdate()
    {
        Gravity();
        WallSlide();
        GroundCheck();
        WallCheck();
        WallJump();
        if (!isWallJumping)
        {
            Movement();
            FlipChar();
        }
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
        }

        if (context.canceled) // If the player stopped holding Shift
        {
            _movementSpeed = defaultSpeed; // They walk
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
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundCheckMask))
        {
            isGrounded = true;
            _jumpRemaining = maxJump;
        }
        else isGrounded = false;
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
            wallJumpTimer = wallJumpTime;
            CancelInvoke();
        }
        else if (wallJumpTimer > 0f) wallJumpTimer -= Time.deltaTime;
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
