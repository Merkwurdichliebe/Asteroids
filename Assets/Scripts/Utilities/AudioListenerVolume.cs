using UnityEngine;

[RequireComponent(typeof(AudioListener))]

public class AudioListenerVolume : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    public float volume;

	void Start () {
        AudioListener.volume = volume;
	}
}
