using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (timeManager.gameIsPaused ^ Cursor.visible)
        {
            Cursor.visible = !Cursor.visible;
        }

        InGameCursor.gameObject.SetActive(!Cursor.visible);
    }
}
