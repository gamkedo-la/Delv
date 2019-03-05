using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ByPassMenu : MonoBehaviour
{
    public GameObject gameManagerPrefab;
    private MainMenuScript BypassScript;
    // Start is called before the first frame update
    void Start()
    {
        if (Camera.main)
        {
            Debug.Log("There is a main camera, therefore we got here from main menu");
        } 
        else 
        {
            Debug.Log("No camera detected, started from editor");
            BypassScript = GetComponent<MainMenuScript>();
            Instantiate(gameManagerPrefab);
            GameManagerScript.instance.PlayerCount = 2;
            GameManagerScript.instance.InitializeGame();
            //SceneManager.LoadScene(1);
            BypassScript.dialogueBox = GameObject.Find("DialogueManager");
            BypassScript.CompanionManager = GameObject.Find("CompanionManager");
            //dialogueBox.SetActive(true);
        }
    }
}
