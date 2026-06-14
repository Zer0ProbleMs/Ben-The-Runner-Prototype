using System;
using UnityEngine;

public class Buttonpress : MonoBehaviour
{
    [SerializeField] bool inzone = false;
    [SerializeField] AudioSource sound;
    [SerializeField] GameObject canpress;

    // Update is called once per frame
    void Update()
    {
        canpress.SetActive(inzone);
        if (Input.GetKeyDown(KeyCode.E)) // Only plays if the player is in the trigger
            sound.Play(); // Plays whatever sound you put in after player presses E
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // If the *Player* is in the trigger
            inzone = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            inzone = false;
    }
}
