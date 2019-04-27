using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAndStopTitleMusic : MonoBehaviour
{
    private FMOD.Studio.EventInstance TitleMusic;
    // Start is called before the first frame update
    void Start()
    {
        TitleMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Title Music");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayTitleMusic()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Music/Title Music");
    }

    public void StopTitleMusic()
    {
        TitleMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    /*if (!gameStarted)
        {
            Invoke(PlayTitleMusic, 2);
        }*/
}
