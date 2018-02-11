using UnityEngine;

/// <summary>
/// This Monobehaviour randomly sets the sprite on the gameobject
/// from an array on sprites in the Inspector.
/// </summary>

public class SetRandomSpriteOnAwake : MonoBehaviour {

    //
    // Inspector field
    //
    public Sprite[] sprites;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
