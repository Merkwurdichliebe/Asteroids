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
        MovePlayerControlled.OnPlayerAccelerating += PlayAccelerate;
        MovePlayerControlled.OnPlayerStopped += PlayStop;
    }

    private void OnDisable()
    {
        MovePlayerControlled.OnPlayerAccelerating -= PlayAccelerate;
        MovePlayerControlled.OnPlayerStopped -= PlayStop;
    }

    private void Start()
    {
        audioSource.clip = engine;
        audioSource.volume = 1f;
    }

    void PlayAccelerate(float speed)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    void PlayStop()
    {
        audioSource.Stop();
    }
}
