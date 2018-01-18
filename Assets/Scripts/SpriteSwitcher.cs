using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour {

    public Sprite[] playerSprite;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

	void Start ()
    {
        SpriteIdle();
	}

    public void SpriteIdle()
    {
        sr.sprite = playerSprite[0];
    }

    public void SpriteThrust()
    {
        sr.sprite = playerSprite[1];
    }
}
