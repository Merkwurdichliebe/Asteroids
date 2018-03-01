using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTrigger : MonoBehaviour {

	public static Action OnGameLevelComplete;
	public static Action OnGameOver;

	public void OnGameOverAndLevelComplete()
	{
		if (OnGameOver != null) { OnGameOver(); }
		if (OnGameLevelComplete != null) { OnGameLevelComplete(); }
	}
}
