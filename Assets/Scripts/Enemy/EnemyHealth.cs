using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public float Health = 100;
    public int Armor = 0;
    public int Shields = 0;
    public float iFrames = 0f;
    public float iAmount = 0.5f;
    int initLayer;
    public GameObject DeathParticle;
    public GameObject ScrapeParticle;
    public GameObject TinyScrapeParticle;

    void Awake()
    {
        initLayer = gameObject.layer;
    }
    void Start()
    {

    }
    // This is deprecated from the old script, just keeping it as reference.
    //void OnTriggerEnter2D()
    //{
    //    Debug.Log("Collision Detected %Health%");

    //    Instantiate(ScrapeParticle, transform.position + offset, transform.rotation);
    //    Health -= 1;
    //    iFrames = iAmount;
    //    gameObject.layer = 11;

    //}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "PlayerProjectile")
        {
            //Code wizard note: If anyone has a simple way of pushing the player away from the damage contact points, I'd be all ears. Had an idea to inverse the vector of where it came from and add force but not sure.
            Debug.Log("Player Projectile Encountered");
        }
    }

    void Update()
    {
        //This handles iFrames, probably won't use them for most enemies, maybe bosses. Will leave it in for melee type enemies, since they might use it. 
        iFrames -= Time.deltaTime;
        if (iFrames <= 0)
            gameObject.layer = initLayer;
        if (Health <= 0)
        {
            Die();
        }

    }
    //Will handle damage through sendmessage even though its a bit slow resource wise. Small game should be able to handle it.
    //Will also add Damage type for resistences maybe as a stretch // , string DMGtype
    void DamageHealth(float DMG)
    {

        FloatingTextController.CreateFloatingText(DMG.ToString(),transform,DMG);
        Debug.Log(gameObject + " health damaged by " + DMG);
        Health -= DMG;
        iFrames = iAmount;
        gameObject.layer = 12;
        Debug.Log(gameObject + " health is now " + Health);
        if (DMG > 10)
        {
            Instantiate(ScrapeParticle, transform.position, transform.rotation);
        }
        if (DMG < 10)
        {
            Instantiate(TinyScrapeParticle, transform.position, transform.rotation);
        }

    }
    void Die()
    {
        Instantiate(DeathParticle, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
