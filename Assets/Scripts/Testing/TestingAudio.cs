using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TestingAudio : MonoBehaviour
{

    public AudioSource audioSource;

    private void OnValidate()
    {
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
    }
    public void PlayAudio()
    {
        if (audioSource)
        {
            audioSource.Play();
        }
    }
}
