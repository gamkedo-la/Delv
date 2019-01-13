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
        GM.EnablePlayer1();
        SceneManager.LoadScene(1);
    }
    public void Multiplayer()
    {
        GM.EnablePlayer1();
        GM.EnablePlayer2();
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
