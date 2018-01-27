using UnityEngine;

public class SpriteSwitcher : MonoBehaviour {

    public Sprite[] playerSprite;

    private SpriteRenderer rend;

    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

	void Start ()
    {
        SpriteIdle();
	}

    public void SpriteIdle()
    {
        rend.sprite = playerSprite[0];
    }

    public void SpriteThrust()
    {
        rend.sprite = playerSprite[1];
    }
}
