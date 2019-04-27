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

    private FMOD.Studio.EventInstance TitleMusicEventInstance;
    private FMOD.Studio.PLAYBACK_STATE TitleMusicPlaybackState;
    private bool TitleMusicCoroutineRunning;
    private IEnumerator PlayTitleAfterTwoSeconds;

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

        TitleMusicEventInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Title Music");
        TitleMusicCoroutineRunning = false;
        PlayTitleAfterTwoSeconds = ExecuteAfterTime(2);
    }

    private void Update()
    {
        musicEmitter.SetParameter("Music", m_Music);
        m_Scene = SceneManager.GetActiveScene();
        StartMusic();
        StopMusic();
        MusicParameter();
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        if (TitleMusicCoroutineRunning)
            yield break;
        TitleMusicCoroutineRunning = true;
        yield return new WaitForSeconds(time);
        Debug.Log("Hello Execute After Time");
        TitleMusicEventInstance.start();
        TitleMusicCoroutineRunning = false;
    }

    private void StartMusic()
    {

        if (!gameStarted)
        {
            TitleMusicEventInstance.getPlaybackState(out TitleMusicPlaybackState);
            Debug.Log(TitleMusicPlaybackState);
        }
        if (!gameStarted && m_Scene.name == "MainMenu" && TitleMusicPlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {

            StartCoroutine(PlayTitleAfterTwoSeconds);
            }

        
            
        if (m_Scene.name != "MainMenu")
        {
            
            gameStarted = true;
            StopCoroutine(PlayTitleAfterTwoSeconds);
            TitleMusicEventInstance.getPlaybackState(out TitleMusicPlaybackState);
            if (TitleMusicPlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                TitleMusicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            }

            musicEmitter.Play();

            //musicEv.start();
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
