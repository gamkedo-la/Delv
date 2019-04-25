using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    public float rawDMG = 10;
    public string DMGtype = "fire";
    public GameObject Owner;
    public GameObject DeathParticle;
    //These are for stretch goals/new projectile types. would probably put them in different scripts.
    public float KnockbackForce = 0;
    public Vector3 kbvector;
    private GameObject ParticleManager;

    void Start()
    {
        Owner = GameObject.FindGameObjectWithTag("Player");
        ParticleManager = GameObject.Find("ParticleManager");
        if (ParticleManager == null)
        {
            Debug.Log("ParticleManager is not present - spawning particles in heirarchy");
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Fireball made contact with" + coll.gameObject);
        if (coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Breakable")
        {
            Debug.Log(coll.gameObject + "has been struck by projectile");
            coll.gameObject.SendMessage("DamageHealth", rawDMG);
            // Above is the damage send value, below is where I'm working on knockback. //results inconclusive.
            Vector3 offset = Owner.transform.position - coll.gameObject.transform.position;
            float magsqr = offset.sqrMagnitude;

            if (magsqr > 0.0f)
            {
                Vector3 newPOS = coll.gameObject.transform.position;
                Vector2 tempV2 = (Vector2)coll.gameObject.transform.position;
                tempV2 += coll.GetContact(0).normal * (KnockbackForce * offset.normalized / magsqr);
                newPOS.x = tempV2.x;
                newPOS.y = tempV2.y;
                coll.gameObject.transform.position = newPOS;
            }

            //EnemyRB.AddExplosionForce2D(KnockbackForce, kbvector, expRadius, 3.0F); //This is a 3D function, now I have to think about how to pull it off in 2D
            Die();
        }
        if (coll.gameObject.tag == "Environment")
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("Fireball made contact with" + coll.gameObject);
        if (coll.gameObject.tag == "Enemy")
        {
            Debug.Log(coll.gameObject + "has been struck by projectile");
            coll.gameObject.SendMessage("DamageHealth", rawDMG);
            Die();
        }
    }

    void Die()
    {
        if (GameManagerScript.instance.ParticleIntensity > 1)
        {
            if (ParticleManager == null)
            {
                Debug.Log("Projectile Collector is not present - spawning shots in heirarchy");
            }

            GameObject TempParticle = Instantiate(DeathParticle, transform.position, transform.rotation);
            TempParticle.transform.SetParent(ParticleManager.transform);

            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Player/Weapons/fire_wand/firewand_explosion", gameObject);
        }
        Destroy(gameObject);
    }

}