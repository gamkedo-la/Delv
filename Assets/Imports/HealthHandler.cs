using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{

    public int Health = 100;
    public int Armor = 0;
    public int Shields = 0;
    public float iFrames = 0f;
    public float iAmount = 0.5f;
    int initLayer;
    public GameObject DeathParticle;
    public GameObject ScrapeParticle;
    public Vector3 offset = new Vector3(0, 0, 0);

    void Start()
    {
        initLayer = gameObject.layer;
    }

    void OnTriggerEnter2D()
    {
        Debug.Log("Collision Detected %Health%");

        Instantiate(ScrapeParticle, transform.position + offset, transform.rotation);
        Health -= 1;
        iFrames = iAmount;
        gameObject.layer = 11;

    }
    void Update()
    {
        iFrames -= Time.deltaTime;
        if (iFrames <= 0)
            gameObject.layer = initLayer;
        if (Health <= 0)
        {
            Die();
        }

    }
	void Die ()
    {
        Instantiate(DeathParticle, transform.position + offset, transform.rotation);
        Destroy(gameObject);
	}
}
