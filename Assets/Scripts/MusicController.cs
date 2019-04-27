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

    private AudioSettings audioSettings;
    private float beforeMuteMasterVolume;
    public bool mute;

    public GameObject gameOverUX;
    Scene m_Scene;
    private bool gameStarted = false;

    private void Start()
    {
        audioSettings = GetComponent<AudioSettings>();
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
        if (m_Scene.name != "MainMenu" && !gameStarted)
        {
            musicEmitter.Play();
            gameStarted = true;
            //musicEv.start();
            Debug.Log("MUSIC STARTED");
        }
    }
    
    
    private void StopMusic()
    {
        if (gameStarted)
        {
            if (mute)
            {
                if (!Mathf.Approximately(audioSettings.MasterVolume, 0))
                {
                    beforeMuteMasterVolume = audioSettings.MasterVolume;
                    audioSettings.MasterVolumeLevel(0);
                    Debug.Log("Mute enabled");
                    Debug.Log("MasterVolume: " + audioSettings.MasterVolume);
                }
            } 
            else
            {
                if (Mathf.Approximately(audioSettings.MasterVolume, 0))
                {
                    audioSettings.MasterVolumeLevel(beforeMuteMasterVolume);
                    Debug.Log("Mute disabled");
                    Debug.Log("MasterVolume: " + audioSettings.MasterVolume);
                }
            }

            if (m_Scene.name == "MainMenu" || gameOverUX.activeInHierarchy)
            {
                gameStarted = false;
                musicEmitter.Stop();
                //musicEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }
        }
    }

    public void changeMute()
    {
        if (mute)
        {
            mute = false;
            return;
        }
        mute = true;
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
