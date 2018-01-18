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

    // Entities

    // Player and its Rigidbody
    public GameObject player;
    private Rigidbody2D player_rb;
    private Vector3 playerVP;

    void Start()
    {
        // Initialize Player references
        player_rb = player.GetComponent<Rigidbody2D>();
        textAnnounce.text = "";
    }

    void Update()
    {
        // Update UI text to show player speed to one decimal place
        textSpeed.text = string.Format("SPEED: {0:0.0}", player_rb.velocity.sqrMagnitude);
    }

    public void UpdateScore(int score)
    {
        textScore.text = string.Format("{0}", score);
    }

    public void UpdateLives(int lives)
    {
        textLives.text = string.Format("{0}", lives);
    }

    public void DisplayGameOver()
    {
        textAnnounce.text = "GAME OVER";
    }
}