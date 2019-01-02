using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDeathTimer : MonoBehaviour {

    public float lifetime = 4.9f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
