using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour {

    public float rawDMG = 1;
    public string DMGtype = "normal";
    public GameObject Owner;
    public GameObject DeathParticle;
    //These are for stretch goals/new projectile types. would probably put them in different scripts.
    public bool explosiveDeath;
    public bool implosiveDeath;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Debug.Log("Player has been struck by projectile");
            coll.SendMessage("DamageHealth", rawDMG);
        }
        if (coll.gameObject.tag == "Environment")
        {
            Debug.Log("EnemyBulletHitWall");
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    void Die()
    {
        //Instantiate(DeathParticle, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
