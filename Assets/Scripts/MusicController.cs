using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string Music;
    private FMOD.Studio.EventInstance musicEv;

    Scene m_Scene;
    private bool gameStarted = false;

    private void Start()
    {
        musicEv = FMODUnity.RuntimeManager.CreateInstance(Music);
    }

    private void Update()
    {
        m_Scene = SceneManager.GetActiveScene();
        StartMusic();
        StopMusic();
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
                musicEv.release();
            }
        }
    }
}
