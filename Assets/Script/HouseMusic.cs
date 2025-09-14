using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFader : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 30f;
    public float minDistance = 1f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > maxDistance)
        {
            audioSource.volume = 0;
        }
        else
        {
            float volume = 1 - Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));
            audioSource.volume = volume;
        }
    }
}
