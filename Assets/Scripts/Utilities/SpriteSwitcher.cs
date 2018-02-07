using UnityEngine;

public class SpriteSwitcher : MonoBehaviour {

    public Sprite[] playerSprite;
    private SpriteRenderer rend;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        MovePlayerControlled.OnPlayerAccelerating += SpriteThrust;
        MovePlayerControlled.OnPlayerStopped += SpriteIdle;
    }

    private void OnDisable()
    {
        MovePlayerControlled.OnPlayerAccelerating -= SpriteThrust;
        MovePlayerControlled.OnPlayerStopped -= SpriteIdle;
    }

    private void Start ()
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
