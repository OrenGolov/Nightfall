using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleDoor : MonoBehaviour
{
    Animator animator;
    AudioSource sound;
    bool isDoorOpen = false;  // Variable to track the state of the door

    void Start()
    {
        // Get Animator and AudioSource components
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();

        if (animator == null)
        {
            Debug.LogError("Animator component is missing on " + gameObject.name);
        }

        if (sound == null)
        {
            Debug.LogError("AudioSource component is missing on " + gameObject.name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered the trigger area");

        // Open the door if it's closed
        if (!isDoorOpen && animator != null && sound != null)
        {
            animator.SetBool("CastleDoorOpen", true);  // Trigger door open animation
            sound.PlayDelayed(0.5f);                  // Play sound with delay if needed
            isDoorOpen = true;                        // Mark the door as open
            Debug.Log("Door is opening");
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Player exited the trigger area");

        // Close the door if it's open
        if (isDoorOpen && animator != null)
        {
            animator.SetBool("CastleDoorOpen", false);  // Trigger door close animation
            if (!sound.isPlaying)  // Ensure the sound is not already playing
            {
                sound.Play();  // Play the sound directly
            }
            isDoorOpen = false;  // Mark the door as closed
            Debug.Log("Door is closing");
        }
    }

    // Update is called once per frame (optional)
    void Update()
    {
        // Additional monitoring can be added here if needed
    }
}
