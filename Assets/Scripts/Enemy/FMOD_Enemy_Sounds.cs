using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMOD_Enemy_Sounds : MonoBehaviour
{


    public void CutSceneSound(string path)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, transform.position);
    }

    public void FirstSlamSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Skeleton Boss/Skeleton_Boss_Punch", transform.position);
    }

    public void BigSlimeMovement()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Slime/Big_Slime_Movement", transform.position);
    }

}
