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
    public Text textDebug;
    public Text textRoaming;
    public GameObject canvas;

    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
        EnableGameUI(false);
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

    void HandleCenterIsClear(bool b)
    {
        UpdateDebug("Center is clear : " + b);
    }

    void CenterOccupiedHandler()
    {
        UpdateDebug("Center is occupied");
    }

    void UpdateDebug(string text)
    {
        textDebug.text = text;
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

    public void ShowPointsAtScreenPosition(GameObject obj, int points)
    {
        Text t = Instantiate(textRoaming);
        t.transform.SetParent(canvas.transform, false);
        t.transform.position = Camera.main.WorldToScreenPoint(obj.transform.position);
        t.text = points.ToString();
        Destroy(t.gameObject, 1.0f);
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerLivesChanged += UpdateLives;
        PlayerController.OnPlayerLivesZero += HandleGameOver;
        MovePlayerControlled.OnPlayerSpeedChanged += UpdateSpeed;
        SpawnSafeZoneManager.OnSpawnSafeZoneCleared += HandleCenterIsClear;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerLivesChanged -= UpdateLives;
        PlayerController.OnPlayerLivesZero -= HandleGameOver;
        MovePlayerControlled.OnPlayerSpeedChanged -= UpdateSpeed;
        SpawnSafeZoneManager.OnSpawnSafeZoneCleared -= HandleCenterIsClear;
    }
}