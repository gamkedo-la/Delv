using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuScript : MonoBehaviour {

    [SerializeField]
    private GameManagerScript GM;

	// Use this for initialization
	void Start ()
    {
        GameObject GMGO = GameObject.Find("GameManager");
        GM = GMGO.GetComponent<GameManagerScript>();
	}
	


    public void Singleplayer()
    {
        GM.PlayerCount = 1;
        GM.InitializeGame();
        SceneManager.LoadScene(1);
    }
    public void Multiplayer()
    {
        GM.PlayerCount = 2;
        GM.InitializeGame();
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }

}
