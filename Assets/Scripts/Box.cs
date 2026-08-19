using System;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] Sprite brokenBox;
    Rigidbody2D _rb;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (transform.position.y < -15f)
            Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(Math.Abs(collision.relativeVelocity.y));
        if (Math.Abs(collision.relativeVelocity.y) > 18)
        {
            GetComponent<Animator>().SetBool("IsBroken", true);
            _rb.linearVelocityY = 5;
            BoxCollider2D.Destroy(GetComponent<BoxCollider2D>());
        }
    }
}
