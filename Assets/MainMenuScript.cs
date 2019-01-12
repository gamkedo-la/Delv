using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Singleplayer()
    {
        SceneManager.LoadScene(1);
    }
    public void Multiplayer()
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
