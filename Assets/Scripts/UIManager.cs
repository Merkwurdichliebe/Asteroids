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

    // Player and its Rigidbody
    private Rigidbody2D player_rb;
    private PlayerController playerController;




    void Start()
    {
        // Initialize Player references
        playerController = FindObjectOfType<PlayerController>();
        player_rb = playerController.gameObject.GetComponent<Rigidbody2D>();
        textAnnounce.text = "";

        GameManager.OnScoreChanged += UpdateScore;
        GameManager.OnLivesChanged += UpdateLives;
        GameManager.OnAnnounceMessage += UpdateAnnounceMessage;
        UFOController.OnHitByPlayerProjectile += ShowPointsAtWorldPosition;
    }


    void MessageSample()
    {
        Debug.Log("Sample");
    }


    void Update()
    {
        // Update UI text to show player speed to one decimal place
        if (player_rb != null)
        {
            textSpeed.text = string.Format("SPEED: {0:0.0}", player_rb.velocity.magnitude);
        }

        textDebug.text = string.Format("Asteroids : {0} -- Center Free : {1}", AsteroidController.countAsteroids, GameManager.CenterIsFree);
    }



    public void UpdateScore(int score)
    {
        textScore.text = string.Format("{0}", score);
    }



    public void UpdateLives(int lives)
    {
        textLives.text = string.Format("{0}", playerController.lives);
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

    public void ShowPointsAtWorldPosition(Entity entity, Transform transform, int points)
    {
        Text t = Instantiate(textRoaming);
        t.transform.SetParent(canvas.transform, false);
        t.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        t.text = points.ToString();
        Destroy(t, 1.0f);
    }
}