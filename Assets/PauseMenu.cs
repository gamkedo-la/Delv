using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;
    private GameManagerScript GameManager;
    private TimeManager TimeManager;
    public GameObject PauseContainerGO;


    // Use this for initialization
    void Start ()
    {
        PauseContainerGO = this.transform.GetChild(0).gameObject;
        GameObject GameManagerGO = GameObject.FindGameObjectWithTag("GameManager");
        GameManager = GameManagerGO.GetComponent<GameManagerScript>();
        TimeManager = GameManager.GetComponent<TimeManager>();

    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (GameIsPaused == true)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        PauseContainerGO.SetActive(true);
        GameIsPaused = true;
        //this.transform.GetChild(0).gameObject.SetActive(true);
        Debug.Log("Paused");


    }

    public void Resume()
    {
        PauseContainerGO.SetActive(false);
        GameIsPaused = false;
        //this.transform.GetChild(0).gameObject.SetActive(false);
        Debug.Log("Unpaused");
    }

    public void MainMenu()
    {

    }

    public void Settings()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }

}
