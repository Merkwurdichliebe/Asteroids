//using UnityEngine;

//public class UFOController : Spawnable, IKillable
//{
//    //
//    // Inspector fields 
//    //
//    public GameObject explosion;

//    //
//    // Initialisation 
//    //
//    public override void Awake()
//    {
//        base.Awake();
//    }

//    //
//    // (Required by IKillable)
//    // UFO kill sequence.
//    //
//    public void Kill()
//    {
//        Debug.Log("[UFOController/Kill]");
//        Instantiate(explosion, transform.position, Quaternion.identity);
//        Destroy(gameObject);
//    }
//}