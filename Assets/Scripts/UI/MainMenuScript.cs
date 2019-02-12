using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField]
    private Scene currentScene;
    public GameObject dialogueBox;

    // Use this for initialization
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainMenu")
        {
            dialogueBox.SetActive(false);
        }
    }

    public void AIBot(bool isOn)
    {
        GameManagerScript.instance.isAIBot = isOn;
        Debug.Log("AI bot is on? " + GameManagerScript.instance.isAIBot);
    }

    public void Singleplayer()
    {
        GameManagerScript.instance.PlayerCount = 1;
        GameManagerScript.instance.InitializeGame();
        SceneManager.LoadScene(1);
        dialogueBox.SetActive(true);
    }
    public void Multiplayer()
    {
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
