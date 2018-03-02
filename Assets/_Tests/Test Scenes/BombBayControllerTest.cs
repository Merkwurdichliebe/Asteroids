using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBayControllerTest : MonoBehaviour {

	public BombBayController controller;

	private void Start() {
		controller.UpgradeBombBays();
		Debug.Log("----------------");
		controller.UpgradeBombBays();
		Debug.Log("----------------");
		controller.ResetBombBays();
		Debug.Log("----------------");
		controller.UpgradeBombBays();
		controller.UpgradeBombBays();
		controller.UpgradeBombBays();
		Debug.Log("----------------");
		controller.UpgradeBombBays();
		controller.UpgradeBombBays();
		controller.UpgradeBombBays();
	}
}
