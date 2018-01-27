using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    // UI

    // Reference to UI Speed Text Field
    public Text textSpeed;

    // Reference to UI Score Text Field
    public Text textScore;

    // Reference to UI Lives Text Field
    public Text textLives;

    // Reference to UI Announce Text Field
    public Text textAnnounce;

    // Reference to UI DebugText Field
    public Text textDebug;

    public Text textRoaming;
    public GameObject canvas;



    void Start()
    {
        textAnnounce.text = "";

        GameManager.OnScoreChanged += UpdateScore;
        GameManager.OnLivesChanged += UpdateLives;
        GameManager.OnAnnounceMessage += UpdateAnnounceMessage;
        GameManager.OnCenterClear += CenterClearHandler;
        GameManager.OnCenterOccupied += CenterOccupiedHandler;
        UFOController.OnScorePoints += ShowPointsAtWorldPosition;
        PlayerController.OnPlayerSpeedChanged += UpdateSpeed;
    }

    void DoSomething<T>(T value)
    {
        Debug.Log(value);
    }

    void CenterClearHandler()
    {
        UpdateDebug("Center is clear");
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

    public void UpdateAnnounceMessage(string text, float duration)
    {
        textAnnounce.text = text;
        Invoke("ClearAnnounceMessage", duration);
    }

    public void ClearAnnounceMessage()
    {
        textAnnounce.text = "";
    }

    public void ShowPointsAtWorldPosition(Entity e)
    {
        Text t = Instantiate(textRoaming);
        t.transform.SetParent(canvas.transform, false);
        t.transform.position = Camera.main.WorldToScreenPoint(e.gameObject.transform.position);
        t.text = e.pointValue.ToString();
        Destroy(t, 1.0f);
    }
}