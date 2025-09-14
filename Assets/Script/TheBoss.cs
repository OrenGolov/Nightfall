using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBoss : MonoBehaviour
{
    public GameObject player; // Player reference
    public float audioDistance = 7f; // Distance to trigger the audio
    public AudioSource bossAudioSource; // Reference to the boss's audio source
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        bossAudioSource = GetComponent<AudioSource>(); // Get the attached AudioSource component
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // If the player is near, make the boss face the player
        if (distance < audioDistance)
        {
            // Make the Boss face the player
            Vector3 target = player.transform.position - transform.position;
            target.y = 0; // Prevent the boss from tilting up/down
            Vector3 temp_target = Vector3.RotateTowards(transform.forward, target, Time.deltaTime, 0);
            transform.rotation = Quaternion.LookRotation(temp_target, new Vector3(0, 1, 0));

            // Play audio if it's not already playing
            if (!bossAudioSource.isPlaying)
            {
                bossAudioSource.Play();
            }
        }
        else
        {
            // Stop the audio if the player moves away
            if (bossAudioSource.isPlaying)
            {
                bossAudioSource.Stop();
            }
        }
    }
}
