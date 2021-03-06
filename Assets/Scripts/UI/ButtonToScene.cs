﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonToScene : MonoBehaviour, IPointerClickHandler
{
	public string sceneName = "null";
	public bool startTime = false;
	public AudioClip clickSound;
	public UnityEvent OnButtonPress;
    public FMODUnity.StudioEventEmitter gameOverSound;
	private AudioSource aud = null;

	private TimeManager timeManager;

	void Start( )
	{
		aud = GetComponent<AudioSource>( );
		if ( aud == null )
			aud = FindObjectOfType<AudioSource>( );

		timeManager = TimeManager.instance;
        gameOverSound.GetComponent<FMODUnity.StudioEventEmitter>();
    }

    void ClickEvent()
	{
		if (sceneName == "Quit")
		{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
		}
		else if (sceneName == "Reset")
		{
			if (startTime)
			{
				Time.timeScale = 1f;
				timeManager.gameIsPaused = false;
			}
			SceneManager.LoadScene(gameObject.scene.name);
		}
		else
		{
			if (startTime)
			{
				Time.timeScale = 1f;
				timeManager.gameIsPaused = false;
			}
			OnButtonPress.Invoke();
			SceneManager.LoadScene(sceneName);
		}
	}
	
	void OnMouseOver( )
	{
		if ( Input.GetMouseButtonDown( 0 ) )
		{
			if ( aud != null )
				aud.PlayOneShot( clickSound );
			ClickEvent();

			if ( startTime )
				Time.timeScale = 1f;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
    {
		ClickEvent();
    }

	public void GoToScene( )
	{
		SceneManager.LoadScene( sceneName );
	}

    public void StopGameOverSound()
    {
        gameOverSound.Stop();
    }
}
