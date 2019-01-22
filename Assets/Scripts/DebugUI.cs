using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour
{

    public void ParticleCount0()
    {
        GameManagerScript.ParticleIntensity = 0;
    }
    public void ParticleCount1()
    {
        GameManagerScript.ParticleIntensity = 1;

    }
    public void ParticleCount2()
    {
        GameManagerScript.ParticleIntensity = 2;

    }
    public void ParticleCount3()
    {
        GameManagerScript.ParticleIntensity = 3;

    }

    public void ToggleScreenshake()
    {
        GameManagerScript.Screenshake = !(GameManagerScript.Screenshake);
    }

    public void ToggleDamageText()
    {
        GameManagerScript.DamageText = !(GameManagerScript.DamageText);
    }
}
