using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ByPassMenu : MonoBehaviour
{
    public GameObject gameManagerPrefab;
    public bool AIEnabled = true;
    public int numberOfPlayers = 2;
    private bool previousAIOnState;
    private int previousNumberOfPlayers;

    private MainMenuScript BypassScript;
    private AICompanion AI;
    private GameObject player2;

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
            AI = GetComponent<AICompanion>();
            Instantiate(gameManagerPrefab);
            GameManagerScript.instance.isAIBot = AIEnabled;
            GameManagerScript.instance.PlayerCount = numberOfPlayers;
            previousNumberOfPlayers = numberOfPlayers;
            previousAIOnState = AIEnabled; 
            GameManagerScript.instance.InitializeGame();
            if (numberOfPlayers == 2) 
            {
                player2 = GameManagerScript.instance.Player2GO;
            }
            BypassScript.dialogueBox = GameObject.Find("DialogueManager");
            BypassScript.CompanionManager = GameObject.Find("CompanionManager");
        }
    }

    private void Update()
    {
        StartCoroutine(checkSceneVariables());
    }

    IEnumerator checkSceneVariables()
    {
        yield return new WaitForSeconds(2.0f);
        if (previousAIOnState != AIEnabled || previousNumberOfPlayers != numberOfPlayers)
        {
            previousAIOnState = AIEnabled;
            previousNumberOfPlayers = numberOfPlayers;
            GameManagerScript.instance.isAIBot = AIEnabled;
            GameManagerScript.instance.PlayerCount = numberOfPlayers;
            if (numberOfPlayers == 1)
            {
                player2.SetActive(false);
                previousAIOnState = AIEnabled = false;
                GameManagerScript.instance.isAIBot = AIEnabled;
            }
            else
            {
                player2.SetActive(true);
            }
            GameManagerScript.instance.InitializeGame();
        }
    }
}
