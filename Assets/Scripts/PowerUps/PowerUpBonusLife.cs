using UnityEngine;

public class PowerUpBonusLife : Entity {

    public GameObject pickUpFX;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(pickUpFX, transform.position, Quaternion.identity);
            collision.GetComponent<PlayerController>().Lives += 1;
            Destroy(gameObject);
        }
    }
}