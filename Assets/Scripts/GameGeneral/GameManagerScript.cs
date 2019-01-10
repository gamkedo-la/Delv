using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    //Type in the name of the Scene you would like to load in the Inspector
    public string m_Scene;
    //Assign your GameObject you want to move Scene in the Inspector
    public GameObject m_MyGameObject;
    //public GameObject m_MainCamera;
    public PlayerController PC;
    public float PlayerIndex;
    public bool Screenshake = true;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        PC = m_MyGameObject.GetComponentInChildren<PlayerController>();
        PlayerIndex = PC.PlayerIndex;
    }

    // Use this for initialization
    void Start ()
    {
        //Sends player to the start as soon as the GM is starting
        PC.SendMessage("GoToStart");
        FloatingTextController.Initialize();
    }





    public void GoToScene(string sentscene)
    {
        m_Scene = sentscene;
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        //Set the current Scene to be able to unload it later
        Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_Scene, LoadSceneMode.Additive);

        //Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (asyncLoad.isDone)
        {
        //SceneManager.MoveGameObjectToScene(m_MyGameObject, SceneManager.GetSceneByName(m_Scene));
        //SceneManager.MoveGameObjectToScene(m_MainCamera, SceneManager.GetSceneByName(m_Scene));
            SceneManager.UnloadSceneAsync(currentScene);
            PC = m_MyGameObject.GetComponentInChildren<PlayerController>();
            PC.SendMessage("GoToStart");
            Debug.Log("Go to start message sent");
            Debug.Log("Scene loaded Successfully");
        //Unload the previous Scene
        }
        

    }



    
    
}