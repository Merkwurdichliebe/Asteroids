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

	public static Action<GameObject> OnPlayerEnabled;
    public static Action<GameObject> OnPlayerDisabled;
    public static Action OnPlayerLivesZero;

	private void Awake()
	{
		// Create the Player & Safe Zone
        spawnSafeZone = Instantiate(spawnSafeZonePrefab);
		spawnPosition = Vector2.zero;
		spawnSafeZone.SetActive(true);
		if (gameSettings.spawnPlayer)
        {
            Player = Instantiate(playerPrefab);
			// DisablePlayer();
        }
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
			StartCoroutine(EnablePlayerInSeconds(3f));
		}
		else
		{
			if (OnPlayerLivesZero != null) { OnPlayerLivesZero(); }
			Destroy(Player.gameObject, 3);
		}
	}

	IEnumerator EnablePlayerInSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		EnablePlayer();
	}

	private void EnablePlayer()
	{
		StartCoroutine(EnablePlayerWhenClear());
	}

	// Wait for a number of seconds,
	// then start the safe zone clear check.
	IEnumerator EnablePlayerWhenClear()
	{
		centerIsClear = false;
		spawnSafeZone.SetActive(true);
		while (!centerIsClear) { yield return null; }
		Player.ResetPosition(spawnPosition);
		if (Player != null)
			Player.gameObject.SetActive(true);
		if (OnPlayerEnabled != null) OnPlayerEnabled(Player.gameObject);
		Debug.Log("[Player] enabled");
	}

	private void DisablePlayer()
	{
		if (Player != null)
			Player.gameObject.SetActive(false);
		if (OnPlayerDisabled != null) OnPlayerDisabled(Player.gameObject);
		Debug.Log("[Player] disabled");
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
