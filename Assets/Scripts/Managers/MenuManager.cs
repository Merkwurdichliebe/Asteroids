using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    // References
    public Text textHighscore;

    void Start()
    {
        int highscore;
        if (PlayerPrefs.HasKey("highscore"))
        {
            highscore = PlayerPrefs.GetInt("highscore");
        }
        else
        {
            highscore = 0;
        }
        textHighscore.text = string.Format("High Score : {0}", highscore);
    }
	


	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Main");
        }
	}
}
