using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Asteroids/Game Settings")]
public class GameSettings : ScriptableObject {

	[Header("Options")]
	public bool spawnAsteroids;
	public bool spawnPlayer;
	public bool spawnOthers;
	public bool playIntro;
	public bool playMusic;

	[Header("Settings")]
	public int level;
	public int asteroids;
	public int lives;
	public int bonusLifeEveryPoints;
	public int startingScore;
}
