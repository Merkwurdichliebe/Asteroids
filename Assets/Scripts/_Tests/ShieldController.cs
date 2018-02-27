using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour {

	public GameObject shieldPrefab;
	public int ShieldsLevel { get; private set; }

	private GameObject[] shieldFX;
	private float lastCollisionTime;

	private void OnEnable()
	{
		ShieldsLevel = 3;

		shieldFX = new GameObject[3];
		for (int i = 0; i < ShieldsLevel; i++)
		{
			shieldFX[i] = Instantiate(shieldPrefab, this.gameObject.transform);
		}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (Time.time - lastCollisionTime < 0.5f)
			return;
		// Debug.Log(other.gameObject.name);
		ShieldsLevel -= 1;
		Destroy(shieldFX[ShieldsLevel]);
		Debug.Log(ShieldsLevel);
		if (ShieldsLevel <= 0)
			gameObject.SetActive(false);
		lastCollisionTime = Time.time;
	}

}
