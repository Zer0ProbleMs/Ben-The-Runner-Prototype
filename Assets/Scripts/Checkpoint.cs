using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Player _player;   
    Vector3 currentPosition;
    
    private void Start()
    {
        currentPosition = gameObject.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player.spawnPoint = currentPosition;
        }
    }
}
