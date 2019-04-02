using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityStandardAssets._2D;

public enum ControllerKind
{
    Keyboard,
    XInput,
    DualShock,
    AI
}

public class GameManagerScript : MonoBehaviour
{

	public static GameManagerScript instance;

	//Directors
	public GameObject DebugUIGO;
	public string m_Scene;
	public GameObject m_MyGameObject;
	public Animator ScreenTrans;
	public Camera2DFollow CAM;
	public GameObject P2CAM;

	///Settings///
	//General Settings
	public int ParticleIntensity = 3;
	public bool Screenshake = true;
	public bool DamageText = true;

	[Space]
	//PlayerCount
	[Space]
	public int PlayerCount = 1;
	public bool isAIBot {
		get {
			return (PlayerCount == 2) && (p2ControllerKind == ControllerKind.AI);
		}
		set {
			if (value) {
				PlayerCount = 2;
				p2ControllerKind = ControllerKind.AI;
			} else {
				p2ControllerKind = ControllerKind.Keyboard;
			}
		}
	}

	[Space]
    public float masterVolume = .5f;
    public float sfxVolume = .5f;
    public float musicVolume = .5f;

	[Space]
	// Player 1 info and controls
    public ControllerKind p1ControllerKind = ControllerKind.Keyboard;
	public GameObject Player1GO;
	public PlayerController PC1;
	public GameObject Player1UI;
	[Space]
	// Player 2 info and controls
    public ControllerKind p2ControllerKind = ControllerKind.AI;
	public GameObject Player2GO;
	public PlayerController PC2;
	public GameObject Player2UI;
	[Space]
	public GameObject[] ThingsToWake;
	///

	GameObject dialogueManager;

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	void Start() {
        dialogueManager = GameObject.Find("DialogueManager");
	}

	public void StartTheGame() {
		InitializeGame();
		if (dialogueManager != null) {
			dialogueManager.SetActive(true);
		}
		SceneManager.LoadScene(1);
	}

	public void InitializeGame()
	{
		EnablePlayer1();
		if (PlayerCount == 2)
		{
			EnablePlayer2();
		}
		foreach (GameObject things in ThingsToWake)
		{
			things.SetActive(true);
		}
		FloatingTextController.Initialize();
	}
	public void LinkPlayers()
	{
		PC1 = Player1GO.GetComponent<PlayerController>();
		if (PlayerCount == 2)
		{
			PC2 = Player2GO.GetComponent<PlayerController>();
			return;
		}
		Player2GO.SetActive(false);
	}

	public void EnablePlayer1()
	{
		LinkPlayers();
		Player1UI.SetActive(true);
		Player1GO.SetActive(true);
		CAM.target = Player1GO.transform;
	}
	public void EnablePlayer2()
	{
		LinkPlayers();
		Debug.Log("player 2 is bot: " + isAIBot);
		PC2.isBot = isAIBot;
		Player2UI.SetActive(true);
		Player2GO.SetActive(true);
		if (PC2.isBot)
		{
			PC2.name = "AI Companion";
			return;
		}
		P2CAM.SetActive(true);
		CAM.target = P2CAM.transform;
	}

	public void DisablePlayer2()
	{
		PlayerCount = 1;
		LinkPlayers();
		Player2GO.SetActive(false);
	}



	/// <summary>
	///  This is the scene swapper area, simple and rudimentary. Will probably update it if project complexity
	///  increases. For now this is all we need.
	/// </summary>
	///



	public void GoToScene(string sentscene)
	{
		m_Scene = sentscene;
		ScreenTrans.SetTrigger("FadeOut");
	}

	public void FadeComplete()
	{
		Debug.Log("Loading scene " + m_Scene);
		SceneManager.LoadScene(m_Scene);
		ScreenTrans.SetTrigger("FadeIn");
	}

	public void ToggleDebugUI()
	{
		DebugUIGO.SetActive(!(DebugUIGO));
	}

	//Pulls player 2 back to player 1. Happens when room's combat starts.
	public void RecallPlayer2()
	{
		Player2GO.transform.position = Player1GO.transform.position;
		Debug.Log("Player 2 recalled");
	}

	//IEnumerator LoadNextScene()
	//{
	//    //Set the current Scene to be able to unload it later
	//    Scene currentScene = SceneManager.GetActiveScene();

	//    // The Application loads the Scene in the background at the same time as the current Scene.
	//    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_Scene, LoadSceneMode.Additive);

	//    //Wait until the last operation fully loads to return anything
	//    while (!asyncLoad.isDone)
	//    {
	//        yield return null;
	//    }
	//    if (asyncLoad.isDone)
	//    {
	//    //SceneManager.MoveGameObjectToScene(m_MyGameObject, SceneManager.GetSceneByName(m_Scene));
	//    //SceneManager.MoveGameObjectToScene(m_MainCamera, SceneManager.GetSceneByName(m_Scene));
	//        SceneManager.UnloadSceneAsync(currentScene);
	//        PC1.SendMessage("GoToStart");
	//        Debug.Log("Go to start message sent");
	//        Debug.Log("Scene loaded Successfully");

	//    //Unload the previous Scene
	//    }
	//}

	//new Vector3(3.3f, -0.66f, 0.05f); //<- Player Default Start Position
}
