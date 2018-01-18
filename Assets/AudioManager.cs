using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private AudioSource audiosource;
    public AudioClip laser;
    public AudioClip destroyed;
    public AudioClip engine;
    public AudioClip hit;

    void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }

}
