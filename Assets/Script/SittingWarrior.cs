using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingWarrior : MonoBehaviour
{
    public GameObject player;
    public AudioClip standUpSound;
    public AudioClip sitDownSound;
    public float soundDelay = 0.4f; // Delay in seconds

    private Animator animator;
    private AudioSource audioSource;
    private int previousSitState = 0;
    private bool isPlayingSound = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        int currentSitState = animator.GetInteger("Sit");

        if (distance < 8)
        {
            if (currentSitState != 1)
            {
                animator.SetInteger("Sit", 1);
                if (previousSitState != 1 && !isPlayingSound)
                {
                    StartCoroutine(PlaySoundWithDelay(standUpSound));
                }
            }
        }
        else
        {
            if (currentSitState != 0)
            {
                animator.SetInteger("Sit", 0);
                if (previousSitState != 0 && !isPlayingSound)
                {
                    StartCoroutine(PlaySoundWithDelay(sitDownSound));
                }
            }
        }

        previousSitState = currentSitState;
    }

    IEnumerator PlaySoundWithDelay(AudioClip clip)
    {
        isPlayingSound = true;
        yield return new WaitForSeconds(soundDelay);

        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }

        isPlayingSound = false;
    }
}
