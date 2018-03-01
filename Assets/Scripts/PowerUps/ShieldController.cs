using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This MonoBehaviour handles the player shield, which is activated
/// by the PowerUpShield powerup. The GUI effects are handled by the ShieldFX script
/// on the same gameObject.
/// </summary>

public class ShieldController : MonoBehaviour {

	//
	// Inspector fields
	//
	public int maxShieldsLevel;
	public int ShieldsLevel { get; private set; }

	//
	// Private fields
	//	
	private ShieldFX shieldFX;
	private float lastCollisionTime;
	private Rigidbody2D rb;
	private CircleCollider2D col;
	private AudioSource au;

	//
	// Initialisation:
	// Set the starting shields level
	// and get a reference to the GUI script
	// Collider and AudioSource.
	//
	private void Awake() {
		shieldFX = GetComponent<ShieldFX>();
		col = GetComponent<CircleCollider2D>();
		au = GetComponent<AudioSource>();

		// We don't want the shields collider active
		// when we have no shields.
		col.enabled = false;
	}

	//
	// Initialise the GUI script
	//
	private void Start()
	{
		shieldFX.Initialize(maxShieldsLevel);
	}

	//
	// This is called when the PowerUp is picked up
	//
	public void Activate()
	{
		ShieldsLevel = maxShieldsLevel;
		col.enabled = true;
		UpdateShield();
	}

	//
	// Check collision with shield
	//
	private void OnCollisionEnter2D(Collision2D other) {

		// Since destroyed asteroids can spawn new ones
		// right next to the player, we don't want to trigger this
		// immediately after a previous shield collision.
		if (Time.time - lastCollisionTime < 0.3f)
			return;
		lastCollisionTime = Time.time;

		// Reduce shields by one
		ShieldsLevel -= 1;
		UpdateShield();

		// Play audio clip
		au.Play();
		
		// If shields are at zero, disable the Collider
		if (ShieldsLevel <= 0)
			col.enabled = false;
	}

	//
	// Update the shield GUI script and the radius
	// of the Collider. The scaling factor only works if
	// the ShieldFX script implement the scaling as well.
	//
	private void UpdateShield()
	{
		col.radius = 0.85f * Mathf.Pow(1.10f, ShieldsLevel);
		shieldFX.SetLevel(ShieldsLevel);
	}
}