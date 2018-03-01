using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	}

	//
	// Called by the PowerUp
	//
	public void EnableBombBay()
	{
		// We don't want to go over the maximum number of levels
		// configured in the Inspector.
		if (currentBayLevel > enabledBaysPerLevel.Count - 1)
			return;

		// Loop through all the available weapon bays
		foreach (GameObject bay in Bays)
		{
			// Get the IFire interface in each of them
			IFire bayComponent = bay.GetComponent<IFire>();

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

			// Enable IFire on the current bay
			bayComponent.IsEnabled = shouldEnable;
		}

		// We increase the currentBayLevel when done
		// rather than at the beginning of the loop
		// to make iterating through the array simpler
		// (level 1 is actually array index 0);
		currentBayLevel += 1;
	}
}

[System.Serializable]
public struct EnabledBaysPerLevel
{
	public GameObject[] baysToEnable;
}