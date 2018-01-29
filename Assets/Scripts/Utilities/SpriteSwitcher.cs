using UnityEngine;

public class SpriteSwitcher : MonoBehaviour {

    public Sprite[] playerSprite;

    private SpriteRenderer rend;

    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        PlayerMoveManager.OnPlayerAccelerate += SpriteThrust;
        PlayerMoveManager.OnPlayerStop += SpriteIdle;
    }

    private void OnDisable()
    {
        PlayerMoveManager.OnPlayerAccelerate -= SpriteThrust;
        PlayerMoveManager.OnPlayerStop -= SpriteIdle;
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
