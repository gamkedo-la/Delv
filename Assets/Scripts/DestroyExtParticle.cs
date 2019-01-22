using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExtParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        if (GameManagerScript.ParticleIntensity < 2)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
    void Start()
    {
        if (GameManagerScript.ParticleIntensity < 2)
        {
            GameObject.Destroy(this.gameObject);
        }
    }


}
