using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHazard : MonoBehaviour {




    public float DMG = .2f;
    public float fliptimer = 5;
    private float swapTimer;
    public bool isFiring = true;
    public ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents;


    void Start()
    {
        swapTimer = fliptimer;
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void FixedUpdate()
    {
        if (swapTimer <= 0)
        {
            isFiring = !isFiring;
            swapTimer = fliptimer;
            ParticleSystem.EmissionModule em = part.emission;
            em.enabled = !em.enabled;
        }
        if(swapTimer > 0)
        {
            swapTimer -= (Time.deltaTime);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Debug.Log(other.gameObject + "Has been hit by Fire");
        int i = 0;

        while (i < numCollisionEvents)
        {
            if (other.tag == "Player")
            {
                other.SendMessage("DamageHealth", DMG);
            }
            i++;
        }
    }
}
