using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    Animator animator;
    AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {
        animator.SetBool("DoorOpen", true);
        sound.PlayDelayed(0.4f);
    }

    void OnTriggerExit(Collider other)
    {
        animator.SetBool("DoorOpen", false);
        sound.PlayDelayed(1.2f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
