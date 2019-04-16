using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string Music;
    private FMOD.Studio.EventInstance musicEv;
    float m_Music;

    public EnemyCheck enemyCheck;
    public PlayerController playerController;
    Scene m_Scene;
    private bool gameStarted = false;

    private void Start()
    {
        musicEv = FMODUnity.RuntimeManager.CreateInstance(Music);
        enemyCheck.GetComponent<EnemyCheck>();
    }

    private void Update()
    {
        musicEv.setParameterValue("Music", m_Music);
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
                musicEv.start();
            }
        }
    }

    private void StopMusic()
    {
        if (gameStarted)
        {
            if (m_Scene.name == "MainMenu")
            {
                gameStarted = false;
                musicEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
            musicEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
