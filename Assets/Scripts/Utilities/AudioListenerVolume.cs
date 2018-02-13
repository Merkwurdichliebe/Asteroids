using UnityEngine;

/// <summary>
/// Overall volume slider.
/// This MonoBehaviour allows the volume of an AudioListener (e.g. the Camera)
/// to be adjusted in the Inspector.
/// </summary>

[RequireComponent(typeof(AudioListener))]

public class AudioListenerVolume : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    public float volume;

	void Start () {
        AudioListener.volume = volume;
	}
}