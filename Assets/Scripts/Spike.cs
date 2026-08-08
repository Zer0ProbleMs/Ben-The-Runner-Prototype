using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    Animator _anim;
    AudioSource _audio;
    Player _player;
    bool playerpresent = false;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _player = FindObjectOfType<Player>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            _player.Death();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerpresent = true;
            _audio.pitch = Random.Range(0.9f, 1.1f);
            _audio.Play();
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerpresent = false;
            _audio.pitch = Random.Range(0.9f, 1.1f);
            _audio.Play();
        }

    }

    private void Update()
    {
        _anim.SetBool("Player Present", playerpresent);
    }
}
