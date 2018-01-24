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

    // Player and its Rigidbody
    private Rigidbody2D player_rb;
    private PlayerController playerController;
    private GameObject player;



    void Start()
    {
        // Initialize Player references
        playerController = FindObjectOfType<PlayerController>();
        player_rb = playerController.gameObject.GetComponent<Rigidbody2D>();
        textAnnounce.text = "";

        EventManager.OnUIUpdateLives += UpdateLives;
        EventManager.OnUIUpdateScore += UpdateScore;
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



    public void Announce(string text)
    {
        textAnnounce.text = text;
    }
}