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

    private void Awake()
    {
        AudioSource au = GetComponent<AudioSource>();
        au.pitch = Random.Range(au.pitch - randomizePitch, au.pitch + randomizePitch);
        au.volume = Random.Range(au.volume - randomizeVolume, au.volume + randomizeVolume);
    }
}