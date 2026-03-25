using UnityEngine;
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
        anim.SetBool("IsRunning", true);
        transform.position = _pointA.transform.position;
    }

    private void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint.position.x >= _pointB.transform.position.x)
            rb.linearVelocity = new Vector2(-movespeed, 0);
        else
            rb.linearVelocity = new Vector2(movespeed, 0);

        if (Vector2.Distance(transform.position, currentPoint.position) < 10f && currentPoint == _pointB.transform)
            currentPoint = _pointA.transform;

        if (Vector2.Distance(transform.position, currentPoint.position) < 10f && currentPoint == _pointA.transform)
            currentPoint = _pointB.transform;
    }
}
