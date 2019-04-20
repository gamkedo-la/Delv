using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    //[FMODUnity.EventRef]
    //public string Music;
    //private FMOD.Studio.EventInstance musicEv;
    FMODUnity.StudioEventEmitter musicEmitter;

    float m_Music;


    public EnemyCheck enemyCheck;
    public PlayerController playerController;
    public GameObject gameOverUX;
    Scene m_Scene;
    private bool gameStarted = false;

    private void Start()
    {
        musicEmitter = GetComponent<FMODUnity.StudioEventEmitter>();
        //musicEv = FMODUnity.RuntimeManager.CreateInstance(Music);
        enemyCheck.GetComponent<EnemyCheck>();
        //gameOverCondition = GetComponent<GameOverConditionScript>();
    }

    private void Update()
    {
        musicEmitter.SetParameter("Music", m_Music);
        m_Scene = SceneManager.GetActiveScene();
        StartMusic();
        StopMusic();
        MusicParameter();
    }

    private void StartMusic()
    {
        if (!gameStarted)
        {
            if (m_Scene.name != "MainMenu")
            {
                gameStarted = true;
                musicEmitter.Play();
                //musicEv.start();
            }
        }
    }

    private void StopMusic()
    {
        if (gameStarted)
        {
            if (m_Scene.name == "MainMenu" || gameOverUX.activeInHierarchy)
            {
                gameStarted = false;
                musicEmitter.Stop();
                //musicEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
    }

    public void MusicParameter()
    {
        if(enemyCheck.EnemyList.Count <= 0)
        {
            m_Music = 0;
        }

        if (enemyCheck.EnemyList.Count > 0)
        {
            m_Music = 1;
        }

        //check if the player die

        if(playerController.isDead)
        {
            musicEmitter.Stop();
            //musicEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
