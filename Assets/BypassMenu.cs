using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BypassMenu : MonoBehaviour
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

            // workaround to ensure player2 GO is always assessible
            GameManagerScript.instance.isAIBot = false;
            GameManagerScript.instance.PlayerCount = 2;
            GameManagerScript.instance.InitializeGame();

            player2 = GameManagerScript.instance.Player2GO;

            changeGameManagerBasedOnNumberOfPlayers();
            previousNumberOfPlayers = numberOfPlayers;
            previousAIOnState = AIEnabled;
            GameManagerScript.instance.isAIBot = AIEnabled;
            GameManagerScript.instance.PlayerCount = numberOfPlayers;

            GameManagerScript.instance.InitializeGame();

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
            changeGameManagerBasedOnNumberOfPlayers();

            previousAIOnState = AIEnabled;
            previousNumberOfPlayers = numberOfPlayers;
            GameManagerScript.instance.isAIBot = AIEnabled;
            GameManagerScript.instance.PlayerCount = numberOfPlayers;

            GameManagerScript.instance.InitializeGame();
        }
    }

    public void changeGameManagerBasedOnNumberOfPlayers()
    {
        if (numberOfPlayers == 1)
        {
            player2.SetActive(false);
            if (AIEnabled) 
            {
                Debug.Log("AI not enabled because there is only one player");
                previousAIOnState = AIEnabled = false;
            }
        }
        else
        {
            player2.SetActive(true);
        }
    }
}
