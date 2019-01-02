using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    public float rawDMG = 10;
    public string DMGtype = "fire";
    public GameObject Owner;
    public GameObject DeathParticle;
    //These are for stretch goals/new projectile types. would probably put them in different scripts.
    public float KnockbackForce = 0;
    public Vector3 kbvector;



    void Start()
    {
        Owner = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("Fireball made contact with" + coll.gameObject);
        if (coll.gameObject.tag == "Enemy")
        {
            Debug.Log( coll.gameObject + "has been struck by projectile");
            coll.SendMessage("DamageHealth", rawDMG);
            // Above is the damage send value, below is where I'm working on knockback. //results inconclusive.
            GameObject EnemyHit = coll.gameObject;
            Rigidbody2D EnemyRB = EnemyHit.GetComponent <Rigidbody2D>();
            Vector3 offset;
            float magsqr;
            offset = Owner.transform.position - EnemyHit.transform.position;
            magsqr = offset.sqrMagnitude;
            if (magsqr > 0f)
            {
            EnemyRB.AddForce((KnockbackForce * offset.normalized / magsqr) * 10);
            }
            //EnemyRB.AddExplosionForce2D(KnockbackForce, kbvector, expRadius, 3.0F); //This is a 3D function, now I have to think about how to pull it off in 2D
            Die();
        }
        if (coll.gameObject.tag == "Environment")
        {
            Debug.Log("PlayerBulletHitWall");
            Die();
        }
    }

    // Use this for initialization


    // Update is called once per frame
    void Update()
    {

    }
    void Die()
    {
        Instantiate(DeathParticle, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}