using UnityEngine;

public class PowerUpPoints : Entity {

    public GameObject pickUpFX;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(pickUpFX, transform.position, Quaternion.identity);
            GetComponent<ScoreController>().ScorePoints();
            Destroy(gameObject);
        }
    }
}