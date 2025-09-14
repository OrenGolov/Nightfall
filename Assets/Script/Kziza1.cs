using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kziza1 : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    public float audioDistance = 10f; // Distance to trigger the audio
    public AudioSource npcAudioSource; // Reference to the NPC's audio source
    Animator animator;

    private bool isAudioScheduled = false; // To track if audio is already scheduled to play

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        npcAudioSource = GetComponent<AudioSource>(); // Get the attached AudioSource component
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // If the player is near, play the talking animation and audio
        if (distance < 10)
        {
            if (animator.GetInteger("State") != 1)
            {
                animator.SetInteger("State", 1); // Switch to talking animation
            }

            // Make the NPC face the player
            Vector3 target = player.transform.position - transform.position;
            target.y = 0; // Prevent the NPC from tilting up/down
            Vector3 temp_target = Vector3.RotateTowards(transform.forward, target, Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(temp_target, new Vector3(0, 1, 0));

            // Play audio when the player is within audioDistance
            if (distance < audioDistance && !isAudioScheduled && !npcAudioSource.isPlaying)
            {
                isAudioScheduled = true; // Set the flag to prevent multiple triggers
                Invoke("PlayAudio", 0); // Play audio immediately or add delay (e.g., Invoke("PlayAudio", 2) for 2-second delay)
            }
        }
        else
        {
            // Switch back to idle animation when the player is far away
            if (animator.GetInteger("State") != 0)
            {
                animator.SetInteger("State", 0); // Switch to idle animation
            }

            // Stop the audio if the player moves out of range
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
