﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    public static FMOD.Studio.Bus Master;
    public static FMOD.Studio.Bus SFX;
    public static FMOD.Studio.Bus Music;

    float MasterVolume = 1f;
    float MusicVolume = .5f;
    float SFXVolume = .5f;

    void Awake()
    {
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
    }

    void Update()
    {
        Master.setVolume(MasterVolume);
        Music.setVolume(MusicVolume);
        SFX.setVolume(SFXVolume);
    }

    public void MasterVolumeLevel(float newMasterVolume)
    {
        MasterVolume = newMasterVolume;
    }

    public void MusicVolumeLevel(float newMusicVolume)
    {
        MusicVolume = newMusicVolume;
    }

    public void SFXVolumeLevel(float newSFXVolume)
    {
        SFXVolume = newSFXVolume;
    }
}