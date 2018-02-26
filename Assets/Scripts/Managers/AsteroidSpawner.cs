using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {

	public CloneWhenKilled asteroidPrefab;

	private readonly Vector2 halfUnit = new Vector2(0.5f, 0.5f);
	private Transform asteroidsParent;
	private Camera cam;

    //
    // Initialization
    //
	private void Awake()
	{
		cam = Camera.main;
        asteroidsParent = new GameObject().transform;
        asteroidsParent.gameObject.name = "Asteroids";
	}

	//
    // Spawn the first asteroids.
    // Set the SourcePrefab property to point to the asteroidPrefab used here,
    // so that an asteroid can clone itself.
    // Get a random vector inside a unit circle,
    // shift it to the center of the viewport (0.5, 0.5)
    // and scale it down so that it draws a circle around the player.
    //
    public void Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // Instantiate
            CloneWhenKilled asteroid = Instantiate(asteroidPrefab, Vector2.zero, Quaternion.identity, asteroidsParent.transform);
            asteroid.SourcePrefab = asteroidPrefab;

            // Set position
            Vector2 pos = Random.insideUnitCircle.normalized + halfUnit;
            Vector3 worldPos = cam.ViewportToWorldPoint(pos) / 2;
            worldPos.z = 0;
            asteroid.gameObject.transform.position = worldPos;
        }
    }
}
