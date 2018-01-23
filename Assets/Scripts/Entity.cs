using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    public virtual void HitByPlayer()
    {
        Debug.Log("Entity base class was hit by player.");
    }



    public virtual void HitByEnemy()
    {
        Debug.Log("Entity base class was hit by enemy.");
    }
}
