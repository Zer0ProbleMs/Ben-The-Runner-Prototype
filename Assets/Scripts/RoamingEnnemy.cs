using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.Image;

public class RoamingEnnemy : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Rigidbody2D rb;
    Player _player;
    [SerializeField] LayerMask _mask;
    [SerializeField] float movespeed = 3f;
    [SerializeField] bool obstacle;
    [SerializeField] bool grounded;
    [SerializeField] float direction = -1f;
    [SerializeField] float _footOffset = 0.1f;
    [SerializeField] BoxCollider2D weakspot;
    [SerializeField] BoxCollider2D playerkill;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        direction = -1;
    }

    private void OnDrawGizmos()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Gizmos.color = Color.red;

        //All the ground checks
        Vector2 origin = new Vector2(transform.position.x - _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y); //Left
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.2f);
        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y); //Right
        Gizmos.DrawLine(origin, origin + Vector2.down * 0.2f);

        //All the wall checks
        origin = new Vector2(transform.position.x - _spriteRenderer.bounds.extents.x + 0.5f, transform.position.y - 0.5f); //Left
        Gizmos.DrawLine(origin, origin + Vector2.left * 0.2f);
        origin = new Vector2(transform.position.x + _spriteRenderer.bounds.extents.x - 0.5f, transform.position.y - 0.5f); //Right
        Gizmos.DrawLine(origin, origin + Vector2.right * 0.2f);
    }

    private void Update()
    {
        grounded = true; // Could be converted in INT to make him stop moving during fall and animations too

        WallCheck();
        GroundCheck();

        rb.linearVelocity = new Vector2(direction * movespeed, rb.linearVelocityY);

        if (direction == 1f)
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;

    }

    void WallCheck()
    {
        Vector2 origin = new Vector2(transform.position.x - _spriteRenderer.bounds.extents.x + 0.5f, transform.position.y - 0.5f); //Left
        RaycastHit2D wall = Physics2D.Raycast(origin, Vector2.left, 0.2f, _mask);
        if (wall.collider)
            direction = 1f;

        origin = new Vector2(transform.position.x + _spriteRenderer.bounds.extents.x - 0.5f, transform.position.y - 0.5f); //Right
        wall = Physics2D.Raycast(origin, Vector2.right, 0.2f, _mask);
        if (wall.collider)
            direction = -1f;
    }

    void GroundCheck()
    {
        Vector2 origin = new Vector2(transform.position.x - _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y); //Left
        RaycastHit2D groundleft = Physics2D.Raycast(origin, Vector2.down, 0.2f, _mask);
        if (!groundleft.collider)
            direction = 1f;

        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y); //Right
        RaycastHit2D groundright = Physics2D.Raycast(origin, Vector2.down, 0.2f, _mask);
        if (!groundright.collider)
            direction = -1f;

        origin = new Vector2(transform.position.x + _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y); //Right
        RaycastHit2D groundcenter = Physics2D.Raycast(origin, Vector2.down, 0.2f, _mask);
        if (!groundcenter.collider)
            grounded = false;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            _player.Death();
    }

}
