using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Importations")]
    [SerializeField] PlayerNew player;
    [SerializeField] Animator _animator;
    
    [Header("Positions")]
    Vector2 currentPosition;
    
    private void Start()
    {
        currentPosition = gameObject.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.spawnPoint = currentPosition;
            _animator.SetBool("Flag Touched", true);
        }
    }
}
