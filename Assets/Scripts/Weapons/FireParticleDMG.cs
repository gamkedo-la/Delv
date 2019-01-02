using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireParticleDMG : MonoBehaviour {



    public float DMG = .2f;
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Debug.Log(other.gameObject + "Has been hit by Fire");
        int i = 0;

        while (i < numCollisionEvents)
        {
            if (other.tag == "Enemy")
            {
                other.SendMessage("DamageHealth", DMG);
            }
            i++;
        }
    }
}
