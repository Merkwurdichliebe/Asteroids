using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	//
	// Inspector fields
	//
	public PlayerController playerPrefab;
	public GameSettings gameSettings;
	public PlayerController Player { get; private set; }
	public GameObject spawnSafeZonePrefab;

	//
	// Private fields
	//
	private GameObject spawnSafeZone;
	private bool centerIsClear;
	private Vector2 spawnPosition;

	//
	// Events
	//
	public static Action<GameObject> OnPlayerEnabled;
    public static Action<GameObject> OnPlayerDisabled;
    public static Action OnPlayerLivesZero;

	//
	// Initialisation
	//
	private void Awake()
	{
		// Create the Player & Safe Zone
        spawnSafeZone = Instantiate(spawnSafeZonePrefab);
		spawnPosition = Vector2.zero;
		spawnSafeZone.SetActive(true);
		if (gameSettings.spawnPlayer)
        	Player = Instantiate(playerPrefab);
	}

	private void Start()
	{
		// This needs to be set in Start in order to let
		// other objects subscribe in OnEnable.
		Player.Lives = gameSettings.lives;
	}

	//
    // Event handler for when the center spawn safe zone is clear.
    // It gets the position where the safe zone
    // has repositioned itself and checked it is clear of hostiles,
    // and sets the centerIsClear variable, which is used to break out
    // of the loop in the coroutine below.
    //
    private void HandleCenterIsClear(bool clear, Vector2 zonePosition)
    {
        centerIsClear = clear;
        spawnPosition = zonePosition;
    }

	//
	// React to player death based on number of lives left.
	//
	private void HandlePlayerDied()
	{
		DisablePlayer();
		
		// Check if we should respawn.
		// Otherwise the game is over and we can destroy the player object.
		if (Player.Lives > 0) {
			StartCoroutine(EnablePlayerInSeconds(3f));
		}
		else
		{
			if (OnPlayerLivesZero != null) { OnPlayerLivesZero(); }
			Destroy(Player.gameObject, 3);
		}
	}

	//
	// Coroutine only used for adding respawn delay.
	//
	IEnumerator EnablePlayerInSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		EnablePlayer();
	}

	//
	// This can't be integrated as part of the coroutine
	// because it's called by the Action subscribtion in OnEnable
	// and therefore needs to match the delegate signature
	// of GameManager.OnGameLevelStart.
	//
	private void EnablePlayer()
	{
		StartCoroutine(EnablePlayerWhenClear());
	}

	//
	// Activate the safe zone. When it's clear, set the
	// player position and enable the player.
	//
	IEnumerator EnablePlayerWhenClear()
	{
		centerIsClear = false;
		spawnSafeZone.SetActive(true);
		while (!centerIsClear) { yield return null; }
		Player.ResetPosition(spawnPosition);
		if (Player != null)
			Player.gameObject.SetActive(true);
		if (OnPlayerEnabled != null) OnPlayerEnabled(Player.gameObject);
}

	private void DisablePlayer()
	{
		if (Player != null)
			Player.gameObject.SetActive(false);
		if (OnPlayerDisabled != null) OnPlayerDisabled(Player.gameObject);
	}

	private void OnEnable()
	{
		GameManager.OnGameLevelIntro += DisablePlayer;
		GameManager.OnGameLevelStart += EnablePlayer;
		PlayerController.OnPlayerDestroyed += HandlePlayerDied;
		SafeZone.OnSafeZoneClear += HandleCenterIsClear;
	}

	private void OnDisable()
	{
		GameManager.OnGameLevelIntro -= DisablePlayer;
		GameManager.OnGameLevelStart -= EnablePlayer;
		PlayerController.OnPlayerDestroyed -= HandlePlayerDied;
		SafeZone.OnSafeZoneClear -= HandleCenterIsClear;
	}
}
