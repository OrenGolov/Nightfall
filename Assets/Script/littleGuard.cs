using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class littleGuard : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    public float audioDistance = 6f; // Distance to trigger the audio
    public AudioSource guardAudioSource; // Reference to the guard's audio source
    Animator animator;

    private bool isAudioScheduled = false; // To track if audio is already scheduled to play

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        guardAudioSource = GetComponent<AudioSource>(); // Get the attached AudioSource component
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // If the player is near, make the guard face the player
        if (distance < audioDistance)
        {
            if (!guardAudioSource.isPlaying && !isAudioScheduled) // Check if audio isn't already playing or scheduled
            {
                isAudioScheduled = true; // Set flag to avoid multiple triggers
                Invoke("PlayAudio", 0); // Play audio immediately or add a delay like Invoke("PlayAudio", 2) for a 2-second delay
            }

            // Make the guard face the player
            Vector3 target = player.transform.position - transform.position;
            target.y = 0; // Keep the guard's orientation level
            Vector3 temp_target = Vector3.RotateTowards(transform.forward, target, Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(temp_target, new Vector3(0, 1, 0));
        }
        else
        {
            // Stop the audio if the player moves away
            if (guardAudioSource.isPlaying)
            {
                guardAudioSource.Stop(); // Stop the audio
            }

            isAudioScheduled = false; // Reset the flag so the audio can trigger again
        }
    }

    // Function to play the audio
    void PlayAudio()
    {
        guardAudioSource.Play(); // Play the audio clip
        isAudioScheduled = false; // Reset the flag
    }
}
