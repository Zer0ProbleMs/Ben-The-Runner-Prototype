using UnityEngine;

public class CoinPick : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerNew player = collision.GetComponent<PlayerNew>();
            if (player != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
