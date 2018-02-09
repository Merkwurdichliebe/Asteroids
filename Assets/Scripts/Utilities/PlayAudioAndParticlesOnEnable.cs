using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]

public class PlayAudioAndParticlesOnEnable : MonoBehaviour {

    private ParticleSystem ps;
    private AudioSource au;

    public float secondsBeforeDestroy;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        au = GetComponent<AudioSource>();
    }
	
    private void OnEnable()
    {
        transform.parent = null;
        ps.Play();
        au.Play();
        Destroy(gameObject, secondsBeforeDestroy);
    }

}
