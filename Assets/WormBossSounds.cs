using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBossSounds : MonoBehaviour
{
    // FMOD Animation Event
    [FMODUnity.EventRef]
    public string HeadSound;
    FMOD.Studio.EventInstance HeadSoundEv;

    [FMODUnity.EventRef]
    public string earthquakeSound;
    public FMOD.Studio.EventInstance earthquakeSoundEv;

    public GameObject dugtail;
    public GameObject wormgroup;

    bool dugtailIsPlaying = false;
    private void Start()
    {
        HeadSoundEv = FMODUnity.RuntimeManager.CreateInstance(HeadSound);
        earthquakeSoundEv = FMODUnity.RuntimeManager.CreateInstance(earthquakeSound);

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(HeadSoundEv, GetComponent<Transform>(), GetComponent<Rigidbody>());
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(earthquakeSoundEv, GetComponent<Transform>(), GetComponent<Rigidbody>());

    }

    private void Update()
    {
        HeadSoundEv.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform, GetComponent<Rigidbody2D>()));
        earthquakeSoundEv.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform, GetComponent<Rigidbody2D>()));
        CheckDugTail();
    }

    public void HeadAnimationSound()
    {
        HeadSoundEv.start();
    }

    public void CheckDugTail()
    {
        if (dugtail.activeInHierarchy && wormgroup.activeInHierarchy && !dugtailIsPlaying)
        {
            earthquakeSoundEv.start();
            dugtailIsPlaying = true;
        }

        if (!dugtail.activeInHierarchy == false && !dugtailIsPlaying)
        {
            earthquakeSoundEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            earthquakeSoundEv.release();
            dugtailIsPlaying = false;
        }
    }
}
