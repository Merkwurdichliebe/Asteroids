//using System;
//using UnityEngine;
//using UnityEngine.Assertions;
//using Random = UnityEngine.Random;


//public class AsteroidController : Entity
//{
//    //
//    // Inspector fields
//    //
//    //public GameObject explosion;
//    //public Sprite[] sprites;

//    //
//    public static int Count { get; private set; }

//    //
//    //  Events
//    //
//    public static Action OnAsteroidLastDestroyed;

//    //
//    // Initialisation
//    //
//    public override void Awake()
//    {
//        //base.Awake();

//        // Set a random sprite variation.
//        // spriteSheet = Resources.Load<Sprite>("Sprites/AsteroidsSheet");
//        // Debug.Log(spriteSheet);
//        // Assert.IsNotNull(sprites);
//        //rend.sprite = sprites[Random.Range(0, sprites.Length)];

//        // Increase static asteroid count with each instantiation.
//        Count += 1;

//        // Set default phase property. This is changed only by Break().
//        //Phase = 0;
//    }

//    //
//    // If it's a new asteroid (instantiated by GameManager),
//    // set its transform to a random value.
//    //
//    //void Start()
//    //{
//    //    if (_phase == 0) 
//    //    {
//    //        transform.position = new Vector2(Random.Range(-15f, 15f), Random.Range(3f, 6f)); 
//    //    }
//    //}


//    //
//    // (Required by IKillable)
//    // Asteroid kill sequence.
//    //
//    //public void Kill()
//    //{
//    //    // Instantiate an explosion
//    //    Instantiate(explosion, transform.position, Quaternion.identity);

//    //    // Asteroid should break if it's not the smallest size
//    //    //if (_phase < 2) { Break(_phase + 1); }

//    //    Destroy(gameObject);
//    //}

//    //
//    // When destroyed, decrease the Count property by one.
//    // If it's the last asteroid in the scene, fire an event.
//    //
//    private void OnDestroy()
//    {
//        Count -= 1;
//        if (Count == 0)
//        {
//            if (OnAsteroidLastDestroyed != null) { OnAsteroidLastDestroyed(); }
//        }
//    }

//    //
//    // Instantiate two copies of this asteroid.
//    // Set them to the next phase to make them smaller
//    // (this will be handle by the Phase property).
//    //
//    //public void Break(int newPhase)
//    //{
//    //    for (int i = 0; i < 2; i++)
//    //    {
//    //        GameObject obj = Instantiate(gameObject, Vector2.zero, Quaternion.identity);
//    //        obj.transform.position = transform.position;
//    //        AsteroidController astController = obj.GetComponent<AsteroidController>();
//    //        astController.Phase = newPhase;
//    //    }
//    //}
//}
