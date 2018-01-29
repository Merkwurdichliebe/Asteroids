using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class PlayerAudioManager : MonoBehaviour {

    public AudioClip destroyed;
    public AudioClip engine;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PlayerMoveManager.OnPlayerAccelerate += HandleAccelerate;
        PlayerMoveManager.OnPlayerStop += HandleStop;
        PlayerController.OnPlayerDestroyed += HandlePlayerDestroyed;
    }

    private void OnDisable()
    {
        PlayerMoveManager.OnPlayerAccelerate -= HandleAccelerate;
        PlayerMoveManager.OnPlayerStop -= HandleStop;
        PlayerController.OnPlayerDestroyed -= HandlePlayerDestroyed;
    }

    private void Start()
    {
        audioSource.clip = engine;
        audioSource.volume = 1f;
    }

    void HandleAccelerate()
    {
        audioSource.Play();
    }

    void HandleStop()
    {
        audioSource.Stop();
    }

    void HandlePlayerDestroyed()
    {
        audioSource.Stop();
        audioSource.volume = 0.7f;
        audioSource.loop = false;
        audioSource.PlayOneShot(destroyed);
    }
}
