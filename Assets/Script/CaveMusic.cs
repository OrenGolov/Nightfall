using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveMusic : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Make sure the AudioSource component is on the same object
        audioSource = GetComponent<AudioSource>();
    }

    // Called when another object enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            audioSource.Play();  // Start playing the music
        }
    }

    // Called when another object exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        // Check if the object that exited the trigger is the player
        if (other.CompareTag("Player"))
        {
            audioSource.Stop();  // Stop the music when the player leaves
        }
    }
}
