using UnityEngine;

/// <summary>
/// This MonoBehaviour handles game-level music and sound effects.
/// </summary>

public class MusicManager : MonoBehaviour {

    //
    // Inspector fields
    //
    public GameSettings gameSettings;
    public AudioClip backgroundMusic;
    public AudioClip gameOverSFX;
    public AudioClip levelCompleteSFX;

    //
    // Private fields
    //
    private AudioSource audioMusic;
    private AudioSource audioSFX;

    //
    // Initialise 2 separate AudioSource components
    // for music and sound FX. 
    //
    void Awake()
    {
        audioMusic = gameObject.AddComponent<AudioSource>();
        audioSFX = gameObject.AddComponent<AudioSource>();

        audioMusic.clip = backgroundMusic;
        audioMusic.loop = true;
        audioMusic.volume = 1.0f;
        audioMusic.playOnAwake = false;

        audioSFX.volume = 1.0f;
        audioMusic.playOnAwake = false;
    }

    //
    // Play music when scene starts
    //
    void Start()
    {
        if (gameSettings.playMusic)
            audioMusic.Play();
    }

    //
    // Play SFX on GameOver
    //
    private void HandleGameOver()
    {
        audioMusic.Stop();
        audioSFX.Stop();
        audioSFX.volume = 1.0f;
        audioSFX.clip = gameOverSFX;
        audioSFX.Play();
    }

    //
    // Play SFX when level is complete
    //
    private void HandleGameLevelComplete()
    {
        // Same check as in GameManager because LevelComplete
        // and GameOver can occur simultaneously.
        if (GameOverManager.IsGameOver)
            return;
        audioSFX.clip = levelCompleteSFX;
        audioSFX.volume = 0.8f;
        audioSFX.Play();
    }

    //
    // Event subscriptions
    //
    private void OnEnable() 
    {
        GameManager.OnGameLevelComplete += HandleGameLevelComplete;
        GameOverManager.OnGameOver += HandleGameOver;
    }

    private void OnDisable() 
    {
        GameManager.OnGameLevelComplete -= HandleGameLevelComplete;
        GameOverManager.OnGameOver -= HandleGameOver;
    }
}
