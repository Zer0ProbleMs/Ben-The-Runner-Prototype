using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EnnemyPatrol : MonoBehaviour
{
    [SerializeField] GameObject _pointA;
    [SerializeField] GameObject _pointB;
    [SerializeField] float movespeed;
    Transform currentPoint;
    Rigidbody2D rb;
    Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = _pointB.transform;
    }

    private void FixedUpdate()
    {
        float gravity = rb.linearVelocityY;
        Vector2 point = transform.position;
        if (point.x < currentPoint.position.x && currentPoint == _pointA.transform)
        {
            currentPoint = _pointB.transform;
        }
        else if (point.x >= currentPoint.position.x && currentPoint == _pointA.transform)
        {
            rb.linearVelocity = new Vector2(-movespeed, gravity);
            transform.localScale = new Vector2(1, 1);
            
        }
        else if (point.x > currentPoint.position.x && currentPoint == _pointB.transform)
        {
            currentPoint = _pointA.transform;
        }
        else if (point.x <= currentPoint.position.x && currentPoint == _pointB.transform)
        {
            rb.linearVelocity = new Vector2(movespeed, gravity);
            transform.localScale = new Vector2(-1, 1);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            SceneManager.LoadScene(0);
    }
}
