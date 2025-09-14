using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peasent_man : MonoBehaviour
{
    public GameObject player; // Player reference
    public float audioDistance = 8f; // Distance to trigger the audio
    public AudioSource npcAudioSource; // Reference to the NPC's audio source
    public float audioDelay = 0f; // Delay before audio starts (in seconds)
    private bool isAudioScheduled = false; // To prevent re-triggering the audio

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        npcAudioSource = GetComponent<AudioSource>(); // Get the attached AudioSource component
    }

    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // If the player is near, switch to the talking animation
        if (distance < 10)
        {
            if (animator.GetInteger("Peasent_men") != 1)
            {
                animator.SetInteger("Peasent_men", 1); // Switch to talking animation
            }

            // Make the NPC face the player
            Vector3 target = player.transform.position - transform.position;
            target.y = 0; // Prevent the character from tilting up/down
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, target, Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
        else
        {
            // Switch back to the idle animation when the player is far away
            if (animator.GetInteger("Peasent_men") != 0)
            {
                animator.SetInteger("Peasent_men", 0); // Switch to idle animation
            }
        }

        // Play audio with delay when player is close enough
        if (distance < audioDistance)
        {
            if (!isAudioScheduled && !npcAudioSource.isPlaying) // Ensure audio isn't scheduled or playing
            {
                isAudioScheduled = true; // Set the flag to prevent multiple triggers
                Invoke("PlayAudio", audioDelay); // Schedule the audio to play after a delay
            }
        }
        else
        {
            // Stop the audio when the player moves away
            if (npcAudioSource.isPlaying)
            {
                npcAudioSource.Stop(); // Stop the audio
            }
            isAudioScheduled = false; // Reset the flag to allow re-triggering
        }
    }

    // Function to play the audio
    void PlayAudio()
    {
        npcAudioSource.Play(); // Play the audio after the delay
        isAudioScheduled = false; // Reset the flag so it can trigger again
    }
}
