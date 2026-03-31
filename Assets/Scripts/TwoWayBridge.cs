using UnityEngine;

public class TwoWayBridge : MonoBehaviour
{
    [SerializeField] GameObject player;
    BoxCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _collider.enabled = true;

        if (player.transform.position.y < transform.position.y + 1f)
            _collider.enabled = false;

        if (Input.GetKey(KeyCode.S) && _collider.enabled)
            _collider.enabled = false;
    }

    private void Update()
    {

    }
}
