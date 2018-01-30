using UnityEngine;
public interface ICanFireAtTarget {

    GameObject Target { get; set; }
    void FireAtTarget();

}
