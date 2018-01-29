using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class MusicManager : MonoBehaviour {

    AudioSource backgroundMusic;

    void Awake()
    {
        backgroundMusic = GetComponent<AudioSource>();
    }

    void Start()
    {
        backgroundMusic.Play();
    }
}
