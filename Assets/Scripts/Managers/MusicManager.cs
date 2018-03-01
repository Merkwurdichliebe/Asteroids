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
    public AudioClip scoreBonusLifeByPointsSFX;

    //
    // Private fields
    //
    private AudioSource audioMusic;
    private AudioSource audioSFX1;
    private AudioSource audioSFX2;

    //
    // Initialise 2 separate AudioSource components
    // for music and sound FX. 
    //
    void Awake()
    {
        audioMusic = gameObject.AddComponent<AudioSource>();
        audioSFX1 = gameObject.AddComponent<AudioSource>();
        audioSFX2 = gameObject.AddComponent<AudioSource>();

        audioMusic.clip = backgroundMusic;
        audioMusic.loop = true;
        audioMusic.volume = 1.0f;
        audioMusic.playOnAwake = false;

        audioSFX1.volume = 1.0f;
        audioSFX1.playOnAwake = false;

        audioSFX2.volume = 1.0f;
        audioSFX2.playOnAwake = false;
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
        audioSFX1.Stop();
        audioSFX1.volume = 1.0f;
        audioSFX1.clip = gameOverSFX;
        audioSFX1.Play();
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
        audioSFX1.clip = levelCompleteSFX;
        audioSFX1.volume = 0.8f;
        audioSFX1.Play();
    }

    //
    // Play SFX on ScoreBonusLife
    //
    private void HandleScoreBonusLifeByPoints()
    {
        audioSFX2.Stop();
        audioSFX2.volume = 1.2f;
        audioSFX2.pitch = 1.5f;
        audioSFX2.clip = scoreBonusLifeByPointsSFX;
        audioSFX2.Play();
    }

    //
    // Event subscriptions
    //
    private void OnEnable() 
    {
        GameManager.OnGameLevelComplete += HandleGameLevelComplete;
        GameOverManager.OnGameOver += HandleGameOver;
        ScoreManager.OnScoreBonusLifeByPoints += HandleScoreBonusLifeByPoints;
    }

    private void OnDisable() 
    {
        GameManager.OnGameLevelComplete -= HandleGameLevelComplete;
        GameOverManager.OnGameOver -= HandleGameOver;
        ScoreManager.OnScoreBonusLifeByPoints -= HandleScoreBonusLifeByPoints;
    }
}
