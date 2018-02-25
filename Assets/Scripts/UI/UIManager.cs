using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    // Editor references to UI elements
    public Text textSpeed;
    public Text textScore;
    public Text textLives;
    public Text labelScore;
    public Text labelLives;
    public Text textAnnounce;
    public Text textRoaming;
    public GameObject canvas;

    public static UIManager Instance;

    private Camera cam;

    void Awake()
    {
        Instance = this;
        EnableGameUI(false);
        cam = Camera.main;
    }

    void Start()
    {
        textAnnounce.text = "";
    }

    public void DisplayLevelNumber(int level)
    {
        UpdateAnnounceMessage(string.Format("LEVEL {0}", level), 3.0f);
    }

    public void DisplayGameUI()
    {
        ClearAnnounceMessage();
        EnableGameUI(true);
    }

    void EnableGameUI(bool state)
    {
        textSpeed.gameObject.SetActive(state);
        textLives.gameObject.SetActive(state);
        textScore.gameObject.SetActive(state);
        labelLives.gameObject.SetActive(state);
        labelScore.gameObject.SetActive(state);
    }

    void DisplayLevelNumber()
    {
        UpdateAnnounceMessage("LEVEL " + GameManager.CurrentLevel.ToString(), 3.0f);
    }

    void UpdateSpeed(float velocity)
    {
        textSpeed.text = string.Format("SPEED: {0:0.0}", velocity);
    }

    public void UpdateScore(int score)
    {
        textScore.text = string.Format("{0}", score);
    }

    public void UpdateLives(int lives)
    {
        textLives.text = string.Format("{0}", lives);
    }

    public void HandleGameOver()
    {
        UpdateAnnounceMessage("GAME OVER", 6.0f);
    }

    public void UpdateAnnounceMessage(string text, float duration)
    {
        textAnnounce.text = text;
    }

    public void ClearAnnounceMessage()
    {
        textAnnounce.text = "";
    }

    public void ShowTextAtScreenPosition(GameObject obj, string text)
    {
        Text t = Instantiate(textRoaming, canvas.transform);
        t.transform.position = cam.WorldToScreenPoint(obj.transform.position);
        t.text = text;
        Destroy(t.gameObject, 1.0f);
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerLivesChanged += UpdateLives;
        PlayerController.OnPlayerLivesZero += HandleGameOver;
        MovePlayerControlled.OnPlayerAccelerating += UpdateSpeed;
        GameManager.OnGameLevelReady += DisplayLevelNumber;
        GameManager.OnGameLevelStart += DisplayGameUI;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerLivesChanged -= UpdateLives;
        PlayerController.OnPlayerLivesZero -= HandleGameOver;
        MovePlayerControlled.OnPlayerAccelerating -= UpdateSpeed;
        GameManager.OnGameLevelReady -= DisplayLevelNumber;
        GameManager.OnGameLevelStart -= DisplayGameUI;
    }
}