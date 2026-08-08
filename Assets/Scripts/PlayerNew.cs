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
    public int maxJump = 1;

    private int _jumpRemaining;

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
    
    private float _defaultFallSpeed;

    [Header("Character Flip")] 
    public float playerScale;
    private bool _isFacingRight;
    
    
    private void Start()
    {
        _movementSpeed = defaultSpeed;
        _defaultFallSpeed = fallSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        Gravity();
        GroundCheck();
        WallCheck();
        FlipChar();
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        _horizontalInput = context.ReadValue<Vector2>().x;
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed && _jumpRemaining > 0)
        {
            isJumping = true;
            Jump();
            _jumpRemaining--;
        }

        if (context.canceled)
        {
            isJumping = false;
            rb.linearVelocityY *= 0.5f;
        }
    }

    public void RunInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _movementSpeed = runSpeed;
        }

        if (context.canceled)
        {
            _movementSpeed = defaultSpeed;
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

    private void WallCheck()
    {
        isWalled = false;
        
        if (Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallCheckMask) && !isGrounded)
        {
            fallSpeed = walledFallSpeed;
            isWalled = true;
        }
        fallSpeed = _defaultFallSpeed;
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
