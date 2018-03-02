using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This MonoBehaviour is a basic pause menu script
/// which is intended to be used on a Canvas.
/// </summary>

public class PauseMenuController : MonoBehaviour {

	public static bool GameIsPaused;
	public GameObject menuPanel;

	//
	// Make sure the menu is not visible when starting
	//
	private void Start() 
	{
		menuPanel.SetActive(false);
	}

	//
	// Check for escape key press
	//
	private void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SwitchStates();
		}
	}

	//
	// Switch between:
	// - GameIsPaused variable value
	// - Active state of the panel
	// - Time scale
	//
	private void SwitchStates()
	{
		GameIsPaused = !GameIsPaused;
		menuPanel.SetActive(!menuPanel.activeSelf);
		Time.timeScale = (1 - Time.timeScale);
	}

	//
	// Resume Button handler
	//
	public void Resume()
	{
		SwitchStates();
	}

	//
	// Menu Button handler
	//
	public void LoadMenu(string sceneName)
	{
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(sceneName);
	}

	//
	// Quit Button handler
	//
	public void Quit()
    {
        //If we are running in a standalone build of the game
    	#if UNITY_STANDALONE
        //Quit the application
        Application.Quit();
    	#endif
 
        //If we are running in the editor
    	#if UNITY_EDITOR
        //Stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
    	#endif
    }
}
