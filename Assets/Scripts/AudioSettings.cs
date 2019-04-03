using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    public static FMOD.Studio.Bus Master;
    public static FMOD.Studio.Bus SFX;
    public static FMOD.Studio.Bus Music;

    public float MasterVolume {
        get {
            float currentVolume, finalVolume;
            Master.getVolume(out currentVolume, out finalVolume);
            return currentVolume;
        }
        set {
            Master.setVolume(value);
        }

    }

    public float MusicVolume {
        get {
            float currentVolume, finalVolume;
            Music.getVolume(out currentVolume, out finalVolume);
            return currentVolume;
        }
        set {
            Music.setVolume(value);
        }
    }

    public float SFXVolume {
        get {
            float currentVolume, finalVolume;
            SFX.getVolume(out currentVolume, out finalVolume);
            return currentVolume;
        }
        set {
            SFX.setVolume(value);
        }
    }

    void Awake()
    {
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
    }

/*
    public void MasterVolumeLevel(float newMasterVolume)
    {
        MasterVolume = newMasterVolume;
        Master.setVolume(MasterVolume);
    }

    public void MusicVolumeLevel(float newMusicVolume)
    {
        MusicVolume = newMusicVolume;
        Music.setVolume(MusicVolume);
    }

    public void SFXVolumeLevel(float newSFXVolume)
    {
        SFXVolume = newSFXVolume;
        SFX.setVolume(SFXVolume);
    }
    */
}
