using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverConditionScript : MonoBehaviour
{
	public GameObject[] players;
	public GameObject gameOverUX;
	TimeManager timeManager;

    public const string gameOverMusicEvent = "event:/Music/GameOver";

    void Start()
    {
		timeManager = TimeManager.instance;
    }

    void Update()
    {
        if(!gameOverUX.active)
        {
            if ((players[0].activeSelf == false || players[0].GetComponent<PlayerController>().Health <= 0f)
            && (players[1].activeSelf == false || players[1].GetComponent<PlayerController>().Health <= 0f))
            {
                gameOverUX.SetActive(true);
                timeManager.gameIsPaused = true;
                FMODUnity.RuntimeManager.PlayOneShot(gameOverMusicEvent, transform.position);
            }
        }

    }
}
