using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExtParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        if (GameManagerScript.instance.ParticleIntensity < 2)
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        if (GameManagerScript.instance.ParticleIntensity < 2)
        {
            Destroy(this.gameObject);
        }
    }


}
