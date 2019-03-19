using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField]
    private Scene currentScene;
    public GameObject dialogueBox;
    public GameObject CompanionManager;
	public GameObject pauseMenu;

    // Use this for initialization
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainMenu")
        {
			GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
			dialogueBox = gameManager.transform.GetChild(1).gameObject;
			CompanionManager = gameManager.transform.GetChild(9).gameObject;
			pauseMenu = gameManager.transform.GetChild(2).GetChild(2).gameObject;

			dialogueBox.SetActive(false);
        }
    }

    public void AIBot(bool isOn)
    {
        GameManagerScript.instance.isAIBot = isOn;
        Debug.Log("AI bot is on? " + GameManagerScript.instance.isAIBot);
        if (GameManagerScript.instance.isAIBot)
        {
            CompanionManager.SetActive(true);
            return;
        }
        CompanionManager.SetActive(false);
    }

    public void Singleplayer()
    {
		pauseMenu.GetComponent<PauseMenu>().Resume();
		GameManagerScript.instance.PlayerCount = 1;
        GameManagerScript.instance.InitializeGame();
        SceneManager.LoadScene(1);
        dialogueBox.SetActive(true);
		
    }
    public void Multiplayer()
    {
		pauseMenu.GetComponent<PauseMenu>().Resume();
		GameManagerScript.instance.PlayerCount = 2;
        GameManagerScript.instance.InitializeGame();
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
