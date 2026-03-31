using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.Image;

public class RoamingEnnemy : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Rigidbody2D rb;
    [SerializeField] LayerMask _mask;
    [SerializeField] float movespeed = 3f;
    [SerializeField] bool wallleft;
    [SerializeField] float _footOffset = 0.1f;
    [SerializeField] bool _noGroundleft;
    [SerializeField] BoxCollider2D weakspot;
    [SerializeField] BoxCollider2D playerkill;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
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
        WallCheck();
        GroundCheck();
        if (wallleft || _noGroundleft)
        {
            transform.localScale = new Vector2(-1, 1);
            rb.linearVelocity = new Vector2(movespeed, rb.linearVelocityY);
        }
        else if (!wallleft || !_noGroundleft)
        {
            transform.localScale = new Vector2(1, 1);
            rb.linearVelocity = new Vector2(-movespeed, rb.linearVelocityY);
        }
    }

    void WallCheck()
    {
        Vector2 origin = new Vector2(transform.position.x - _spriteRenderer.bounds.extents.x + 0.5f, transform.position.y - 0.5f); //Left
        RaycastHit2D wall = Physics2D.Raycast(origin, Vector2.left, 0.2f, _mask);
        if (wall.collider)
        {
            wallleft = true;
        }

        origin = new Vector2(transform.position.x + _spriteRenderer.bounds.extents.x - 0.5f, transform.position.y - 0.5f); //Right
        wall = Physics2D.Raycast(origin, Vector2.right, 0.2f, _mask);
        if (wall.collider)
        {
            wallleft = false;
        }
    }

    void GroundCheck()
    {
        Vector2 origin = new Vector2(transform.position.x + _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y); //Right
        RaycastHit2D ground = Physics2D.Raycast(origin, Vector2.down, 0.2f, _mask);
        if (!ground.collider)
            _noGroundleft = false;

        origin = new Vector2(transform.position.x - _footOffset, transform.position.y - _spriteRenderer.bounds.extents.y); //Left
        ground = Physics2D.Raycast(origin, Vector2.down, 0.2f, _mask);
        if (!ground.collider)
            _noGroundleft = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && collision.collider == playerkill)
            SceneManager.LoadScene(0);
    }
}
