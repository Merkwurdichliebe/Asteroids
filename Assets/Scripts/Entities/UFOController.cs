using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class UFOController : Entity, IKillable
{
    public static event Action<Entity> OnScorePoints;
    public static Action OnUFOSpawned;
    public static Action OnUFODespawned;

    private GameObject target;
    private Transform childExplosion;


    public override void Awake()
    {
        base.Awake();
        childExplosion = transform.GetChild(0);
    }

    private void Start()
    {
        pointValue = GameManager.level * 20;
        if (OnUFOSpawned != null) OnUFOSpawned();
        MoveToCenter();
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerLivesZero += CleanUp;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerLivesZero -= CleanUp;
    }

    void CleanUp()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public void Kill()
    {
        Debug.Log("[UFOController/Kill]");
        childExplosion.gameObject.SetActive(true);
        Despawn();
    }

    // We need this in order to handle when the UFO leaves the screen
    private void OnBecameInvisible()
    {
        Debug.Log("[UFOController/OnBecameInvisible]");
        if (isAlive) Despawn();
    }

    private void OnBecameVisible()
    {
        Debug.Log("[UFOController/OnBecameVisible]");
    }

    void Despawn()
    {
        Debug.Log("[UFOController/Despawn]");
        SetActive(false); // Needed for "isAlive" variable
        rb.velocity = Vector2.zero;
        if (OnUFODespawned != null) OnUFODespawned();
        Destroy(gameObject);
    }

    private void ScorePoints()
    {
        OnScorePoints(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string objTag = collision.gameObject.tag;

        if (objTag == "PlayerProjectile")
        {
            collision.gameObject.SetActive(false);
            ScorePoints();
            Kill();
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        string objTag = collision.gameObject.tag;

        if (objTag == "Asteroid" || objTag == "Player")
        {
            Kill();   
        }
    }
}
