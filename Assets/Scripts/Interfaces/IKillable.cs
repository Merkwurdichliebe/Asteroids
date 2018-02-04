using UnityEngine;

public interface IKillable {

    GameObject gameObject { get; }
    void Kill();

}