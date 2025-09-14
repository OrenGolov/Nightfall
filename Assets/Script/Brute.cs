using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brute : MonoBehaviour
{
    public GameObject player;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // If the player is near, switch to the talking animation
        if (distance < 10)
        {
            if (animator.GetInteger("Status") != 1)
            {
                animator.SetInteger("Status", 1); // Switch to talking animation
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
            if (animator.GetInteger("Status") != 0)
            {
                animator.SetInteger("Status", 0); // Switch to idle animation
            }
        }
    }
}
