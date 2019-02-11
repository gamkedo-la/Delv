using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuScript : MonoBehaviour {

    [SerializeField]
    private GameManagerScript GM;
    private Scene currentScene;
    public  GameObject dialogueBox;

	// Use this for initialization
	void Start ()
    {
        GameObject GMGO = GameObject.Find("GameManager");
        GM = GMGO.GetComponent<GameManagerScript>();
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainMenu")
        {
            dialogueBox.SetActive(false);
        }
    }
	
    public void AIBot(bool isOn)
    {
        GM.isAIBot = isOn;
        Debug.Log("AI bot is on? " + GM.isAIBot);
    }

    public void Singleplayer()
    {
        GM.PlayerCount = 1;
        GM.InitializeGame();
        SceneManager.LoadScene(1);
        dialogueBox.SetActive(true);
    }
    public void Multiplayer()
    {
        GM.PlayerCount = 2;
        GM.InitializeGame();
        SceneManager.LoadScene(1);
        dialogueBox.SetActive(true);
    }

    public void Settings()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }

}
