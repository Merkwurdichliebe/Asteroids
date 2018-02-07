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
        MovePlayerControlled.OnPlayerAccelerate += PlayAccelerate;
        MovePlayerControlled.OnPlayerStop += PlayStop;

        EventManager.Instance.OnPlayerDestroyed += HandlePlayerDestroyed;
    }

    private void OnDisable()
    {
        MovePlayerControlled.OnPlayerAccelerate -= PlayAccelerate;
        MovePlayerControlled.OnPlayerStop -= PlayStop;

        EventManager.Instance.OnPlayerDestroyed -= HandlePlayerDestroyed;
    }

    private void Start()
    {
        audioSource.clip = engine;
        audioSource.volume = 1f;
    }

    void PlayAccelerate()
    {
        audioSource.Play();
    }

    void PlayStop()
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
