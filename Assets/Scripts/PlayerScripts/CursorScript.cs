using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorScript : MonoBehaviour
{
    public SpriteRenderer InGameCursor;

    TimeManager timeManager;

	// Use this for initialization
	void Start ()
    {
        timeManager = TimeManager.instance;
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        SetCorrectCursor();

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3 (mousePosition.x,mousePosition.y,0);
    }

    void SetCorrectCursor()
    {
		Cursor.visible = timeManager.gameIsPaused || SceneManager.GetActiveScene().name == "MainMenu";

        InGameCursor.gameObject.SetActive(!Cursor.visible);
    }
}
