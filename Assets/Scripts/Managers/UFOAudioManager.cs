using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class UFOAudioManager : MonoBehaviour
{

    private AudioSource au;

    private void Awake()
    {
        au = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        au.Play();
    }

    private void OnDisable()
    {
        au.Stop();
    }
}
