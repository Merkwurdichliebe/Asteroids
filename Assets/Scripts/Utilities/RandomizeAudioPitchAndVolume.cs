using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class RandomizeAudioPitchAndVolume : MonoBehaviour {

    [Range(0, 1f)]
    public float randomizePitch;
    [Range(0, 1f)]
    public float randomizeVolume;

    private void Awake()
    {
        GetComponent<AudioSource>().pitch = Random.Range(1 - randomizePitch, 1 + randomizePitch);
        GetComponent<AudioSource>().volume = Random.Range(1 - randomizeVolume, 1 + randomizeVolume);
    }
}
