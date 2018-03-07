using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This MonoBehaviour is responsible for upgrading and resetting the
/// bomb bays on the player. All the bomb bays are children of
/// the bomb bay object. Each of them can fire a projectile.
/// This MonoBehaviour exposes a List of EnabledBaysPerLevels,
/// a struct defined below. Each list item contains an array
/// of bays to enable. First array contains 0 elements,
/// and increases with each level. The bomb bays themselves
/// (the children of the bomb bay object ) have to be assigned
/// in the Inspector in order to configure each bomb bay level.
/// </summary>

public class BombBayController : MonoBehaviour {

    //
    // List of all the child GameObjects of the Bomb Bay
    // which are tagged with "Weapon".
    //
    public List<GameObject> Bays { get; private set; }

	//
	// List of EnabledBaysPerLevels, a struct which holds
	// an array of GameObjects. This list holds
	// the specific bomb bays which should be enabled
	// at each level. This list is populated with GameObjects
	// in the Inspector.
	//
	public List<EnabledBaysPerLevel> enabledBaysPerLevel;

	private int currentBayLevel;

	//
	// Initialisation
	//
	private void Awake()
	{
		// Build a list of all child gameobjects tagged with "Weapon"
        Bays = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.tag == "Weapon")
            {
                Bays.Add(child);
            }
        }

		currentBayLevel = 0;
	}

	//
	// Called by the PowerUp
	//
	private void ConfigureBombBays()
	{
		// We don't want to go over the maximum number of levels
		// configured in the Inspector.
		if (currentBayLevel > enabledBaysPerLevel.Count - 1)
			return;

		// Loop through all the available weapon bays
		foreach (GameObject bay in Bays)
		{
			// For each current bay, loop through the array of the bays
			// that should be enabled and set shouldEnable to true
			// if there's a match.
			bool shouldEnable = false;
			for (int i = 0; i < enabledBaysPerLevel[currentBayLevel].baysToEnable.Length; i++)
			{
				if (enabledBaysPerLevel[currentBayLevel].baysToEnable[i].name == bay.name)
				{
					shouldEnable = true;
				}
			}

			// Set the IsEnabled property of IFire on the current bay
			// We don't cache this because it only runs when picking up the powerup
			bay.GetComponent<IFire>().IsEnabled = shouldEnable;
		}
	}

	//
	// Increase bomb bay level
	// (this is called by the PowerUpBombBay script)
	//
	public void UpgradeBombBays()
	{
		currentBayLevel += 1;
		ConfigureBombBays();
	}

	//
	// Reset bay level to zero
	//
	public void ResetBombBays()
	{
		currentBayLevel = 0;
		ConfigureBombBays();
	}

	//
	// Event subscriptions
	//
	private void OnEnable()
	{
		PlayerController.OnPlayerDestroyed += ResetBombBays;
	}

	private void OnDisable()
	{
		PlayerController.OnPlayerDestroyed -= ResetBombBays;
	}
}

[System.Serializable]
public struct EnabledBaysPerLevel
{
	public GameObject[] baysToEnable;
}