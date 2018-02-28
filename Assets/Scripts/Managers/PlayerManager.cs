using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public PlayerController playerPrefab;
	public GameSettings gameSettings;
	public PlayerController Player { get; private set; }
	public GameObject spawnSafeZonePrefab;

	private GameObject spawnSafeZone;
	private bool centerIsClear;
	private Vector2 spawnPosition;

    public static Action OnPlayerLivesZero;

	private void Awake()
	{
		// Create the Player spawn safe zone
        spawnSafeZone = Instantiate(spawnSafeZonePrefab);

		if (gameSettings.spawnPlayer)
        {
            Player = Instantiate(playerPrefab);
            Player.Lives = gameSettings.lives;
			DisablePlayer();
        }

		spawnPosition = Vector2.zero;

		spawnSafeZone.SetActive(true);
	}

	//
    // Event handler for when the center spawn safe zone is clear.
    // It gets the position where the safe zone
    // has repositioned itself and checked it is clear of hostiles.
    //
    private void HandleCenterIsClear(bool clear, Vector2 zonePosition)
    {
        centerIsClear = clear;
        spawnPosition = zonePosition;
    }

	private void HandlePlayerDied()
	{
		DisablePlayer();
		
		// Check if we should respawn.
		// Otherwise the game is over and we can destroy the player object.
		if (Player.Lives > 0) {
			StartCoroutine(EnablePlayerWhenClear());
		}
		else
		{
			if (OnPlayerLivesZero != null) { OnPlayerLivesZero(); }
			Destroy(gameObject, 3);
		}
	}

	// Wait for a number of seconds,
	// then start the safe zone clear check.
	IEnumerator EnablePlayerWhenClear()
	{
		yield return new WaitForSeconds(3);
		centerIsClear = false;
		spawnSafeZone.SetActive(true);
		while (!centerIsClear) { yield return null; }
		EnablePlayer();
	}


	private void EnablePlayer()
	{
		Player.ResetPosition(spawnPosition);
		if (Player != null)
			Player.gameObject.SetActive(true);
	}

	private void DisablePlayer()
	{
		if (Player != null)
			Player.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		GameManager.OnGameLevelReady += DisablePlayer;
		GameManager.OnGameLevelStart += EnablePlayer;
		PlayerController.OnPlayerDestroyed += HandlePlayerDied;
		SafeZone.OnSafeZoneClear += HandleCenterIsClear;
	}

	private void OnDisable()
	{
		GameManager.OnGameLevelReady -= DisablePlayer;
		GameManager.OnGameLevelStart -= EnablePlayer;
		PlayerController.OnPlayerDestroyed -= HandlePlayerDied;
		SafeZone.OnSafeZoneClear -= HandleCenterIsClear;
	}

}
