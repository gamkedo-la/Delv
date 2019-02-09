using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public PauseMenu PauseMenu;
    public SpriteRenderer InGameCursor;

	// Use this for initialization
	void Start ()
    {
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
        if (PauseMenu.GameIsPaused ^ Cursor.visible)
        {
            Cursor.visible = !Cursor.visible;
        }

        InGameCursor.gameObject.SetActive(!Cursor.visible);
    }
}
