using UnityEngine;

public class PowerUpPoints : Spawnable {

    public GameObject pickUpFX;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(pickUpFX, transform.position, Quaternion.identity);
            GetComponent<ScoreController>().ScorePoints();
            Destroy(gameObject);
        }
    }
}