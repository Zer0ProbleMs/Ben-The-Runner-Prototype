using TMPro;
using UnityEngine;

public class FragileBridge : MonoBehaviour
{
    Animator _anim;
    AudioSource _audio;
    float _timer;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider)
        {
            _anim.SetBool("OnBridge", true);
            _timer = Time.time + 5f;
            _audio.Play();
        }
    }
    private void Update()
    {
        if (Time.time >= _timer)
        {
            _anim.SetBool("Repair", true);
            _anim.SetBool("OnBridge", false);
        }
        else
            _anim.SetBool("Repair", false);

    }
}
