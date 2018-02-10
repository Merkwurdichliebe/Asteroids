using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class ApplyTorqueOnStart : MonoBehaviour {

    public float torqueAmountMin;
    public float torqueAmountMax;

	void Start () {
        GetComponent<Rigidbody2D>().AddTorque(Random.Range(torqueAmountMin, torqueAmountMax));
	}
	
}
