using UnityEngine;

public class FragmentExploder : MonoBehaviour {

    public GameObject fragmentPrefab;
    public Sprite[] fragmentSprites;

	public void Explode () {
        // Instantiate the fragments, pull them apart randomly
        foreach (Sprite sprite in fragmentSprites)
        {
            GameObject go = Instantiate(fragmentPrefab, transform.position, transform.rotation);
            go.GetComponent<SpriteRenderer>().sprite = sprite;
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            Vector2 randomVector = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));

            // We add the velocity at impact to the random vector
            // This looks more natural
            rb.AddForce((randomVector) * 3.0f, ForceMode2D.Impulse);
            rb.AddTorque(50);
            Destroy(go, 3.0f);
        }
	}
}
