using UnityEngine;

// This class expects sprites of fragments and should be passed
// the velocity of the parent object

public class FragmentExploder : MonoBehaviour {

    public GameObject fragmentPrefab;
    public Sprite[] fragmentSprites;

    public void Explode (Vector2 vel) {
        // Instantiate the fragments, pull them apart randomly
        foreach (Sprite sprite in fragmentSprites)
        {
            GameObject go = Instantiate(fragmentPrefab, transform.position, transform.rotation, gameObject.transform);
            go.GetComponent<SpriteRenderer>().sprite = sprite;
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            Vector2 randomVector = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));

            // We add the velocity at impact to the random vector
            // This looks more natural
            rb.velocity = vel;
            rb.AddForce((randomVector) * 2, ForceMode2D.Impulse);
            rb.AddTorque(vel.sqrMagnitude + Random.Range(30, 100));
            Destroy(go, 3.0f);
        }
	}
}
