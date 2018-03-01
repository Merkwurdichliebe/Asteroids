using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Monobehaviour is the GUI script used with ShieldController.
/// It is reponsible for displaying the correct number of shield layers
/// based on input from ShieldController.
/// </summary>

public class ShieldFX : MonoBehaviour {

	//
	// Inspector fields
	//
	public GameObject shieldPrefab;

	//
	// Private fields
	//
	private GameObject[] shieldLayers;
	private ParticleSystem ps;

	//
	// Initialisation
	//
	private void Awake()
	{
		ps = GetComponent<ParticleSystem>();
	}

	//
	// Create an array of GameObjects based on the FX prefab.
	// Parent them to the shield object, scale them then deactivate them.
	// Also, 
	//
	public void Initialize(int maxShieldLevel)
	{
		shieldLayers = new GameObject[maxShieldLevel];
		for (int i = 0; i < maxShieldLevel; i++)
		{
			shieldLayers[i] = Instantiate(shieldPrefab, transform);
			float scaleFactor = i / 10f + shieldPrefab.transform.localScale.x;
			shieldLayers[i].transform.localScale = new Vector3(scaleFactor, scaleFactor, 0);
			shieldLayers[i].SetActive(false);
		}
	}

	//
	// Display the correct number of shield layer
	//
	public void SetLevel(int level)
	{
		// Play the particle system if we have at least 1 layer
		if (level > 0)
			ps.Play();
		else
			ps.Stop();

		// Activate all layer up to the number requested.
		// Normalize their animation start time from 0 to 1
		// based on the total number of layers displayed.
		for (int layer = 0; layer < shieldLayers.Length; layer++)
		{
			if (level > layer)
			{
				shieldLayers[layer].SetActive(true);
				Animator animator = shieldLayers[layer].GetComponent<Animator>();
				animator.Play("ShieldAnimationClip", 0, (float)layer / shieldLayers.Length);
			}
			else
				shieldLayers[layer].SetActive(false);
		}
	}
}
