using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellClickedAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;
    public float volume = 1.5f;

    public void Play()
    {
        audioSource.PlayOneShot(clip, volume);
    }
}