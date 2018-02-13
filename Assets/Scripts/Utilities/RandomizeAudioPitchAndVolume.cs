using UnityEngine;

/// <summary>
/// This MonoBehaviour adds some randomness to the pitch and volume
/// of an Audiosource component, defined by two sliders in the Inspector.
/// </summary>

[RequireComponent(typeof(AudioSource))]

public class RandomizeAudioPitchAndVolume : MonoBehaviour {
    
    [Range(0, 1f)]
    public float randomizePitch;
    [Range(0, 1f)]
    public float randomizeVolume;
    public AudioSource audioComponent;

    private void Awake()
    {
        audioComponent.pitch = Random.Range(audioComponent.pitch - randomizePitch, audioComponent.pitch + randomizePitch);
        audioComponent.volume = Random.Range(audioComponent.volume - randomizeVolume, audioComponent.volume + randomizeVolume);
    }
}