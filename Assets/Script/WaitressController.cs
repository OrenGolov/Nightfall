using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaitressWithThreePoints : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;

    public GameObject player; // Reference to the player object
    public GameObject point1; // Target point 1
    public GameObject point2; // Target point 2
    public GameObject point3; // Target point 3

    private AudioSource audioSource; // AudioSource component
    public AudioClip point1Audio; // Audio clip for reaching point 1
    public AudioClip point2Audio; // Audio clip for reaching point 2
    public AudioClip point3Audio; // Audio clip for reaching point 3

    private bool goingToPoint1 = false; // Track if heading to point 1
    private int currentPoint = 1; // Tracks which point the Waitress is going to next
    private bool startMoving = true; // Start moving immediately
    public float audioPlayDistance = 20f; // Maximum distance at which audio can play

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // Automatically find the AudioSource component attached to the Waitress
        audioSource = GetComponent<AudioSource>();

        // Set NavMeshAgent speed to match the animation speed
        agent.speed = 3.75f; // Speed matched to the animation

        // Set initial destination to point 2 (since the Waitress starts at point 1)
        agent.SetDestination(point2.transform.position);
        animator.SetInteger("Status", 1); // Start walking animation immediately
        currentPoint = 2; // Heading to point 2 first
    }

    void Update()
    {
        // Calculate the distance to the current destination
        float distance = Vector3.Distance(agent.destination, transform.position);

        // If close enough to the target and movement is ongoing
        if (distance < 3 && startMoving)
        {
            animator.SetInteger("Status", 0); // Stop walking animation
            agent.isStopped = true;
            startMoving = false;

            // Calculate distance between player and Waitress
            float playerDistance = Vector3.Distance(player.transform.position, transform.position);

            // Only play audio if the player is within the audioPlayDistance
            if (playerDistance <= audioPlayDistance)
            {
                // Play the appropriate audio and set the next destination
                if (currentPoint == 2) // Reached point 2, go back to point 1
                {
                    audioSource.PlayOneShot(point2Audio); // Play audio for reaching point 2
                    agent.SetDestination(point1.transform.position); // Go back to point 1
                    currentPoint = 1;
                }
                else if (currentPoint == 1) // Reached point 1, decide next destination
                {
                    audioSource.PlayOneShot(point1Audio); // Play audio for reaching point 1

                    // Alternate between point 2 and point 3 based on the last visited point
                    if (goingToPoint1) // Just came back from point 2, go to point 3
                    {
                        agent.SetDestination(point3.transform.position); // Go to point 3
                        currentPoint = 3;
                    }
                    else // Just came back from point 3, go to point 2
                    {
                        agent.SetDestination(point2.transform.position); // Go to point 2
                        currentPoint = 2;
                    }
                    goingToPoint1 = !goingToPoint1; // Toggle between point 2 and point 3
                }
                else if (currentPoint == 3) // Reached point 3, go back to point 1
                {
                    audioSource.PlayOneShot(point3Audio); // Play audio for reaching point 3
                    agent.SetDestination(point1.transform.position); // Go back to point 1
                    currentPoint = 1;
                }
            }

            // Resume movement after updating the destination
            startMoving = true;
            agent.isStopped = false;
            animator.SetInteger("Status", 1); // Start walking animation again
        }
    }
}
