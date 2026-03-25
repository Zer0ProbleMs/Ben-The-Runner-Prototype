using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.Image;

public class Player : MonoBehaviour
{
    [SerializeField] TrailRenderer _tr;
    [SerializeField] AudioSource pickupsound;
    [SerializeField] AudioSource jump1;
    [SerializeField] AudioSource jump2;
    [SerializeField] AudioSource dash;
    [SerializeField] LayerMask _mask;
    [SerializeField] float _hspeed = 4f;
    [SerializeField] float _sprintmult = 1.5f;
    [SerializeField] float _jumpVelocity = 5f;
    [SerializeField] float _footOffset = 0.1f;
    [SerializeField] float _jumpTime = 0.25f;
    [SerializeField] bool _isGrounded;
    [SerializeField] bool _isWalled;
    public int CoinCount = 0;

    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rb;
    Animator anim;
    float _endJump;
    float xinput;
    float yinput;
    float dashingPower = 24f;
    float dashingTime = 0.2f;
    float dashingCooldown = 1f;
    bool canDash = true;
    bool isDashing;
    int _jumplimit;

    private void Awake() //Used for caching different components even before start
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnDrawGizmos() //Used to see where the Raycasts would be and how long
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Gizmos.color = Color.red;

        //All the ground checks
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.2f);
        origin = new Vector2(transform.position.x - _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.2f);
        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.2f);

        //All the wall checks
        origin = new Vector2(transform.position.x - _spriteRenderer.bounds.extents.x + 0.3f, transform.position.y); //Left
        Gizmos.DrawLine(origin, origin + Vector2.left * 0.2f);
        origin = new Vector2(transform.position.x + _spriteRenderer.bounds.extents.x - 0.3f, transform.position.y); //Right
        Gizmos.DrawLine(origin, origin + Vector2.right * 0.2f);
    }
    void Update()
    {
        GroundCheck();
        WallCheck();
        DeathFall();

        if (isDashing)
            return;

        xinput = Input.GetAxis("Horizontal") * _hspeed;
        yinput = _rb.linearVelocityY;

        if (Input.GetButtonDown("Fire1"))
        {
            _endJump = Time.time + _jumpTime;
            _jumplimit--;
        }
        if (Input.GetButtonDown("Fire1") && _isGrounded)
        {
            jump1.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            jump1.Play();
        }
        if (Input.GetButtonDown("Fire1") && !_isGrounded && _jumplimit == 0)
        {
            jump2.pitch = UnityEngine.Random.Range(1.1f, 1.2f);
            jump2.Play();
        }
        if (Input.GetButton("Fire1") && _endJump > Time.time && _jumplimit >= 0)
            yinput = _jumpVelocity;

        if (Input.GetKey(KeyCode.Mouse1))
            xinput *= _sprintmult;

        if (Input.GetKeyDown(KeyCode.LeftAlt) && canDash)
            StartCoroutine(Dash());

        if (!isDashing)
            _rb.linearVelocity = new Vector2(xinput, yinput);

        UpdateSprite();
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;
    }

    void UpdateSprite()
    {
        anim.SetFloat("Horizontal Speed", Math.Abs(xinput));
        anim.SetFloat("Vertical Speed", _rb.linearVelocityY);
        anim.SetBool("IsGrounded", _isGrounded);
        anim.SetInteger("Jumps Left", _jumplimit);
        anim.SetBool("IsWalled", _isWalled);

        if (xinput > 0)
            transform.localScale = new Vector3(1, 1, 0);
        if (xinput < 0)
            transform.localScale = new Vector3(-1, 1, 0);
    }

    void GroundCheck()
    {
        _isGrounded = false;

        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _spriteRenderer.bounds.extents.y);
        RaycastHit2D ground = Physics2D.Raycast(origin, Vector2.down, 0.2f, _mask);
        if (ground.collider && !ground.collider.isTrigger)
            _isGrounded = true;

        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        ground = Physics2D.Raycast(origin, Vector2.down, 0.2f, _mask);
        if (ground.collider && !ground.collider.isTrigger)
            _isGrounded = true;

        origin = new Vector2(transform.position.x - _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y);
        ground = Physics2D.Raycast(origin, Vector2.down, 0.2f, _mask);
        if (ground.collider && !ground.collider.isTrigger)
            _isGrounded = true;

        if (_isGrounded)
            _jumplimit = 1;
    }
    void WallCheck()
    {
        _isWalled = false;

        Vector2 origin = new Vector2(transform.position.x - _spriteRenderer.bounds.extents.x + 0.3f, transform.position.y); //Left
        RaycastHit2D wall = Physics2D.Raycast(origin, Vector2.left, 0.2f, _mask);
        if (!_isGrounded && wall.collider)
            _isWalled = true;
        origin = new Vector2(transform.position.x + _spriteRenderer.bounds.extents.x - 0.3f, transform.position.y); //Right
        wall = Physics2D.Raycast(origin, Vector2.right, 0.2f, _mask);
        if (!_isGrounded && wall.collider)
            _isWalled = true; 
    }

    public void AddCoin()
    {
        CoinCount++;
        pickupsound.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
        pickupsound.Play();
    }
    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;
        _rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        Vector3 originalscale = new Vector3(transform.localScale.x, transform.localScale.y, 0);
        _tr.emitting = true;
        transform.localScale = new Vector3(transform.localScale.x * 1.3f, 0.7f, 0);
        dash.Play();
        Debug.Log(canDash);
        Debug.Log(isDashing);
        Debug.Log(_rb.gravityScale);
        yield return new WaitForSeconds(dashingTime);
        _tr.emitting = false;
        _rb.gravityScale = originalGravity;
        isDashing = false;
        transform.localScale = originalscale;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    void DeathFall()
    {
        if (transform.position.y <= -20f)
            SceneManager.LoadScene(0);
    }
}
