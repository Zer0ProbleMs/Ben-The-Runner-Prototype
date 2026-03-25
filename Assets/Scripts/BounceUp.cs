using UnityEngine;

public class BounceUp : MonoBehaviour
{
    
    private void Awake()
    {
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.relativeVelocity);
    }
}
