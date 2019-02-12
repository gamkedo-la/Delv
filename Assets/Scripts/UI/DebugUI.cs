using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour
{

    public void ParticleCount0()
    {
        GameManagerScript.instance.ParticleIntensity = 0;
    }
    public void ParticleCount1()
    {
        GameManagerScript.instance.ParticleIntensity = 1;

    }
    public void ParticleCount2()
    {
        GameManagerScript.instance.ParticleIntensity = 2;

    }
    public void ParticleCount3()
    {
        GameManagerScript.instance.ParticleIntensity = 3;

    }

    public void ToggleScreenshake()
    {
        GameManagerScript.instance.Screenshake = !(GameManagerScript.instance.Screenshake);
    }

    public void ToggleDamageText()
    {
        GameManagerScript.instance.DamageText = !(GameManagerScript.instance.DamageText);
    }
}
