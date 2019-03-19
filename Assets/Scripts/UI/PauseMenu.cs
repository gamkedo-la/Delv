using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public GameObject PauseContainerGO;


    // Use this for initialization
    void Start()
    {
        PauseContainerGO = this.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
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
        //Resume();
        SceneManager.LoadScene(0);
        Debug.Log("Loading MainMenu");
    }

    public void Settings()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }

}
